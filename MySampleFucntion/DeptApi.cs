using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MySampleFucntion.Models;
using MySampleFucntion.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MySampleFucntion
{
    public class DeptApi
    {
        private readonly ILogger _logger;
        private IServices<Department, int> deptServ;
        private ResponseObject<Department> response;    

        public DeptApi(ILoggerFactory loggerFactory, IServices<Department, int> serv)
        {
            _logger = loggerFactory.CreateLogger<DeptApi>();
            deptServ = serv;
            response = new ResponseObject<Department>();
        }   

        [FunctionName("DeptApi")]
        public  async Task<ResponseObject<Department>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
           
             



            switch (req.Method)
            {
                case "GET":
                    response = await deptServ.GetAsync();
                   
                    break;
                case "POST":
                    var postbody = new StreamReader(req.Body).ReadToEnd();
                    var newdept = JsonSerializer.Deserialize<Department>(postbody);
                    response = await deptServ.CreateAsync(newdept);
                   
                    break;
                case "PUT":
                    var id = Convert.ToInt32(req.Query["id"]);
                    var putbody = new StreamReader(req.Body).ReadToEnd();
                    var depttoupdate = JsonSerializer.Deserialize<Department>(putbody);
                    response = await deptServ.UpdateAsync(id, depttoupdate);
                     
                    break;
                case "DELETE":
                     id = Convert.ToInt32( req.Query["id"]);
                    response = await deptServ.DeleteAsync(id);
                    break;
                default:
                    break;
            }
            return response;
            
        }
    }
}
