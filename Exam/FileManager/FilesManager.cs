using MediatorServices.Abstract;

namespace Exam.FileManager
{
    public class FilesManager : IFileManager
    {
        private IWebHostEnvironment _environment;
        private ILogger<FilesManager> _logger;

        public FilesManager(IWebHostEnvironment webHostEnvironment,
                            ILogger<FilesManager> logger)
        {
            _environment = webHostEnvironment;
            _logger = logger;
        }

        public Task<bool> RemoveAllFiles(string entityCatalog, string catalog)
        {
            string directory = Path.Combine(_environment.WebRootPath, "Files", entityCatalog, catalog);
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                directoryInfo.Delete(true);
                return Task.FromResult(true);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromResult(false);
            }
        }

        public Task<bool> RemoveFile(string filePathWithoutRoot)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, "Files", filePathWithoutRoot);
            try 
            { 
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Task.FromResult(false);
            }
        }

        public async Task<bool> TempSave(string fileNameOnlyWithExtencion, IFormFile file)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, "Temp" , fileNameOnlyWithExtencion);

            try
            {
                Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Temp"));

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> SaveFile(string filePathWithoutRoot, string fileNameInTempRoot)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, "Files", filePathWithoutRoot);
            string tempFullPath = Path.Combine(_environment.WebRootPath, "Temp", fileNameInTempRoot);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (FileStream outputStream = File.OpenRead(tempFullPath))
                {
                    using (FileStream inputStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await outputStream.CopyToAsync(inputStream);
                        
                    }
                }

                File.Delete(tempFullPath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<byte[]> GetFile(string filePathWithoutRoot, string viewNameVithExtencion)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, "Files", filePathWithoutRoot);
            try
            {
                return await File.ReadAllBytesAsync(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }

        public string GetFullPath(string pathWithOutRoot)
        {
           return Path.Combine(_environment.WebRootPath, "Files", pathWithOutRoot);
        }
    }
}
