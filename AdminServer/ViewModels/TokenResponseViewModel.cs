namespace AdminServer.ViewModels
{
    public class TokenResponseViewModel
    {
        public string token { get; set; }

        private TokenResponseViewModel()
        {
        }

        public static TokenResponseViewModel WithToken(string token)
        {
            return new TokenResponseViewModel
            {
                token = token
            };
        }
    }
}