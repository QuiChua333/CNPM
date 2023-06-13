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

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomPageVM : BaseVM
    {
        public void LoadEditRoomType(EditRoomType w1)
        {
            RoomType_id = SelectedItemRoomType.RoomTypeId;
            RoomTypeName = SelectedItemRoomType.RoomTypeName;
            RoomTypePrice = SelectedItemRoomType.RoomTypePrice;
        }

        public async Task UpdateRoomTypeFunc(System.Windows.Window p)
        {
            if (RoomType_id != null && IsValidDataRoomType())
            {
                RoomTypeDTO roomtype = new RoomTypeDTO
                {
                    RoomTypeId = RoomType_id,
                    RoomTypeName = RoomTypeName.Trim(),
                    RoomTypePrice = RoomTypePrice,
                };

                (bool successUpdateRoomType, string messageFromUpdateRoomType) = await RoomTypeService.Ins.UpdateRoomType(roomtype);

                if (successUpdateRoomType)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromUpdateRoomType, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    LoadRoomTypeListView(Operation.UPDATE, roomtype);
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdateRoomType, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
