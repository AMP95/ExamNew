namespace Utilities.Interfaces
{
    public interface ITokenService<T>
    {
        string GetToken(T user);
    }
}
