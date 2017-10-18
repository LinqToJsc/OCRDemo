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
    
    public partial class SystemModule : SystemEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemModule()
        {
            this.SystemModuleRoles = new HashSet<SystemModuleRole>();
            this.SystemActions = new HashSet<SystemAction>();
        }
    
        public string Code { get; set; }
        public string Desc { get; set; }
        public string IconClass { get; set; }
        public bool Displayed { get; set; }
        public string Name { get; set; }
        public System.Guid ParentId { get; set; }
        public int Sort { get; set; }
        public bool Disabled { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemModuleRole> SystemModuleRoles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemAction> SystemActions { get; set; }
    }
}