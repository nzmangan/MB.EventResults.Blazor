using Microsoft.Extensions.Hosting;

namespace MB.EventResults.Uploader;

public record WatchOptions(bool Once);

public class WatchService(ILogger<WatchService> _Logger, UploaderConfiguration _Configuration, WatchOptions _Options, IofFileStore _Store, ResultPublisher _Publisher, IHostApplicationLifetime _Lifetime) : BackgroundService {
  private readonly SemaphoreSlim _Changed = new(0, 1);
  private long _ChangeCount = 0;
  private string _LastPublished = null;

  protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
    await Task.Yield();

    if (_Options.Once) {
      Environment.ExitCode = await Process(stoppingToken) ? 0 : 1;
      _Lifetime.StopApplication();
      return;
    }

    using var watcher = new FileSystemWatcher(_Store.Folder, "*.xml") {
      NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
    };

    watcher.Created += (_, _) => Signal();
    watcher.Changed += (_, _) => Signal();
    watcher.Renamed += (_, _) => Signal();
    watcher.Deleted += (_, _) => Signal();
    watcher.Error += (_, e) => _Logger.LogWarning(e.GetException(), "File watcher error, relying on polling.");
    watcher.EnableRaisingEvents = true;

    _Logger.LogInformation($"Watching {_Store.Folder} for IOF XML files.");

    try {
      while (!stoppingToken.IsCancellationRequested) {
        await Process(stoppingToken);

        // Polling catches what the watcher misses, such as changes on network shares, and retries failures.
        if (await _Changed.WaitAsync(TimeSpan.FromSeconds(_Configuration.PollSeconds), stoppingToken)) {
          await WaitUntilQuiet(stoppingToken);
        }
      }
    } catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) {
    }
  }

  private void Signal() {
    Interlocked.Increment(ref _ChangeCount);

    try {
      _Changed.Release();
    } catch (SemaphoreFullException) {
    }
  }

  // A single save raises several events, and large files take a while to be written.
  private async Task WaitUntilQuiet(CancellationToken cancellationToken) {
    long count;

    do {
      count = Interlocked.Read(ref _ChangeCount);
      await Task.Delay(TimeSpan.FromSeconds(_Configuration.DebounceSeconds), cancellationToken);
    } while (count != Interlocked.Read(ref _ChangeCount));

    await _Changed.WaitAsync(TimeSpan.Zero, cancellationToken);
  }

  private async Task<bool> Process(CancellationToken cancellationToken) {
    try {
      var current = _Store.Scan();

      if (!_Store.HasFiles) {
        _Logger.LogDebug("No IOF XML files found.");
        return false;
      }

      if (current == _LastPublished) {
        return true;
      }

      await _Publisher.Publish(cancellationToken);
      _LastPublished = current;
      return true;
    } catch (Exception ex) when (ex is not OperationCanceledException) {
      _Logger.LogError(ex, "Could not publish results. The previously published results stay online.");
      return false;
    }
  }
}
