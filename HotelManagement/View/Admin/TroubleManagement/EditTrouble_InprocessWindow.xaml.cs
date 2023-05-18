﻿using HotelManagement.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelManagement.View.Admin.TroubleManagement
{
    /// <summary>
    /// Interaction logic for EditTrouble_InprocessWindow.xaml
    /// </summary>
    public partial class EditTrouble_InprocessWindow : Window
    {
        public EditTrouble_InprocessWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void cbbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbb = sender as ComboBox;
            if (cbb.SelectedIndex == 0)
            {
                finishdate.Visibility = Visibility.Visible;
                fixprice.Visibility = Visibility.Visible;
            }
            if (cbb.SelectedIndex == 1)
            {
                gridfixdate.Visibility = Visibility.Collapsed;
                finishdate.Visibility = Visibility.Collapsed;
                fixprice.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                tb.Text = "0";
        }
    }
}
