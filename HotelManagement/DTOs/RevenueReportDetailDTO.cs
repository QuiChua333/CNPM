using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RevenueReportDetailDTO
    {
        public int STT { get; set; }
        public int RevenueReportDetailId { get; set; }
        public string RevenueReportId { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public double Revenue { get; set; }
        public double Ratio { get; set; }
        public string RatioStr
        {
            get
            {
                double t = Math.Round(Ratio*100, 2, MidpointRounding.AwayFromZero);
                return t.ToString("N2") + " %";
            }
        }
        public string RevenueStr
        {
            get
            {
                return Helper.FormatVNMoney(Revenue);
            }
        }
    }
}
