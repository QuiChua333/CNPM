﻿using HotelManagement.DTOs;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelManagement.View.CustomerTypeManagement
{
    /// <summary>
    /// Interaction logic for CustomerTypeManagement.xaml
    /// </summary>
    public partial class CustomerTypeManagement : Page
    {
        public CustomerTypeManagement()
        {
            InitializeComponent();
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as CustomerTypeDTO).CustomerTypeName.ToString().IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(CustomerTypeListView.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                result.Content = CustomerTypeListView.Items.Count;
                CollectionViewSource.GetDefaultView(CustomerTypeListView.ItemsSource).Refresh();
            }
        }
    }
}
