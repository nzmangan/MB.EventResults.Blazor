using Amazon.S3;
using Amazon.S3.Model;

namespace MB.EventResults.Blazor.Server;

public class S3FileService(ILogger<S3FileService> _Logger, IAmazonS3 _S3Client, AppConfiguration _AppConfiguration) : IFileService {
  public async Task Delete(string filePath) {
    try {
      var request = new DeleteObjectRequest {
        BucketName = _AppConfiguration.S3BucketName,
        Key = filePath
      };
      await _S3Client.DeleteObjectAsync(request);
    } catch (AmazonS3Exception e) {
      Console.WriteLine("Error deleting object " + filePath + ". " + e.Message);
    }
  }

  public async Task<bool> Exists(string filePath) {
    try {
      var request = new GetObjectMetadataRequest {
        BucketName = _AppConfiguration.S3BucketName,
        Key = filePath
      };
      await _S3Client.GetObjectMetadataAsync(request);
      return true;
    } catch (AmazonS3Exception e) {
      _Logger.LogError(e.ToString());
      if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
        return false;
      }
      throw;
    }
  }

  public string GetPath(string filePath) {
    // In S3, the filePath itself is the path within the bucket
    return filePath;
  }

  public async Task<DateTime> LastUpdated(string filePath) {
    try {
      var request = new GetObjectMetadataRequest {
        BucketName = _AppConfiguration.S3BucketName,
        Key = filePath
      };
      var response = await _S3Client.GetObjectMetadataAsync(request);
      return response.LastModified;
    } catch (AmazonS3Exception e) {
      _Logger.LogError(e.ToString());
      throw;
    }
  }

  public async Task<string> Load(string filePath) {
    try {
      var request = new GetObjectRequest {
        BucketName = _AppConfiguration.S3BucketName,
        Key = filePath
      };
      using var response = await _S3Client.GetObjectAsync(request);
      using var streamReader = new StreamReader(response.ResponseStream);
      return await streamReader.ReadToEndAsync();
    } catch (AmazonS3Exception e) {
      _Logger.LogError(e.ToString());
      throw;
    }
  }

  public async Task Save(string filePath, string content) {
    try {
      var request = new PutObjectRequest {
        BucketName = _AppConfiguration.S3BucketName,
        Key = filePath,
        ContentBody = content
      };
      await _S3Client.PutObjectAsync(request);
    } catch (AmazonS3Exception e) {
      _Logger.LogError(e.ToString());
      throw;
    }
  }
}