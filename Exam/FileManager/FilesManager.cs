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

        public Task<IFormFile> GetFile(string path, string name)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, path);
            IFormFile file = null;
            try
            {
                using (var stream = new MemoryStream(File.ReadAllBytes(fullPath).ToArray())) 
                {
                    file = new FormFile(stream, 0, stream.Length, null, name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return Task.FromResult<IFormFile>(file);
        }

        public Task<bool> RemoveFile(string path)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, path);
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

        public async Task<bool> SaveFile(string path, IFormFile file)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, path);
            try
            {
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    return true;
                }
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex,ex.Message);
                return false;
            }
        }
    }
}
