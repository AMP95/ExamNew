using Microsoft.AspNetCore.Http;

namespace MediatorServices.Abstract
{
    public interface IFileManager
    {
        bool RemoveFile(string path);
        bool SaveFile(string path, IFormFile file);
        IFormFile GetFile(string path, IFormFile file);
    }
}
