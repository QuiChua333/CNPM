using HotelManagement.DTOs;
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

namespace HotelManagement.View.HistoryManagement
{
    /// <summary>
    /// Interaction logic for HistoryManagementPage.xaml
    /// </summary>
    public partial class HistoryManagementPage : Page
    {
        public HistoryManagementPage()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as BillDTO).BillId.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as BillDTO).CustomerName.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as BillDTO).Address.IndexOf(SearchBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);

        }

        private void Search_SearchTextChange(object sender, EventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(BillListView.ItemsSource);
            if (view != null)
            {
                view.Filter = Filter;
                CollectionViewSource.GetDefaultView(BillListView.ItemsSource).Refresh();
            }
        }
    }
}
