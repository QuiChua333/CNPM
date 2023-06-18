using HotelManagement.DTOs;
using HotelManagement.Utils;
using MaterialDesignThemes.Wpf;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class BillService
    {
        public BillService() { }
        private static BillService _ins;
        public static BillService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BillService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public async Task<List<BillDetailDTO>> GetListRoomBill(List<string> ListRoomNumber)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    List<BillDetailDTO> list = new List<BillDetailDTO>();
                    foreach (var roomNumberStr in ListRoomNumber)
                    {
                        int numPerForUnitPrice = (int) context.Parameters.FirstOrDefault(x => x.ParameterKey == "SoKhachKhongTinhPhuPhi").ParamaterValue;
                        var listSurcharge = await context.SurchargeRates.ToListAsync();
                        var listCusType = await context.CustomerTypes.ToListAsync();

                        int roomNumer = int.Parse(roomNumberStr);
                        Room r =  context.Rooms.FirstOrDefault(x => x.RoomNumber == roomNumer);
                        double RoomTypePrice = (double)r.RoomType.Price;

                        var listRentalContract = await context.RentalContracts.Where(x=> x.Room.RoomId == r.RoomId && x.Room.RoomStatus==ROOM_STATUS.RENTING).ToListAsync();
                        listRentalContract.Reverse();
                        RentalContract rentalContract = listRentalContract[0];
                        int numPer = rentalContract.RentalContractDetails.Count;
                        double PricePerDay = RoomTypePrice;
                        if (numPer > numPerForUnitPrice)
                        {
                            for (int i = numPerForUnitPrice+1; i <= numPer; i++)
                            {
                                PricePerDay += RoomTypePrice * (double)listSurcharge[i - (numPerForUnitPrice+1)].Rate;
                            }
                        }

                        double heSo = (double)1;
                        foreach (var item in rentalContract.RentalContractDetails)
                        {
                            string cusTypeId = item.CustomerTypeId ;
                            foreach (var item2 in listCusType)
                            {
                                string cusType2Id = item2.CustomerTypeId;
                                double heSo2 = (double)item2.CoefficientSurcharge;
                                if (cusTypeId == cusType2Id)
                                {
                                    if (heSo2 > heSo)
                                    {
                                        heSo = heSo2;
                                    }
                                }
                            }
                        }
                        PricePerDay *= heSo;
                        TimeSpan t = (TimeSpan)(DateTime.Now - rentalContract.CreateDate);
                        int NumberOfRentalDays = (int)t.TotalDays + 1;
                        double Price = PricePerDay * NumberOfRentalDays;

                        BillDetailDTO billDetailDTO = new BillDetailDTO
                        {
                            RoomId = r.RoomId,
                            RoomNumber = (int)r.RoomNumber,
                            NumberOfRentalDays = NumberOfRentalDays,
                            PricePerDay = PricePerDay,
                            Price = Price
                        };
                        list.Add(billDetailDTO);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(bool, string)> SaveBill(BillDTO billDTO, List<BillDetailDTO> billDetailDTOs)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var maxBillId = await context.Bills.MaxAsync(x => x.BillId);
                    string billId = CreateNextBillId(maxBillId);

                    Bill bill = new Bill
                    {
                       BillId= billId,
                       CustomerName= billDTO.CustomerName,
                       Address= billDTO.Address,
                       CreateDate= DateTime.Now,
                       TotalPrice= billDTO.TotalPrice
                    };
                    context.Bills.Add(bill);
                    await context.SaveChangesAsync();
                    var list = billDetailDTOs.Select(x=> new BillDetail
                    {
                        BillId= billId,
                        RoomId = x.RoomId,
                        NumberOfRentalDays= x.NumberOfRentalDays,
                        PricePerDay= x.PricePerDay,
                        Price = x.Price,
                    }).ToList();
                    
                     
                     context.BillDetails.AddRange(list);
                    await context.SaveChangesAsync();
                    foreach (var item in list)
                    {
                        Room r = await context.Rooms.FindAsync(item.RoomId);
                        r.RoomStatus = ROOM_STATUS.READY;
                        await context.SaveChangesAsync();
                    }

                    int year = DateTime.Now.Year;
                    int month = DateTime.Now.Month;
                    bool isExistRevenueReport = context.RevenueReports.Any(x => x.MonthReport.Value.Year == year && x.MonthReport.Value.Month == month);
                    if (!isExistRevenueReport)
                    {
                        var maxRevenueReportId = await context.RevenueReports.MaxAsync(x => x.RevenueReportId);
                        string revenueId = CreateNextRevenueReportId(maxRevenueReportId);
                        RevenueReport revenueReport = new RevenueReport
                        {
                            RevenueReportId = revenueId,
                            MonthReport = DateTime.Now,
                        };

                        context.RevenueReports.Add(revenueReport);
                        await context.SaveChangesAsync();
                       
                       var listRoomTypeId = await context.RoomTypes.Select(x=> x.RoomTypeId).ToListAsync();
                        foreach(var roomTypeId in listRoomTypeId)
                        {
                            RevenueReportDetail revenueReportDetail = new RevenueReportDetail
                            {
                                RevenueReportId = revenueId,
                                RoomTypeId = roomTypeId,
                                Revenue = 0,
                            };
                            context.RevenueReportDetails.Add(revenueReportDetail);
                            await context.SaveChangesAsync();
                        }
                       
                    }
                    var revenueReportDetails = context.RevenueReports.FirstOrDefault(x => x.MonthReport.Value.Year == year && x.MonthReport.Value.Month == month).RevenueReportDetails;
                   
                    foreach (var item in revenueReportDetails)
                    {
                        foreach (var i in list)
                        {
                            if (item.RoomTypeId == i.Room.RoomTypeId)
                            {
                                item.Revenue += i.Price;
                                break;
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                    double totalPrice = (double)context.Bills.Where(x => x.CreateDate.Value.Month == month && x.CreateDate.Value.Year == year).Sum(x => x.TotalPrice);
                     revenueReportDetails = context.RevenueReports.FirstOrDefault(x => x.MonthReport.Value.Year == year && x.MonthReport.Value.Month == month).RevenueReportDetails;

        
                    
                    foreach (var i in revenueReportDetails)
                    {
                      
                        i.Ratio = i.Revenue/ totalPrice;

                    }
                    await context.SaveChangesAsync();   


                    return (true, "Thanh toán thành công!");
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu!");
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống!");
            }
        }

       
        private string CreateNextBillId(string maxBillId)
        {
            if (maxBillId is null) return "HD001";
            int num = int.Parse(maxBillId.Substring(2));
            string nextNumString = (num + 1).ToString();
            while (nextNumString.Length < 3) nextNumString = "0" + nextNumString;
            return "HD" + nextNumString;

        }
        private string CreateNextRevenueReportId(string maxRevenueReportId)
        {
            if (maxRevenueReportId is null) return "DT001";
            int num = int.Parse(maxRevenueReportId.Substring(2));
            string nextNumString = (num + 1).ToString();
            while (nextNumString.Length < 3) nextNumString = "0" + nextNumString;
            return "DT" + nextNumString;

        }
    }
}
