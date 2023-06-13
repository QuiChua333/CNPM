using HotelManagement.DTOs;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class HistoryService
    {
        public HistoryService() { }
        private static HistoryService _ins;
        public static HistoryService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new HistoryService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }
        public List<string> GetListFilterYear()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var listYear = context.Bills.Select(x => x.CreateDate.Value.Year).ToList();
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
        public async Task<List<BillDTO>> GetListBill(string yearstr, string monthstr)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    
                    var list = await context.Bills.Select(x => new BillDTO
                    {
                        BillId = x.BillId,
                        CustomerName = x.CustomerName,
                        Address = x.Address,
                        TotalPrice = x.TotalPrice,
                        CreateDate = x.CreateDate,
                        Bills = x.BillDetails.Select(t=> new BillDetailDTO
                        {
                            BillId= t.BillId,
                            BillDetailId= t.BillDetailId,
                            RoomId = t.RoomId,
                            RoomNumber = (int)t.Room.RoomNumber,
                            NumberOfRentalDays = (int)t.NumberOfRentalDays,
                            PricePerDay = (double)t.PricePerDay,
                            Price = (double)t.Price,

                        }).ToList(),
                    }).ToListAsync();
                    if (yearstr!="Tất cả")
                    {
                        int year = int.Parse(yearstr.Substring(4));
                        list = new List<BillDTO>(list.Where(x => x.CreateDate.Value.Year == year).ToList());
                    }
                    if (monthstr!="Tất cả")
                    {
                        int month = int.Parse(monthstr.Substring(6));
                        list = new List<BillDTO>(list.Where(x => x.CreateDate.Value.Month == month).ToList());
                    }
                    list.Reverse();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<(bool,string)> DeleteBill(BillDTO billDTO)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var list = await context.BillDetails.Where(x=> x.BillId == billDTO.BillId).ToListAsync();
                     context.BillDetails.RemoveRange(list);
                    await context.SaveChangesAsync();
                    var bill = await context.Bills.FindAsync(billDTO.BillId);
                    context.Bills.Remove(bill);
                    await context.SaveChangesAsync();
                    return (true, "Xóa hóa đơn thành công!");
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
    }
}
