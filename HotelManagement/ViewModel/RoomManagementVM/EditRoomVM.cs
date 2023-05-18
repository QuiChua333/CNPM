using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomManagement;
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

        public ICommand LoadEditRoomCM { get; set; }

        public void LoadEditRoom(EditRoom w1)
        {
            RoomId = SelectedItem.RoomId;
            RoomNumber = (int)SelectedItem.RoomNumber;
            RoomNote = SelectedItem.Note;
            RoomStatus = SelectedItem.RoomStatus;
            if (SelectedItem.RoomTypeId == "LP001")
            {
                w1.loaiphong.SelectedIndex = 0;
            }
            else if (SelectedItem.RoomTypeId == "LP002")
            {
                w1.loaiphong.SelectedIndex = 1;
            }
            else w1.loaiphong.SelectedIndex = 2;

        }

        public async Task UpdateRoomFunc(System.Windows.Window p)
        {
            string rtn = CbRoomType.Tag.ToString();
            string rti = await RoomTypeService.Ins.GetRoomTypeID(rtn);

            if (RoomId != null && IsValidData())
            {
                RoomDTO room = new RoomDTO
                {
                    RoomId = RoomId,
                    Note = RoomNote,
                    RoomNumber = RoomNumber,
                    RoomTypeId = rti,
                    RoomTypeName = CbRoomType.Tag.ToString(),
                    RoomStatus = RoomStatus,
                };

                (bool successUpdateRoom, string messageFromUpdateRoom) = await RoomService.Ins.UpdateRoom(room);

                if (successUpdateRoom)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromUpdateRoom, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    LoadRoomListView(Operation.UPDATE, room);
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateRoom, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
