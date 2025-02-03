namespace MB.EventResults.Blazor.Client;

public static class ViewHelperService {
  public static string GetAbbrivation(string name) {
    return string.Concat(name.Where(c => char.IsUpper(c)));
  }

  public static string FormatStatus(this Runner runner) {
    if (runner.Status is null) {
      return "Unknown";
    }

    if (runner.Status == Constants.StatusOK) {
      return FormatTime(runner.TimeInSeconds);
    }

    if (runner.Status == Constants.StatusActive && runner.StartTime is not null) {
      return GetHourMinSec(runner.StartTime.Value);
    }

    return FormatStatus(runner.Status);
  }

  public static string FormatLeg(this Split leg) {
    return FormatCore(leg?.Leg, leg?.LegPosition);
  }

  public static string FormatTotal(this Split leg) {
    return FormatCore(leg?.Total, leg?.TotalPosition);
  }

  public static string FormatCore(double? time, int? position) {
    var strTime = FormatTime(time);

    if (!String.IsNullOrWhiteSpace(strTime) && position.HasValue) {
      var strPosition = position.ToString();

      if (position == 1) {
        strPosition = "🥇";
      } else if (position == 2) {
        strPosition = "🥈";
      } else if (position == 3) {
        strPosition = "🥉";
      }

      return $"{strTime} - {strPosition}";
    }

    if (!String.IsNullOrWhiteSpace(strTime)) {
      return strTime;
    }

    return "";
  }

  private static string To2Digits(this int input) {
    return input.ToString().PadLeft(2, '0');
  }

  private static string To2Digits(this double input) {
    return Convert.ToInt32(input).ToString().PadLeft(2, '0');
  }

  public static string GetHourMinSec(this double? input) {
    return input.HasValue ? input.Value.GetHourMinSec() : "";
  }


  public static string GetHourMinSec(this DateTime? input) {
    return input.HasValue ? input?.ToString("HH:mm:ss") : "";
  }

  public static string GetHourMinSec(this double input) {
    var value = Math.Abs(input);

    var hourMod = value % 3600;
    var hourVal = ((value - hourMod) / 3600);
    var hour = Convert.ToInt32(hourVal);
    value -= (hourVal * 3600);
    var sec = value % 60;
    var min = ((value - sec) / 60);

    return $"{hour}:{min.To2Digits()}:{sec.To2Digits()}";
  }

  public static string FormatTime(this double time) {
    if (time == 0) {
      return "00:00";
    }

    var prefix = time < 0 ? "-" : "";
    time = Math.Abs(time);
    var sec = time % 60;
    var min = (time - sec) / 60;

    return $"{prefix}{Convert.ToInt32(min).To2Digits()}:{Convert.ToInt32(sec).To2Digits()}";
  }

  public static string FormatTime(this double? time) {
    if (!time.HasValue) {
      return "";
    }

    return time.Value.FormatTime();
  }

  public static string DecimalMinuteToNormalMinute(this double? decimalMinute) {
    if (!decimalMinute.HasValue) {
      return "";
    }

    int wholeMinutes = (int)decimalMinute;

    double decimalPart = decimalMinute.Value - wholeMinutes;

    int seconds = (int)(decimalPart * 60);

    return $"{wholeMinutes}.{seconds:D2}";
  }

  public static string FormatStatus(this string status) {
    if (status is null) {
      return "";
    }

    return status.Replace(" ", "");
  }

  public static string Round(this double? numberToRound, string suffix) {
    if (!numberToRound.HasValue) {
      return "";
    }

    return (Math.Round(numberToRound.Value * 10000) / 100).ToString() + suffix;
  }

  public static double? Round(this double? numberToRound, int digits) {
    return numberToRound.HasValue ? numberToRound.Value.Round(digits) : null;
  }

  public static double Round(this double numberToRound, int digits) {
    return Math.Round(numberToRound, digits);
  }

  public static string Error(this double? index) {
    var m = Mistake(index);
    return m == 0 ? "" : $"mistake-{m}";
  }

  public static string Pack(this double index) {
    return index switch {
      < -20 => "pm5",
      < -15 => "pm4",
      < -10 => "pm3",
      < -5 => "pm2",
      < 0 => "pm1",
      > 20 => "pp1",
      > 15 => "pp2",
      > 10 => "pp3",
      > 5 => "pp4",
      >= 0 => "pp5",
      _ => ""
    };
  }

  private static int Mistake(this double? index) {
    return GetLevel(index, [115, 112, 109, 106, 103, 100, 97, 94, 91, 88, 85]);
  }

  private static int GetLevel(double? index, List<int> levels) {
    if (!index.HasValue) {
      return 0;
    }

    for (var i = 0; i < levels.Count; i++) {
      if (index * 100 >= levels[i]) {
        return levels[i];
      }
    }

    return 82;
  }
}