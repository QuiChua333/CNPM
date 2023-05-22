using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.ViewModel.StatisticalManagementVM
{
    public class StatisticalManagementVM : BaseVM
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
        private RevenueReportDTO _RevenueReport;
        public RevenueReportDTO RevenueReport
        {
            get { return _RevenueReport; }
            set { _RevenueReport = value; OnPropertyChanged(); }
        }

        private SeriesCollection _RoomTypeRevenuePieChart;
        public SeriesCollection RoomTypeRevenuePieChart
        {
            get { return _RoomTypeRevenuePieChart; }
            set { _RoomTypeRevenuePieChart = value; OnPropertyChanged(); }
        }
        public ICommand FirstLoadCM { get; set; }
        public ICommand ChangeTimeCM { get; set; }
        public StatisticalManagementVM()
        {
            ListFilterYear = new List<string>( StatisticalService.Ins.GetListFilterYear());
            SelectedYear = ListFilterYear[0];
            ListFilterMonth = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                ListFilterMonth.Add("Tháng " + i.ToString());
            }
            SelectedMonth = "Tháng " + (DateTime.Now.Month.ToString());
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
               
            });
            ChangeTimeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeView();
            });
        }

       
        
        private async Task ChangeView()
        {
            RevenueReport =  StatisticalService.Ins.GetRevenueReport(SelectedYear, SelectedMonth);
            if (RevenueReport == null)
            {

                RevenueReport = new RevenueReportDTO
                {
                    TotalRevenue= 0,
                    revenueReportDetailDTOs = new List<RevenueReportDetailDTO>()
                };
                var listRoomType =await RoomTypeService.Ins.GetListRoomTypName();
                int index = 1;
                foreach (var roomType in listRoomType)
                {
                    RevenueReport.revenueReportDetailDTOs.Add(new RevenueReportDetailDTO
                    {
                        STT = index++,
                        RoomTypeName = roomType,
                        Ratio = 0,
                        Revenue = 0,
                    }) ;
                }
            }
            SeriesCollection listRoomChart = new SeriesCollection();
            foreach (var item in RevenueReport.revenueReportDetailDTOs)
            {
                PieSeries p = new PieSeries
                {
                    Values = new ChartValues<double> { item.Revenue },
                    Title = item.RoomTypeName,
                };
                listRoomChart.Add(p);
            }
            RoomTypeRevenuePieChart = listRoomChart;
        }
    }
}
