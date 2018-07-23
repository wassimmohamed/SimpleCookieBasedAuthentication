namespace SimpleCookieBasedAuthentication.Models.Common
{
    public class AppConfig
    {
        public string FirmwareFolderPath { get; set; }
        public string FirmwareFilterExt { get; set; }
    }

    public class AdminConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}