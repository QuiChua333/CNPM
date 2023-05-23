using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.View.CustomMessageBoxWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.ViewModel.CustomerTypeManagementVM
{
    public partial class CustomerTypeManagementVM : BaseVM
    {
        public async Task SaveCustomerTypeFunc(System.Windows.Window p)
        {
            if (IsValidDataCustomerType())
            {
                CustomerTypeDTO customertype = new CustomerTypeDTO
                {   // check ở đây
                    CustomerTypeName = CustomerTypeName,
                    CoefficientSurcharge = CoefficientSurcharge,
                };

                (bool successAddCustomerType, string messageFromAddCustomerType, CustomerTypeDTO newCustomerType) = await CustomerTypeService.Ins.AddCustomerType(customertype);

                if (successAddCustomerType)
                {
                    isSaving = false;
                    CustomMessageBox.ShowOk(messageFromAddCustomerType, "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                    ReloadListViewCustomerType();
                    p.Close();
                }
                else
                {
                    CustomMessageBox.ShowOk(messageFromAddCustomerType, "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
