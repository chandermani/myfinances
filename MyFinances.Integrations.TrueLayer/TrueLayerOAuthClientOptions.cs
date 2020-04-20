namespace MyFinances.Integrations.TrueLayer
{
    public class TrueLayerOAuthClientOptions
    {
        public string AuthUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}