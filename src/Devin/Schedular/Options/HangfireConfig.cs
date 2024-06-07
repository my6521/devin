namespace Devin.Schedular.Options
{
    public class HangfireConfig
    {
        public string ConnectionString { get; set; }
        public int Db { get; set; }
        public string Prefix { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int JobExpirationTimeout { get; set; }
    }
}