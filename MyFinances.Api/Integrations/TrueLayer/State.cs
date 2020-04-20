namespace MyFinances.Api.Integrations
{
    public class State
    {
        public State(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; }
    }
}