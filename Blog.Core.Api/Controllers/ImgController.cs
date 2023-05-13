using Blog.Core.Model;
using Blog.Core.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 图片管理
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImgController : BaseApiController
    {
        
        private readonly IWebHostEnvironment _env;

        public ImgController(IWebHostEnvironment webHostEnvironment)
        {
            _env = webHostEnvironment;
        }


        // GET: api/Download
        /// <summary>
        /// 下载图片（支持中文字符）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/images/Down/Pic")]
        public FileStreamResult DownImg()
        {
            string foldername = "";
            string filepath = Path.Combine(_env.WebRootPath, foldername, "测试下载中文名称的图片.png");
            var stream = System.IO.File.OpenRead(filepath);
            string fileExt = ".jpg";  // 这里可以写一个获取文件扩展名的方法，获取扩展名
            //获取文件的ContentType
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExt];
            var fileName = Path.GetFileName(filepath);


            return File(stream, memi, fileName);
        }

        /// <summary>
        /// 上传图片,多文件
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/images/Upload/Pic")]
        public async Task<MessageModel<string>> InsertPicture([FromForm]UploadFileDto dto)
        {
            
            if (dto.file == null || !dto.file.Any()) return Failed("请选择上传的文件。");
            //格式限制
            var allowType = new string[] { "image/jpg", "image/png", "image/jpeg" };
            
            var allowedFile = dto.file.Where(c => allowType.Contains(c.ContentType));
            if (!allowedFile.Any()) return Failed("图片格式错误");
            if (allowedFile.Sum(c => c.Length) > 1024 * 1024 * 4) return Failed("图片过大");

            string foldername = "images";
            string folderpath = Path.Combine(_env.WebRootPath, foldername);
            if (!Directory.Exists(folderpath))
            {
                Directory.CreateDirectory(folderpath);
            }
            foreach (var file in allowedFile)
            {
                string strpath = Path.Combine(foldername, DateTime.Now.ToString("MMddHHmmss") + Path.GetFileName(file.FileName));
                var path = Path.Combine(_env.WebRootPath, strpath);

                using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    await file.CopyToAsync(stream);
                }
            }

            var excludeFiles = dto.file.Except(allowedFile);

            if (excludeFiles.Any())
            {
                var infoMsg = $"{string.Join('、', excludeFiles.Select(c => c.FileName))} 图片格式错误";
                return Success<string>(null, infoMsg);
            }

            return Success<string>(null, "上传成功");

        }



        [HttpGet]
        [Route("/images/Down/Bmd")]
        [AllowAnonymous]
        public FileStreamResult DownBmd(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return null;
            }
            // 前端 blob 接收，具体查看前端admin代码
            string filepath = Path.Combine(_env.WebRootPath, Path.GetFileName(filename));
            if (System.IO.File.Exists(filepath))
            {
                var stream = System.IO.File.OpenRead(filepath);
                //string fileExt = ".bmd";
                //获取文件的ContentType
                var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
                //var memi = provider.Mappings[fileExt];
                var fileName = Path.GetFileName(filepath);

                HttpContext.Response.Headers.Add("fileName", fileName);

                return File(stream, "application/octet-stream", fileName);
            }
            else
            {
                return null;
            }
        }

        // POST: api/Img
        [HttpPost]
        public void Post([FromBody] object formdata)
        {
        }

        // PUT: api/Img/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
