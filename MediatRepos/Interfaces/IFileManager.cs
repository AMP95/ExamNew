using Microsoft.AspNetCore.Http;

namespace MediatorServices.Abstract
{
    public interface IFileManager
    {
        Task<byte[]> GetFile(string filePathWithoutRoot, string viewNameVithExtencion);
        Task<bool> TempSave(string filename, IFormFile file);
        Task<bool> SaveFile(string filePathWithoutRoot, string fileNameInTempRoot);
        Task<bool> RemoveFile(string filePathWithoutRoot);
        Task<bool> RemoveAllFiles(string entityCatalog, string catalog);
        string GetFullPath(string pathWithOutRoot);
    }
}
