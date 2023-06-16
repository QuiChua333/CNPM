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
                double CoefficientSurchargeTemp;
                bool isDouble = Double.TryParse(CoefficientSurcharge, out CoefficientSurchargeTemp);
                if (!isDouble)
                {
                    CustomMessageBox.ShowOk("Vui lòng nhập kiểu số thực cho hệ số phụ thu!", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                    return;
                }
                else
                {
                    if (CoefficientSurchargeTemp <= 0)
                    {
                        CustomMessageBox.ShowOk("Hệ số phụ thu phải lớn hơn 0!", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        CustomerTypeDTO customertype = new CustomerTypeDTO
                        {   // check ở đây
                            CustomerTypeId = CustomerTypeId,
                            CustomerTypeName = CustomerTypeName.Trim(),
                            CoefficientSurcharge = CoefficientSurchargeTemp,
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
                }
               
                
            }
            else
            {
                CustomMessageBox.ShowOk("Vui lòng nhập đủ thông tin!", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
            }
        }
    }
}
