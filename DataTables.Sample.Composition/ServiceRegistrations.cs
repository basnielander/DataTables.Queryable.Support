using Datatables.Sample.Data.Repositories;
using Datatables.Sample.Domain.Interfaces;
using Datatables.Sample.Domain.Managers;
using DataTables.Sample.Data.Models;
using DataTables.Queryable.Support;
using DataTables.Queryable.Support.Queryables;
using DataTables.Queryable.Support.Queryables.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Datatables.Sample.Composition
{
    public static class ServiceRegistrations
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataTablesContext>(o => o.UseSqlServer(configuration.GetConnectionString("DataTablesDatabase")));

            services.AddScoped<IToDoManager, ToDoManager>();
            services.AddScoped<IToDoRepository, ToDoRepository>();

            //services.AddScoped<IPropertyExpressionCreator, StringContainsExpressionCreator>();
            //services.AddScoped<IPropertyExpressionCreator, IntegerEqualsExpressionCreator>();            
            //services.AddScoped<IPropertyExpressionCreator, BooleanEqualsExpressionCreator>();
            //services.AddScoped<IPropertyExpressionCreator, DateTimeEqualsExpressionCreator>();
            //services.AddScoped<IPropertyExpressionCreator, DecimalEqualsExpressionCreator>();

            services.AddScoped<IDataTablesRequestProcessor, QueryablesProcessor>();
        }
    }
}
