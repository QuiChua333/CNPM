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
using System.Windows.Input;

namespace HotelManagement.ViewModel.RoomManagementVM
{
    public partial class RoomPageVM : BaseVM
    {
        public async Task LoadEditRoom(EditRoom w1)
        {
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
            RoomId = SelectedItem.RoomId;
            RoomNumber = (int)SelectedItem.RoomNumber;
            RoomNote = SelectedItem.Note;
            RoomStatus = SelectedItem.RoomStatus;
            CbRoomType = SelectedItem.RoomTypeName;

        }

        public async Task UpdateRoomFunc(System.Windows.Window p)
        {
            string rtn = CbRoomType;
            string rti = await RoomTypeService.Ins.GetRoomTypeID(rtn);

            if (RoomId != null && IsValidData())
            {
                RoomDTO room = new RoomDTO
                {
                    RoomId = RoomId,
                    Note = RoomNote,
                    RoomNumber = RoomNumber,
                    RoomTypeId = rti,
                    RoomTypeName = CbRoomType,
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
