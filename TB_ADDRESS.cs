//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjektSemestralny
{
    using System;
    using System.Collections.Generic;
    
    public partial class TB_ADDRESS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_ADDRESS()
        {
            this.TB_CLIENT = new HashSet<TB_CLIENT>();
        }
    
        public int ID_ADDRESS { get; set; }
        public string STREET_NUMBER { get; set; }
        public string ZIP_CODE { get; set; }
        public string CITY { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_CLIENT> TB_CLIENT { get; set; }
    }
}
