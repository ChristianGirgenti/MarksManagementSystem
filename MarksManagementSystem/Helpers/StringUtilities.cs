using System.Globalization;

namespace MarksManagementSystem.Helpers
{
    public class StringUtilities
    {
        public static string Capitalise(string str)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(str);
        }
    }
}
