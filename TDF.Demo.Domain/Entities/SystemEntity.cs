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
    
    public abstract partial class SystemEntity : EntityBase
    {
        public Nullable<System.Guid> CreatorId { get; set; }
        public string CreatorName { get; set; }
        public Nullable<System.Guid> ModifierId { get; set; }
        public string ModifierName { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        public bool Deleted { get; set; }
        public Nullable<System.DateTime> DeletedTime { get; set; }
    }
}
