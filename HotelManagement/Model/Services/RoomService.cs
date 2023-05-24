using HotelManagement.DTOs;
using HotelManagement.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

                    var list = await context.RoomTypes.Select(x => new ListRoomTypeDTO
                    {   
                        RoomTypeName= x.RoomTypeName,
                        RoomTypeId= x.RoomTypeId,
                        Price= (double)x.Price,
                        Rooms = x.Rooms.Select(t=> new RoomDTO
                        {
                            RoomId= t.RoomId,
                            RoomTypeId= x.RoomTypeId,
                            RoomTypeName = x.RoomTypeName,
                            RentalContractId = t.RentalContracts.FirstOrDefault(m=> m.Room.RoomStatus == ROOM_STATUS.RENTING).RentalContractId,
                            RoomStatus= t.RoomStatus,
                            RoomNumber= t.RoomNumber,
                            Note= t.Note,
                            StartDate = t.RentalContracts.FirstOrDefault(m => m.Room.RoomStatus == ROOM_STATUS.RENTING).CreateDate ,
                            NumberOfPerson = t.RentalContracts.FirstOrDefault(m => m.Room.RoomStatus == ROOM_STATUS.RENTING).RentalContractDetails.Count(),
                        }).ToList(),
                    }).ToListAsync();
                    return list;
                }
            }
           catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
