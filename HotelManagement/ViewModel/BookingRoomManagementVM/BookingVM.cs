using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HotelManagement.ViewModel.BookingRoomManagementVM
{
    public partial class BookingRoomManagementVM : BaseVM
    {
        public async Task LoadReadyRoom()
        {
            ListReadyRoom = new ObservableCollection<RoomDTO>(await BookingRoomService.Ins.GetListReadyRoom());
        }
        public async Task SaveRentalContract(Window p)
        {
            RentalContractDTO temp = new RentalContractDTO
            {
                CreateDate = CreateDate,
                RoomId = SelectedRoom.RoomId
            };
            (bool isSucsses, string message) = await BookingRoomService.Ins.SaveRental(temp, new List<RentalContractDetailDTO>(ListCustomer));
            if (isSucsses)
            {
                CustomMessageBox.ShowOk(message, "Thông báo", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                RentalContractList = new ObservableCollection<RentalContractDTO>(await BookingRoomService.Ins.GetRentalContractList());
                await RoomService.Ins.ChangeRoomStatus(temp.RoomId);
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk(message, "Lỗi", "OK", CustomMessageBoxImage.Error);
            }
        }
        public void SaveCustomerFunc(System.Windows.Window p)
        {
            if (IsValidDataCustomer())
            {
                STT++;
                RentalContractDetailDTO cus = new RentalContractDetailDTO();
                // check ở đây
                cus.CustomerName = CustomerName;
                cus.Address = AddressCustomer;
                cus.CCCD = CCCD;
                cus.CustomerType = CustomerType.Content.ToString();
                cus.STT = STT;
                foreach (var i in ListCustomer)
                {
                    if (CCCD.Equals(i.CCCD))
                    {
                        CustomMessageBox.ShowOk("Số CCCD này đã tồn tại!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                foreach (var i in CCCD)
                {
                    if (!"0123456789".Contains(i))
                    {
                        CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                if (CCCD.Length != 12)
                {
                    CustomMessageBox.ShowOk("Định dạng CCCD đang khác 12 số. Vui lòng kiểm tra lại!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
                ListCustomer.Add(cus);
                CustomMessageBox.ShowOk("Thêm khách ở thành công !", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }

        public void EditCustomerFunc(System.Windows.Window p)
        {
            if (IsValidDataCustomer())
            {
                foreach (var i in CCCD)
                {
                    if (!"0123456789".Contains(i))
                    {
                        CustomMessageBox.ShowOk("Sai định dạng CCCD!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                if (CCCD.Length != 12)
                {
                    CustomMessageBox.ShowOk("Định dạng CCCD đang khác 12 số. Vui lòng kiểm tra lại!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                    return;
                }
                foreach (var i in ListCustomer)
                {
                    if (CCCD.Equals(i.CCCD) && CCCD != cccd_Last)
                    {
                        CustomMessageBox.ShowOk("Số CCCD này đã tồn tại!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                SelectedCustomer.CustomerName = CustomerName;
                SelectedCustomer.CCCD = CCCD;
                SelectedCustomer.Address = AddressCustomer;
                SelectedCustomer.CustomerType = CustomerType.Content.ToString();
                
                CustomMessageBox.ShowOk("Cập nhật thông tin khách ở thành công !", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
