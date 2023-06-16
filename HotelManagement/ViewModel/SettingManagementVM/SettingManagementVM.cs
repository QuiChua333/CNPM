using CinemaManagementProject.Utilities;
using HotelManagement.DTOs;
using HotelManagement.Model;
using HotelManagement.Utilities;
using HotelManagement.View;
using HotelManagement.View.SettingManagement;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private bool _IsEditMaxCus { get; set; }
        public bool IsEditMaxCus
        {
            get { return _IsEditMaxCus; }
            set { _IsEditMaxCus = value; OnPropertyChanged(); }
        }
        private bool _IsEditSoKhachPhuPhi { get; set; }
        public bool IsEditSoKhachPhuPhi
        {
            get { return _IsEditSoKhachPhuPhi; }
            set { _IsEditSoKhachPhuPhi = value; OnPropertyChanged(); }
        }
        
        private ObservableCollection<SurchargeRateDTO> listSurchargeRate;
        public ObservableCollection<SurchargeRateDTO> ListSurchargeRate
        {
            get { return listSurchargeRate; }
            set { listSurchargeRate = value; OnPropertyChanged(); }
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
        public ICommand FirstLoadSettingViewCM { get; set; }
        public ICommand EditMaxCusCM { get; set; }
        public ICommand EditNumCusSurchargeCM { get; set; }
        public ICommand AutoStartAppCM { get; set; }
        public ICommand ColorPickerCM { get; set; }
        public ICommand ChooseColorCM { get; set; }
        public ICommand CloseEditSurchargeRateCM { get; set; }
        public ICommand SaveSurchargeListCM { get; set; }
        public ICommand CLoseColorPickerCM { get; set; }
        public ICommand OpenEditSurchargeCM { get; set; }
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
                IsEditMaxCus = false;
                ColorPicked = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
            });
            FirstLoadSettingViewCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (reg.GetValue("HotelManagementApp") == null)
                    IsCheckedAutoStart = false;
                else
                    IsCheckedAutoStart = true;
                ColorPicked = (SolidColorBrush)new BrushConverter().ConvertFrom(Properties.Settings.Default.MainAppColor);
            });
            OpenEditSurchargeCM = new RelayCommand<PackIcon>((p) => { return true; }, (p) =>
            {
                try
                {
                    EditSurchargeFee editSurchargeFee = new EditSurchargeFee(p);
                    ListSurchargeRate = new ObservableCollection<SurchargeRateDTO>();
                    GetValue();
                    editSurchargeFee.ShowDialog();
                }
                catch (EntityException ex)
                {
                    CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                }
            });
            EditMaxCusCM = new RelayCommand<PackIcon>((p) => { return true; }, (p) =>
            {
                if(IsEditMaxCus == false)
                {
                    IsEditMaxCus = true;
                    p.Kind = PackIconKind.Tick;
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
                        if (SoKhachToiDa == SoKhachKhongTinhPhuPhi)
                        {
                            Equal();
                            p.Kind = PackIconKind.Pencil;
                            IsEditMaxCus = false;
                            return;
                        }
                        EditSurchargeFee editSurchargeFee = new EditSurchargeFee(p);
                        ListSurchargeRate = new ObservableCollection<SurchargeRateDTO>();
                        GetValue();
                        editSurchargeFee.ShowDialog();
                    }
                    catch (EntityException ex)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                }    
            });
            EditNumCusSurchargeCM = new RelayCommand<PackIcon>((p) => { return true; }, (p) =>
            {
                if (IsEditSoKhachPhuPhi == false)
                {
                    IsEditSoKhachPhuPhi = true;
                    p.Kind = PackIconKind.Tick;
                }
                else
                {
                    try
                    {
                        if (SoKhachKhongTinhPhuPhi < 1)
                        {
                            CustomMessageBox.ShowOk("Số khách không tính phụ phí phải từ 1 trở lên", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                            return;
                        }
                        if (SoKhachKhongTinhPhuPhi > SoKhachToiDa)
                        {
                            CustomMessageBox.ShowOk("Số khách không tính phụ phí phải nhỏ hơn số khách tối đa", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                            return;
                        }
                        if (  SoKhachKhongTinhPhuPhi == SoKhachToiDa)
                        {
                            Equal();
                            p.Kind = PackIconKind.Pencil;
                            IsEditSoKhachPhuPhi = false;
                            return;
                        }
                        EditSurchargeFee editSurchargeFee = new EditSurchargeFee(p);
                        ListSurchargeRate = new ObservableCollection<SurchargeRateDTO>();
                        GetValue();
                        editSurchargeFee.ShowDialog();
                    }
                    catch (EntityException ex)
                    {
                        CustomMessageBox.ShowOk("Mất kết nối cơ sở dữ liệu", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
                    catch (Exception e)
                    {
                        CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                    }
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
            CloseEditSurchargeRateCM = new RelayCommand<Rectangle>((p) => { return true; }, (p) =>
            {
                IsEditMaxCus = false;
                IsEditSoKhachPhuPhi = false;
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    SoKhachToiDa = (int)db.Parameters.FirstOrDefault(item => item.ParameterKey == "SoKhachToiDa").ParamaterValue;
                    SoKhachKhongTinhPhuPhi = (int)db.Parameters.FirstOrDefault(item => item.ParameterKey == "SoKhachKhongTinhPhuPhi").ParamaterValue;
                }
                    
            });
            SaveSurchargeListCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                for(int i = 0; i < ListSurchargeRate.Count; i++)
                {
                    double price;
                    bool isDouble = Double.TryParse(ListSurchargeRate[i].RateStr, out price);
                    if (!isDouble)
                    {
                        CustomMessageBox.ShowOk("Vui lòng nhập kiểu số thực cho tỷ lệ phụ thu của khách thứ " + (i + 1 + SoKhachKhongTinhPhuPhi), "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        if(price < 0 || price > 1)
                        {
                            CustomMessageBox.ShowOk("Tỷ lệ phụ thu khách thứ " + (i + 1 + SoKhachKhongTinhPhuPhi) + " phải nhỏ hơn 1 và lớn hơn 0", "Cảnh báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Warning);
                            return;
                        }
                        ListSurchargeRate[i].Rate = price;
                    }
                }    
                if(await SaveEditSurchargeRate())
                    CustomMessageBox.ShowOk("Lưu thông tin thành công", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                else
                    CustomMessageBox.ShowOk("Lỗi hệ thống", "Lỗi", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Error);
                ListSurchargeRate.Clear();
                p.Close();
                IsEditMaxCus = false;
                IsEditSoKhachPhuPhi = false;
            });
            CLoseColorPickerCM = new RelayCommand<Grid>((p) => { return true; }, (p) =>
            {
                p.Visibility = Visibility.Collapsed;
            });
        }
        public async void GetValue()
        {
            using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
            {
                for(int i = SoKhachKhongTinhPhuPhi; i < SoKhachToiDa; i++)
                {
                    SurchargeRateDTO srDTO = new SurchargeRateDTO();
                    srDTO.STT = i + 1;
                    SurchargeRate sr = await db.SurchargeRates.FirstOrDefaultAsync(item => item.CustomerIndex == srDTO.STT);
                    if(sr == null)
                        srDTO.Rate = 0;
                    else
                        srDTO.Rate = (double)sr.Rate;
                    srDTO.RateStr = System.Convert.ToString(srDTO.Rate);
                    ListSurchargeRate.Add(srDTO);
                }
                
            }
        }
        public async void Equal()
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    var listSurchargeRate = await db.SurchargeRates.ToListAsync();
                    if (listSurchargeRate == null || listSurchargeRate.Count == 0)
                    {
                        CustomMessageBox.ShowOk("Lưu thông tin thành công", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
                        return;
                    }
                    Parameter p = await db.Parameters.FirstOrDefaultAsync(item => item.ParameterKey == "SoKhachToiDa");
                    Parameter p2 = await db.Parameters.FirstOrDefaultAsync(item => item.ParameterKey == "SoKhachKhongTinhPhuPhi");
                    var list = await db.SurchargeRates.ToListAsync();
                    db.SurchargeRates.RemoveRange(list);
                    p.ParamaterValue =p2.ParamaterValue= SoKhachKhongTinhPhuPhi;
                    await db.SaveChangesAsync();
                    CustomMessageBox.ShowOk("Lưu thông tin thành công", "Thông báo", "OK", View.CustomMessageBoxWindow.CustomMessageBoxImage.Success);
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
  


        public async Task<bool> SaveEditSurchargeRate()
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    List<SurchargeRate> srList = db.SurchargeRates.ToList();
                    int OldLength = 0;
          
                    if (srList.Count != 0)
                        OldLength = srList.Last().CustomerIndex;
                    for (int i = SoKhachKhongTinhPhuPhi; i < SoKhachToiDa; i++)
                    {
                        SurchargeRate sr = await db.SurchargeRates.FirstOrDefaultAsync(item => item.CustomerIndex == i + 1);
                        if(sr == null)
                        {
                            sr = new SurchargeRate();
                            sr.CustomerIndex = i + 1;
                            sr.Rate = ListSurchargeRate[i - SoKhachKhongTinhPhuPhi].Rate;
                            db.SurchargeRates.Add(sr);
                        }    
                        else
                        {
                            sr.Rate = ListSurchargeRate[i - SoKhachKhongTinhPhuPhi].Rate;
                        }    
                    }
                    for(int i = SoKhachToiDa; i < OldLength; i++)
                    {
                        SurchargeRate sr = await db.SurchargeRates.FirstOrDefaultAsync(item => item.CustomerIndex == i + 1);
                        if(sr != null)
                            db.SurchargeRates.Remove(sr);
                    }
                    for (int i=2; i<=SoKhachKhongTinhPhuPhi; i++)
                    {
                        SurchargeRate sr = await db.SurchargeRates.FirstOrDefaultAsync(item => item.CustomerIndex == i);
                        if (sr != null)
                            db.SurchargeRates.Remove(sr);
                    }

                    (await db.Parameters.FirstOrDefaultAsync(item => item.ParameterKey == "SoKhachToiDa")).ParamaterValue = SoKhachToiDa;
                    (await db.Parameters.FirstOrDefaultAsync(item => item.ParameterKey == "SoKhachKhongTinhPhuPhi")).ParamaterValue = SoKhachKhongTinhPhuPhi;

                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch(EntityException e)
            {
                return false;
            }
        }
    }
}
