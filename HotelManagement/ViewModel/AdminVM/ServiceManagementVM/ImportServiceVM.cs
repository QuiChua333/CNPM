using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.Admin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static HotelManagement.Utilities.Helper;

namespace HotelManagement.ViewModel.AdminVM.ServiceManagementVM
{
    public partial class ServiceManagementVM
    {
        public async Task ImportService(ServiceDTO serviceSelected, Window wd, AdminWindow mainWD)
        {
            try
            {
                string ImportQuantity = serviceSelected.ImportQuantity.ToString();
                string ImportPrice = serviceSelected.ImportPrice.ToString();

                if (string.IsNullOrEmpty(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập số lượng", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập giá nhập", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsNumeric(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập chữ số cho trường số lượng", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsNumeric(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập chữ số cho trường giá nhập", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsPositive(ImportPrice))
                {
                    CustomMessageBox.ShowOk("Giá nhập không được âm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                if (!Number.IsPositive(ImportQuantity))
                {
                    CustomMessageBox.ShowOk("Số lượng là một số lớn hơn 0", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }

                (bool isSuccess, string messageReturn) = await Task.Run(() => ServiceHelper.Ins.ImportService(serviceSelected, serviceSelected.ServicePrice, serviceSelected.ImportQuantity));
                if (isSuccess)
                {
                    CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    serviceCache.Quantity = serviceCache.Quantity + serviceCache.ImportQuantity;
                    LoadProductListView(Operation.UPDATE_PROD_QUANTITY, new ServiceDTO(serviceCache));
                    wd.Close();
                    mainWD.MaskOverSideBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            catch (EntityException e)
            {
                CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
            catch (Exception e)
            {
                CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
        }
    }
}
