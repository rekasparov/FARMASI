using FARMASI.BusinessLayer.Abstract;
using FARMASI.BusinessLayer.Concrete;
using FARMASI.Common.BaseSettings.RabbitMQ;
using FARMASI.Common.BaseSettings.Redis;
using FARMASI.WebAPI.Consumers;
using FARMASI.WebAPI.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FARMASI.WebAPI
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
            services.AddCors(setupAction =>
            {
                setupAction.AddDefaultPolicy(policy =>
                {
                    policy
                        .WithOrigins("http://localhost:21001", "http://localhost:21002")
                        .AllowAnyHeader()
                        .WithMethods("GET", "POST", "PUT", "DELETE")
                        .AllowCredentials();
                });
            });

            services.AddMongoDBSettings(Configuration);
            services.AddRedisSettings(Configuration);

            services.AddScoped<IProductBL, ProductBL>();
            services.AddScoped<IShoppingCartBL, ShoppingCartBL>();

            services.AddMassTransit(configure =>
            {
                configure.AddConsumer<DecreaseStockConsumer>();
                configure.AddConsumer<StockNotAvailableConsumer>();
                configure.AddConsumer<AddToCartConsumer>();
                configure.AddConsumer<AddedToCartConsumer>();
                configure.AddConsumer<DeleteFromCartConsumer>();
                configure.AddConsumer<DeletedFromCartConsumer>();

                configure.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(Configuration.GetSection($"{typeof(RabbitMQSettings).Name}:{nameof(RabbitMQSettings.ConnectionString)}").Value));

                    configurator.ReceiveEndpoint(RabbitMQQueueNames.DecreaseStockRequestMessageQueueName, configuration =>
                    {
                        configuration.ConfigureConsumer<DecreaseStockConsumer>(context);
                    });

                    configurator.ReceiveEndpoint(RabbitMQQueueNames.StockNotAvailableResponseMessageQueueName, configuration =>
                    {
                        configuration.ConfigureConsumer<StockNotAvailableConsumer>(context);
                    });

                    configurator.ReceiveEndpoint(RabbitMQQueueNames.AddToCartRequestMessageQueueName, configuration =>
                    {
                        configuration.ConfigureConsumer<AddToCartConsumer>(context);
                    });

                    configurator.ReceiveEndpoint(RabbitMQQueueNames.AddedToCartResponseMessageQueueName, configuration =>
                    {
                        configuration.ConfigureConsumer<AddedToCartConsumer>(context);
                    });

                    configurator.ReceiveEndpoint(RabbitMQQueueNames.DeleteFromCartRequestMessageQueueName, configuration =>
                    {
                        configuration.ConfigureConsumer<DeleteFromCartConsumer>(context);
                    });

                    configurator.ReceiveEndpoint(RabbitMQQueueNames.DeletedFromCartResponseMessageQueueName, configuration =>
                    {
                        configuration.ConfigureConsumer<DeletedFromCartConsumer>(context);
                    });
                });
            });

            services.AddSignalR();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}
