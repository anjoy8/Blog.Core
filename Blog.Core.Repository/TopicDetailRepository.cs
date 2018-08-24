using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.IRepository;
using Blog.Core.Model.Models;
using Blog.Core.Repository.Base;

namespace Blog.Core.Repository
{
    public class TopicDetailRepository: BaseRepository<TopicDetail>, ITopicDetailRepository
    {
    }
}
