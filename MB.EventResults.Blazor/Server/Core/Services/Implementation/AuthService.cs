using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;

namespace MB.EventResults.Blazor.Server;

public class AuthService(AppConfiguration optionsAccessor) : IAuthService {
  private readonly string _Username = optionsAccessor.UploadUsername;
  private readonly string _Password = optionsAccessor.UploadPassword;
  private readonly string _ApiKey = optionsAccessor.UploadKey;
  private readonly string _AuthMode = optionsAccessor.AuthMode ?? "";

  public bool CheckAuth(IHeaderDictionary headers) {
    if (_AuthMode.Equals("none", StringComparison.InvariantCultureIgnoreCase)) {
      return true;
    }

    if (_AuthMode.Equals("key", StringComparison.InvariantCultureIgnoreCase)) {
      return CheckApiKey(headers);
    }

    if (_AuthMode.Equals("basic", StringComparison.InvariantCultureIgnoreCase)) {
      return CheckBasicAuth(headers);
    }

    return false;
  }

  private bool CheckApiKey(IHeaderDictionary headers) {
    if (!headers.TryGetValue("x-api-key", out StringValues key)) {
      return false;
    }

    if (String.IsNullOrWhiteSpace(_ApiKey)) {
      return false;
    }

    return SecureEquals(_ApiKey, key.ToString());
  }

  private bool CheckBasicAuth(IHeaderDictionary headers) {
    string authHeader = headers.Authorization;

    if (authHeader == null || !authHeader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase)) {
      return false;
    }

    if (String.IsNullOrWhiteSpace(_Username) || String.IsNullOrWhiteSpace(_Password)) {
      return false;
    }

    string encodedUsernamePassword = authHeader["Basic ".Length..].Trim();
    string usernamePassword;

    try {
      usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
    } catch (FormatException) {
      return false;
    }

    int seperatorIndex = usernamePassword.IndexOf(':');

    if (seperatorIndex < 0) {
      return false;
    }

    var username = usernamePassword[..seperatorIndex];
    var password = usernamePassword[(seperatorIndex + 1)..];

    var valid = IsValidUser(username, password);

    return valid;
  }

  public bool IsValidUser(string username, string password) {
    return SecureEquals(_Username, username) & SecureEquals(_Password, password);
  }

  // Hashing first keeps the comparison from revealing the length of the secret.
  private static bool SecureEquals(string expected, string actual) {
    return CryptographicOperations.FixedTimeEquals(SHA256.HashData(Encoding.UTF8.GetBytes(expected)), SHA256.HashData(Encoding.UTF8.GetBytes(actual)));
  }
}