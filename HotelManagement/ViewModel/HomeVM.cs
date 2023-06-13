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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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
        public static HomeVM homeVM;
        private void Statistical(object obj)
        {
            CurrentView = new StatisticalManagementVM.StatisticalManagementVM();
            HomeWindow.lbPageName.Content = "BÁO CÁO DOANH THU THÁNG";
        }
        private void RoomLoookup(object obj)
        {
             CurrentView = new RoomLookupManagementVM.RoomLookupManagementVM();
            HomeWindow.lbPageName.Content= "TRA CỨU PHÒNG";
        }
        private void BookingRoom(object obj)
        {
            CurrentView = new BookingRoomManagementVM.BookingRoomManagementVM();
            HomeWindow.lbPageName.Content = "QUẢN LÝ THUÊ PHÒNG";
        }
        private void Room(object obj)
        {
            CurrentView = new RoomManagementVM.RoomPageVM();
            HomeWindow.lbPageName.Content = "QUẢN LÝ PHÒNG - LOẠI PHÒNG";
        }
        private void History(object obj)
        {
            CurrentView = new HistoryManagementVM.HistoryManagementVM();
            HomeWindow.lbPageName.Content = "LỊCH SỬ THANH TOÁN";
        }
        private void Help(object obj)
        {
            CurrentView = new HelpScreenVM.HelpScreenVM();
            HomeWindow.lbPageName.Content = "MÀN HÌNH GIÚP ĐỠ";

        }
        private void Setting(object obj)
        {
            CurrentView = new SettingManagementVM.SettingManagementVM();
            HomeWindow.lbPageName.Content = "MÀN HÌNH CÀI ĐẶT";

        }
        private void CustomerType(object obj)
        {
            CurrentView = new CustomerTypeManagementVM.CustomerTypeManagementVM();
            HomeWindow.lbPageName.Content = "QUẢN LÝ LOẠI KHÁCH";
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand RoomLookupCommand { get; set; }
        public ICommand BookingRoomCommand { get; set; }
        public ICommand RoomCommand { get; set; }
        public ICommand StatisticalCommand { get; set; }
        public ICommand HelpScreenCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand CustomerTypeCommand { get; set; }
        public HomeVM()
        {
            homeVM = this;
            FirstLoadCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                p.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
            });
            _currentView = new StatisticalManagementVM.StatisticalManagementVM();
            StatisticalCommand = new RelayCommand(Statistical);
            BookingRoomCommand = new RelayCommand(BookingRoom);
            RoomLookupCommand = new RelayCommand(RoomLoookup);
            HistoryCommand = new RelayCommand(History);
            RoomCommand = new RelayCommand(Room);
            HelpScreenCommand = new RelayCommand(Help);
            SettingCommand = new RelayCommand(Setting);
            CustomerTypeCommand = new RelayCommand(CustomerType);

        }
        public void setNavigateHelpScreen()
        {
            CurrentView = new HelpScreenVM.HelpScreenVM();
        }
    }

}
