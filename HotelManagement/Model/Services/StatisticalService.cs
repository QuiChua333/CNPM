using HotelManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Model.Services
{
    public class StatisticalService
    {
        public StatisticalService() { }
        private static StatisticalService _ins;
        public static StatisticalService Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new StatisticalService();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public  List<string> GetListFilterYear()
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    var listYear =  context.RevenueReports.Select(x => x.MonthReport.Value.Year).ToList();
                    if (listYear == null) listYear = new List<int>();
                    if (!listYear.Contains(DateTime.Now.Year))
                    {
                        listYear.Add(DateTime.Now.Year);
                    }
                    var listYearStr = listYear.Select(x=> "Năm " + x.ToString()).ToList();
                    listYearStr.Reverse();
                    List<string> years = new List<string>();
                    foreach (var year in listYearStr)
                    {
                        if (!years.Contains(year)) years.Add(year);
                    }
                    return years;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RevenueReportDTO GetRevenueReport(string yearstr, string monthstr)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    int year = int.Parse(yearstr.Substring(4));
                    int month = int.Parse(monthstr.Substring(6));
                    int index = 1;
                    var revenueReport =  context.RevenueReports.FirstOrDefault(x=> x.MonthReport.Value.Year== year && x.MonthReport.Value.Month == month);
                    if (revenueReport != null)
                    {
                        RevenueReportDTO revenueReportDTO = new RevenueReportDTO
                        {
                            RevenueReportId = revenueReport.RevenueReportId,
                            MonthReport = (DateTime)revenueReport.MonthReport,
                            TotalRevenue = (double)revenueReport.RevenueReportDetails.Sum(x => x.Revenue),
                            revenueReportDetailDTOs = revenueReport.RevenueReportDetails.Select(x => new RevenueReportDetailDTO
                            {
                                STT = index++,
                                RevenueReportId = revenueReport.RevenueReportId,
                                RoomTypeId = x.RoomTypeId,
                                Revenue = x.Revenue ?? 0,
                                RoomTypeName = x.RoomType.RoomTypeName,
                                Ratio = x.Ratio ?? 0,
                            }).ToList(),
                        };
                        return revenueReportDTO;
                    }
                    
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

    }
}
