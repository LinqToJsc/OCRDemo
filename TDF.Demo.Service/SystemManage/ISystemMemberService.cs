using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Models;
using TDF.Core.Operator;
using TDF.Demo.Service.Dtos.SystemManage;

namespace TDF.Demo.Service.SystemManage
{
    public interface ISystemMemberService
    {
        OperatorModel Login(string userName, string password);

        void LoginOut();

        /// <summary>
        /// 获取权限分页列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IPagedList<SystemRoleDto> GetRolePagedList(RoleCriteria criteria);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        void AddRole(SystemRoleDto role);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        void UpdateRole(SystemRoleDto role);

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        SystemRoleDto GetRoleById(Guid roleId);

        /// <summary>
        /// 设置角色是否启用
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="enabled"></param>
        void EnabledRole(Guid roleId, bool enabled);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId"></param>
        void DeleteRole(Guid roleId);

        /// <summary>
        /// 添加系统用户
        /// </summary>
        /// <param name="systemMemberDto"></param>
        void AddMember(SystemMemberDto systemMemberDto);

        /// <summary>
        /// 获取系统用户分页列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IPagedList<SystemMemberDto> GetMemberPagedList(MemberCriteria criteria);

        /// <summary>
        /// 设置系统用户是否启用
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="enabled"></param>
        void EnableMember(Guid memberId, bool enabled);

        /// <summary>
        /// 获得系统用户信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        SystemMemberDto GetMemberById(Guid memberId);

        /// <summary>
        /// 更新系统用户信息
        /// </summary>
        /// <param name="model"></param>
        void UpdateMember(SystemMemberDto model);

        /// <summary>
        /// 修改系统用户密码
        /// </summary>
        /// <param name="model"></param>
        void UpdatePassWord(PassWordDto model);

        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="isLogic">是否是逻辑删除</param>
        void DeleteMember(Guid memberId, bool isLogic = true);

        /// <summary>
        /// 根据系统用户Id获得此用户的所有角色列表
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        List<SystemRoleDto> GetRolesByMemberId(Guid memberId);

        /// <summary>
        /// 获得所有的角色列表
        /// </summary>
        /// <param name="enabled">是否启用</param>
        /// <returns></returns>
        List<SystemRoleDto> GetAllRoles(bool? enabled = null);
    }
}
