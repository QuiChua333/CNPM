using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomPageVM : BaseVM
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
        private string _cbRoomType;
        public string CbRoomType
        {
            get { return _cbRoomType; }
            set { _cbRoomType = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _listRoomType;
        public ObservableCollection<string> ListRoomType
        {
            get => _listRoomType;
            set
            {
                _listRoomType = value;
                OnPropertyChanged();
            }
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand LoadAddRoomCM { get; set; }
        public ICommand LoadDeleteRoomCM { get; set; }
        public ICommand LoadNoteRoomCM { get; set; }
        public ICommand LoadEditRoomCM { get; set; }
        public ICommand SaveRoomCM { get; set; }
        public ICommand UpdateRoomCM { get; set; }


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
            CbRoomType = null;
        }
        public bool IsValidData()
        {
            return !string.IsNullOrEmpty(RoomNote) &&
                !string.IsNullOrEmpty(RoomStatus) &&
                CbRoomType != null;
        }
    }
}
