﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HotelManagement.View
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        public static Label lbPageName;
        public HomeWindow()
        {
            InitializeComponent();
            lbPageName = this.PageName;

            StartClock();
        }
        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            DayMonthYear.Text = dt.ToString("dd MMMM yyyy HH:mm:ss");
            if (dt.Hour > 19)
            {
                Honorifics.Text = "Good Evening";
                AvatarGreeding.Fill = Brushes.Black;
                return;
            }
            if (dt.Hour > 12)
            {
                Honorifics.Text = "Good Afternoon";
                AvatarGreeding.Fill = Brushes.Orange;
                return;
            }
            Honorifics.Text = "Good Morning";
            AvatarGreeding.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom("#FED600");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AdminWD.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (AdminWD.WindowState == WindowState.Normal)
                AdminWD.WindowState = WindowState.Maximized;
            else
                AdminWD.WindowState = WindowState.Normal;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AdminWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void Tg_Btn_Checked(object sender, RoutedEventArgs e)
        {
            Mask.Visibility = Visibility.Visible;
        }

        private void Tg_Btn_Unchecked(object sender, RoutedEventArgs e)
        {
            Mask.Visibility = Visibility.Collapsed;
        }

        private void Mask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mask.Visibility = Visibility.Collapsed;

            DoubleAnimation animation = new DoubleAnimation(62, TimeSpan.FromSeconds(0.2));
            GridNav.BeginAnimation(Border.WidthProperty, animation);
            Tg_Btn.IsChecked = false;
        }

        private void MenuItems_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mask.Visibility = Visibility.Collapsed;

            DoubleAnimation animation = new DoubleAnimation(62, TimeSpan.FromSeconds(0.2));
            GridNav.BeginAnimation(Border.WidthProperty, animation);
            Tg_Btn.IsChecked = false;
        }
    }
}
