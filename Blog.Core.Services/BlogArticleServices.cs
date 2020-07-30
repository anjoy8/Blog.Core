using AutoMapper;
using Blog.Core.Common;
using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Blog.Core.Services.BASE;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBaseRepository<BlogArticle> _dal;
        IMapper _mapper;
        public BlogArticleServices(IBaseRepository<BlogArticle> dal, IMapper mapper)
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
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            //var bloglist = await base.Query(a => a.IsDeleted==false, a => a.bID);
            var blogArticle = (await base.Query(a => a.bID == id && a.bcategory == "技术博文")).FirstOrDefault();

            BlogViewModels models = null;

            if (blogArticle != null)
            {
                models = _mapper.Map<BlogViewModels>(blogArticle);

                //要取下一篇和上一篇，以当前id开始，按id排序后top(2)，而不用取出所有记录
                //这样在记录很多的时候也不会有多大影响
                var nextBlogs = await base.Query(a => a.bID >= id && a.IsDeleted == false && a.bcategory == "技术博文", 2, "bID");
                if (nextBlogs.Count == 2)
                {
                    models.next = nextBlogs[1].btitle;
                    models.nextID = nextBlogs[1].bID;
                }
                var prevBlogs = await base.Query(a => a.bID <= id && a.IsDeleted == false && a.bcategory == "技术博文", 2, "bID desc");
                if (prevBlogs.Count == 2)
                {
                    models.previous = prevBlogs[1].btitle;
                    models.previousID = prevBlogs[1].bID;
                }

                //BlogArticle prevblog;
                //BlogArticle nextblog;


                //int blogIndex = bloglist.FindIndex(item => item.bID == id);
                //if (blogIndex >= 0)
                //{
                //    try
                //    {
                //        prevblog = blogIndex > 0 ? bloglist[blogIndex - 1] : null;
                //        nextblog = blogIndex + 1 < bloglist.Count() ? bloglist[blogIndex + 1] : null;


                //        // 注意就是这里,mapper
                //        models = _mapper.Map<BlogViewModels>(blogArticle);

                //        if (nextblog != null)
                //        {
                //            models.next = nextblog.btitle;
                //            models.nextID = nextblog.bID;
                //        }

                //        if (prevblog != null)
                //        {
                //            models.previous = prevblog.btitle;
                //            models.previousID = prevblog.bID;
                //        }
                //        var entity2Viewmodel = _mapper.Map<BlogArticle>(models);

                //    }
                //    catch (Exception ex) { throw new Exception(ex.Message); }
                //}


                blogArticle.btraffic += 1;
                await base.Update(blogArticle, new List<string> { "btraffic" });
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
