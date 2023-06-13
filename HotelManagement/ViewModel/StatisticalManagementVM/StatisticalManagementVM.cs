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
using System.Windows.Forms;
using System.Windows.Input;

namespace HotelManagement.ViewModel.StatisticalManagementVM
{
    public class StatisticalManagementVM : BaseVM
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
        public ICommand ExportFileExcelCM { get; set; }
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
            ExportFileExcelCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                IsExport = false;
                await ExportFile();
                if (IsExport)
                {
                    CustomMessageBox.ShowOk("Xuất file thành công!", "Thông báo", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    IsExport = false;
                }
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
        public async Task ExportFile()
        {
            using (SaveFileDialog box = new SaveFileDialog() { Filter = "Excel | *.xlsx | Excel 2003 | *.xls", ValidateNames = true })
            {
                if (box.ShowDialog() == DialogResult.OK)
                {
                    await Task.Run(() =>
                    {
                        IsExport = true;
                        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                        app.Visible = false;
                        Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Add(1);
                        Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
                        ws.Name = "Danh sách phát hành";
                        ws.Cells.Style.Font.Size = 12;
                        ws.Cells.Style.Font.Name = "Times New Roman";

                        ws.Cells[1, 8] = "Thống kê doanh thu " + SelectedMonth + " " + SelectedYear.ToLower();
                        ws.Cells[2, 8] = "Tổng doanh thu:\t  " + RevenueReport.TotalRevenueStr;
       

                        ws.Cells[4, 8] = "Bảng thống kê doanh thu theo loại phòng : ";

                        ws.Cells[5, 8] = "STT";
                        ws.Cells[5, 9] = "Tên loại phòng";
                        ws.Cells[5, 10] = "Doanh số";
                        ws.Cells[5, 11] = "Tỉ lệ";

                        int i1 = 6;

                        foreach (var item in RevenueReport.revenueReportDetailDTOs)
                        {

                            ws.Cells[i1, 8] = item.STT;
                            ws.Cells[i1, 9] = item.RoomTypeName;
                            ws.Cells[i1, 10] = item.RevenueStr;
                            ws.Cells[i1, 11] = item.RatioStr;
                            i1++;
                        }


                        ws.SaveAs(box.FileName);
                        wb.Close();
                        app.Quit();


                    });
                }
                else
                {
                    IsExport = false;
                }
            }
        }
    }
}
