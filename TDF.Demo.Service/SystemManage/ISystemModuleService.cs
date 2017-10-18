using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;
using TDF.Demo.Domain.Entities.Partial.SystemManage.Enums;
using TDF.Demo.Service.Dtos.SystemManage;

namespace TDF.Demo.Service.SystemManage
{
    public interface ISystemModuleService
    {
        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="module"></param>
        void AddMoudule(SystemModuleDto module);

        /// <summary>
        /// 获取模块分页列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IPagedList<SystemModuleDto> GetModulePagedList(ModuleCriteria criteria);

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SystemModuleDto GetModuleById(Guid id);

        /// <summary>
        /// 更新模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        void UpdateModule(SystemModuleDto module);

        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="moduleId"></param>
        void DeleteModuleById(Guid moduleId);

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        List<SystemModuleDto> GetModuleList();

        /// <summary>
        /// 获取菜单列表且包含Action信息
        /// </summary>
        /// <param name="operId">当前登录系统的用户(非超级管理员)</param>
        /// <param name="roleIds">当前用户所具有的角色权限</param>
        /// <returns></returns>
        List<SystemModuleDto> GetModuleIncludeActionList(Guid? operId = null, List<Guid> roleIds = null);

        /// <summary>
        /// 启动菜单
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="enabled"></param>
        void EnableModule(Guid moduleId, bool enabled);

        /// <summary>
        /// 获得资源列表
        /// </summary>
        /// <returns></returns>
        List<SystemActionDto> GetActionList();

        List<SystemActionDto> GetActionByActionType(SystemActionType actionType);

        /// <summary>
        /// 获得资源列表
        /// </summary>
        /// <returns></returns>
        List<SystemActionDto> GetActionTableList();

        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="action"></param>
        void AddAction(SystemActionDto action);

        /// <summary>
        /// 更新资源
        /// </summary>
        /// <param name="action"></param>
        void UpdateAction(SystemActionDto action);

        /// <summary>
        /// 删除资源信息
        /// </summary>
        /// <param name="id"></param>
        void DeleteActionById(Guid id);

        /// <summary>
        /// 获得资源列表信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<SystemActionDto> GetActionListByRoleId(Guid roleId);

        /// <summary>
        /// 给角色赋权
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="moduleIds">菜单id集合</param>
        /// <param name="actionIds">资源id集合</param>
        void EmpowermentToRole(Guid roleId, List<Guid> moduleIds, List<Guid> actionIds);
    }
}
