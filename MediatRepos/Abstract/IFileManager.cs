using Microsoft.AspNetCore.Http;

namespace MediatorServices.Abstract
{
    public interface IFileManager
    {
        bool Save(string path, IFormFile file);
        IFormFile Get(string path);
    }
}
