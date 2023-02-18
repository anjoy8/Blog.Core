using System.Threading.Tasks;
using Blog.Core.IServices.BASE;
using Blog.Core.Model.Models;

namespace Blog.Core.IServices;

public interface ITenantService : IBaseServices<SysTenant>
{
    public Task SaveTenant(SysTenant tenant);

    public Task InitTenantDb(SysTenant tenant);
}