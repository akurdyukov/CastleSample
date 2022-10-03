using System.Collections.Generic;
using Castle.Windsor;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.Mapping.Attributes;
using NHibernate.NetCore;
using Steeltoe.Connector;
using Steeltoe.Connector.PostgreSql;
using Steeltoe.Connector.Services;
using Steeltoe.Management.Endpoint;
using Environment = NHibernate.Cfg.Environment;

namespace CastleSample
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
            services.AddPostgresConnection(Configuration);
            services.AddPostgresHealthContributor(Configuration);
            services.AddAllActuators(Configuration);
            services.ActivateActuatorEndpoints();
            services.AddControllers()
                .AddControllersAsServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "CastleSample", Version = "v1"});
            });

            // datatabase stuff
            services.AddHibernate(BuildHibernateConfig());
            var postgresConfig = new PostgresProviderConnectorOptions(Configuration);
            var connString = postgresConfig.ToString();
            services.AddFluentMigratorCore()
                .ConfigureRunner(b =>
                {
                    b.AddPostgres()
                        .ScanIn(typeof(Startup).Assembly).For.Migrations()
                        .WithGlobalConnectionString(connString);
                })
                .AddLogging(c => c.AddFluentMigratorConsole());
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(IWindsorContainer container)
        {
            container.Install(new LocalInstaller());
            
            // inject NHibernate-specific object factory
            Environment.ObjectsFactory = new WindsorObjectsFactory(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CastleSample"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.Migrate();
        }
        
        private static Configuration BuildHibernateConfig()
        {
            // register NHibernate things
            var configProperties = new Dictionary<string, string>
            {
                {Environment.ConnectionDriver, typeof(NpgsqlDriver).AssemblyQualifiedName},
                {Environment.Dialect, typeof (NHibernate.Dialect.PostgreSQLDialect).FullName},
                {Environment.ConnectionProvider, typeof(InjectedConnectionProvider).AssemblyQualifiedName},
                {"cache.use_second_level_cache", "true"},
                {"cache.use_query_cache", "true"},
                {"expiration", "86400"}, // = 1 day
            };
        
            var serializer = HbmSerializer.Default;
            serializer.Validate = true;

            var config = new Configuration()
                //.SetNamingStrategy(new PostgresNamingStrategy())
                .SetProperties(configProperties)
                .AddInputStream(serializer.Serialize(typeof(LocalInstaller).Assembly));
            return config;
        }
    }

    public static class MigrationExtension
    {
        public static IApplicationBuilder Migrate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var runner = scope.ServiceProvider.GetService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
            return app;
        }
    }
}