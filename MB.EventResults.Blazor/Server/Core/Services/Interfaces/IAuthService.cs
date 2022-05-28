namespace MB.EventResults.Blazor.Server;

public interface IAuthService {
  bool CheckAuth(IHeaderDictionary headers);
  bool IsValidUser(string username, string password);
}
