using System;


namespace Avidity
{
    public static partial class Utils
    {
        public static class Display
        {
            public static String Duration(float? seconds)
                => Duration((int) seconds);
            
            public static String Duration(int? seconds)
            {
                if (seconds == null)
                {
                    return "--:--";
                }

                var mins = seconds / 60;
                var m = mins.ToString();

                var secs = seconds % 60;
                var s = secs.ToString().PadLeft(2, '0');

                return $"{m}:{s}";
            }
        }
    }
}
