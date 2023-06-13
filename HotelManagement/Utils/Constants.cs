
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Utils
{
    public enum Operation
    {
        CREATE,
        READ,
        UPDATE,
        DELETE,
        UPDATE_PASSWORD,
        UPDATE_PROD_QUANTITY,
        UPDATECLEAN
    }
   
    public class ROOM_STATUS
    {
        public static readonly string READY = "Phòng trống";
        public static readonly string RENTING = "Phòng đang thuê";
        public static readonly string UNABLE = "Ngưng sử dụng";
    }
   

   
    
    public class HOTEL_INFO
    {
        public static readonly string PHONE = "0123456789";
    }

}
