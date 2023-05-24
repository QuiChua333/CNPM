using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class BillDetailDTO
    {
        public int BillDetailId { get; set; }
        public string BillId { get; set; }
        public string RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int NumberOfRentalDays { get; set; }
        public double PricePerDay { get; set; }
        public double Price { get; set; }
        public string RoomName
        {
            get
            {
                return "Phòng "+RoomNumber.ToString();
            }
        }
        public string PricePerDayStr
        {
            get
            {
                return Helper.FormatVNMoney(PricePerDay);
            }
        }
        public string PriceStr
        {
            get
            {
                return Helper.FormatVNMoney(Price);
            }
        }
    }
}
