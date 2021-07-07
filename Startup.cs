using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using TodoItems.Repositories;
using TodoItems.Settings;

namespace TodoItems
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
            SetupMongoDB(services);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoItems", Version = "v1" });
            });
        }

        //SetupMongoDB
        private void SetupMongoDB(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            // var mongoDbSettings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSetting>();

            // services.AddSingleton<IMongoClient>(serviceProvider =>
            // {
            //     return new MongoClient(mongoDbSettings.ConnectionString);
            // });

            var mongoDbSettings = new MongoDbSetting();
            Configuration.Bind(nameof(MongoDbSettings), mongoDbSettings);
            services.AddSingleton<IMongoClient>(s =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });


            services.AddSingleton<IItemsRepository, TodoItemRepository>();
        }

        private object MongoDbSettings()
        {
            throw new NotImplementedException();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoItems v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
