using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.Admin.RoomManagement;
using HotelManagement.View.Admin.RoomTypeManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;


namespace HotelManagement.ViewModel.AdminVM.RoomManagementVM
{
    public partial class RoomManagementVM : BaseVM
    {
        private string _RoomId;
        public string RoomId
        {
            get { return _RoomId; }
            set { _RoomId = value; OnPropertyChanged(); }
        }
        private string _roomTypeID;
        public string RoomTypeID
        {
            get { return _roomTypeID; }
            set { _roomTypeID = value; OnPropertyChanged(); }
        }

        private int _roomNumber;
        public int RoomNumber
        {
            get { return _roomNumber; }
            set { _roomNumber = value; OnPropertyChanged(); }
        }

        private string _roomNote;
        public string RoomNote
        {
            get { return _roomNote; }
            set { _roomNote = value; OnPropertyChanged(); }
        }

        private string _RoomStatus;
        public string RoomStatus
        {
            get { return _RoomStatus; }
            set { _RoomStatus = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _cbRoomType;
        public ComboBoxItem CbRoomType
        {
            get { return _cbRoomType; }
            set { _cbRoomType = value; OnPropertyChanged(); }
        }

        private ComboBoxItem _cbRoomTinhTrang;
        public ComboBoxItem CbRoomTinhTrang
        {
            get { return _cbRoomTinhTrang; }
            set { _cbRoomTinhTrang = value; OnPropertyChanged(); }
        }

        private RoomDTO _selectedItem;
        public RoomDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        private bool isloadding;
        public bool IsLoadding
        {
            get { return isloadding; }
            set { isloadding = value; OnPropertyChanged(); }
        }

        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }

        private ObservableCollection<RoomDTO> _roomList;
        public ObservableCollection<RoomDTO> RoomList
        {
            get => _roomList;
            set
            {
                _roomList = value;
                OnPropertyChanged();
            }
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand LoadDeleteRoomCM { get; set; }
        public ICommand LoadNoteRoomCM { get; set; }
        public ICommand SaveRoomCM { get; set; }
        public ICommand UpdateRoomCM { get; set; }
        public RoomManagementVM()
        {
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

            LoadAddRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowData();
                AddNewRoom addRoomType = new AddNewRoom();
                addRoomType.ShowDialog();
            });
            LoadEditRoomCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                View.Admin.RoomManagement.EditRoom w1 = new View.Admin.RoomManagement.EditRoom();
                LoadEditRoom(w1);
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

                string message = "Bạn có chắc muốn xoá phim này không? Dữ liệu không thể phục hồi sau khi xoá!";
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
        }


        public async void ReloadListView()
        {
            RoomList = new ObservableCollection<RoomDTO>();
            try
            {
                IsLoadding = true;
                RoomList = new ObservableCollection<RoomDTO>(await RoomService.Ins.GetAllRoom());
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
        }
        public void LoadRoomListView(Operation oper = Operation.READ, RoomDTO r = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    RoomList.Add(r);
                    break;
                case Operation.UPDATE:
                    var roomFound = RoomList.FirstOrDefault(x => x.RoomId == r.RoomId);
                    RoomList[RoomList.IndexOf(roomFound)] = r;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < RoomList.Count; i++)
                    {
                        if (RoomList[i].RoomTypeId == SelectedItem?.RoomTypeId)
                        {
                            RoomList.Remove(RoomList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        public void RenewWindowData()
        {

            RoomId = null;
            RoomNumber = 0;
            RoomNote = null;
            RoomTypeID = null;
            RoomStatus = "Phòng trống";
            CbRoomTinhTrang = null;
            CbRoomType = null;
        }
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(RoomNote) &&
                !string.IsNullOrEmpty(RoomStatus) &&
                !string.IsNullOrEmpty(CbRoomTinhTrang.Tag.ToString()) &&
                !string.IsNullOrEmpty(CbRoomType.Tag.ToString());
        }
    }
}
