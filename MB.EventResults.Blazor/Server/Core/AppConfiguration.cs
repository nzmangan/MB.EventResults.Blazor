namespace MB.EventResults.Blazor.Server.Core;

public class AppConfiguration {
  public string Mode { get; set; }
  public string ResultsSourceFile { get; set; }
  public string CourseDataSourceFile { get; set; }
  public string UploadUsername { get; set; }
  public string UploadPassword { get; set; }
  public string UploadFolder { get; set; }
  public string UploadKey { get; set; }
  public string AuthMode { get; set; }
  public string S3BucketName { get; set; }
}