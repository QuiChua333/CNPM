using HotelManagement.DTOs;
using HotelManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class RoomService
    {
        public RoomService() { }
        private static RoomService _ins;
        public static RoomService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new RoomService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<ListRoomTypeDTO>> GetListListRoomType()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {

                    var listRentalContract = await context.RentalContracts.Where(x=> x.Room.RoomStatus==ROOM_STATUS.RENTING).ToListAsync();
                    listRentalContract.Reverse();
                    var listRoomIdRenting = await context.Rooms.Where(x => x.RoomStatus == ROOM_STATUS.RENTING).Select(x=>x.RoomId).ToListAsync();
                    var list = await context.RoomTypes.Select(x => new ListRoomTypeDTO
                    {   
                        RoomTypeName= x.RoomTypeName,
                        RoomTypeId= x.RoomTypeId,
                        Price= (double)x.Price,
                        Rooms = x.Rooms.Where(t=> t.RoomStatus!=ROOM_STATUS.UNABLE).Select(t=> new RoomDTO
                        {
                            RoomId= t.RoomId,
                            RoomTypeId= x.RoomTypeId,
                            RoomTypeName = x.RoomTypeName,
                            RoomStatus= t.RoomStatus,
                            RoomNumber= t.RoomNumber,
                            Note= t.Note,
                        }).ToList(),
                    }).ToListAsync();
                    foreach (var item in list)
                    {
                        foreach (var room in item.Rooms)
                        {
                            if (listRoomIdRenting.Contains(room.RoomId))
                            {
                                var listRentalByRoomId = listRentalContract.Where(x => x.RoomId == room.RoomId).ToList();
                                var rentalContract = listRentalByRoomId[0];
                                room.RentalContractId = rentalContract.RentalContractId;
                                room.StartDate = rentalContract.CreateDate;
                                room.NumberOfPerson = rentalContract.RentalContractDetails.Count;
                            }
                        }
                    }
                  
                    return list;
                }
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }
        public RentalContract FindTrueRentalContract(string roomId, List<RentalContract> list)
        {
            var list2 = list.Where(x=> x.RoomId == roomId).ToList();
            list2.Reverse();
            var res = list2[0];
            return res;
        }
        public async Task<List<RoomDTO>> GetAllRoom()
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    List<RoomDTO> RoomDTOs = await (
                        from r in db.Rooms
                        join temp in db.RoomTypes
                        on r.RoomTypeId equals temp.RoomTypeId into gj
                        from d in gj.DefaultIfEmpty()
                        where r.RoomStatus != ROOM_STATUS.UNABLE
                        select new RoomDTO
                        {
                            // DTO = db
                            RoomId = r.RoomId,
                            RoomNumber = (int)r.RoomNumber,
                            RoomTypeName = d.RoomTypeName,
                            RoomTypeId = d.RoomTypeId,
                            Note = r.Note,
                            RoomStatus = r.RoomStatus,
                        }
                    ).ToListAsync();

                    var list2 = await db.Rooms.Where(r => r.RoomStatus == ROOM_STATUS.UNABLE).Select(r => new RoomDTO
                    {
                        RoomId = r.RoomId,
                        RoomNumber = (int)r.RoomNumber,
                        RoomTypeName = r.RoomType.RoomTypeName,
                        RoomTypeId = r.RoomType.RoomTypeId,
                        Note = r.Note,
                        RoomStatus = r.RoomStatus,
                    }).ToListAsync();
                    RoomDTOs.AddRange(list2);

                    return RoomDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private string CreateNextRoomCode(string maxCode)
        {
            if (maxCode == "")
            {
                return "PH001";
            }
            int index = (int.Parse(maxCode.Substring(2)) + 1);
            string CodeID = index.ToString();
            while (CodeID.Length < 3) CodeID = "0" + CodeID;

            return "PH" + CodeID;
        }
        public async Task<(bool, string, RoomDTO)> AddRoom(RoomDTO newRoom)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    Room r = context.Rooms.Where((Room Room) => Room.RoomNumber == newRoom.RoomNumber).FirstOrDefault();

                    if (r != null)
                    {

                        return (false, $"Phòng {r.RoomNumber} đã tồn tại!", null);

                    }
                    else
                    {
                        var listid = await context.Rooms.Select(s => s.RoomId).ToListAsync();
                        string maxId = "";

                        if (listid.Count > 0)
                            maxId = listid[listid.Count - 1];
                        string id = CreateNextRoomCode(maxId);
                        Room room = new Room
                        {
                            RoomId = id,
                            RoomNumber = newRoom.RoomNumber,
                            RoomTypeId = newRoom.RoomTypeId,
                            Note = newRoom.Note,
                            RoomStatus = newRoom.RoomStatus,
                        };
                        context.Rooms.Add(room);
                        await context.SaveChangesAsync();
                        newRoom.RoomId = room.RoomId;
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
            return (true, "Thêm phòng thành công", newRoom);
        }

        public async Task<(bool, string)> DeleteRoom(string Id)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    Room room = await (from p in context.Rooms
                                       where p.RoomId == Id
                                       select p).FirstOrDefaultAsync();
                    if (room.RoomStatus == "Phòng đang thuê")
                    {
                        return (false, "Phòng đang thuê nên không thể xóa!");
                    }
                    context.Rooms.Remove(room);
                    await context.SaveChangesAsync();
                    return (true, "Xóa phòng thành công");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống!");
            }
        }
        public async Task<(bool, string)> UnableRoom(string Id)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    Room room = await (from p in context.Rooms
                                       where p.RoomId == Id
                                       select p).FirstOrDefaultAsync();
                    room.RoomStatus = ROOM_STATUS.UNABLE;
                     await context.SaveChangesAsync();
                    return (true, "Ngưng sử dụng phòng thành công!");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống!");
            }
        }
        public async Task<(bool, string)> EnableRoom(string Id)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    Room room = await (from p in context.Rooms
                                       where p.RoomId == Id
                                       select p).FirstOrDefaultAsync();
                    room.RoomStatus = ROOM_STATUS.READY;
                    await context.SaveChangesAsync();
                    return (true, "Kích hoạt sử dụng phòng thành công!");
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống!");
            }
        }

        public async Task<(bool, string)> UpdateRoom(RoomDTO updatedRoom)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())

                {
                    Room r = context.Rooms.Where((Room Room) =>Room.RoomId!= updatedRoom.RoomId && Room.RoomNumber == updatedRoom.RoomNumber ).FirstOrDefault();

                    if (r != null)
                    {

                        return (false, $"Phòng {r.RoomNumber} đã tồn tại!");

                    }
                    Room room = context.Rooms.Find(updatedRoom.RoomId);

                    if (room is null)
                    {
                        return (false, "Phòng này không tồn tại!");
                    }

                    room.RoomId = updatedRoom.RoomId;
                    room.RoomNumber = updatedRoom.RoomNumber;
                    room.RoomStatus = updatedRoom.RoomStatus;
                    room.Note = updatedRoom.Note;
                    room.RoomTypeId = updatedRoom.RoomTypeId;

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

        public async Task<List<RoomDTO>> GetRooms()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var roomList = await (from r in context.Rooms
                                          join t in context.RoomTypes
                                          on r.RoomTypeId equals t.RoomTypeId
                                          select new RoomDTO
                                          {
                                              RoomId = r.RoomId,
                                              RoomNumber = r.RoomNumber,
                                              RoomTypeId = r.RoomTypeId,
                                              Note = r.Note,
                                              RoomStatus = r.RoomStatus,
                                              Price = (double)t.Price,
                                          }
                                          ).ToListAsync();
                    return roomList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task ChangeRoomStatus(string roomId)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    Room r = await context.Rooms.FindAsync(roomId);
                    r.RoomStatus = "Phòng đang thuê";
                    await context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<RoomTypeDTO>> GetRoomTypes()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var roomTypeList = await (from r in context.RoomTypes
                                              select new RoomTypeDTO
                                              {
                                                  RoomTypeId = r.RoomTypeId,
                                                  RoomTypeName = r.RoomTypeName.Trim(),
                                                  RoomTypePrice = (double)r.Price,
                                              }
                                          ).ToListAsync();
                    return roomTypeList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
