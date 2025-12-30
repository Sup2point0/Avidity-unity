namespace Avidity
{
    public static partial class Utils
    {
        public static class Display
        {
            public static string Duration(float? seconds)
            {
                if (seconds is null) return "--:--";

                var secs = (int) seconds;
                var m = secs / 60;
                var mm = m.ToString();

                var s = secs % 60;
                var ss = s.ToString().PadLeft(2, '0');

                return $"{mm}:{ss}";
            }
            
            public static string Timespan(float? seconds, bool may_exceed = false)
            {
                if (seconds is null) return "Unknown Duration";

                var secs = (int) seconds;
                var h = secs / 60 / 60;
                var m = secs / 60;

                if (h > 0) {
                    return $"{h} h, {m} m";
                }
                else {
                    var s = secs % 60;
                    return $"{m} m, {s} s";
                }
            }
        }
    }
}
