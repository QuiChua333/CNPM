using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    }
}
