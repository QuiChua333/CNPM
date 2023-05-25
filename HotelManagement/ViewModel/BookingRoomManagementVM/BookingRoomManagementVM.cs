﻿using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.BookingRoomManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace HotelManagement.ViewModel.BookingRoomManagementVM
{
    public partial class BookingRoomManagementVM : BaseVM
    {
        private ObservableCollection<RoomDTO> _ListReadyRoom;
        public ObservableCollection<RoomDTO> ListReadyRoom
        {
            get => _ListReadyRoom;
            set
            {
                _ListReadyRoom = value;
                OnPropertyChanged();
            }
        }

        private RoomDTO _SelectedRoom;
        public RoomDTO SelectedRoom
        {
            get => _SelectedRoom;
            set
            {
                _SelectedRoom = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<RentalContractDTO> _RentalContractList;
        public ObservableCollection<RentalContractDTO> RentalContractList
        {
            get => _RentalContractList;
            set
            {
                _RentalContractList = value;
                OnPropertyChanged();
            }
        }

        private RentalContractDTO _SelectedItem;
        public RentalContractDTO SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
            }
        }
        private string _RoomId;
        public string RoomId
        {
            get { return _RoomId; }
            set { _RoomId = value; OnPropertyChanged(); }
        }

        private int _roomNumber;
        public int RoomNumber
        {
            get { return _roomNumber; }
            set { _roomNumber = value; OnPropertyChanged(); }
        }

        private DateTime _CreateDate;
        public DateTime CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; OnPropertyChanged(); }
        }

        private string _RentalContractDetailId;
        public string RentalContractDetailId
        {
            get { return _RentalContractDetailId; }
            set { _RentalContractDetailId = value; OnPropertyChanged(); }
        }
        private string _RentalContractId;
        public string RentalContractId
        {
            get { return _RentalContractId; }
            set { _RentalContractId = value; OnPropertyChanged(); }
        }
        private ObservableCollection<RentalContractDetailDTO> _ListCustomer;
        public ObservableCollection<RentalContractDetailDTO> ListCustomer
        {
            get => _ListCustomer;
            set
            {
                _ListCustomer = value;
                OnPropertyChanged();
            }
        }

        private RentalContractDetailDTO _SelectedCustomer;
        public RentalContractDetailDTO SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged();
            }
        }
        private bool IsSave = false;
        private string _CustomerType;
        public string CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; OnPropertyChanged(); }
        }

        private string _AddressCustomer;
        public string AddressCustomer
        {
            get { return _AddressCustomer; }
            set { _AddressCustomer = value; OnPropertyChanged(); }
        }

        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; OnPropertyChanged(); }
        }
        private int _STT;
        public int STT
        {
            get { return _STT; }
            set { _STT = value; OnPropertyChanged(); }
        }
        
        private string _CCCD;
        public string CCCD
        {
            get { return _CCCD; }
            set { _CCCD = value; OnPropertyChanged(); }
        }
        public string cccd_Last = "";

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }
        private DateTime _RentalDay;
        public DateTime RentalDay
        {
            get { return _RentalDay; }
            set { _RentalDay = value; OnPropertyChanged(); }
        }


        private ObservableCollection<string> _listCustomerType;
        public ObservableCollection<string> ListCustomerType
        {
            get => _listCustomerType;
            set
            {
                _listCustomerType = value;
                OnPropertyChanged();
            }
        }
        private List<string> _ListFilterYear;
        public List<string> ListFilterYear
        {
            get { return _ListFilterYear; }
            set { _ListFilterYear = value; OnPropertyChanged(); }
        }
        private string _SelectedYear;
        public string SelectedYear
        {
            get { return _SelectedYear; }
            set { _SelectedYear = value; OnPropertyChanged(); }
        }
        private List<string> _ListFilterMonth;
        public List<string> ListFilterMonth
        {
            get { return _ListFilterMonth; }
            set { _ListFilterMonth = value; OnPropertyChanged(); }
        }
        private string _SelectedMonth;
        public string SelectedMonth
        {
            get { return _SelectedMonth; }
            set { _SelectedMonth = value; OnPropertyChanged(); }
        }
        public ICommand CloseCM { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadBookingCM { get; set; }
        public ICommand LoadFormInfoCustomerCM { get; set; }
        public ICommand ConfirmCustomerCM { get; set; }
        public ICommand LoadEditCustomerCM { get; set; }
        public ICommand LoadDeleteCustomerCM { get; set; }
        public ICommand ConfirmEditCustomerCM { get; set; }
        public ICommand LoadRentalContractInfoCM { get; set; }
        public ICommand ConfirmSaveRentalContract { get; set; }
        public ICommand LoadDeleteRentalContractCM { get; set; }
        public ICommand ChangeTimeCM { get; set; }

        public BookingRoomManagementVM() 
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            ListFilterYear = new List<string>(BookingRoomService.Ins.GetListFilterYear());
            SelectedYear = ListFilterYear[0];
            ListFilterMonth = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                ListFilterMonth.Add("Tháng " + i.ToString());
            }
            ListFilterMonth.Insert(0, "Tất cả");
            SelectedMonth = "Tất cả";

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                STT = 0;
                SelectedRoom = null;
                await LoadReadyRoom();
                RentalContractList = new ObservableCollection<RentalContractDTO>(await BookingRoomService.Ins.GetRentalContractList());
            });
            ChangeTimeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeView();
            });
            LoadBookingCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                ListCustomer = new ObservableCollection<RentalContractDetailDTO>();
                Booking booking = new Booking();
                booking.ShowDialog();
            });
            LoadFormInfoCustomerCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                if (SelectedRoom == null)
                {
                    CustomMessageBox.ShowOk("Vui lòng chọn phòng trước!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
                else
                {
                    int maxPer = BookingRoomService.Ins.GetMaxNumOfPer();
                    if (ListCustomer.Count == maxPer)
                    {
                        CustomMessageBox.ShowOk($"Mỗi phòng chỉ được ở tối đa {maxPer} khách!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                  
                    try
                    {
                        ListCustomerType = new ObservableCollection<string>((await CustomerTypeService.Ins.GetAllCustomerType()).Select(x => x.CustomerTypeName));
                    }
                    catch (System.Data.Entity.Core.EntityException e)
                    {
                        Console.WriteLine(e);
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }

                    RenewWindowDataCusTomer();
                    EnterInfoCustomer w = new EnterInfoCustomer();
                    w.cbbCusType.SelectedIndex= 0;
                    w.ShowDialog();
                    
                
                }
            });
            ConfirmCustomerCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, (p) =>
            {
                IsSaving = true;
                SaveCustomerFunc(p);
                IsSaving = false;
            });
            ConfirmEditCustomerCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                IsSave = true;
                EditCustomerFunc(p);
                SelectedCustomer = null;
            });
            LoadDeleteCustomerCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                string message = "Bạn có chắc muốn xoá khách hàng này không?";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                    ListCustomer.Remove(SelectedCustomer);
            });
            LoadEditCustomerCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, async (p) =>
            {
                try
                {
                    ListCustomerType = new ObservableCollection<string>((await CustomerTypeService.Ins.GetAllCustomerType()).Select(x => x.CustomerTypeName));
                }
                catch (System.Data.Entity.Core.EntityException e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }

                cccd_Last = SelectedCustomer.CCCD.ToString();
                editEnterInfoCustomer w = new editEnterInfoCustomer();
                CustomerName = SelectedCustomer.CustomerName;
                CCCD = SelectedCustomer.CCCD;
                AddressCustomer = SelectedCustomer.Address;
                w.cbbCusType.Text = SelectedCustomer.CustomerType.ToString();
                w.ShowDialog();
            });
            LoadDeleteRentalContractCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                string message = "Bạn có chắc muốn xoá phiếu thuê này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo","Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    (bool successDelete, string messageFromDelete) = await BookingRoomService.Ins.DeleteRentalContract(SelectedItem.RentalContractId);

                    if (successDelete)
                    {
                        RentalContractList = new ObservableCollection<RentalContractDTO>(await BookingRoomService.Ins.GetRentalContractList());
                        SelectedItem = null;
                        CustomMessageBox.ShowOk(messageFromDelete,"Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDelete,"Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });
            LoadRentalContractInfoCM = new RelayCommand<RentalContractInfo>((p) => { return true; }, async (p) =>
            {
                RentalDay = DateTime.Today;
                RentalContractInfo w = new RentalContractInfo();
                await LoadInforRentalContract(w);
                w.ShowDialog();
            });
            ConfirmSaveRentalContract = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                await SaveRentalContract(p);
            });

            CloseCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
        public async Task  LoadInforRentalContract(RentalContractInfo w)
        {
            w.roomNumberInfo.Text = SelectedItem.RoomNumber.ToString();
            w.createDateInfo.Text = ((DateTime)SelectedItem.CreateDate).ToString("dd/MM/yyyy");
            double  RentalContractPrice = await BookingRoomService.Ins.GetRentalContractPrice(SelectedItem.RentalContractId);
            w.priceInfo.Text = Helper.FormatVNMoney2(RentalContractPrice);
            w.lsvCustomerInfo.ItemsSource = SelectedItem.RentalContracts;
        }
        private async Task ChangeView()
        {
            RentalContractList = new ObservableCollection<RentalContractDTO>(await BookingRoomService.Ins.GetRentalContractListFilter(SelectedYear, SelectedMonth));
        }
        public void RenewWindowDataCusTomer()
        {
            CustomerName = null;
            CCCD = null;
            AddressCustomer = null;
            
        }
        public bool IsValidDataCustomer()
        {
            return (!string.IsNullOrEmpty(CustomerName) &&
                !string.IsNullOrEmpty(CCCD) &&
                !string.IsNullOrEmpty(AddressCustomer) &&
                !string.IsNullOrEmpty(CustomerType));
        }
    }
}
