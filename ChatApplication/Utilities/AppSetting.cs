namespace ChatApplication.Utilities
{
    public class AppSetting
    {
        public ConncetionString ConncetionString { get; set; }
        public JwtSetting JwtSetting { get; set; }
    }

    public class ConncetionString
    {
        public string ChatDbConnnectionString { get; set; }
    }

    public class JwtSetting
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
