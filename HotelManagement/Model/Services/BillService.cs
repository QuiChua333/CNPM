using HotelManagement.DTOs;
using HotelManagement.Utils;
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
                        double rateForeign =(double)  context.CustomerTypes.FirstOrDefault(x => x.CustomerTypeName == "Nước ngoài").CoefficientSurcharge;
                        var listSurcharge = await context.SurchargeRates.ToListAsync();
                        int roomNumer = int.Parse(roomNumberStr);
                        Room r =  context.Rooms.FirstOrDefault(x => x.RoomNumber == roomNumer);
                        RentalContract rentalContract = context.RentalContracts.FirstOrDefault(x => x.RoomId == r.RoomId);
                        int numPer = rentalContract.RentalContractDetails.Count;
                        bool isHasForeign = rentalContract.RentalContractDetails.Any(x => x.CustomerType.CustomerTypeName == "Nước ngoài");
                        double PricePerDay = (double)r.RoomType.Price;
                        if (numPer > numPerForUnitPrice)
                        {
                            for (int i = 0; i < numPer - numPerForUnitPrice; i++)
                            {
                                PricePerDay += (double)r.RoomType.Price *(double)listSurcharge[i].Rate;
                            }
                        }
                        if (isHasForeign)
                        {
                            PricePerDay *= rateForeign;
                        }
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
                    return (true, "Thanh toán thành công!");
                }
            }
            catch (EntityException e)
            {
                return (false, "Mất kết nối cơ sở dữ liệu");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
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

    }
}
