using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MySampleFucntion.Models;
using MySampleFucntion.Services;
using System.Text.Json;

namespace MySampleFucntion
{
    public  class RestAPI
    {
        private readonly CompanyContext _context = new CompanyContext();
        ClsMessaging messaging = new ClsMessaging();
        //public RestAPI(CompanyContext context)
        //{
        //    _context = context;
        //}

        [FunctionName("Get")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Departments")] HttpRequest req,
            ILogger log)
        {

            try
            {
                // check for the querystring count for keys
                if (req.Query.Keys.Count > 0)
                {
                    // read the 'id' value from the querystring
                    int id = Convert.ToInt32(req.Query["id"]);
                    if (id > 0)
                    {
                        // read data based in 'id'
                        Department Departments = new Department();
                        Departments = await _context.Departments.FindAsync(id);
                        return new OkObjectResult(Departments);

                    }
                    else
                    {
                        // return all records
                        List<Department> Departmentss = new List<Department>();
                        Departmentss = await _context.Departments.ToListAsync();
                        return new OkObjectResult(Departmentss);
                    }
                }
                else
                {
                    List<Department> Departmentss = new List<Department>();
                    Departmentss = await _context.Departments.ToListAsync();
                    return new OkObjectResult(Departmentss);
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult(ex.Message);
            }


        }

        [FunctionName("Post")]
        public async Task<IActionResult> Post(
           [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Departments")] HttpRequest req,
           ILogger log)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Department Department = JsonConvert.DeserializeObject<Department>(requestBody);
                var dept = await _context.Departments.AddAsync(Department);
                await _context.SaveChangesAsync();
                // Add the Data in Queue
                await messaging.AddMessageAsync(System.Text.Json.JsonSerializer.Serialize(Department));
                return new OkObjectResult(dept.Entity);
            }
            catch (Exception ex)
            {
                return new OkObjectResult($"{ex.Message} {ex.InnerException}");
            }
        }

        [FunctionName("Put")]
        public async Task<IActionResult> Put(
          [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Departmentss/{id:int}")] HttpRequest req, int id,
          ILogger log)
        {
            try
            {

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Department Departments = JsonConvert.DeserializeObject<Department>(requestBody);
                if (Departments.DeptNo == id)
                {
                    _context.Entry<Department>(Departments).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return new OkObjectResult(Departments);
                }
                else
                {
                    return new OkObjectResult($"Record is not found against the Departments Row Id as {id}");
                }

            }
            catch (Exception ex)
            {
                return new OkObjectResult($"{ex.Message} {ex.InnerException}");
            }
        }

        [FunctionName("Delete")]
        public async Task<IActionResult> Delete(
          [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Departmentss/{id:int}")] HttpRequest req, int id,
          ILogger log)
        {
            try
            {
                var prd = await _context.Departments.FindAsync(id);
                if (prd == null)
                {
                    return new OkObjectResult($"Record is not found against the Departments Row Id as {id}");
                }
                else
                {
                    _context.Departments.Remove(prd);
                    await _context.SaveChangesAsync();
                    return new OkObjectResult($"Record deleted successfully based on Departments Row Id {id}");
                }
            }
            catch (Exception ex)
            {
                return new OkObjectResult($"{ex.Message} {ex.InnerException}");
            }
        }
    }
}
