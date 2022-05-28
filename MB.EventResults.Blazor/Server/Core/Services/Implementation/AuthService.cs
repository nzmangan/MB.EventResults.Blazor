using System.Text;

namespace MB.EventResults.Blazor.Server;

public class AuthService : IAuthService {
  private readonly string _Username;
  private readonly string _Password;

  public AuthService(AppConfiguration optionsAccessor) {
    _Username = optionsAccessor.UploadUsername;
    _Password = optionsAccessor.UploadPassword;
  }

  public bool CheckAuth(IHeaderDictionary headers) {
    return CheckAuth(headers["Authorization"]);
  }

  public bool CheckAuth(string authHeader) {
    if (String.IsNullOrWhiteSpace(_Username) || String.IsNullOrWhiteSpace(_Password)) {
      return true;
    }

    if (authHeader == null && !authHeader.StartsWith("Basic")) {
      return false;
    }

    string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
    string usernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

    int seperatorIndex = usernamePassword.IndexOf(':');

    var username = usernamePassword.Substring(0, seperatorIndex);
    var password = usernamePassword.Substring(seperatorIndex + 1);

    var valid = IsValidUser(username, password);

    return valid;
  }

  public bool IsValidUser(string username, string password) {
    return _Username == username && _Password == password;
  }
}