//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TDF.Demo.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SystemActionRole : SystemEntity
    {
        public System.Guid ActionId { get; set; }
        public System.Guid RoleId { get; set; }
    
        public virtual SystemRole SystemRole { get; set; }
        public virtual SystemAction SystemAction { get; set; }
    }
}
