using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SalesMine.Core.Mediator;
using SalesMine.Customers.API.Application.Commands;
using SalesMine.Customers.API.Application.Events;
using SalesMine.Customers.API.Data;
using SalesMine.Customers.API.Data.Repository;
using SalesMine.Customers.API.Models;

namespace SalesMine.Customers.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<AddCustomerCommand, ValidationResult>, CustomerCommandHandler>();
            services.AddScoped<INotificationHandler<RegisteredCustomerEvent>, CustomerEventHandler>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<CustomerContext>();
            return services;
        }
    }
}
