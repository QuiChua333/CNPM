using HotelManagement.View.Admin.StatisticalManagement;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManagement.ViewModel.AdminVM.StatisticalManagementVM
{
    public partial class StatisticalManagementVM : BaseVM
    {
        public Frame mainFrame { get; set; }
        public Card ButtonView { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; OnPropertyChanged(); }
        }

        public ICommand LoadViewCM { get; set; }
        public ICommand StoreButtonNameCM { get; set; }
        public ICommand LoadAllStatisticalCM { get; set; }
        public ICommand ChangeIncomePeriodCM { get; set; }

        public ICommand LoadRoomTypeAndServiceStatiscalCM { get; set; }
        
        public ICommand ChangeRoomTypeRevenueCM { get; set; }
        public ICommand ChangeServiceTypeRevenueCM { get; set; }
        public StatisticalManagementVM() 
        {

            LoadViewCM = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                mainFrame = p;
            });

            StoreButtonNameCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ButtonView = p;
                p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
                p.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
            });

            LoadAllStatisticalCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new IncomeStatiscalManagement();
            });

            LoadRoomTypeAndServiceStatiscalCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new RoomTypeAndService();
            });

            ChangeIncomePeriodCM = new RelayCommand<ComboBox>((p) => { return true; }, async (p) =>
            {
                IsLoading = true;
                await ChangeIncomePeriod();
                IsLoading = false;
            });
            ChangeRoomTypeRevenueCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeRoomTypeRevenue();
            });
            ChangeServiceTypeRevenueCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                await ChangeServiceTypeRevenue();
            });

        }
        public void ChangeView(Card p)
        {
            ButtonView.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            ButtonView.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
            ButtonView = p;
            p.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("Transparent");
            p.SetValue(ElevationAssist.ElevationProperty, Elevation.Dp3);
        }
    }
}
