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
    public class CustomerTypeService
    {
        public CustomerTypeService() { }
        private static CustomerTypeService _ins;
        public static CustomerTypeService Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new CustomerTypeService();
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<List<CustomerTypeDTO>> GetAllCustomerType()
        {
            try
            {
                using (HotelManagementNMCNPMEntities db = new HotelManagementNMCNPMEntities())
                {
                    List<CustomerTypeDTO> CustomerTypeDTOs = await (
                        from ct in db.CustomerTypes
                        select new CustomerTypeDTO
                        {
                            // DTO = db
                            CustomerTypeId = ct.CustomerTypeId,
                            CustomerTypeName = ct.CustomerTypeName,
                            CoefficientSurcharge = (double)ct.CoefficientSurcharge,
                        }
                    ).ToListAsync();
                 
                    return CustomerTypeDTOs;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string CreateNextCustomerTypeCode(string maxCode)
        {
            if (maxCode == "")
            {
                return "LK001";
            }
            int index = (int.Parse(maxCode.Substring(2)) + 1);
            string CodeID = index.ToString();
            while (CodeID.Length < 3) CodeID = "0" + CodeID;

            return "LK" + CodeID;
        }
        public async Task<(bool, string, CustomerTypeDTO)> AddCustomerType(CustomerTypeDTO newCustomerType)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    CustomerType ct = context.CustomerTypes.Where((CustomerType CustomerType) => CustomerType.CustomerTypeId == newCustomerType.CustomerTypeId).FirstOrDefault();

                    if (ct != null)
                    {
                        return (false, $"Loại phòng {ct.CustomerTypeName} đã tồn tại!", null);
                    }
                    else
                    {
                        var listid = await context.CustomerTypes.Select(s => s.CustomerTypeId).ToListAsync();
                        string maxId = "";

                        if (listid.Count > 0)
                            maxId = listid[listid.Count - 1];

                        string id = CreateNextCustomerTypeCode(maxId);
                        CustomerType customertype = new CustomerType
                        {
                            CustomerTypeId = id,
                            CustomerTypeName = newCustomerType.CustomerTypeName,
                            CoefficientSurcharge = newCustomerType.CoefficientSurcharge,
                        };
                        context.CustomerTypes.Add(customertype);
                        await context.SaveChangesAsync();
                        newCustomerType.CustomerTypeId = customertype.CustomerTypeId;
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
            return (true, "Thêm loại khách thành công", newCustomerType);
        }

        public async Task<(bool, string)> UpdateCustomerType(CustomerTypeDTO updatedCustomerType)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    CustomerType customertype = context.CustomerTypes.Find(updatedCustomerType.CustomerTypeId);

                    if (customertype is null)
                    {
                        return (false, "Loại khách này không tồn tại!");
                    }
                    customertype.CustomerTypeId = updatedCustomerType.CustomerTypeId;
                    customertype.CustomerTypeName = updatedCustomerType.CustomerTypeName;
                    customertype.CoefficientSurcharge = updatedCustomerType.CoefficientSurcharge;

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

        public async Task<(bool, string)> DeleteCustomerType(string Id)
        {
            try
            {
                using (var context = new HotelManagementNMCNPMEntities())
                {
                    RentalContractDetail rcd = await (from p in context.RentalContractDetails
                                       where p.CustomerTypeId == Id
                                       select p).FirstOrDefaultAsync();
                    CustomerType ct = await (from p in context.CustomerTypes
                                               where p.CustomerTypeId == Id
                                               select p).FirstOrDefaultAsync();

                    if (rcd is null)
                    {
                        if (ct is null)
                        {
                            return (false, "Không tìm thấy loại khách này !");
                        }
                        else
                        {
                            context.CustomerTypes.Remove(ct);
                            await context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        return (false, "Loại khách này đang được áp dụng và đang nằm trong phiếu thuê. Không thể xóa!");
                    }
                }
            }
            catch (Exception)
            {
                return (false, "Lỗi hệ thống");
            }
            return (true, "Xóa loại khách thành công");
        }
    }
}
