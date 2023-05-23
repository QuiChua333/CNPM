using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class RoomTypeService
    {
        public RoomTypeService() { }
        private static RoomTypeService _ins;
        public static RoomTypeService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new RoomTypeService();
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<string> GetRoomTypeID(string rtn)
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    var item = await db.RoomTypes.Where(x => x.RoomTypeName == rtn).FirstOrDefaultAsync();
                    return item.RoomTypeId;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<RoomTypeDTO>> GetAllRoomType()
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    List<RoomTypeDTO> RoomTypeDTOs = await (
                        from rt in db.RoomTypes
                        select new RoomTypeDTO
                        {
                            // DTO = db
                            RoomTypeId = rt.RoomTypeId,
                            RoomTypeName = rt.RoomTypeName,
                            RoomTypePrice = (double)rt.Price,
                        }
                    ).ToListAsync();
                    RoomTypeDTOs.Reverse();
                    return RoomTypeDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private string CreateNextRoomTypeCode(string maxCode)
        {
            if (maxCode == "")
            {
                return "LP001";
            }
            int index = (int.Parse(maxCode.Substring(2)) + 1);
            string CodeID = index.ToString();
            while (CodeID.Length < 3) CodeID = "0" + CodeID;

            return "LP" + CodeID;
        }
        public async Task<(bool, string, RoomTypeDTO)> AddRoomType(RoomTypeDTO newRoomType)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    RoomType rt = context.RoomTypes.Where((RoomType RoomType) => RoomType.RoomTypeId == newRoomType.RoomTypeId).FirstOrDefault();

                    if (rt != null)
                    {

                        return (false, $"Loại phòng {rt.RoomTypeId} đã tồn tại!", null);

                    }
                    else
                    {
                        var listid = await context.RoomTypes.Select(s => s.RoomTypeId).ToListAsync();
                        string maxId = "";

                        if (listid.Count > 0)
                            maxId = listid[listid.Count - 1];

                        string id = CreateNextRoomTypeCode(maxId);
                        RoomType roomtype = new RoomType
                        {
                            RoomTypeId = id,
                            RoomTypeName = newRoomType.RoomTypeName,
                            Price = newRoomType.RoomTypePrice
                        };
                        context.RoomTypes.Add(roomtype);
                        await context.SaveChangesAsync();
                        newRoomType.RoomTypeId = roomtype.RoomTypeId;
                    }
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return (false, "DbEntityValidationException", null);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, $"Error Server {e}", null);
            }
            return (true, "Thêm loại phòng thành công", newRoomType);
        }


        public async Task<(bool, string)> UpdateRoomType(RoomTypeDTO updatedRoomType)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    RoomType roomtype = context.RoomTypes.Find(updatedRoomType.RoomTypeId);

                    if (roomtype is null)
                    {
                        return (false, "Phòng này không tồn tại!");
                    }

                    roomtype.RoomTypeId = updatedRoomType.RoomTypeId;
                    roomtype.RoomTypeName = updatedRoomType.RoomTypeName;
                    roomtype.Price = updatedRoomType.RoomTypePrice;

                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công");
                }
            }
            catch (DbEntityValidationException)
            {
                return (false, "DbEntityValidationException");
            }
            catch (DbUpdateException e)
            {
                return (false, $"DbUpdateException: {e.Message}");
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }

        }

        public async Task<(bool, string)> DeleteRoomType(string Id)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    Room room = await (from p in context.Rooms
                                       where p.RoomTypeId == Id
                                       select p).FirstOrDefaultAsync();
                    RoomType roomtype = await (from p in context.RoomTypes
                                       where p.RoomTypeId == Id
                                       select p).FirstOrDefaultAsync();

                    if (room is null) 
                    {
                        if (roomtype is null) 
                        {
                            return (false, "Không tìm thấy loại phòng này !");
                        }
                        else
                        {
                            context.RoomTypes.Remove(roomtype);
                            await context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        return (false, "Loại phòng này đã áp dụng trước đây và đang có khách đặt. Không thể xóa!");
                    }
                }
            }
            catch (Exception)
            {
                return (false, "Loại phòng này đã áp dụng trước đây và đang có khách đặt. Không thể xóa!");
            }
            return (true, "Xóa loại phòng thành công");
        }

    }
}
