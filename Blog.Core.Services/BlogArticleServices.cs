using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Core.Common;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Model.VeiwModels;
using Blog.Core.Services.BASE;

namespace Blog.Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository dal;
        IMapper IMapper;
        public BlogArticleServices(IBlogArticleRepository dal, IMapper IMapper)
        {
            this.dal = dal;
            base.baseDal = dal;
            this.IMapper = IMapper;
        }
        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogViewModels> getBlogDetails(int id)
        {
            var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);
            var blogArticle = (await dal.Query(a => a.bID == id)).FirstOrDefault();

            BlogViewModels models = null;

            if (blogArticle != null)
            {
                BlogArticle prevblog;
                BlogArticle nextblog;


                int blogIndex = bloglist.FindIndex(item => item.bID == id);
                if (blogIndex >= 0)
                {
                    try
                    {
                        prevblog = blogIndex > 0 ? (((BlogArticle)(bloglist[blogIndex - 1]))) : null;
                        nextblog = (BlogArticle)(bloglist[blogIndex + 1]);


                        // 注意就是这里,mapper
                        models = IMapper.Map<BlogViewModels>(blogArticle);

                        if (nextblog != null)
                        {
                            models.next = nextblog.btitle;
                            models.nextID = nextblog.bID;
                        }

                        if (prevblog != null)
                        {
                            models.previous = prevblog.btitle;
                            models.previousID = prevblog.bID;
                        }

                    }
                    catch (Exception) { }
                }


                blogArticle.btraffic += 1;
                await dal.Update(blogArticle, new List<string> { "btraffic" });
            }

            return models;

        }


        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<BlogArticle>> getBlogs()
        {
            var bloglist = await dal.Query(a => a.bID > 0, a => a.bID);

            return bloglist;

        }
    }
}
