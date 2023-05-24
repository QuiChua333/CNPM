using CinemaManagementProject.Utilities;
using HotelManagement.Properties;
using HotelManagement.View;
using HotelManagement.ViewModel.BookingRoomManagementVM;
using HotelManagement.ViewModel.RoomManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManagement.ViewModel
{
    public class HomeVM : BaseVM
    {

        
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }
        private void Statistical(object obj)
        {
            CurrentView = new StatisticalManagementVM.StatisticalManagementVM();
            HomeWindow.lbPageName.Content = "THỐNG KÊ";
        }
        private void RoomLoookup(object obj)
        {
             CurrentView = new RoomLookupManagementVM.RoomLookupManagementVM();
            HomeWindow.lbPageName.Content= "TRA CỨU PHÒNG";
        }
        private void BookingRoom(object obj) => CurrentView = new BookingRoomManagementVM.BookingRoomManagmentVM();
        private void Room(object obj) => CurrentView = new RoomManagementVM.RoomManagementVM();
        private void History(object obj)
        {
            CurrentView = new HistoryManagementVM.HistoryManagementVM();
            HomeWindow.lbPageName.Content = "LỊCH SỬ THANH TOÁN";
        }
        private void Help(object obj) => CurrentView = new HelpScreenVM.HelpScreenVM();
        private void Setting(object obj) => CurrentView = new SettingManagementVM.SettingManagementVM();


        public ICommand RoomLookupCommand { get; set; }
        public ICommand BookingRoomCommand { get; set; }
        public ICommand RoomCommand { get; set; }
        public ICommand StatisticalCommand { get; set; }
        public ICommand HelpScreenCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public HomeVM()
        {
            _currentView = new StatisticalManagementVM.StatisticalManagementVM();
            StatisticalCommand = new RelayCommand(Statistical);
            BookingRoomCommand = new RelayCommand(BookingRoom);
            RoomLookupCommand = new RelayCommand(RoomLoookup);
            HistoryCommand = new RelayCommand(History);
            RoomCommand = new RelayCommand(Room);
            HelpScreenCommand = new RelayCommand(Help);
            SettingCommand = new RelayCommand(Setting);

        }
    }

}
