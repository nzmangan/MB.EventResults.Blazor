namespace MB.EventResults.Blazor.Client;

public static class DateHelper {
  public static string ToReadableDate(this DateTime input) {
    return input.ToString("dd/MM/yyyy HH:mm:ss");
  }
  public static string ToReadableDate(this DateTime? input) {
    if (!input.HasValue) {
      return "";
    }

    return input.ToReadableDate();
  }

  public static string ToReadableShortDate(this DateTime input) {
    return input.ToString("dd/MM/yyyy");
  }

  public static string ToReadableShortDate(this DateTime? input) {
    if (!input.HasValue) {
      return "";
    }

    return input.Value.ToReadableShortDate();
  }
}
