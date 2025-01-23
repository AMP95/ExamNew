﻿using Microsoft.AspNetCore.Http;

namespace MediatorServices.Abstract
{
    public interface IFileManager
    {
        Task<bool> RemoveFile(string path);
        Task<bool> SaveFile(string directory, string name, IFormFile file);
        Task<IFormFile> GetFile(string path, string name);
    }
}
