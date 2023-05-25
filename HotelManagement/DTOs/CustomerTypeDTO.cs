﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DTOs
{
    public class CustomerTypeDTO
    {
        public string CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public double CoefficientSurcharge { get; set; }
        public string CoefficientSurchargeStr
        {
            get
            {
                return Convert.ToString(CoefficientSurcharge);
            }
        }

    }
}
