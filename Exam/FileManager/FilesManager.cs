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

        public Task<IFormFile> GetFile(string filePathWithoutRoot, string viewNameVithExtencion)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, filePathWithoutRoot);
            IFormFile file = null;
            try
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(fullPath).ToArray())) 
                {
                    file = new FormFile(stream, 0, stream.Length, null, viewNameVithExtencion);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Task.FromResult<IFormFile>(file);
        }

        public Task<bool> RemoveAllFiles(string entityCatalog, string catalog)
        {
            string directory = Path.Combine(_environment.WebRootPath, entityCatalog, catalog);
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
            string fullPath = Path.Combine(_environment.WebRootPath, filePathWithoutRoot);
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
    }
}
