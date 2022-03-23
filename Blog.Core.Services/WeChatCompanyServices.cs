using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Blog.Core.IRepository.Base;
using Blog.Core.IRepository.UnitOfWork;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;
using Blog.Core.Services.BASE;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Core.Services
{
    /// <summary>
	/// WeChatCompanyServices
	/// </summary>
    public class WeChatCompanyServices : BaseServices<WeChatCompany>, IWeChatCompanyServices
    {
        readonly IBaseRepository<WeChatCompany> _dal;
        readonly IUnitOfWork _unitOfWork;
        readonly ILogger<WeChatCompanyServices> _logger;
        public WeChatCompanyServices(IBaseRepository<WeChatCompany> dal,IUnitOfWork unitOfWork, ILogger<WeChatCompanyServices> logger)
        {
            this._dal = dal;
            base.BaseDal = dal;
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }  
        
    }
}