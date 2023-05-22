using HotelManagement.DTOs;
using HotelManagement.Model.Services;
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
        public ICommand ChangeTimeCM { get; set; }
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
        }

        private async Task ChangeView()
        {
            ListBill = await HistoryService.Ins.GetListBill(SelectedYear, SelectedMonth);
        }
    }
}
