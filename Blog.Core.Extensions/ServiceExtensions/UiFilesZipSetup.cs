using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.IO.Compression;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// 将前端UI压缩文件进行解压
    /// </summary>
    public static class UiFilesZipSetup
    {
        public static void AddUiFilesZipSetup(this IServiceCollection services, IWebHostEnvironment _env)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            string zipUiItemFiles = Path.Combine(_env.ContentRootPath, "wwwroot", "ui.zip");
            ZipFile.ExtractToDirectory(zipUiItemFiles, Path.Combine(_env.ContentRootPath, "wwwroot"));
        }
    }
}
