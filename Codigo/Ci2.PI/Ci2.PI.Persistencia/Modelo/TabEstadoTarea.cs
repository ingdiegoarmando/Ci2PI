//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ci2.PI.Persistencia.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class TabEstadoTarea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TabEstadoTarea()
        {
            this.TabTarea = new HashSet<TabTarea>();
        }
    
        public long Ci2EstadoTareaId { get; set; }
        public string Ci2Nombre { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TabTarea> TabTarea { get; set; }
    }
}