using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomManagementVM : BaseVM
    {
        public ICommand LoadAddRoomCM { get; set; }
        public async Task SaveRoomFunc(System.Windows.Window p)
        {
            if (IsValidData())
            {
                string rtn = CbRoomType.Tag.ToString();
                string rti = await RoomTypeService.Ins.GetRoomTypeID(rtn);
                RoomDTO room = new RoomDTO
                {   // check ở đây
                    RoomNumber = RoomNumber,
                    Note = RoomNote,
                    RoomTypeId = rti,
                    RoomTypeName = CbRoomType.Tag.ToString(),
                    RoomStatus = "Phòng trống",
                };

                (bool successAddRoom, string messageFromAddRoom, RoomDTO newRoom) = await RoomService.Ins.AddRoom(room);

                if (successAddRoom)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromAddRoom, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    ReloadListView();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromAddRoom, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
