﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View.SettingManagement
{
    /// <summary>
    /// Interaction logic for ResetPassWindow.xaml
    /// </summary>
    public partial class ResetPassWindow : Window
    {
        public ResetPassWindow()
        {
            InitializeComponent();
        }

        private void ResetPassWD_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ConfirmText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (NewPassText.Password.Length == 0)
            {
                Error.Content = "Không được để trống password";
                Error.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }
            if (NewPassText.Password == ConfirmText.Password)
            {
                Error.Content = "Chính xác";
                Error.Foreground = new SolidColorBrush(Colors.DarkSeaGreen);
                ConfirmButton.Opacity = 1;
                ConfirmButton.IsEnabled = true;
            }
            else
            {
                Error.Content = "Chưa chính xác";
                Error.Foreground = new SolidColorBrush(Colors.Red);
                ConfirmButton.Opacity = 0.6;
                ConfirmButton.IsEnabled = false;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
