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
    
    public partial class TB_CLIENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TB_CLIENT()
        {
            this.TB_TRANSACTIONS = new HashSet<TB_TRANSACTIONS>();
        }
    
        public int ID_CLIENT { get; set; }
        public string NAME { get; set; }
        public string SURNAME { get; set; }
        public string PESEL { get; set; }
        public Nullable<int> NIP { get; set; }
        public int ID_CLIENT_ADDRESS { get; set; }
    
        public virtual TB_ADDRESS TB_ADDRESS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TB_TRANSACTIONS> TB_TRANSACTIONS { get; set; }
    }
}
