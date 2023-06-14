using HotelManagement.DTOs;
using HotelManagement.ViewModel.RoomLookupManagementVM;
using System;
using System.CodeDom;
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

namespace HotelManagement.View.RoomLookupManagement
{
    /// <summary>
    /// Interaction logic for RoomLookupPage.xaml
    /// </summary>
    public partial class RoomLookupPage : Page
    {
        public RoomLookupPage()
        {
            InitializeComponent();
            listRoomList = new List<ListBox>();
        }
        List<ListBox> listRoomList;
        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }


        private void SearchBox_SearchTextChange(object sender, EventArgs e)
        {
            for (int i = 0; i < listRoomList.Count; i++)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listRoomList[i].ItemsSource);
                if (view != null)
                {
                    view.Filter = Filter;
                    CollectionViewSource.GetDefaultView(listRoomList[i].ItemsSource).Refresh();
                }
            }
        }
        private bool Filter(object item)
        {
            if (String.IsNullOrEmpty(SearchBox.Text))
                return true;
            else
                return ((item as RoomDTO).RoomName.ToString().IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0
                    || (item as RoomDTO).RoomTypeName.ToString().IndexOf(SearchBox.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void listListRoomType_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox a = sender as ListBox;

        }

        private void listRoom_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox a = sender as ListBox;
            if(a!=null)
                listRoomList.Add(a);
        }

    }
}
