using Models.Main;

namespace MediatorServices.Abstract
{
    public interface ITokenService
    {
        string GetToken(Logist logist);
    }
}
