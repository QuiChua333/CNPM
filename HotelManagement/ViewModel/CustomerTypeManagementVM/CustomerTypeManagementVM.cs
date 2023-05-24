using HotelManagement.DTOs;
using HotelManagement.Model.Services;
using HotelManagement.Utils;
using HotelManagement.View.CustomerTypeManagement;
using HotelManagement.View.CustomMessageBoxWindow;
using HotelManagement.View.RoomManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HotelManagement.ViewModel.CustomerTypeManagementVM
{
    public partial class CustomerTypeManagementVM:BaseVM
    {
        private string _CustomerTypeId;
        public string CustomerTypeId
        {
            get { return _CustomerTypeId; }
            set { _CustomerTypeId = value; OnPropertyChanged(); }
        }

        private string _CustomerTypeName;
        public string CustomerTypeName
        {
            get { return _CustomerTypeName; }
            set { _CustomerTypeName = value; OnPropertyChanged(); }
        }

        private string _CoefficientSurcharge;
        public string CoefficientSurcharge
        {
            get { return _CoefficientSurcharge; }
            set { _CoefficientSurcharge = value; OnPropertyChanged(); }
        }

        private CustomerTypeDTO _SelectedItemCustomerType;
        public CustomerTypeDTO SelectedItemCustomerType
        {
            get { return _SelectedItemCustomerType; }
            set { _SelectedItemCustomerType = value; OnPropertyChanged(); }
        }

        private bool isloaddingCustomerType;
        public bool IsLoaddingCustomerType
        {
            get { return isloaddingCustomerType; }
            set { isloaddingCustomerType = value; OnPropertyChanged(); }
        }
        private bool isSaving;
        public bool IsSaving
        {
            get { return isSaving; }
            set { isSaving = value; OnPropertyChanged(); }
        }
        private ObservableCollection<CustomerTypeDTO> _CustomerTypeList;
        public ObservableCollection<CustomerTypeDTO> CustomerTypeList
        {
            get => _CustomerTypeList;
            set
            {
                _CustomerTypeList = value;
                OnPropertyChanged();
            }
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand CloseCM { get; set; }
        public ICommand LoadAddCustomerTypeCM { get; set; }
        public ICommand LoadDeleteCustomerTypeCM { get; set; }
        public ICommand LoadEditCustomerTypeCM { get; set; }
        public ICommand SaveCustomerTypeCM { get; set; }
        public ICommand UpdateCustomerTypeCM { get; set; }


        public CustomerTypeManagementVM()
        {
            FirstLoadCM = new RelayCommand<System.Windows.Controls.Page>((p) => { return true; }, async (p) =>
            {
                CustomerTypeList = new ObservableCollection<CustomerTypeDTO>();
                try
                {
                    IsLoaddingCustomerType = true;
                    CustomerTypeList = new ObservableCollection<CustomerTypeDTO>(await Task.Run(() => CustomerTypeService.Ins.GetAllCustomerType()));
                    IsLoaddingCustomerType = false;
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
            });

            LoadAddCustomerTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                RenewWindowDataCustomerType();
                AddCustomerType addCusType = new AddCustomerType();
                addCusType.ShowDialog();
            });

            SaveCustomerTypeCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await SaveCustomerTypeFunc(p);
                IsSaving = false;
            });
            LoadEditCustomerTypeCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                EditCustomerType w = new EditCustomerType();
                LoadEditCustomerType(w);
                w.ShowDialog();
            });
            UpdateCustomerTypeCM = new RelayCommand<System.Windows.Window>((p) => { if (IsSaving) return false; return true; }, async (p) =>
            {
                IsSaving = true;
                await UpdateCustomerTypeFunc(p);
                IsSaving = false;
            });
            LoadDeleteCustomerTypeCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                string message = "Bạn có chắc muốn xoá phòng này không? Dữ liệu không thể phục hồi sau khi xoá!";
                CustomMessageBoxResult kq = CustomMessageBox.ShowOkCancel(message, "Cảnh báo", "Xác nhận", "Hủy", CustomMessageBoxImage.Warning);

                if (kq == CustomMessageBoxResult.OK)
                {
                    IsLoaddingCustomerType = true;

                    (bool successDelete, string messageFromDel) = await CustomerTypeService.Ins.DeleteCustomerType(SelectedItemCustomerType.CustomerTypeId);

                    IsLoaddingCustomerType = false;

                    if (successDelete)
                    {
                        LoadCustomerTypeListView(Operation.DELETE);
                        SelectedItemCustomerType = null;
                        CustomMessageBox.ShowOk(messageFromDel, "Thông báo", "OK", CustomMessageBoxImage.Success);
                    }
                    else
                    {
                        CustomMessageBox.ShowOk(messageFromDel, "Lỗi", "OK", CustomMessageBoxImage.Error);
                    }
                }
            });
            CloseCM = new RelayCommand<System.Windows.Window>((p) => { return true; }, (p) =>
            {
                SelectedItemCustomerType = null;
                p.Close();
            });
        }

        public async void ReloadListViewCustomerType()
        {
            CustomerTypeList = new ObservableCollection<CustomerTypeDTO>();
            try
            {
                IsLoaddingCustomerType = true;
                CustomerTypeList = new ObservableCollection<CustomerTypeDTO>(await CustomerTypeService.Ins.GetAllCustomerType());
                IsLoaddingCustomerType = false;
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
        }
        public void LoadCustomerTypeListView(Operation oper = Operation.READ, CustomerTypeDTO ct = null)
        {

            switch (oper)
            {
                case Operation.CREATE:
                    CustomerTypeList.Add(ct);
                    break;
                case Operation.UPDATE:
                    var roomTypeFound = CustomerTypeList.FirstOrDefault(x => x.CustomerTypeId == ct.CustomerTypeId);
                    CustomerTypeList[CustomerTypeList.IndexOf(roomTypeFound)] = ct;
                    break;
                case Operation.DELETE:
                    for (int i = 0; i < CustomerTypeList.Count; i++)
                    {
                        if (CustomerTypeList[i].CustomerTypeId == SelectedItemCustomerType?.CustomerTypeId)
                        {
                            CustomerTypeList.Remove(CustomerTypeList[i]);
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public void RenewWindowDataCustomerType()
        {
            CustomerTypeId = null;
            CustomerTypeName = null;
            CoefficientSurcharge = "";
        }
        public bool IsValidDataCustomerType()
        {
            return !string.IsNullOrEmpty(CustomerTypeName);
        }

    }
}
