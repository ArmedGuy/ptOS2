//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ptOS.Core
{
    using System;
    using System.Collections.Generic;
    
    public partial class Statistic
    {
        public long Id { get; set; }
        public Nullable<int> ServerId { get; set; }
        public Nullable<long> PlayerId { get; set; }
        public string AltId { get; set; }
        public string Type { get; set; }
        public Nullable<System.DateTime> At { get; set; }
        public decimal Value { get; set; }
    
        public virtual Player Player { get; set; }
        public virtual Server Server { get; set; }
    }
}
