using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TDF.Demo.Service.Dtos.SystemManage;

namespace TDF.Demo.AdminWeb.Areas.System.Models
{
    public class MenuJsTree
    {
        public Guid id { get; set; }

        public Guid parentId { get; set; }

        public string text { get; set; }

        public JsTreeState state { get; set; }

        public int sort { get; set; }

        public bool displayed { get; set; }

        public bool disabled { get; set; }

        public string url { get; set; }

        public string name { get; set; }

        public Guid moduleId { get; set; }

        public bool isAction { get; set; }

        public List<MenuJsTree> children { get; set; }

        public string iconClass { get; set; }

        public string icon
        {
            get
            {
                if (disabled)
                {
                    return "fa fa-warning icon-state-danger";
                }
                if (!displayed)
                {
                    return "fa fa-folder icon-state-default";
                }
                return "";
            }
        }
    }

    public class JsTreeState
    {
        public bool opened { get; set; }

        public bool disabled { get; set; }

        public bool selected { get; set; }
    }

    public static class MenuJsTreeExtensions
    {
        public static MenuJsTree ToJsTree(this SystemModuleDto module)
        {
            return new MenuJsTree()
            {
                id = module.Id ?? Guid.Empty,
                disabled = module.Disabled,
                displayed = module.Displayed,
                parentId = module.ParentId,
                sort = module.Sort,
                iconClass = module.IconClass,
                state = new JsTreeState()
                {
                    opened = true
                },
                text = module.Name,
                children = module.SystemActionDtos.Select(x => new MenuJsTree()
                {
                    moduleId = x.ModuleId,
                    children = null,
                    id = x.Id ?? Guid.Empty,
                    state = new JsTreeState()
                    {
                        opened = true
                    },
                    displayed = x.Displayed,
                    text = x.Name,
                    parentId = x.ModuleId,
                    disabled = x.Disabled,
                    sort = x.Sort,
                    isAction = true,
                    name = x.Name,
                    url = x.Url
                }).OrderBy(x => x.sort).ToList()
            };
        }

        public static List<MenuJsTree> ToMenuJsTrees(this List<SystemModuleDto> modules)
        {
            var allTree = modules.Where(x => x.ParentId != Guid.Empty)
                .Select(x => x.ToJsTree())
                .OrderBy(x => x.sort)
                .ToList();
            var tree = modules.Where(x => x.ParentId == Guid.Empty).Select(x => x.ToJsTree()).OrderBy(x => x.sort).ToList();
            foreach (var item in tree)
            {
                LoadTree(item, allTree);
            }
            return tree;
        }

        public static void LoadTree(MenuJsTree parentTree, List<MenuJsTree> trees)
        {
            var children = trees.Where(x => x.parentId == parentTree.id).OrderBy(x => x.sort).ToList();
            if (children.Count == 0)
            {
                return;
            }
            parentTree.children = children;
            foreach (var item in parentTree.children)
            {
                LoadTree(item, trees);
            }
        }
    }
}