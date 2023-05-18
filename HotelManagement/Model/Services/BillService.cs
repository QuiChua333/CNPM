using HotelManagement.DTOs;
using HotelManagement.Utils;
using HotelManagement.ViewModel.StaffVM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public async Task<List<BillDTO>> GetBillByListRentalContract(List<RentalContractDTO> rentalContractDTOs)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                   var listRentalContractId = rentalContractDTOs.Select(x=> x.RentalContractId).ToList();

                    var list = await context.RentalContracts.Where(x => listRentalContractId.Contains(x.RentalContractId))
                         .Select(x => new BillDTO
                         {
                             RentalContractId = x.RentalContractId,
                             CustomerId = x.CustomerId,
                             CustomerName = x.Customer.CustomerName,
                             CustomerAddress = x.Customer.CustomerAddress,
                             RoomId = x.RoomId,
                             RoomNumber = (int)x.Room.RoomNumber,
                             RoomTypeName = x.Room.RoomType.RoomTypeName,
                             StartDate = x.StartDate,
                             StartTime = x.StartTime,
                             CheckOutDate = x.CheckOutDate,
                             PersonNumber = x.RoomCustomers.Count(),
                             IsHasForeignPerson = (x.RoomCustomers.Where(t => t.CustomerType == "Nước ngoài").Count() > 0),
                             RoomPrice = x.Room.RoomType.Price,
                             ListListServicePayment = x.ServiceUsings.Select(t => new ServiceUsingDTO
                             {
                                 RentalContractId = t.RentalContractId,
                                 ServiceId = t.ServiceId,
                                 ServiceName = t.Service.ServiceName,
                                 ServiceType = t.Service.ServiceType,
                                 UnitPrice = t.Service.ServicePrice,
                                 Quantity = t.Quantity,
                             }).ToList(),
                             ListTroubleByCustomer = x.TroubleByCustomers.Select(t => new TroubleByCustomerDTO {
                                 RentalContractId = t.RentalContractId,
                                 TroubleId = t.TroubleId,
                                 Title = t.Trouble.Title,
                                 PredictedPrice = t.PredictedPrice,
                                 Level = t.Trouble.Level,
                             }).ToList(),

                         }).ToListAsync();
                    
                    foreach (var item in list)
                    {
                        var listService = (item.ListListServicePayment).Where(x => x.ServiceName != "Giặt sấy")
                                                            .GroupBy(x => x.ServiceId)
                                                            .Select(t => new ServiceUsingDTO
                                                            {
                                                                RentalContractId = t.First().RentalContractId,
                                                                ServiceId = t.First().ServiceId,
                                                                ServiceName = t.First().ServiceName,
                                                                ServiceType = t.First().ServiceType,
                                                                UnitPrice = t.First().UnitPrice,
                                                                Quantity = t.Sum(g => g.Quantity)
                                                            }).ToList();
                        foreach (var item2 in item.ListListServicePayment)
                        {
                            if (item2.ServiceName == "Giặt sấy")
                            {
                                listService.Insert(0, item2);
                            }
                        }
                        item.ListListServicePayment = listService;
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<(bool, string)> SaveBill(BillDTO bill)
        {
            try
            {
                using (var context = new HotelManagementEntities())
                {
                    var maxBillId = await context.Bills.MaxAsync(x=> x.BillId);
                    Bill newBill = new Bill
                    {
                        BillId = CreateNextBillId(maxBillId),
                        RentalContractId=bill.RentalContractId,
                        StaffId= bill.StaffId,
                        NumberOfRentalDays= bill.NumberOfRentalDays,
                        ServicePrice= bill.ServicePrice,
                        TroublePrice= bill.TroublePrice,
                        TotalPrice= bill.TotalPrice,
                        CreateDate= bill.CreateDate,    
                    };
                    context.Bills.Add(newBill);
                    RentalContract rental = await context.RentalContracts.FindAsync(bill.RentalContractId);
                    rental.PersonNumber=rental.RoomCustomers.Count();
                    await context.SaveChangesAsync();
                    return (true, "Thanh toán thành công!");
                }
            }
            catch (Exception ex)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        private string CreateNextBillId(string maxBillId)
        {
            if (maxBillId is null) return "HD001";
            int num = int.Parse(maxBillId.Substring(2));
            string nextNumString = (num + 1).ToString();
            while (nextNumString.Length <3) nextNumString = "0" + nextNumString;
            return "HD" + nextNumString;

        }
        //public async Task<List<BillDTO>> GetBillsByRentalContracts(List<string> rentalContractIds)
        //{
        //    try
        //    {
        //        using (var context = new HotelManagementEntities())
        //        {
        //            List<BillDTO> listBillDTO = new List<BillDTO>();
        //            foreach (var item in rentalContractIds)
        //            {
        //                var itemBillDTO = await (from r in context.Rooms
        //                                         join c in context.RentalContracts
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
