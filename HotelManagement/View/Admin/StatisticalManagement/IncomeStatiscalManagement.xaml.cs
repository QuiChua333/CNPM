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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;

namespace HotelManagement.View.Admin.StatisticalManagement
{
    /// <summary>
    /// Interaction logic for IncomeStatiscalManagement.xaml
    /// </summary>
    public partial class IncomeStatiscalManagement : Page
    {
        public IncomeStatiscalManagement()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("vi-VN");
        }

        private void periodbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem s = (ComboBoxItem)periodbox.SelectedItem;
            if (s != null)
            {
                switch (s.Content.ToString())
                {
                    case "Theo năm":
                        {
                            GetYearSource(Timebox);
                            return;
                        }
                    case "Theo tháng":
                        {
                            GetMonthSource(Timebox);
                            return;
                        }
                }
            }
        }
        private void periodbox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GetYearSource(Timebox);
        }
        public void GetYearSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();

            int now = -1;
            for (int i = 2020; i <= System.DateTime.Now.Year; i++)
            {
                now++;
                l.Add(i.ToString());
            }
            cbb.ItemsSource = l;
            cbb.SelectedIndex = now;
        }
        public void GetMonthSource(ComboBox cbb)
        {
            if (cbb is null) return;

            List<string> l = new List<string>();
            if (Properties.Settings.Default.isEnglish)
            {
                l.Add("January");
                l.Add("February");
                l.Add("March");
                l.Add("April");
                l.Add("May");
                l.Add("June");
                l.Add("July");
                l.Add("August");
                l.Add("September");
                l.Add("October");
                l.Add("November");
                l.Add("December");
            }
            else
            {
                l.Add("Tháng 1");
                l.Add("Tháng 2");
                l.Add("Tháng 3");
                l.Add("Tháng 4");
                l.Add("Tháng 5");
                l.Add("Tháng 6");
                l.Add("Tháng 7");
                l.Add("Tháng 8");
                l.Add("Tháng 9");
                l.Add("Tháng 10");
                l.Add("Tháng 11");
                l.Add("Tháng 12");
            }

            cbb.ItemsSource = l;
            cbb.SelectedIndex = DateTime.Today.Month - 1;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!string.IsNullOrEmpty(tb.Text))
            { 
                if (tb.Text.StartsWith("-"))
                {
                    if (tb.Text == "-2")
                    {
                        tb.Text = "Tăng";
                        tb.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    else
                    {
                        tb.Foreground = new SolidColorBrush(Colors.Red);
                    }
                }

                else
                    tb.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void TextBox_TextChanged1(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!string.IsNullOrEmpty(tb.Text))
            {
                if (tb.Text.StartsWith("-"))
                {
                    if (tb.Text == "-2")
                    {
                        tb.Text = "Tăng";
                        tb.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        tb.Foreground = new SolidColorBrush(Colors.Green);
                    }
                }

                else
                    tb.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
