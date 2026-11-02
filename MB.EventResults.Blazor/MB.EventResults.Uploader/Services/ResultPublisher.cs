using System.IO.Compression;
using System.Text;

namespace MB.EventResults.Uploader;

public class ResultPublisher(ILogger<ResultPublisher> _Logger, IResultBuilderService _ResultBuilderService, IJsonSerializerService _JsonSerializerService, IResultTarget _ResultTarget) {
  public async Task Publish(CancellationToken cancellationToken) {
    var results = await _ResultBuilderService.Build();
    var json = Encoding.UTF8.GetBytes(_JsonSerializerService.Serialize(results));
    var compressed = Compress(json);

    // The compressed copy lets the web server send it without using any cpu on compression.
    await _ResultTarget.Upload([
      new TargetFile(UrlConstants.StaticResultsFileName, json),
      new TargetFile($"{UrlConstants.StaticResultsFileName}.gz", compressed)
    ], cancellationToken);

    _Logger.LogInformation($"Published {results.Results.Count} grades to {_ResultTarget.Description} ({json.Length / 1024} KB, {compressed.Length / 1024} KB compressed).");
  }

  private static byte[] Compress(byte[] content) {
    using var output = new MemoryStream();

    using (var gzip = new GZipStream(output, CompressionLevel.SmallestSize)) {
      gzip.Write(content);
    }

    return output.ToArray();
  }
}
