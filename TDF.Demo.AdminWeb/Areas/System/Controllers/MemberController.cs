using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDF.Core.Exceptions;
using TDF.Core.Ioc;
using TDF.Demo.AdminWeb.Areas.System.Models.Members;
using TDF.Demo.AdminWeb.Attributes;
using TDF.Demo.AdminWeb.Controllers;
using TDF.Demo.AdminWeb.Models;
using TDF.Demo.Service.Dtos.SystemManage;
using TDF.Demo.Service.SystemManage;
using TDF.Web.Attributes.Mvc;
using TDF.Web.Authentication.Services;

namespace TDF.Demo.AdminWeb.Areas.System.Controllers
{
    public class MemberController : AdminControllerBase
    {
        // GET: System/Member  用户列表
        public override ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(Guid memberId)
        {
            var member = Ioc.Resolve<ISystemMemberService>().GetMemberById(memberId);
            return View(member);
        }

        /// <summary>
        /// 添加系统用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public ActionResult Add(
            [Bind(Include = "Id,Account,MobilePhone,Email,Gender,RealName,Password,ComfirmPassword,EnabledMark")] SaveMemberModel
                model)
        {
            Ioc.Resolve<ISystemMemberService>().AddMember(model);
            return Success();
        }

        /// <summary>
        /// 保存修改成员信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelValidation]
        public ActionResult Edit(
            [Bind(Include = "Id,Account,MobilePhone,Email,Gender,RealName,Password,ComfirmPassword,EnabledMark")] SaveMemberModel
                model)
        {
            Ioc.Resolve<ISystemMemberService>().UpdateMember(model);
            return Success();
        }


        [HttpPost]
        [ModelValidation]
        [TdfHandlerAuthorize(true)]
        public ActionResult UpdatePassWord(
        [Bind(Include ="OldPassword,Password,ComfirmPassword")] PassWordModel
        model)
        {
            model.Id = CurrentOperator.Id;
            Ioc.Resolve<ISystemMemberService>().UpdatePassWord(model);
            return Success();
        }

        /// <summary>
        /// 获得成员分页数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [TdfHandlerAuthorize(true)]
        public ActionResult GetPagedList(MemberCriteria criteria)
        {
            var pagedList = Ioc.Resolve<ISystemMemberService>().GetMemberPagedList(criteria);
            return pagedList.ToJqueryDataTableModel();
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public ActionResult Enable(Guid memberId, bool enabled)
        {
            //CheckOperation(memberId);
            Ioc.Resolve<ISystemMemberService>().EnableMember(memberId, enabled);
            return Success();
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMember(Guid memberId)
        {
            CheckOperation(memberId);
            Ioc.Resolve<ISystemMemberService>().DeleteMember(memberId);
            return Success();
        }

        /// <summary>
        /// 用户角色授权界面加载数据
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Assignment(Guid memberId)
        {
            var service = Ioc.Resolve<ISystemMemberService>();

            var model = new AssignmentModel();
            //获取当前被操作授权的用户
            var user = service.GetMemberById(memberId);
            var allRoles = service.GetAllRoles(true);
            var inRoles = service.GetRolesByMemberId(memberId);
            model.InRoles = inRoles;
            model.NotInRoles = allRoles.Where(x => !inRoles.Select(y => y.Id).Contains(x.Id)).ToList();
            model.MemberId = memberId;
            model.UserName = user.Account;
            return Success(model);
        }

        /// <summary>
        /// 提交用户角色授权
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveAssignment(Guid memberId, List<Guid> roleIds)
        {
            Ioc.Resolve<IRoleAuthorizeService>().Assignment(memberId, roleIds);
            return Success();
        }

        /// <summary>
        /// 检查当前用户是否可以操作目标用户
        /// </summary>
        /// <param name="targerMemberId">目标用户Id</param>
        private void CheckOperation(Guid targerMemberId)
        {
            if (CurrentOperator.IsSystem)
            {
                return;
            }
            if (targerMemberId == CurrentOperator.Id)
            {
                throw new KnownException("不能对自己的账号进行操作");
            }
        }
    }
}