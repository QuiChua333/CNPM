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
    
    public partial class RentalContractDetail
    {
        public string RentalContractDetailId { get; set; }
        public string RentalContractId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTypeId { get; set; }
        public string CCCD { get; set; }
        public string Address { get; set; }
    
        public virtual CustomerType CustomerType { get; set; }
        public virtual RentalContract RentalContract { get; set; }
    }
}
