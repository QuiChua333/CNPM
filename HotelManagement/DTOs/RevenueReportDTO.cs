using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RevenueReportDTO
    {
        public string RevenueReportId { get; set; }
        public DateTime MonthReport { get; set; }
        public IList<RevenueReportDetailDTO> revenueReportDetailDTOs { get; set; }
        public double TotalRevenue { get; set; }
       
        public string TotalRevenueStr
        {
            get
            {
               
                return Helper.FormatVNMoney2(TotalRevenue);
            }
        }
    }
}
