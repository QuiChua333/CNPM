using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomerTypeManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.CustomerTypeManagementVM
{
    public partial class CustomerTypeManagementVM : BaseVM
    {
        public void LoadEditCustomerType(EditCustomerType w)
        {
            CustomerTypeName = SelectedItemCustomerType.CustomerTypeName;
            CustomerTypeId = SelectedItemCustomerType.CustomerTypeId;
            CoefficientSurcharge = SelectedItemCustomerType.CoefficientSurchargeStr;
        }

        public async Task UpdateCustomerTypeFunc(System.Windows.Window p)
        {
            if (CustomerTypeId != null && IsValidDataCustomerType())
            {
                double t = Double.Parse(CoefficientSurcharge);
                CustomerTypeDTO customertype = new CustomerTypeDTO
                {
                    CustomerTypeId = CustomerTypeId,
                    CustomerTypeName = CustomerTypeName,
                    CoefficientSurcharge = t,
                };

                (bool successUpdate, string messageFromUpdate) = await CustomerTypeService.Ins.UpdateCustomerType(customertype);

                if (successUpdate)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromUpdate, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    LoadCustomerTypeListView(Operation.UPDATE, customertype);
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromUpdate, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
