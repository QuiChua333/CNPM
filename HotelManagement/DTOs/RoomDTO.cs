using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class RoomDTO
    {
        public string RoomId { get; set; }
        public string RentalContractId { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public string RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public string RoomStatus { get; set; }
        public string Note { get; set; }

        public string RoomName
        {
            get { return "P" + RoomNumber.ToString(); }
        }
        public Nullable<DateTime> StartDate { get; set; }
        public string StartDateStr
        {
            get
            {
                if (StartDate == null)
                {
                    return "";
                }
                return ((DateTime)StartDate).ToString("dd/MM/yyyy");
            }
        }
        public int DayNumber
        {
            get
            {
                if (StartDate == null)
                {
                    return 0;
                }

                TimeSpan t = (TimeSpan)(DateTime.Now - StartDate);
                int res = (int)t.TotalDays + 1;
                return res;
            }
        }
        public int NumberOfPerson { get; set; }
        public double Price { get; set; }
        public string RoomPriceStr
        {
            get { return Helper.FormatVNMoney(Price); }
        }
        public string RoomReadyName
        {
            get { return "PH" + RoomNumber.ToString(); }
        }
    }
}
