using HotelManagement.DTOs;
using HotelManagement.Utils;
using System;
using System.Collections.Generic;
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
                {
                    _ins = new RoomTypeService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<string>> GetListRoomTypName()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {

                    var list =   context.RoomTypes.Select(x => x.RoomTypeName).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
