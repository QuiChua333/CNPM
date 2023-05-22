using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RoomTypeDTO
    {
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public Nullable<double> Price { get; set; }
        public string PriceStr
        {
            get { return Helper.FormatVNMoney((double)Price); }
        }
       
    }
}
