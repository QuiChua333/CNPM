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
    
    public partial class RevenueReportDetail
    {
        public string RevenueReportId { get; set; }
        public string RoomTypeId { get; set; }
        public Nullable<double> Revenue { get; set; }
        public Nullable<double> Ratio { get; set; }
    
        public virtual RoomType RoomType { get; set; }
    }
}