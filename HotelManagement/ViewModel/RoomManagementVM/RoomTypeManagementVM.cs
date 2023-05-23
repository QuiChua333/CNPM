using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomPageVM : BaseVM
    {
        private string _roomType_id;
        public string RoomType_id
        {
            get { return _roomType_id; }
            set { _roomType_id = value; OnPropertyChanged(); }
        }

        private string _roomTypeName;
        public string RoomTypeName
        {
            get { return _roomTypeName; }
            set { _roomTypeName = value; OnPropertyChanged(); }
        }

        private double _roomTypePrice;
        public double RoomTypePrice
        {
            get { return _roomTypePrice; }
            set { _roomTypePrice = value; OnPropertyChanged(); }
        }

        private RoomTypeDTO _selectedItemRoomType;
        public RoomTypeDTO SelectedItemRoomType
        {
            get { return _selectedItemRoomType; }
            set { _selectedItemRoomType = value; OnPropertyChanged(); }
        }

        private bool isloaddingRoomType;
        public bool IsLoaddingRoomType
        {
            get { return isloaddingRoomType; }
            set { isloaddingRoomType = value; OnPropertyChanged(); }
        }

        private bool isSavingRoomType;
        public bool IsSavingRoomType
        {
            get { return isSavingRoomType; }
            set { isSavingRoomType = value; OnPropertyChanged(); }
        }

        private ObservableCollection<RoomTypeDTO> _roomTypeList;
        public ObservableCollection<RoomTypeDTO> RoomTypeList
        {
            get => _roomTypeList;
            set
            {
                _roomTypeList = value;
                OnPropertyChanged();
            }
        }

        public ICommand FirstLoadRoomTypeCM { get; set; }
        public ICommand CloseRoomTypeCM { get; set; }
        public ICommand LoadAddRoomTypeCM { get; set; }
        public ICommand LoadDeleteRoomTypeCM { get; set; }
        public ICommand LoadEditRoomTypeCM { get; set; }
        public ICommand SaveRoomTypeCM { get; set; }
        public ICommand UpdateRoomTypeCM { get; set; }


        public async void ReloadListViewRoomType()
        {
            RoomTypeList = new ObservableCollection<RoomTypeDTO>();
            try
            {
                IsLoaddingRoomType = true;
                RoomTypeList = new ObservableCollection<RoomTypeDTO>(await RoomTypeService.Ins.GetAllRoomType());
                IsLoaddingRoomType = false;
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
        public void LoadRoomTypeListView(Operation oper = Operation.READ, RoomTypeDTO r = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    RoomTypeList.Add(r);
                    break;
                case Operation.UPDATE:
                    var roomTypeFound = RoomTypeList.FirstOrDefault(x => x.RoomTypeId == r.RoomTypeId);
                    RoomTypeList[RoomTypeList.IndexOf(roomTypeFound)] = r;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < RoomTypeList.Count; i++)
                    {
                        if (RoomTypeList[i].RoomTypeId == SelectedItemRoomType?.RoomTypeId)
                        {
                            RoomTypeList.Remove(RoomTypeList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        public void RenewWindowDataRoomType()
        {
            RoomType_id= null;
            RoomTypeName = null;
            RoomTypePrice = 0;
        }
        public bool IsValidDataRoomType()
        {
            return !string.IsNullOrEmpty(RoomTypeName);
        }
    }
}
