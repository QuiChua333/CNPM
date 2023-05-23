using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomManagement;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomPageVM : BaseVM
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
        public ICommand LoadRoomTypeCM { get; set; }
        public ICommand LoadRoomCM { get; set; }


        public RoomPageVM()
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

            LoadRoomTypeCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new RoomTypeManagement();
            });

            LoadRoomCM = new RelayCommand<Card>((p) => { return true; }, (p) =>
            {
                ChangeView(p);
                mainFrame.Content = new RoomManagementPage();
            });


            // RoomManagement
            FirstLoadCM = new RelayCommand<System.Windows.Controls.Page>((p) => { return true; }, async (p) =>
            {
                RoomList = new ObservableCollection<RoomDTO>();
                try
                {
                    IsLoadding = true;
                    RoomList = new ObservableCollection<RoomDTO>(await Task.Run(() => RoomService.Ins.GetAllRoom()));
                    IsLoadding = false;
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
            });

            LoadAddRoomCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                RenewWindowData();
                AddNewRoom addRoomType = new AddNewRoom();

                try
                {
                    IsLoading = true;
                    ListRoomType = new ObservableCollection<string>((await RoomTypeService.Ins.GetAllRoomType()).Select(x => x.RoomTypeName));
                    IsLoading = false;
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

                addRoomType.ShowDialog();
            });
            LoadEditRoomCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                View.RoomManagement.EditRoom w1 = new View.RoomManagement.EditRoom();

                await LoadEditRoom(w1);
                w1.ShowDialog();
            });
            LoadNoteRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                NoteRoom w1 = new NoteRoom();
                RoomNote = SelectedItem.Note;
                w1.ShowDialog();
            });
            LoadDeleteRoomCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                string message = "Bạn có chắc muốn xoá phòng này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    IsLoadding = true;

                    (bool successDeleteRoom, string messageFromDelRoom) = await RoomService.Ins.DeleteRoom(SelectedItem.RoomId);

                    IsLoadding = false;

                    if (successDeleteRoom)
                    {
                        LoadRoomListView(Operation.DELETE);
                        SelectedItem = null;
                        CustomMessageBox.ShowOk(messageFromDelRoom, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDelRoom, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });
            UpdateRoomCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateRoomFunc(p);
                IsSaving = false;
            });
            SaveRoomCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;

                await SaveRoomFunc(p);

                IsSaving = false;
            });

            CloseCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                SelectedItem = null;
                p.Close();
            });



            // RoomTypeManagement
            FirstLoadRoomTypeCM = new RelayCommand<System.Windows.Controls.Page>((p) => { return true; }, async (p) =>
            {
                RoomTypeList = new ObservableCollection<RoomTypeDTO>();
                try
                {
                    IsLoadding = true;
                    RoomTypeList = new ObservableCollection<RoomTypeDTO>(await Task.Run(() => RoomTypeService.Ins.GetAllRoomType()));
                    IsLoadding = false;
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
            });

            LoadAddRoomTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowDataRoomType();
                AddNewRoomType addRoomType = new AddNewRoomType();
                addRoomType.ShowDialog();
            });
            
            SaveRoomTypeCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await SaveRoomTypeFunc(p);
                IsSaving = false;
            });

            LoadEditRoomTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                View.RoomManagement.EditRoomType w1 = new View.RoomManagement.EditRoomType();
                LoadEditRoomType(w1);
                w1.ShowDialog();
            });

            UpdateRoomTypeCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateRoomTypeFunc(p);
                IsSaving = false;
            });

            LoadDeleteRoomTypeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                string message = "Bạn có chắc muốn xoá phòng này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    IsLoadding = true;

                    (bool successDeleteRoomType, string messageFromDelRoomType) = await RoomTypeService.Ins.DeleteRoomType(SelectedItemRoomType.RoomTypeId);

                    IsLoadding = false;

                    if (successDeleteRoomType)
                    {
                        LoadRoomTypeListView(Operation.DELETE);
                        SelectedItemRoomType = null;
                        CustomMessageBox.ShowOk(messageFromDelRoomType, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDelRoomType, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });

            CloseRoomTypeCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                SelectedItemRoomType = null;
                p.Close();
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
