namespace MB.EventResults.Uploader;

public class UploaderConfiguration {
  public string WatchFolder { get; set; }
  public int DebounceSeconds { get; set; } = 2;
  public int PollSeconds { get; set; } = 10;
  public TargetConfiguration Target { get; set; } = new();
}

public class TargetConfiguration {
  public string Type { get; set; } = "Sftp";
  public string Path { get; set; }
  public string Host { get; set; }
  public int Port { get; set; } = 22;
  public string Username { get; set; }
  public string Password { get; set; }
  public string HostKeySha256 { get; set; }
}
