namespace MyFinances.Integrations.TrueLayer
{
    public class TrueLayerOAuthClientOptions
    {
        public string ApiUri { get; set; }
        public string AuthUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}