namespace MyFinances.Api.Integrations
{
    public interface IStateDecoder
    {
        State Decode(string state);
    }
}