using HotelManagement.DTOs;
using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.RoomLookupManagementVM
{
    public partial class RoomLookupManagementVM: BaseVM
    {
        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; OnPropertyChanged(); }
        }
        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; OnPropertyChanged(); }
        }
        private DateTime _CreateDate;
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; OnPropertyChanged(); }
        }
        public string CreateDateStr
        {
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        private double _TotalPrice;
        public double TotalPrice
        {
            get { return _TotalPrice; }
            set { _TotalPrice = value; OnPropertyChanged(); }
        }
        private string _TotalPriceStr;
        public string TotalPriceStr
        {
            get { return _TotalPriceStr; }
            set { _TotalPriceStr = value; OnPropertyChanged(); }

        }
        private List<BillDetailDTO> _ListRoomBill;
        public List<BillDetailDTO> ListRoomBill
        {
            get { return _ListRoomBill; }
            set { _ListRoomBill = value; OnPropertyChanged(); }
        }

    }
}
