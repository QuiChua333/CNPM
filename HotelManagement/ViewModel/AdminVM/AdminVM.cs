using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.View.Admin.StatisticalManagement;
using HotelManagement.ViewModel.StaffVM.RoomCatalogManagementVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace HotelManagement.ViewModel.AdminVM
{
    public class AdminVM : BaseVM
    {

        public static StaffDTO CurrentStaff;

        private string _staffname;
        public string StaffName
        {
            get { return _staffname; }
            set { _staffname = value; OnPropertyChanged(); }
        }

        private string _StaffId;
        public string StaffId
        {
            get { return _StaffId; }
            set { _StaffId = value; OnPropertyChanged(); }
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
        public ICommand FirstLoadCM { get; set; }
        private void Furniture(object obj) => CurrentView = new FurnitureManagementVM.FurnitureManagementVM();
        private void Service(object obj) => CurrentView = new ServiceManagementVM.ServiceManagementVM();
        private void RoomFurniture(object obj) => CurrentView = new RoomFurnitureManagementVM.RoomFurnitureManagementVM();
        private void Setting(object obj) => CurrentView = new SettingVM.SettingVM();
        private void BookingRoom(object obj) => CurrentView = new BookingRoomManagementVM.BookingRoomManagementVM();
        private void Room(object obj) => CurrentView = new RoomManagementVM.RoomManagementVM();
        private void RoomType(object obj) => CurrentView = new RoomTypeManagementVM.RoomTypeManagementVM();
        private void Statiscal(object obj) => CurrentView = new StatisticalManagementVM.StatisticalManagementVM();
        private void HelpScreen(object obj) => CurrentView = new HelpScreenVM.HelpScreenVM();
        private void Customer(object obj) => CurrentView = new CustomerManagementVM.CustomerManagementVM();
        private void Staff(object obj) => CurrentView = new StaffManagementVM.StaffManagementVM();
        private void History(object obj) => CurrentView = new HistoryManagementVM.HistoryManagementVM();
        private void Trouble(object obj) => CurrentView = new TroubleManagementVM.TroubleManagementVM();
        private void RoomCatalog(object obj) => CurrentView = new RoomCatalogManagementVM();

        public ICommand FurnitureCommand { get; set; }
        public ICommand ServiceCommand { get; set; }
        public ICommand RoomFurnitureCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand BookingRoomCommand { get; set; }
        public ICommand RoomCommand { get; set; }
        public ICommand RoomTypeCommand { get; set; }
        public ICommand StatiscalCommand { get; set; }
        public ICommand HelpScreenCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand StaffCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand TroubleCommand { get; set; }
        public ICommand RoomCatalogCommand { get; set; }
        public AdminVM()
        {
            _currentView = new RoomFurnitureManagementVM.RoomFurnitureManagementVM();

            FurnitureCommand = new RelayCommand(Furniture);
            ServiceCommand = new RelayCommand(Service);
            RoomFurnitureCommand = new RelayCommand(RoomFurniture);
            SettingCommand = new RelayCommand(Setting);
            BookingRoomCommand = new RelayCommand(BookingRoom);
            RoomCommand = new RelayCommand(Room);
            RoomTypeCommand = new RelayCommand(RoomType);
            StatiscalCommand=new RelayCommand(Statiscal);
            HelpScreenCommand = new RelayCommand(HelpScreen);
            CustomerCommand=new RelayCommand(Customer);
            StaffCommand=new RelayCommand(Staff);
            HistoryCommand=new RelayCommand(History);
            TroubleCommand=new RelayCommand(Trouble);
            RoomCatalogCommand = new RelayCommand(RoomCatalog);
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                StaffName = CurrentStaff.StaffName;
                StaffId = CurrentStaff.StaffId;
            });
        }
    }
}
