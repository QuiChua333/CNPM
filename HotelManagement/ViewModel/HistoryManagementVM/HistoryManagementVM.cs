using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomLookupManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagement.ViewModel.HistoryManagementVM
{
    public class HistoryManagementVM : BaseVM
    {
        private bool isExport;
        public bool IsExport
        {
            get { return isExport; }
            set { isExport = value; OnPropertyChanged(); }
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
        private List<BillDTO> _ListBill;
        public List<BillDTO> ListBill
        {
            get { return _ListBill; }
            set { _ListBill = value; OnPropertyChanged(); }
        }
        private BillDTO _SelectedItem;
        public BillDTO SelectedItem
        {
            get { return _SelectedItem; }
            set { _SelectedItem = value; OnPropertyChanged(); }
        }
        public ICommand ChangeTimeCM { get; set; }
        public ICommand LoadInfoBillCM { get; set; }
        public ICommand DeleteBillCM { get; set; }
        public HistoryManagementVM()
        {
            ListFilterYear = new List<string>(HistoryService.Ins.GetListFilterYear());
            SelectedYear = ListFilterYear[0];
            ListFilterMonth = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                ListFilterMonth.Add("Tháng " + i.ToString());
            }
            ListFilterMonth.Insert(0, "Tất cả");
            SelectedMonth = "Tất cả";
            ChangeTimeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeView();
            });
            LoadInfoBillCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                BillWindow wd = new BillWindow();
                wd.tbCustomerName.Text = SelectedItem.CustomerName;
                wd.tbAddress.Text = SelectedItem.Address;
                wd.tbCreateDate.Text = ((DateTime)SelectedItem.CreateDate).ToString("dd/MM/yyyy");
                wd.tbTotalPrice.Text = Helper.FormatVNMoney((double)SelectedItem.TotalPrice);
                wd.listRoomBill.ItemsSource = SelectedItem.Bills.ToList();
                wd.ShowDialog();    

            });
            DeleteBillCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel("Bạn có chắc muốn xóa hóa đơn này chứ? Dữ liệu sẽ không thể phục hồi!", "Thông báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);
                if (kq == CustomMessageBoxResult.Cancel) return;
                (bool isSucceed, string message) = await HistoryService.Ins.DeleteBill(SelectedItem);
                if (isSucceed)
                {
                    CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Success);
                    ListBill = await HistoryService.Ins.GetListBill(SelectedYear, SelectedMonth);
                }
                else
                {
                    CustomMessageBox.ShowOk(message, "Thông báo", "Ok", CustomMessageBoxImage.Error);
                }
            });
        }

        private async Task ChangeView()
        {
            ListBill = await HistoryService.Ins.GetListBill(SelectedYear, SelectedMonth);
        }
    }
}
