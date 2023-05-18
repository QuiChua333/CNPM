using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.ViewModel.AdminVM.FurnitureManagementVM;
using HotelManagement.ViewModel.AdminVM.ServiceManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManagement.ViewModel.StaffVM
{
    public class StaffVM : BaseVM
    {
        public static StaffDTO CurrentStaff;

        private string _staffname;
        public string StaffName
        {
            get { return _staffname; }
            set { _staffname = value; OnPropertyChanged(); }
        }
        private ImageSource _staffimgsource;
        public ImageSource Staffimgsource
        {
            get { return _staffimgsource; }
            set { _staffimgsource = value; OnPropertyChanged(); }
        }
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        private void HelpScreen(object obj) => CurrentView = new HelpScreenVM.HelpScreenVM();
        private void BookingRoom(object obj) => CurrentView = new BookingRoomManagementVM.BookingRoomManagementVM();
        private void RoomCatalog(object obj) => CurrentView = new RoomCatalogManagementVM.RoomCatalogManagementVM();
        private void TroubleRp(object obj) => CurrentView = new TroubleReportVM.TroubleReportVM();
        public ICommand BookingRoomCommand { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand RoomCatalogCommand { get; set; }
        public ICommand TroubleRpCommand { get; set; }
        public ICommand HelpScreenCommand { get; set; }

        public StaffVM()
        {
            _currentView = new RoomCatalogManagementVM.RoomCatalogManagementVM();
            RoomCatalogCommand = new RelayCommand(RoomCatalog);
            TroubleRpCommand = new RelayCommand(TroubleRp);
            BookingRoomCommand = new RelayCommand(BookingRoom);
            HelpScreenCommand = new RelayCommand(HelpScreen);
        }
    }
}
