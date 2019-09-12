using System.Text.RegularExpressions;

namespace Subarashii.Core
{
    internal sealed class Constants
    {
        public const int HEADER_LENGTH = 12;
        public static Regex codeRegex = new Regex(@"^[0-9]{2}$");
    }
}
