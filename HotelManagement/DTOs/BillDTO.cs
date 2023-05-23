using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class BillDTO
    {
        public string BillId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }

        public IList<BillDetailDTO> Bills { get;set; }
        public string TotalPriceStr
        {
            get
            {
                return Helper.FormatVNMoney((double)TotalPrice);
            }
        }
       

    }
}
