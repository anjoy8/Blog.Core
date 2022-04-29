using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.ViewModels
{
    public  class UploadFileDto
    {
        //多文件
        [Required]
        public IFormFileCollection Files { get; set; }

        //单文件
        //public IFormFile File { get; set; }

        //其他数据
        public string Foo { get; set; }

        
    }
}
