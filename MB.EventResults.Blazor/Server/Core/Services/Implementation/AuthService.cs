using Microsoft.Extensions.Primitives;

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

    return _ApiKey.Equals(key, StringComparison.InvariantCultureIgnoreCase);
  }

  private bool CheckBasicAuth(IHeaderDictionary headers) {
    string authHeader = headers.Authorization;

    if (authHeader == null || !authHeader.StartsWith("basic", StringComparison.CurrentCultureIgnoreCase)) {
      return false;
    }

    if (String.IsNullOrWhiteSpace(_Username) || String.IsNullOrWhiteSpace(_Password)) {
      return false;
    }

    string encodedUsernamePassword = authHeader["Basic ".Length..].Trim();
    string usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

    int seperatorIndex = usernamePassword.IndexOf(':');

    var username = usernamePassword[..seperatorIndex];
    var password = usernamePassword[(seperatorIndex + 1)..];

    var valid = IsValidUser(username, password);

    return valid;
  }

  public bool IsValidUser(string username, string password) {
    return _Username == username && _Password == password;
  }
}