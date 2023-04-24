using MySampleFucntion.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
 
using System;
using MySampleFucntion.Services;
using Newtonsoft.Json;

[assembly: FunctionsStartup(typeof(MySampleFucntion.Startup))]

namespace MySampleFucntion
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<CompanyContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));
            builder.Services.AddScoped<IServices<Department,int>,DepartmentService>();
            
        }
    }
}
