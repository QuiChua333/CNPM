﻿using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utilities;
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
            if (SelectedRoom == null)
            {
                CustomMessageBox.ShowOk("Vui lòng chọn phòng và thêm khách hàng", "Thông báo", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (ListCustomer==null || ListCustomer.Count == 0)
            {
                CustomMessageBox.ShowOk("Vui lòng thêm khách hàng", "Thông báo", "Ok", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
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
        public async Task SaveCustomerFunc(System.Windows.Window p)
        {
            if (IsValidDataCustomer())
            {
                STT++;
                RentalContractDetailDTO cus = new RentalContractDetailDTO();
                // check ở đây
                cus.CustomerName = CustomerName;
                cus.Address = AddressCustomer;
                cus.CCCD = CCCD;
                cus.CustomerType = CustomerType;
                cus.STT = STT;
                foreach (var i in ListCustomer)
                {
                    if (CCCD.Equals(i.CCCD))
                    {
                        CustomMessageBox.ShowOk("Số CCCD/ ID định danh này đã tồn tại trong danh sách!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
                
                ListCustomer.Add(cus);
                CustomMessageBox.ShowOk("Thêm khách ở thành công !", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);

                Price = await BookingRoomService.Ins.GetPriceBooking(SelectedRoom.RoomId, new List<RentalContractDetailDTO>(ListCustomer));
                PriceStr = Helper.FormatVNMoney2(Price);
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }

        public async Task EditCustomerFunc(System.Windows.Window p)
        {
            if (IsValidDataCustomer())
            {
                foreach (var i in ListCustomer)
                {
                    if (CCCD.Equals(i.CCCD) && i!= SelectedCustomer)
                    {
                        CustomMessageBox.ShowOk("Số CCCD/ ID định danh này đã tồn tại trong danh sách!", "Thông Báo", "OK", CustomMessageBoxImage.Warning);
                        return;
                    }
                }
              
                SelectedCustomer.CustomerName = CustomerName;
                SelectedCustomer.CCCD = CCCD;
                SelectedCustomer.Address = AddressCustomer;
                SelectedCustomer.CustomerType = CustomerType;
                
                CustomMessageBox.ShowOk("Cập nhật thông tin khách ở thành công !", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                 Price = await BookingRoomService.Ins.GetPriceBooking(SelectedRoom.RoomId, new List<RentalContractDetailDTO>(ListCustomer));
                PriceStr = Helper.FormatVNMoney2(Price);
                p.Close();
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
