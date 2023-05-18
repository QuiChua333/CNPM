﻿using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.View.Admin;
using HotelManagement.Utils;
using static HotelManagement.Utilities.Helper;

namespace HotelManagement.ViewModel.AdminVM.ServiceManagementVM
{
    public partial class ServiceManagementVM
    {
        public List<string> filterSource { get; set; }
        public async Task SaveEditProduct(ServiceDTO serviceDTO, Window wd, AdminWindow adWD)
        {
            if(string.IsNullOrEmpty(serviceDTO.ServiceName))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập tên sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(serviceDTO.ServiceType))
            {
                CustomMessageBox.ShowOk("Vui lòng chọn loại sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (!Number.IsNumeric(serviceDTO.ServicePrice.ToString()))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập một số cho giá sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (!Number.IsPositive(serviceDTO.ServicePrice.ToString()))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập một số dương cho giá sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            (bool isSucess, string messageReturn) = await Task.Run(() => ServiceHelper.Ins.SaveEditProduct(serviceDTO));

            if(isSucess)
            {
                CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                LoadProductListView(Operation.UPDATE, serviceDTO);
            }
            else
            {
                CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
            wd.Close();
            adWD.MaskOverSideBar.Visibility = Visibility.Collapsed;

        }
        public async Task SaveEditCleanService(ServiceDTO serviceDTO, Window wd)
        {
            if (!Number.IsNumeric(serviceDTO.ServicePrice.ToString()))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập một số cho giá sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (!Number.IsPositive(serviceDTO.ServicePrice.ToString()))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập một số dương cho giá sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            (bool isSucess, string messageReturn) = await Task.Run(() => ServiceHelper.Ins.SaveEditProduct(serviceDTO));

            if (isSucess)
            {
                CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                ServiceCache.FormatStringUnitAndPrice();
                LoadProductListView(Operation.UPDATECLEAN, serviceDTO);
            }
            else
            {
                CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
            wd.Close();
        }
        public async Task AddProduct(ServiceDTO productCache, Window wd, AdminWindow adWD)
        {
            if (string.IsNullOrEmpty(productCache.ServiceName))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập tên sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(productCache.ServiceType))
            {
                CustomMessageBox.ShowOk("Vui lòng chọn loại sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (!Number.IsNumeric(productCache.ServicePrice.ToString()))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập một số cho giá sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }
            if (!Number.IsPositive(productCache.ServicePrice.ToString()))
            {
                CustomMessageBox.ShowOk("Vui lòng nhập một số dương cho giá sản phẩm", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                return;
            }

            (bool isSucess, string messageReturn) = await Task.Run(() => ServiceHelper.Ins.AddProduct(productCache));

            if (isSucess)
            {
                CustomMessageBox.ShowOk(messageReturn, "Thành công", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                LoadProductListView(Operation.CREATE, productCache);
            }
            else
            {
                CustomMessageBox.ShowOk(messageReturn, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
            }
            ServiceCache = null;
            wd.Close();
            adWD.MaskOverSideBar.Visibility = Visibility.Collapsed;
        }
    }
}
