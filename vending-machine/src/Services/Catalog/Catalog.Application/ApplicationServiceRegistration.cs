using Application.EventDispatcher;
using Domain.Events;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Vending.Application.Behaviours;

namespace Vending.Application
{
    /// <summary>
    /// Class that centralizes dependency injection management for the Application layer. Single Responsability
    /// </summary>
    public static class ApplicationServiceRegistration
    {
        /// <summary>
        /// Extension method to register all injection dependencies
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
