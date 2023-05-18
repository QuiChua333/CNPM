using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.RoomLookupManagementVM
{
    public class RoomLookupManagementVM : BaseVM
    {
        private ObservableCollection<string> _RampTurnsBasedOnList;
        public ObservableCollection<string> RampTurnsBasedOnList
        {
            get { return _RampTurnsBasedOnList; }
            set { _RampTurnsBasedOnList = value; OnPropertyChanged(); }
        }

        public RoomLookupManagementVM()
        {
            RampTurnsBasedOnList = new ObservableCollection<string>();
            RampTurnsBasedOnList.Add("Qui");
            RampTurnsBasedOnList.Add("Tinh");
            RampTurnsBasedOnList.Add("Tinh");
            RampTurnsBasedOnList.Add("Tinh");
            RampTurnsBasedOnList.Add("Tinh");
            RampTurnsBasedOnList.Add("Tinh");
            RampTurnsBasedOnList.Add("Qui");
            RampTurnsBasedOnList.Add("Qui");
            RampTurnsBasedOnList.Add("Qui");
            RampTurnsBasedOnList.Add("Qui");
            RampTurnsBasedOnList.Add("Qui");
            RampTurnsBasedOnList.Add("Tinh");
        }
    }
}
