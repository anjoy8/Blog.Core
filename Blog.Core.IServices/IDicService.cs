using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;


namespace Blog.Core.IServices
{
    public interface IDicService : IBaseServices<DicType>
    {
        public Task<DicType> GetDic(string code);
        public Task<List<DicData>> GetDicData(string code);
    }
}
