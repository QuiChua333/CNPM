using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomLookupManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace HotelManagement.ViewModel.RoomLookupManagementVM
{
    public partial class RoomLookupManagementVM : BaseVM
    {
        private ObservableCollection<string> _ListRoomType;
        public ObservableCollection<string> ListRoomType
        {
            get { return _ListRoomType; }
            set { _ListRoomType = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ListRoomTypeDTO> _ListListRoomType;
        public ObservableCollection<ListRoomTypeDTO> ListListRoomType
        {
            get { return _ListListRoomType; }
            set { _ListListRoomType = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ListRoomTypeDTO> _ListListRoomTypeMini;
        public ObservableCollection<ListRoomTypeDTO> ListListRoomTypeMini
        {
            get { return _ListListRoomTypeMini; }
            set { _ListListRoomTypeMini = value; OnPropertyChanged(); }
        }
        private RadioButton _radioButtonRoomStatus;
        public RadioButton RadioButtonRoomStatus
        {
            get { return _radioButtonRoomStatus; }
            set { _radioButtonRoomStatus = value; OnPropertyChanged(); }
        }
        private RadioButton _radioButtonRoomType;
        public RadioButton RadioButtonRoomType
        {
            get { return _radioButtonRoomType; }
            set { _radioButtonRoomType = value; OnPropertyChanged(); }
        }
        private List<string> ListStoreRoomNumber;
        private DateTime _CurrentDateTime;
        public DateTime CurrentDateTime
        {
            get { return _CurrentDateTime; }
            set { _CurrentDateTime = value; OnPropertyChanged(); }
        }

        MaterialDesignThemes.Wpf.Transitions.TransitioningContent ButtonThanhToan;
        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadRoomInfoCM { get; set; }
        public ICommand ChangeRoomType1CM { get; set; }
        public ICommand ChangeRoomType2CM { get; set; }
        public ICommand CheckboxCM { get; set; }
        public ICommand PaymentCM { get; set; }
        public ICommand SaveBillCM { get; set; }
        public ICommand OpenBillCM { get; set; }

        public RoomLookupManagementVM()
        {
            Color color = new Color();
            CurrentDateTime= DateTime.Now;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {

                await PageSetting(p);

            });
            LoadRoomInfoCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {

                Border border = p.Children.OfType<Border>().FirstOrDefault();
                Label lbRoomStatus = (Label)p.FindName("lbRoomStatus");
                CheckBox checkBox = (CheckBox)p.FindName("checkBoxRoom");
                MaterialDesignThemes.Wpf.PackIcon packIcon = (MaterialDesignThemes.Wpf.PackIcon)p.FindName("icon");

                if (lbRoomStatus != null)
                {
                    if (lbRoomStatus.Content.ToString() == ROOM_STATUS.READY)
                    {
                        color = (Color)ColorConverter.ConvertFromString("#59D66D");
                        border.Background = new SolidColorBrush(color);
                        packIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Check;


                    }
                    else
                    {
                        color = (Color)ColorConverter.ConvertFromString("#72B6DC");
                        border.Background = new SolidColorBrush(color);
                        packIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Account;

                        
                    }

                }
            });
            ChangeRoomType1CM = new RelayCommand<RadioButton>((p) => { return true; }, async (p) =>
            {
                RadioButtonRoomType = p;
                ListListRoomType = new ObservableCollection<ListRoomTypeDTO>(ListListRoomTypeMini);
                ChangeView();

            });
            ChangeRoomType2CM = new RelayCommand<RadioButton>((p) => { return true; }, async (p) =>
            {

                RadioButtonRoomStatus = p;
                ListListRoomType = new ObservableCollection<ListRoomTypeDTO>(ListListRoomTypeMini);
                ChangeView();

            });
            CheckboxCM = new RelayCommand<Grid>((p) => { return true; }, async (p) =>
            {

                CheckBox checkBox = (CheckBox)p.FindName("checkBoxRoom");
                checkBox.IsChecked = !checkBox.IsChecked;
                Label lbRoomStatus = (Label)p.FindName("lbRoomStatus");
                Label labelRoomName = (Label)p.FindName("labelRoomName");
                string number = labelRoomName.Content.ToString().Substring(1);
                TextBlock tbStartDate = (TextBlock)p.FindName("tbStartDate");
                if (lbRoomStatus.Content.ToString() == ROOM_STATUS.RENTING)
                {
                   
                    if (checkBox.IsChecked == true)
                    {
                        
                        if (!ListStoreRoomNumber.Contains(number)) ListStoreRoomNumber.Add(number);
             

                        tbStartDate.Visibility = Visibility.Collapsed;
                        checkBox.Visibility = System.Windows.Visibility.Visible;
                        ButtonThanhToan.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        tbStartDate.Visibility = Visibility.Visible;
                        checkBox.Visibility = System.Windows.Visibility.Collapsed;
                        if (ListStoreRoomNumber.Contains(number)) ListStoreRoomNumber.Remove(number);
                        if (ListStoreRoomNumber.Count==0) ButtonThanhToan.Visibility = Visibility.Hidden;
                     
                        

                    }
               

                }
                
            });
            PaymentCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                PaymentWindow wd = new PaymentWindow();
                ListRoomBill = new List<BillDetailDTO>(await BillService.Ins.GetListRoomBill(ListStoreRoomNumber));
                TotalPrice = 0;
                foreach (var item in ListRoomBill) { TotalPrice += item.Price; }
                TotalPriceStr = Helper.FormatVNMoney(TotalPrice);
                wd.address.Text = "";
                wd.customerName.Text = "";
                wd.ShowDialog();

            });
            SaveBillCM = new RelayCommand<PaymentWindow>((p) => { return true; }, async (p) =>
            {
                if (string.IsNullOrEmpty(p.customerName.Text.ToString()) || string.IsNullOrEmpty(p.address.Text.ToString()))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Thông báo", "Ok", CustomMessageBoxImage.Warning);
                    return;

                }
                CustomerName=CustomerName.Trim();
                Address=Address.Trim();
                BillDTO billDTO = new BillDTO
                {
                    CustomerName = CustomerName,
                    TotalPrice= TotalPrice,
                    Address= Address,
              
                };

                (bool isSucceed, string message) = await BillService.Ins.SaveBill(billDTO, ListRoomBill);
                if (isSucceed)
                {
                    CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                    p.btnConfirm.Visibility = Visibility.Collapsed;
                    p.showBill.Visibility= Visibility.Visible;
                    ListStoreRoomNumber.Clear();
                    await ReloadRoom();

                }
                else
                {
                    CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Error);

                }

            });
            OpenBillCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                BillWindow t = new BillWindow();
                t.Show();

            });
        }

        private async Task PageSetting(Page p)
        {
             await ReloadRoom();
            ListListRoomType = new ObservableCollection<ListRoomTypeDTO>(await RoomService.Ins.GetListListRoomType());
            ListListRoomTypeMini = new ObservableCollection<ListRoomTypeDTO>(ListListRoomType);
            ListRoomType = new  ObservableCollection<string>(await RoomTypeService.Ins.GetListRoomTypName());
            ButtonThanhToan = (MaterialDesignThemes.Wpf.Transitions.TransitioningContent)p.FindName("btnThanhToan");
            ListRoomType.Add("Tất cả các phòng");
            ListStoreRoomNumber = new List<string>();
            
           
        }

        private  async Task  ReloadRoom()
        {
            ListListRoomType = new ObservableCollection<ListRoomTypeDTO>(await RoomService.Ins.GetListListRoomType());

            ListListRoomTypeMini = new ObservableCollection<ListRoomTypeDTO>(ListListRoomType);
            ChangeView();

        }
        private void ChangeView()
        {
            (string roomType, string roomStatus) = GetContentRadioButton(RadioButtonRoomType, RadioButtonRoomStatus);
         
            if (roomType!="Tất cả các phòng")
            {
                ListListRoomType = new ObservableCollection<ListRoomTypeDTO>(ListListRoomType.Where(x => x.RoomTypeName == roomType).ToList());
            }
            if (roomStatus!="Tất cả các phòng")
            {
                ListListRoomType = new ObservableCollection<ListRoomTypeDTO>(ListListRoomType.Select(x=> new ListRoomTypeDTO
                {
                    RoomTypeId= x.RoomTypeId,
                    RoomTypeName= x.RoomTypeName,
                    Price= x.Price,
                    Rooms=x.Rooms.Where(t=> t.RoomStatus==roomStatus).ToList(),
                }).ToList());
            }

        }
        private Tuple<string, string> GetContentRadioButton(RadioButton radioButton1, RadioButton radioButton2)
        {
            string res1, res2;
            if (radioButton1 == null) res1 = "Tất cả các phòng";
            else
            {
                TextBlock t = (TextBlock)radioButton1.FindName("tbRoomType");
                res1 = t.Text.ToString();
            }
            if (radioButton2 == null) res2 = "Tất cả các phòng";
            else
            {
                switch (radioButton2.Name.ToString())
                {
                    case "rdbRoomStatusReady":
                        res2 = ROOM_STATUS.READY;
                        break;
                    case "rdbRoomStatusRenting":
                        res2 = ROOM_STATUS.RENTING;
                        break;
                    default:
                        res2 = "Tất cả các phòng";
                        break;
                }
            }
              
            
           
            var res = Tuple.Create(res1, res2);
            return res;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            CurrentDateTime = DateTime.Now;
        }
    }
}
