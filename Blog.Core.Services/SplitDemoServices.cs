using Blog.Core.IRepository.Base;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.FrameWork.Services
{
    /// <summary>
    /// sysUserInfoServices
    /// </summary>	
    public class SplitDemoServices : BaseServices<SplitDemo>, ISplitDemoServices
    {
        private readonly IBaseRepository<SplitDemo> _splitDemoRepository;
        public SplitDemoServices(IBaseRepository<SplitDemo> splitDemoRepository)
        {
            _splitDemoRepository = splitDemoRepository;
        }


    }
}
