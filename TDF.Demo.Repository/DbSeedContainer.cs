using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDF.Core.Tools;
using TDF.Data.EntityFramework;
using TDF.Demo.Domain.Entities;

namespace TDF.Demo.Repository
{
    public class DbSeedContainer : ISeedContainer
    {
        public DbSeedContainer()
        {

        }
        public void Seed(System.Data.Entity.DbContext context)
        {
            //todo 初始化平台数据
            #region 系统用户

            context.Set<SystemMember>().AddOrUpdate(new SystemMember()
            {
                Account = "Admin",
                CreatedTime = DateTime.Parse("2017/04/21"),
                Email = "Test@qq.com",
                Id = Guid.Empty,
                MobilePhone = "13222222222",
                RealName = "超级管理员",
                IsSuperAdmin = true,
                CreatorId = new Guid("00000000-0000-0000-0000-000000000000"),
                SystemMemberLogon = new SystemMemberLogon()
                {
                    Id = Guid.Empty,
                    Password = "abcd123",
                    SessionKey = string.Empty,
                    Secretkey = string.Empty
                }
            });
            context.Set<SystemMember>().AddOrUpdate(new SystemMember()
            {
                Account = "feijie",
                CreatedTime = DateTime.Parse("2017/04/21"),
                Email = "Test@qq.com",
                Id = Guid.Parse("aa1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                MobilePhone = "13222222222",
                RealName = "Dev",
                IsSuperAdmin = false,
                CreatorId = new Guid("00000000-0000-0000-0000-000000000000"),
                SystemMemberLogon = new SystemMemberLogon()
                {
                    Id = Guid.Parse("aa1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                    Password = "123456",
                    SessionKey = string.Empty,
                    Secretkey = string.Empty
                }
            });

            #endregion

            #region 系统菜单

            context.Set<SystemModule>().AddOrUpdate(x => x.Name, new SystemModule()
            {
                Name = "TCL大学堂系统",
                Id = Guid.Parse("ee1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                ParentId = Guid.Empty,
                Disabled = false,
                Sort = 0,
                Code = Common.BuildKey(),
                CreatedTime = DateTime.Now,
                CreatorName = string.Empty,
                Desc = string.Empty,
                IconClass = string.Empty,
                ModifierName = string.Empty,
                Displayed = true
            });
            context.Set<SystemModule>().AddOrUpdate(x => x.Name, new SystemModule()
            {
                Name = "权限管理",
                Id = Guid.Parse("e11337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                ParentId = Guid.Parse("ee1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                Disabled = false,
                Sort = 1,
                Code = Common.BuildKey(),
                CreatedTime = DateTime.Now,
                CreatorName = string.Empty,
                Desc = string.Empty,
                IconClass = string.Empty,
                ModifierName = string.Empty,
                Displayed = true
            });
            context.Set<SystemModule>().AddOrUpdate(x => x.Name, new SystemModule()
            {
                Name = "用户管理",
                Id = Guid.Parse("e21337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                ParentId = Guid.Parse("ee1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                Disabled = false,
                Sort = 1,
                Code = Common.BuildKey(),
                CreatedTime = DateTime.Now,
                CreatorName = string.Empty,
                Desc = string.Empty,
                IconClass = string.Empty,
                ModifierName = string.Empty,
                Displayed = true
            });
            context.Set<SystemModule>().AddOrUpdate(x => x.Name, new SystemModule()
            {
                Name = "数据字典",
                Id = Guid.Parse("ef1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                ParentId = Guid.Parse("ee1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                Disabled = false,
                Sort = 1,
                Code = Common.BuildKey(),
                CreatedTime = DateTime.Now,
                CreatorName = string.Empty,
                Desc = string.Empty,
                IconClass = string.Empty,
                ModifierName = string.Empty,
                Displayed = true
            });

            #endregion

            #region 系统资源
            context.Set<SystemAction>().AddOrUpdate(x => x.Name, new SystemAction()
            {
                Name = "系统资源列表",
                Id = Guid.Parse("be1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                Disabled = false,
                Sort = 0,
                Code = Common.BuildKey(),
                CreatedTime = DateTime.Now,
                CreatorName = string.Empty,
                Desc = string.Empty,
                ModifierName = string.Empty,
                Url = "/System/Menu/Resources",
                Displayed = true,
                ModuleId = Guid.Parse("e11337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                ActionParentId = Guid.Empty
            });
            context.Set<SystemAction>().AddOrUpdate(x => x.Name, new SystemAction()
            {
                Name = "系统菜单列表",
                Id = Guid.Parse("ae1337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                Disabled = false,
                Sort = 0,
                Code = Common.BuildKey(),
                CreatedTime = DateTime.Now,
                CreatorName = string.Empty,
                Desc = string.Empty,
                ModifierName = string.Empty,
                Url = "/system/menu/",
                Displayed = true,
                ModuleId = Guid.Parse("e11337ee-20be-4d3c-91f6-e6ff3ac4029f"),
                ActionParentId = Guid.Empty
            });
            #endregion
        }
    }
}
