#nullable enable

using System.Linq;


namespace Avidity
{
    public static partial class Utils
    {
        public static string JoinPaths(params string?[] paths)
            => string.Join("/", paths.Where(p => p is not null));
    }
}
