using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace HotelManagement.Model.Services
{
    public class BookingRoomService
    {
        private static BookingRoomService _ins;
        public static BookingRoomService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new BookingRoomService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public BookingRoomService() { }
        public List<string> GetListFilterYear()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var listYear = context.RentalContracts.Select(x => x.CreateDate.Value.Year).ToList();
                    if (listYear == null) listYear = new List<int>();
                    if (!listYear.Contains(DateTime.Now.Year))
                    {
                        listYear.Add(DateTime.Now.Year);
                    }
                    var listYearStr = listYear.Select(x => "Năm " + x.ToString()).ToList();
                    listYearStr.Reverse();
                    List<string> list = new List<string>();
                    foreach (var item in listYearStr)
                    {
                        if (!list.Contains(item)) list.Add(item);
                    }
                    list.Insert(0, "Tất cả");
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<RentalContractDTO>> GetRentalContractListFilter(string yearstr, string monthstr)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {

                    int index = 1;
                    List<RentalContractDTO> RentalContractDTOs = await (
                        from r in context.RentalContracts
                        join room in context.Rooms
                        on r.RoomId equals room.RoomId
                        select new RentalContractDTO
                        {
                            RentalContractId = r.RentalContractId,
                            CreateDate = r.CreateDate,
                            RoomId = r.RoomId,
                            RoomNumber = (int)room.RoomNumber,
                            RentalContracts = r.RentalContractDetails.Select(x => new RentalContractDetailDTO
                            {
                                RentalContractId = x.RentalContractId,
                                CustomerName = x.CustomerName,
                                CCCD = x.CCCD,
                                Address = x.Address,
                                CustomerType = x.CustomerType.CustomerTypeName,
                            }).ToList()
                        }
                    ).ToListAsync();
                    RentalContractDTOs.Reverse();
                    foreach (var r in RentalContractDTOs)
                    {
                        r.STT_RentalContract = index++;
                        int index2 = 1;
                        foreach (var item in r.RentalContracts)
                        {
                            item.STT = index2++;
                        }
                    }
                    if (yearstr != "Tất cả")
                    {
                        int year = int.Parse(yearstr.Substring(4));
                        RentalContractDTOs = new List<RentalContractDTO>(RentalContractDTOs.Where(x => x.CreateDate.Value.Year == year).ToList());
                    }
                    if (monthstr != "Tất cả")
                    {
                        int month = int.Parse(monthstr.Substring(6));
                        RentalContractDTOs = new List<RentalContractDTO>(RentalContractDTOs.Where(x => x.CreateDate.Value.Month == month).ToList());
                    }


                    return RentalContractDTOs;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<(bool, string)> DeleteRentalContract(string rentalContractId)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    RentalContract rentalContract = await context.RentalContracts.FindAsync(rentalContractId);
                    bool isRenting = rentalContract.Room.RoomStatus == "Phòng đang thuê";
                    if (isRenting)
                    {
                        return (false, "Phiếu thuê của phòng đang thuê, không thể xóa!");
                    }
                    context.RentalContractDetails.RemoveRange(rentalContract.RentalContractDetails);
                    await context.SaveChangesAsync();
                    context.RentalContracts.Remove(rentalContract);
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống!");
            }
           
        }
        
        public async Task<(bool, string)> SaveRental(RentalContractDTO rentalContract, List<RentalContractDetailDTO> list)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var maxId = await context.RentalContracts.MaxAsync(s => s.RentalContractId);
                    string rentalId = CreateNextRentalContractId(maxId);
                    RentalContract rc = new RentalContract
                    {
                        RentalContractId = rentalId,
                        RoomId = rentalContract.RoomId,
                        CreateDate = DateTime.Now,
                    };

                    context.RentalContracts.Add(rc);
                    await context.SaveChangesAsync();
                    
                    foreach  (var item in list)
                    {
                        
                        string customerTypeId =  context.CustomerTypes.FirstOrDefault(x => x.CustomerTypeName == item.CustomerType.Trim())
                            .CustomerTypeId;
                        var maxCPId = await context.RentalContractDetails.MaxAsync(s => s.RentalContractDetailId);
                        string CPId = CreateNextRentalContractId(maxCPId);
                        RentalContractDetail rentalContractDetail = new RentalContractDetail
                        {
                            RentalContractDetailId = CPId,
                            RentalContractId = rentalId,
                            CustomerName = item.CustomerName,
                            Address = item.Address,
                            CCCD = item.CCCD,
                            CustomerTypeId = customerTypeId,
                        };
                        context.RentalContractDetails.Add(rentalContractDetail);
                        await context.SaveChangesAsync();
                    }
                    return (true, "Xác nhận đặt phòng thành công!");
                }
            }
            catch (Exception e)
            {
                return (false, "Lỗi hệ thống");
            }
        }
        private string CreateNextRentalContractId(string maxId)
        {
            //KHxxx
            if (maxId is null)
            {
                return "PT001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "PT" + newIdString.Substring(newIdString.Length - 3, 3);
        }
        private string CreateNextRentalContractDetailId(string maxId)
        {
            //KHxxx
            if (maxId is null)
            {
                return "CP001";
            }
            string newIdString = $"000{int.Parse(maxId.Substring(2)) + 1}";
            return "CP" + newIdString.Substring(newIdString.Length - 3, 3);
        }
        public async Task<List<RoomDTO>> GetListReadyRoom()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var list = await context.Rooms.Where(x => x.RoomStatus == "Phòng trống").Select(x => new RoomDTO
                    {
                        RoomId = x.RoomId,
                        RoomNumber = (int)x.RoomNumber,
                        RoomTypeName = x.RoomType.RoomTypeName,
                        RoomTypeId = x.RoomTypeId,
                        Price = (double)x.RoomType.Price,
                    }).ToListAsync();

                    return list;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<double> GetRentalContractPrice(string rentId)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    int numPerForUnitPrice = (int)context.Parameters.FirstOrDefault(x => x.ParameterKey == "SoKhachKhongTinhPhuPhi").ParamaterValue;
                    double rateForeign = (double)context.CustomerTypes.FirstOrDefault(x => x.CustomerTypeName == "Nước ngoài").CoefficientSurcharge;
                    var listSurcharge = await context.SurchargeRates.ToListAsync();
                  
                    
                    RentalContract rentalContract = await context.RentalContracts.FindAsync(rentId);
                    int numPer = rentalContract.RentalContractDetails.Count;
                    bool isHasForeign = rentalContract.RentalContractDetails.Any(x => x.CustomerType.CustomerTypeName == "Nước ngoài");
                    double RoomTypePrice = (double)rentalContract.Room.RoomType.Price;
                    double PricePerDay = RoomTypePrice;
                    if (numPer > numPerForUnitPrice)
                    {
                        for (int i = 0; i < numPer - numPerForUnitPrice; i++)
                        {
                            PricePerDay += RoomTypePrice * (double)listSurcharge[i].Rate;
                        }
                    }
                    if (isHasForeign)
                    {
                        PricePerDay *= rateForeign;
                    }
                    return PricePerDay;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<RentalContractDTO>> GetRentalContractList()
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    int index = 1;
                    List<RentalContractDTO> RentalContractDTOs = await (
                        from r in db.RentalContracts
                        join room in db.Rooms
                        on r.RoomId equals room.RoomId
                        select new RentalContractDTO
                        {
                            RentalContractId = r.RentalContractId,
                            CreateDate = r.CreateDate,
                            RoomId = r.RoomId,
                            RoomNumber = (int)room.RoomNumber,
                            RentalContracts = r.RentalContractDetails.Select(x => new RentalContractDetailDTO
                            {
                                RentalContractId = x.RentalContractId,
                                CustomerName = x.CustomerName,
                                CCCD = x.CCCD,  
                                Address = x.Address,    
                                CustomerType = x.CustomerType.CustomerTypeName,
                            }).ToList()
                        }
                    ).ToListAsync();
                    RentalContractDTOs.Reverse();
                    foreach (var  r in RentalContractDTOs)
                    {
                        r.STT_RentalContract = index++;
                        int index2 = 1;
                        foreach (var item in r.RentalContracts)
                        {
                            item.STT = index2++;
                        }
                    }


                    return RentalContractDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public int GetMaxNumOfPer()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {

                    int res = (int)context.Parameters.FirstOrDefault(x => x.ParameterKey == "SoKhachToiDa").ParamaterValue;
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
