using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;

namespace Blog.Core.IServices
{
    public interface IAdvertisementServices :IBaseServices<Advertisement>
    {
        //int Sum(int i, int j);
        //int Add(Advertisement model);
        //bool Delete(Advertisement model);
        //bool Update(Advertisement model);
        //List<Advertisement> Query(Expression<Func<Advertisement, bool>> whereExpression);

        void ReturnExp();
    }
}
