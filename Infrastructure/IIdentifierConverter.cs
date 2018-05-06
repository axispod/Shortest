namespace Infrastructure
{
    public interface IIdentifierConverter
    {
        string Encode(long id);
        long Decode(string id);
    }
}