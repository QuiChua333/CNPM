//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelManagement.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class GoodsStorage
    {
        public string ServiceId { get; set; }
        public Nullable<int> QuantityService { get; set; }
    
        public virtual Service Service { get; set; }
    }
}
