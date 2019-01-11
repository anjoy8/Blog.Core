using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgController : Controller
    {
        // GET: api/Img
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Img/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

      
        [HttpPost]
        [Route("Pic")]
        public async Task<bool> InsertPicture([FromServices]IHostingEnvironment environment)
        {
            var data = new PicData();
            string path = string.Empty;
            var files = Request.Form.Files;
            if (files == null || files.Count() <= 0) { data.Msg = "请选择上传的文件。"; return false; }
            //格式限制
            var allowType = new string[] { "image/jpg", "image/png", "image/jpeg" };
            if (files.Any(c => allowType.Contains(c.ContentType)))
            {
                if (files.Sum(c => c.Length) <= 1024 * 1024 * 4)
                {
                    foreach (var file in files)
                    {
                        string strpath = Path.Combine("Upload", DateTime.Now.ToString("MMddHHmmss") + file.FileName);
                        path = Path.Combine(environment.WebRootPath, strpath);

                        using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    data.Msg = "上传成功";
                    return true;
                }
                else
                {
                    data.Msg = "图片过大";
                    return false;
                }
            }
            else

            {
                data.Msg = "图片格式错误";
                return false;
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
    public class PicData
    {
        public string Msg { get; set; }
    }
}
