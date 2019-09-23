using System.Text.RegularExpressions;

namespace Subarashii.Core
{
    internal sealed class Constants
    {
        public const int HEADER_LENGTH = 12;
        public const int MAX_PAYLOAD_SIZE = 1500 - sizeof(int) - HEADER_LENGTH;
        public static Regex codeRegex = new Regex(@"^[0-9]{2}$");
    }
}
