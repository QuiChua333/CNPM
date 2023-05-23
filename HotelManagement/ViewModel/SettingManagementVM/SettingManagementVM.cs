using CinemaManagementProject.Utilities;
using HotelManagement.Model;
using HotelManagement.View;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HotelManagement.ViewModel.SettingManagementVM
{
    public class SettingManagementVM : BaseVM
    {
        private int soKhachKhongTinhPhuPhi;
        public int SoKhachKhongTinhPhuPhi
        {
            get { return soKhachKhongTinhPhuPhi; }
            set { soKhachKhongTinhPhuPhi = value; OnPropertyChanged(); }
        }
        private int soKhachToiDa;
        public int SoKhachToiDa
        {
            get { return soKhachToiDa; }
            set { soKhachToiDa = value; OnPropertyChanged(); }
        }
        private bool _isEdit { get; set; }
        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; OnPropertyChanged(); }
        }
        private bool _IsEditMaxCus { get; set; }
        public bool IsEditMaxCus
        {
            get { return _IsEditMaxCus; }
            set { _IsEditMaxCus = value; OnPropertyChanged(); }
        }
        private System.Windows.Media.Brush _colorPicked { get; set; }
        public System.Windows.Media.Brush ColorPicked
        {
            get { return _colorPicked; }
            set { _colorPicked = value; OnPropertyChanged(); }
        }
        private bool _isCheckedAutoStart { get; set; }
        public bool IsCheckedAutoStart
        {
            get { return _isCheckedAutoStart; }
            set { _isCheckedAutoStart = value; OnPropertyChanged(); }
        }
        private RegistryKey reg { get; set; }
        public ICommand FirstLoadCM { get; set; }
        public ICommand EditNoSubFeeCM { get; set; }
        public ICommand EditMaxCusCM { get; set; }
        public ICommand AutoStartAppCM { get; set; }
        public ICommand ColorPickerCM { get; set; }
        public ICommand ChooseColorCM { get; set; }
        public SettingManagementVM()
        {
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (reg.GetValue("HotelManagementApp") == null)
                    IsCheckedAutoStart = false;
                else
                    IsCheckedAutoStart = true;
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    SoKhachKhongTinhPhuPhi = (int)(db.Parameters.FirstOrDefault(item => item.ParameterKey == "SoKhachKhongTinhPhuPhi").ParamaterValue ?? 0);
                    SoKhachToiDa = (int)(db.Parameters.FirstOrDefault(item => item.ParameterKey == "SoKhachToiDa").ParamaterValue ?? 0);
                }
                IsEdit = false;
            });

            EditNoSubFeeCM = new RelayCommand<PackIcon>((p) => { return true; }, async (p) =>
            {
                if(IsEdit == false)
                {
                    IsEdit = true;
                    p.Kind = PackIconKind.ContentSaveEdit;
                }
                else
                {
                    try
                    {
                        if (SoKhachKhongTinhPhuPhi > SoKhachToiDa)
                        {
                            CustomMessageBox.ShowOk("Số khách không tính phụ phí phải nhỏ hơn số khách tối đa", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                            return;
                        }
                        using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                        {
                            Parameter pm = await db.Parameters.FirstOrDefaultAsync(item => item.ParameterKey == "SoKhachKhongTinhPhuPhi");
                            if (pm == null)
                                CustomMessageBox.ShowOk("Không tìm thấy tham số", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                            pm.ParamaterValue = SoKhachKhongTinhPhuPhi;
                            await db.SaveChangesAsync();
                            CustomMessageBox.ShowOk("Cập nhật thành công", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                        }
                    }
                    catch (EntityException ex)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    IsEdit = false;
                    p.Kind = PackIconKind.Pencil;
                }
            });
            EditMaxCusCM = new RelayCommand<PackIcon>((p) => { return true; }, async (p) =>
            {
                if (IsEditMaxCus == false)
                {
                    IsEditMaxCus = true;
                    p.Kind = PackIconKind.ContentSaveEdit;
                }
                else
                {
                    try
                    {
                        if (SoKhachKhongTinhPhuPhi > SoKhachToiDa)
                        {
                            CustomMessageBox.ShowOk("Số khách không tính phụ phí phải nhỏ hơn số khách tối đa", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                            return;
                        }
                        using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                        {
                            Parameter pm = await db.Parameters.FirstOrDefaultAsync(item => item.ParameterKey == "SoKhachToiDa");
                            if (pm == null)
                                CustomMessageBox.ShowOk("Không tìm thấy tham số", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                            pm.ParamaterValue = SoKhachToiDa;
                            await db.SaveChangesAsync();
                            CustomMessageBox.ShowOk("Cập nhật thành công", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                        }
                    }
                    catch (EntityException ex)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    IsEditMaxCus = false;
                    p.Kind = PackIconKind.Pencil;
                }
            });
            AutoStartAppCM = new RelayCommand<ToggleButton>((p) => { return true; }, (p) =>
            {
                if (p.IsChecked == true)
                    reg.SetValue("HotelManagementApp", System.Reflection.Assembly.GetExecutingAssembly().Location);
                else
                    reg.DeleteValue("HotelManagementApp");
            });

            ColorPickerCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = System.Windows.Visibility.Visible;
            });
            ChooseColorCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                HomeWindow tk = Application.Current.Windows.OfType<HomeWindow>().FirstOrDefault();
                ColorPicked = p.Fill;
                tk.Overlay.Fill = p.Fill;
                SolidColorBrush solidColorBrush = (SolidColorBrush)ColorPicked;
                Properties.Settings.Default.MainAppColor = solidColorBrush.Color.ToString();
                Properties.Settings.Default.Save();
            });
        }
    }
}
