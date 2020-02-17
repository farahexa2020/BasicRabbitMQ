using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MicroRabbit.transfer.Application.Intefaces;
using MicroRabbit.transfer.Application.Services;
using MicroRabbit.Transfer.Domain.Interface;
using MicroRabbit.Transfer.Data.Repository;
using static MicroRabbit.Domain.Core.Bus.IEventHandler;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Transfer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TransferDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TransferDbConnection"));
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer Microservices", Version = "v1" });
            });

            services.AddMediatR(typeof(Startup));

            RegisterServices(services);

            // Subscriptions
            services.AddTransient<TransferEventHandler>();

            // Domain Events
            services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            // Application Services
            services.AddTransient<ITransferService, TransferService>();

            // Data
            services.AddTransient<ITransferRepository, TransferRepository>();
            services.AddTransient<TransferDbContext>();
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependenceContainer.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservices v1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ConfigurEventBus(app);
        }

        private void ConfigurEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
        }
    }
}
