
namespace Utilities.Common
{
    public static class FormatSearchLike
    {
        public static string FormatSearch(this string search)
        {
            return "%" + search + "%";
        }
    }
}
