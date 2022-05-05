using AutoMapper;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;

namespace Blog.Core.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<BlogArticle, BlogViewModels>();
            CreateMap<BlogViewModels, BlogArticle>();

            CreateMap<SysUserInfo, SysUserInfoDto>()
                .ForMember(a => a.uID, o => o.MapFrom(d => d.Id))
                .ForMember(a => a.RIDs, o => o.MapFrom(d => d.RIDs))
                .ForMember(a => a.addr, o => o.MapFrom(d => d.Address))
                .ForMember(a => a.age, o => o.MapFrom(d => d.Age))
                .ForMember(a => a.birth, o => o.MapFrom(d => d.Birth))
                .ForMember(a => a.uStatus, o => o.MapFrom(d => d.Status))
                .ForMember(a => a.uUpdateTime, o => o.MapFrom(d => d.UpdateTime))
                .ForMember(a => a.uCreateTime, o => o.MapFrom(d => d.CreateTime))
                .ForMember(a => a.uErrorCount, o => o.MapFrom(d => d.ErrorCount))
                .ForMember(a => a.uLastErrTime, o => o.MapFrom(d => d.LastErrorTime))
                .ForMember(a => a.uLoginName, o => o.MapFrom(d => d.LoginName))
                .ForMember(a => a.uLoginPWD, o => o.MapFrom(d => d.LoginPWD))
                .ForMember(a => a.uRemark, o => o.MapFrom(d => d.Remark))
                .ForMember(a => a.uRealName, o => o.MapFrom(d => d.RealName))
                .ForMember(a => a.name, o => o.MapFrom(d => d.Name))
                .ForMember(a => a.tdIsDelete, o => o.MapFrom(d => d.IsDeleted))
                .ForMember(a => a.RoleNames, o => o.MapFrom(d => d.RoleNames));
            CreateMap<SysUserInfoDto, SysUserInfo>()
                .ForMember(a => a.Id, o => o.MapFrom(d => d.uID))
                .ForMember(a => a.Address, o => o.MapFrom(d => d.addr))
                .ForMember(a => a.RIDs, o => o.MapFrom(d => d.RIDs))
                .ForMember(a => a.Age, o => o.MapFrom(d => d.age))
                .ForMember(a => a.Birth, o => o.MapFrom(d => d.birth))
                .ForMember(a => a.Status, o => o.MapFrom(d => d.uStatus))
                .ForMember(a => a.UpdateTime, o => o.MapFrom(d => d.uUpdateTime))
                .ForMember(a => a.CreateTime, o => o.MapFrom(d => d.uCreateTime))
                .ForMember(a => a.ErrorCount, o => o.MapFrom(d => d.uErrorCount))
                .ForMember(a => a.LastErrorTime, o => o.MapFrom(d => d.uLastErrTime))
                .ForMember(a => a.LoginName, o => o.MapFrom(d => d.uLoginName))
                .ForMember(a => a.LoginPWD, o => o.MapFrom(d => d.uLoginPWD))
                .ForMember(a => a.Remark, o => o.MapFrom(d => d.uRemark))
                .ForMember(a => a.RealName, o => o.MapFrom(d => d.uRealName))
                .ForMember(a => a.Name, o => o.MapFrom(d => d.name))
                .ForMember(a => a.IsDeleted, o => o.MapFrom(d => d.tdIsDelete))
                .ForMember(a => a.RoleNames, o => o.MapFrom(d => d.RoleNames));
        }
    }
}
