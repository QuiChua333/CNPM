using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseVM
    {
        private SeriesCollection _InComeData;
        public SeriesCollection InComeData
        {
            get { return _InComeData; }
            set { _InComeData = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _SelectedIncomePeriod;
        public ComboBoxItem SelectedIncomePeriod
        {
            get { return _SelectedIncomePeriod; }
            set { _SelectedIncomePeriod = value; OnPropertyChanged(); }
        }

        private string _SelectedIncomeTime;
        public string SelectedIncomeTime
        {
            get { return _SelectedIncomeTime; }
            set { _SelectedIncomeTime = value; OnPropertyChanged(); }
        }

        private int selectedYear;
        public int SelectedYear
        {
            get { return selectedYear; }
            set { selectedYear = value; }
        }


        private string _TrueIncome;
        public string TrueIncome
        {
            get { return _TrueIncome; }
            set { _TrueIncome = value; OnPropertyChanged(); }
        }

        private string _TotalIn;
        public string TotalIn
        {
            get { return _TotalIn; }
            set { _TotalIn = value; OnPropertyChanged(); }
        }

        //MID CARD========================
        private string _TotalOut;
        public string TotalOut
        {
            get { return _TotalOut; }
            set { _TotalOut = value; OnPropertyChanged(); }
        }

        private float _TotalInPc;  //this is for the horizontial bar, just for displaying
        public float TotalInPc
        {
            get { return _TotalInPc; }
            set { _TotalInPc = value; OnPropertyChanged(); }
        }

        private float _TotalOutPc;
        public float TotalOutPc
        {
            get { return _TotalOutPc; }
            set { _TotalOutPc = value; OnPropertyChanged(); }
        }

        private int totalBill;
        public int TotalBill
        {
            get { return totalBill; }
            set { totalBill = value; OnPropertyChanged(); }
        }

        private string rentalReve;
        public string RentalReve
        {
            get { return rentalReve; }
            set { rentalReve = value; OnPropertyChanged(); }
        }

        private string serviceReve;
        public string ServiceReve
        {
            get { return serviceReve; }
            set { serviceReve = value; OnPropertyChanged(); }
        }

        private string serviceExpe;
        public string ServiceExpe
        {
            get { return serviceExpe; }
            set { serviceExpe = value; OnPropertyChanged(); }
        }

        private string repairExpe;
        public string RepairExpe
        {
            get { return repairExpe; }
            set { repairExpe = value; OnPropertyChanged(); }
        }

        private string furnitureExpe;
        public string FurnitureExpe
        {
            get { return furnitureExpe; }
            set { furnitureExpe = value; OnPropertyChanged(); }
        }

        private string rentalPc;
        public string RentalPc
        {
            get { return rentalPc; }
            set { rentalPc = value; OnPropertyChanged(); }
        }

        private string servicePc;
        public string ServicePc
        {
            get { return servicePc; }
            set { servicePc = value; OnPropertyChanged(); }
        }

        private string furniturePc;
        public string FurniturePc
        {
            get { return furniturePc; }
            set { furniturePc = value; OnPropertyChanged(); }
        }

        private string serviceExPc;
        public string ServiceExPc
        {
            get { return serviceExPc; }
            set { serviceExPc = value; OnPropertyChanged(); }
        }

        private string repairPc;
        public string RepairPc
        {
            get { return repairPc; }
            set { repairPc = value; OnPropertyChanged(); }
        }

        private string reveRate;
        public string ReveRate
        {
            get { return reveRate; }
            set { reveRate = value; OnPropertyChanged(); }
        }

        private string expeRate;
        public string ExpeRate
        {
            get { return expeRate; }
            set { expeRate = value; OnPropertyChanged(); }
        }


        private int _LabelMaxValue;
        public int LabelMaxValue
        {
            get { return _LabelMaxValue; }
            set { _LabelMaxValue = value; OnPropertyChanged(); }
        }

        public async Task ChangeIncomePeriod()
        {
            if (SelectedIncomePeriod != null)
            {
                switch (SelectedIncomePeriod.Content.ToString())
                {
                    case "Theo năm":
                        {
                            if (SelectedIncomeTime != null)
                            {
                                if (SelectedIncomeTime.Length == 4)
                                    SelectedYear = int.Parse(SelectedIncomeTime);
                                await LoadIncomeByYear();
                            }
                            return;
                        }
                    case "Theo tháng":
                        {
                            if (SelectedIncomeTime != null)
                            {
                                await LoadIncomeByMonth();
                            }
                            return;
                        }
                }
            }
        }

        public async Task LoadIncomeByYear()
        {
            if (SelectedIncomeTime.Length != 4) return;
            LabelMaxValue = 12;
            try
            {

                TotalBill = await Task.Run(() => OverviewStatisticService.Ins.GetBillQuantity(int.Parse(SelectedIncomeTime)));
                (List<double> monthlyRevenue, double Servicereve,double Rentalreve, string YearRevenueRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetRevenueByYear(int.Parse(SelectedIncomeTime)));
                (List<double> monthlyExpense, double ServiceExpense, double RepairCost, double FurnitureExpense,string YearExpenseRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetExpenseByYear(int.Parse(SelectedIncomeTime)));


                RentalReve = Helper.FormatVNMoney(Rentalreve);
                ServiceReve = Helper.FormatVNMoney(Servicereve);
                ServiceExpe = Helper.FormatVNMoney(ServiceExpense);
                RepairExpe = Helper.FormatVNMoney(RepairCost);
                FurnitureExpe = Helper.FormatVNMoney(FurnitureExpense);
                ExpeRate = YearExpenseRateStr;

                monthlyRevenue.Insert(0, 0);
                monthlyExpense.Insert(0, 0);

                CalculateTrueIncome(monthlyRevenue, monthlyExpense);
                Calculate_RevExpPercentage(Rentalreve, Servicereve, ServiceExpense, RepairCost, FurnitureExpense);

                for (int i = 1; i <= 12; i++)
                {
                    monthlyRevenue[i] /= 1000000;
                    monthlyExpense[i] /= 1000000;
                }


                InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(monthlyRevenue),
                Fill = Brushes.Transparent
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(monthlyExpense),
                Fill = Brushes.Transparent
            }
            };
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                throw;
            }
        }

        public void CalculateTrueIncome(List<double> l1, List<double> l2)
        {

            float totalin = 0, totalout = 0;

            foreach (float item in l1)
                totalin += item;
            foreach (float item in l2)
                totalout += item;

            float trueincome = totalin - totalout;
            FindMaxPercentage(totalin, totalout);


            TrueIncome = Helper.FormatVNMoney(trueincome);
            TotalIn = Helper.FormatVNMoney(totalin);
            TotalOut = Helper.FormatVNMoney(totalout);
        }

        public void FindMaxPercentage(float _in, float _out)
        {
            if (_in != 0 && _out != 0)
            {
                if (_in >= _out)
                {
                    TotalInPc = 100;
                    TotalOutPc = _out / _in * 100;
                }
                else
                {
                    TotalOutPc = 100;
                    TotalInPc = _in / _out * 100;
                }
            }
            else
            {
                if (_in == 0 && _out == 0)
                {
                    TotalInPc = 10;
                    TotalOutPc = 10;
                    return;
                }
                if (_in == 0)
                {
                    TotalInPc = 10;
                    TotalOutPc = 90;
                    return;
                }
                if (_out == 0)
                {
                    TotalInPc = 90;
                    TotalOutPc = 10;
                    return;
                }

            }

        }
        public void Calculate_RevExpPercentage(double a1, double a2, double a3, double a4, double a5)
        {
            Calculate_RevPercentage(a1, a2);
            Calculate_ExpPercentage(a3, a4,a5);
        }
        public void Calculate_RevPercentage(double a1, double a2)
        {
            if (a1 == 0)
            {
                if (a2 == 0)
                    RentalPc = ServicePc = "0%";
                else
                {
                    RentalPc = "0%";
                    ServicePc = "100%";
                }
                return;
            }
            if (a2 == 0)
            {
                if (a1 == 0)
                    RentalPc = ServicePc = "0%";
                else
                {
                    RentalPc = "100%";
                    ServicePc = "0%";
                }
                return;
            }

            RentalPc = Math.Round(a1 / (a1 + a2) * 100, 2).ToString() + "%";
            ServicePc = Math.Round(a2 / (a1 + a2) * 100, 2).ToString() + "%";
        }
        public void Calculate_ExpPercentage(double a3, double a4, double a5)
        {
            if (a3 == 0 && a4 == 0 && a5 == 0)
            {
                ServiceExPc = RepairPc = FurniturePc = "0%";
                return;
            }
            
            ServiceExPc = Math.Round(a3 / (a3 + a4 + a5) * 100, 2).ToString() + "%";
            RepairPc = Math.Round(a4 / (a3 + a4 + a5) * 100, 2).ToString() + "%";
            FurniturePc = Math.Round(a5 / (a3 + a4 + a5) * 100, 2).ToString() + "%";
            
        }
        public async Task LoadIncomeByMonth()
        {
            if (SelectedIncomeTime.Length == 4) return;
            LabelMaxValue = 30;
            try
            {
                if (SelectedIncomeTime == "January") SelectedIncomeTime = "Tháng 1";
                if (SelectedIncomeTime == "February") SelectedIncomeTime = "Tháng 2";
                if (SelectedIncomeTime == "March") SelectedIncomeTime = "Tháng 3";
                if (SelectedIncomeTime == "April") SelectedIncomeTime = "Tháng 4";
                if (SelectedIncomeTime == "May") SelectedIncomeTime = "Tháng 5";
                if (SelectedIncomeTime == "June") SelectedIncomeTime = "Tháng 6";
                if (SelectedIncomeTime == "July") SelectedIncomeTime = "Tháng 7";
                if (SelectedIncomeTime == "August") SelectedIncomeTime = "Tháng 8";
                if (SelectedIncomeTime == "September") SelectedIncomeTime = "Tháng 9";
                if (SelectedIncomeTime == "October") SelectedIncomeTime = "Tháng 10";
                if (SelectedIncomeTime == "November") SelectedIncomeTime = "Tháng 11";
                if (SelectedIncomeTime == "December") SelectedIncomeTime = "Tháng 12";

                TotalBill = await OverviewStatisticService.Ins.GetBillQuantity(2021, int.Parse(SelectedIncomeTime.Remove(0, 6)));
                (List<double> dailyRevenue, double MonthServiceReve, double MonthRentalReve, string MonthRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetRevenueByMonth(SelectedYear, int.Parse(SelectedIncomeTime.Remove(0, 6))));
                (List<double> dailyExpense, double MonthServiceExpense, double MonthRepairCost, double FurnitureExpense, string MonthExpenseRateStr) = await Task.Run(() => OverviewStatisticService.Ins.GetExpenseByMonth(SelectedYear, int.Parse(SelectedIncomeTime.Remove(0, 6))));
                RentalReve = Helper.FormatVNMoney(MonthRentalReve);
                ServiceReve = Helper.FormatVNMoney(MonthServiceReve);
                ServiceExpe = Helper.FormatVNMoney(MonthServiceExpense);
                RepairExpe = Helper.FormatVNMoney(MonthRepairCost);
                FurnitureExpe = Helper.FormatVNMoney(FurnitureExpense);
                ReveRate = MonthRateStr;
                ExpeRate = MonthExpenseRateStr;

                dailyRevenue.Insert(0, 0);
                dailyExpense.Insert(0, 0);

                CalculateTrueIncome(dailyRevenue, dailyExpense);
                Calculate_RevExpPercentage(MonthRentalReve, MonthServiceReve, MonthServiceExpense, MonthRepairCost, FurnitureExpense);

                for (int i = 1; i <= dailyRevenue.Count - 1; i++)
                {
                    dailyRevenue[i] /= 1000000;
                    dailyExpense[i] /= 1000000;
                }

                InComeData = new SeriesCollection
            {
            new LineSeries
            {
                Title = "Thu",
                Values = new ChartValues<double>(dailyRevenue),
                Fill = Brushes.Transparent,
            },
            new LineSeries
            {
                Title = "Chi",
                Values = new ChartValues<double>(dailyExpense),
                Fill = Brushes.Transparent,
            }
            };
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
        }
    }
}
