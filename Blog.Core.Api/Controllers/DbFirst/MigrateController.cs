using Blog.Core.Common.Helper;
using Blog.Core.IServices;
using Blog.Core.Model;
using Blog.Core.Model.Models;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Repository.UnitOfWorks;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(Permissions.Name)]
    public class MigrateController : ControllerBase
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IRoleModulePermissionServices _roleModulePermissionServices;
        private readonly IUserRoleServices _userRoleServices;
        private readonly IRoleServices _roleServices;
        private readonly IPermissionServices _permissionServices;
        private readonly IModuleServices _moduleServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly IWebHostEnvironment _env;

        public MigrateController(IUnitOfWorkManage unitOfWorkManage,
            IRoleModulePermissionServices roleModulePermissionServices,
            IUserRoleServices userRoleServices,
            IRoleServices roleServices,
            IPermissionServices permissionServices,
            IModuleServices moduleServices,
            IDepartmentServices departmentServices,
            ISysUserInfoServices sysUserInfoServices,
            IWebHostEnvironment env)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _roleModulePermissionServices = roleModulePermissionServices;
            _userRoleServices = userRoleServices;
            _roleServices = roleServices;
            _permissionServices = permissionServices;
            _moduleServices = moduleServices;
            _departmentServices = departmentServices;
            _sysUserInfoServices = sysUserInfoServices;
            _env = env;
        }


        /// <summary>
        /// 获取权限部分Map数据（从库）
        /// 迁移到新库（主库）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> DataMigrateFromOld2New()
        {
            var data = new MessageModel<string>() { success = true, msg = "" };
            var filterPermissionId = 122;
            if (_env.IsDevelopment())
            {
                try
                {
                    var apiList = await _moduleServices.Query(d => d.IsDeleted == false);
                    var permissionsAllList = await _permissionServices.Query(d => d.IsDeleted == false);
                    var permissions = permissionsAllList.Where(d => d.Pid == 0).ToList();
                    var rmps = await _roleModulePermissionServices.GetRMPMaps();
                    List<PM> pms = new();

                    // 当然，你可以做个where查询
                    rmps = rmps.Where(d => d.PermissionId >= filterPermissionId).ToList();

                    InitPermissionTree(permissions, permissionsAllList, apiList);

                    var actionPermissionIds = permissionsAllList.Where(d => d.Id >= filterPermissionId).Select(d => d.Id).ToList();
                    List<long> filterPermissionIds = new();
                    FilterPermissionTree(permissionsAllList, actionPermissionIds, filterPermissionIds);
                    permissions = permissions.Where(d => filterPermissionIds.Contains(d.Id)).ToList();

                    // 开启事务，保证数据一致性
                    _unitOfWorkManage.BeginTran();

                    // 注意信息的完整性，不要重复添加，确保主库没有要添加的数据

                    // 1、保持菜单和接口
                    await SavePermissionTreeAsync(permissions, pms);

                    long rid = 0;
                    long pid = 0;
                    long mid = 0;
                    long rpmid = 0;

                    // 2、保存关系表
                    foreach (var item in rmps)
                    {
                        // 角色信息，防止重复添加，做了判断
                        if (item.Role != null)
                        {
                            var isExit = (await _roleServices.Query(d => d.Name == item.Role.Name && d.IsDeleted == false)).FirstOrDefault();
                            if (isExit == null)
                            {
                                rid = await _roleServices.Add(item.Role);
                                Console.WriteLine($"Role Added:{item.Role.Name}");
                            }
                            else
                            {
                                rid = isExit.Id;
                            }
                        }

                        pid = (pms.FirstOrDefault(d => d.PidOld == item.PermissionId)?.PidNew).ObjToLong();
                        mid = (pms.FirstOrDefault(d => d.MidOld == item.ModuleId)?.MidNew).ObjToLong();
                        // 关系
                        if (rid > 0 && pid > 0)
                        {
                            rpmid = await _roleModulePermissionServices.Add(new RoleModulePermission()
                            {
                                IsDeleted = false,
                                CreateTime = DateTime.Now,
                                ModifyTime = DateTime.Now,
                                ModuleId = mid,
                                PermissionId = pid,
                                RoleId = rid,
                            });
                            Console.WriteLine($"RMP Added:{rpmid}");
                        }

                    }


                    _unitOfWorkManage.CommitTran();

                    data.success = true;
                    data.msg = "导入成功！";
                }
                catch (Exception)
                {
                    _unitOfWorkManage.RollbackTran();

                }
            }
            else
            {
                data.success = false;
                data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return data;
        }


        /// <summary>
        /// 权限数据库导出tsv
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> SaveData2TsvAsync()
        {
            var data = new MessageModel<string>() { success = true, msg = "" };
            if (_env.IsDevelopment())
            {

                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };

                // 取出数据，序列化，自己可以处理判空
                var SysUserInfoJson = JsonConvert.SerializeObject(await _sysUserInfoServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "SysUserInfo.tsv"), SysUserInfoJson, Encoding.UTF8);

                var DepartmentJson = JsonConvert.SerializeObject(await _departmentServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "Department.tsv"), DepartmentJson, Encoding.UTF8);

                var rolesJson = JsonConvert.SerializeObject(await _roleServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "Role.tsv"), rolesJson, Encoding.UTF8);

                var UserRoleJson = JsonConvert.SerializeObject(await _userRoleServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "UserRole.tsv"), UserRoleJson, Encoding.UTF8);


                var permissionsJson = JsonConvert.SerializeObject(await _permissionServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "Permission.tsv"), permissionsJson, Encoding.UTF8);


                var modulesJson = JsonConvert.SerializeObject(await _moduleServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "Modules.tsv"), modulesJson, Encoding.UTF8);


                var rmpsJson = JsonConvert.SerializeObject(await _roleModulePermissionServices.Query(d => d.IsDeleted == false), microsoftDateFormatSettings);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.json", "RoleModulePermission.tsv"), rmpsJson, Encoding.UTF8);



                data.success = true;
                data.msg = "生成成功！";
            }
            else
            {
                data.success = false;
                data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return data;
        }


        /// <summary>
        /// 权限数据库导出excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<string>> SaveData2ExcelAsync()
        {
            var data = new MessageModel<string>() { success = true, msg = "" };
            if (_env.IsDevelopment())
            {

                JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };

                // 取出数据，序列化，自己可以处理判空
                IExporter exporter = new ExcelExporter();
                var SysUserInfoList = await _sysUserInfoServices.Query(d => d.IsDeleted == false);
                var result = await exporter.ExportAsByteArray(SysUserInfoList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "SysUserInfo.xlsx"), result);

                var DepartmentList = await _departmentServices.Query(d => d.IsDeleted == false);
                var DepartmentResult = await exporter.ExportAsByteArray(DepartmentList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "Department.xlsx"), DepartmentResult);

                var RoleList = await _roleServices.Query(d => d.IsDeleted == false);
                var RoleResult = await exporter.ExportAsByteArray(RoleList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "Role.xlsx"), RoleResult);

                var UserRoleList = await _userRoleServices.Query(d => d.IsDeleted == false);
                var UserRoleResult = await exporter.ExportAsByteArray(UserRoleList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "UserRole.xlsx"), UserRoleResult);

                var PermissionList = await _permissionServices.Query(d => d.IsDeleted == false);
                var PermissionResult = await exporter.ExportAsByteArray(PermissionList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "Permission.xlsx"), PermissionResult);

                var ModulesList = await _moduleServices.Query(d => d.IsDeleted == false);
                var ModulesResult = await exporter.ExportAsByteArray(ModulesList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "Modules.xlsx"), ModulesResult);

                var RoleModulePermissionList = await _roleModulePermissionServices.Query(d => d.IsDeleted == false);
                var RoleModulePermissionResult = await exporter.ExportAsByteArray(RoleModulePermissionList);
                FileHelper.WriteFile(Path.Combine(_env.WebRootPath, "BlogCore.Data.excel", "RoleModulePermission.xlsx"), RoleModulePermissionResult);


                data.success = true;
                data.msg = "生成成功！";
            }
            else
            {
                data.success = false;
                data.msg = "当前不处于开发模式，代码生成不可用！";
            }

            return data;
        }

        private void InitPermissionTree(List<Permission> permissionsTree, List<Permission> all, List<Modules> apis)
        {
            foreach (var item in permissionsTree)
            {
                item.children = all.Where(d => d.Pid == item.Id).ToList();
                item.Module = apis.FirstOrDefault(d => d.Id == item.Mid);
                InitPermissionTree(item.children, all, apis);
            }
        }

        private void FilterPermissionTree(List<Permission> permissionsAll, List<long> actionPermissionId, List<long> filterPermissionIds)
        {
            actionPermissionId = actionPermissionId.Distinct().ToList();
            var doneIds = permissionsAll.Where(d => actionPermissionId.Contains(d.Id) && d.Pid == 0).Select(d => d.Id).ToList();
            filterPermissionIds.AddRange(doneIds);

            var hasDoIds = permissionsAll.Where(d => actionPermissionId.Contains(d.Id) && d.Pid != 0).Select(d => d.Pid).ToList();
            if (hasDoIds.Any())
            {
                FilterPermissionTree(permissionsAll, hasDoIds, filterPermissionIds);
            }
        }

        private async Task SavePermissionTreeAsync(List<Permission> permissionsTree, List<PM> pms, long permissionId = 0)
        {
            var parendId = permissionId;

            foreach (var item in permissionsTree)
            {
                PM pm = new PM();
                // 保留原始主键id
                pm.PidOld = item.Id;
                pm.MidOld = (item.Module?.Id).ObjToLong();

                long mid = 0;
                // 接口
                if (item.Module != null)
                {
                    var moduleModel = (await _moduleServices.Query(d => d.LinkUrl == item.Module.LinkUrl)).FirstOrDefault();
                    if (moduleModel != null)
                    {
                        mid = moduleModel.Id;
                    }
                    else
                    {
                        mid = await _moduleServices.Add(item.Module);
                    }
                    pm.MidNew = mid;
                    Console.WriteLine($"Moudle Added:{item.Module.Name}");
                }
                // 菜单
                if (item != null)
                {
                    var permissionModel = (await _permissionServices.Query(d => d.Name == item.Name && d.Pid == item.Pid && d.Mid == item.Mid)).FirstOrDefault();
                    item.Pid = parendId;
                    item.Mid = mid;
                    if (permissionModel != null)
                    {
                        permissionId = permissionModel.Id;
                    }
                    else
                    {
                        permissionId = await _permissionServices.Add(item);
                    }

                    pm.PidNew = permissionId;
                    Console.WriteLine($"Permission Added:{item.Name}");
                }
                pms.Add(pm);

                await SavePermissionTreeAsync(item.children, pms, permissionId);
            }
        }


    }

    public class PM
    {
        public long PidOld { get; set; }
        public long MidOld { get; set; }
        public long PidNew { get; set; }
        public long MidNew { get; set; }
    }
}
