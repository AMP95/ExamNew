using Microsoft.AspNetCore.Http;

namespace MediatorServices.Abstract
{
    public interface IFileManager
    {
        Task<bool> RemoveFile(string filePathWithoutRoot);
        Task<bool> SaveFile(string filePathWithoutRoot, string fileNameInTempRoot);
        Task<bool> TempSave(string filename, IFormFile file);
        Task<IFormFile> GetFile(string filePathWithoutRoot, string viewNameVithExtencion);
        Task<bool> RemoveAllFiles(string entityCatalog, string catalog);
    }
}
