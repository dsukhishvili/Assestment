using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Helpers
{
    public class ImageHelper
    {
        public IHostingEnvironment _hostingEnv { get; set; }
        public ImageHelper(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }
        public async Task<string> SaveImageToDisk(int imageId, IFormFile image)
        {
            var rootPath = _hostingEnv.WebRootPath + "\\Images";
            var fileName = $"{imageId}_{image.FileName}";
            var fullImagePath = Path.Combine(rootPath,fileName);
            var fileRelativePath = $"~/Images/{fileName}";
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            if (File.Exists(fullImagePath))
                File.Delete(fullImagePath);
            using (var fileStream = new FileStream(fullImagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return fileRelativePath;
        }

        public string GetImageFromDisk (string relativePath)
        {
            var physicalPath = relativePath.Replace("~/", _hostingEnv.WebRootPath+"\\");
            if (!File.Exists(physicalPath))
                return "";
            byte[] contentBytes = File.ReadAllBytes(physicalPath);
            return "data:image/jpeg;base64," + Convert.ToBase64String(contentBytes);
        }
    }
}
