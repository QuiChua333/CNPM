using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomPageVM : BaseVM
    {
        public async Task SaveRoomTypeFunc(System.Windows.Window p)
        {
            if (IsValidDataRoomType())
            {
                RoomTypeDTO roomtype = new RoomTypeDTO
                {   // check ở đây
                    RoomTypeName = RoomTypeName.Trim(),
                    RoomTypePrice = RoomTypePrice,
                };

                (bool successAddRoomType, string messageFromAddRoomType, RoomTypeDTO newRoomType) = await RoomTypeService.Ins.AddRoomType(roomtype);

                if (successAddRoomType)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromAddRoomType, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    ReloadListViewRoomType();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromAddRoomType, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
