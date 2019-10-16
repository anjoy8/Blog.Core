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
using Blog.Core.Model.ViewModels;
using Blog.Core.Services.BASE;

namespace Blog.Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository _dal;
        IMapper _mapper;
        public BlogArticleServices(IBlogArticleRepository dal, IMapper mapper)
        {
            this._dal = dal;
            base.BaseDal = dal;
            this._mapper = mapper;
        }
        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogViewModels> GetBlogDetails(int id)
        {
            var bloglist = await base.Query(a => a.bID > 0, a => a.bID);
            var blogArticle = (await base.Query(a => a.bID == id)).FirstOrDefault();

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
                        nextblog = blogIndex + 1 < bloglist.Count() ? (BlogArticle)(bloglist[blogIndex + 1]) : null;


                        // 注意就是这里,mapper
                        models = _mapper.Map<BlogViewModels>(blogArticle);

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
                        var entity2Viewmodel = _mapper.Map<BlogArticle>(models);

                    }
                    catch (Exception ex) { throw new Exception(ex.Message); }
                }


                blogArticle.btraffic += 1;
                //await base.Update(blogArticle, new List<string> { "btraffic" });
            }

            return models;

        }


        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<BlogArticle>> GetBlogs()
        {
            var bloglist = await base.Query(a => a.bID > 0, a => a.bID);

            return bloglist;

        }
    }
}
