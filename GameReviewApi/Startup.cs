﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using GameReviewApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using GameReviewApi.Repositories;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using GameReviewApi.Services;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Marvin.Cache.Headers;

namespace GameReviewApi
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
            //services.AddDbContext<ReviewContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(setupAction => 
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());

                var jsonOutputFormatter = setupAction.OutputFormatters.OfType<JsonOutputFormatter>().FirstOrDefault();

                if (jsonOutputFormatter != null)
                {
                    jsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.gamesxtime.hateoas+json");
                }
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            });

            services.AddDbContext<ReviewContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory => 
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddTransient<IPropertyMappingService, PropertyMappingService>();

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddHttpCacheHeaders(
                (expirationModelOptions)
                =>
                {
                    expirationModelOptions.MaxAge = 600;
                },
                (validationModelOptions)
                =>
                {
                    validationModelOptions.AddMustRevalidate = true;
                });

            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
             ILoggerFactory loggerFactory, ReviewContext reviewContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, 
                                exceptionHandlerFeature.Error,
                                exceptionHandlerFeature.Error.Message);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("A unexpected fault happened. Please try again later!");
                    });
                });
            }

            //Add a seed menthod for data

            //reviewContext.EnsureSeedDataForContext();
            //ReviewContextExtensions.EnsureSeedDataForContext(reviewContext);

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Review, Models.ReviewDto>()
                .ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src =>
                src.DatePosted.ToShortDateString()));

                cfg.CreateMap<Entities.Game, Models.GameDto>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src =>
                src.ReleaseDate.ToShortDateString()));

                cfg.CreateMap<Entities.Comment, Models.CommentDto>()
                .ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src =>
                src.DatePosted.ToShortDateString()));

                cfg.CreateMap<Models.ReviewForCreationDto, Entities.Review>();

                cfg.CreateMap<Models.ReviewForUpdateDto, Entities.Review>();

                cfg.CreateMap<Models.GameForCreationDto, Entities.Game>();                

                cfg.CreateMap<Models.GameForUpdateDto, Entities.Game>();

                cfg.CreateMap<Entities.Game, Models.GameForUpdateDto>();

                cfg.CreateMap<Models.CommentForCreationDto, Entities.Comment>();

                cfg.CreateMap<Models.CommentForUpdateDto, Entities.Comment>();

                cfg.CreateMap<Entities.Comment, Models.CommentForUpdateDto>();

            });

            //reviewContext.EnsureSeedDataForContext();

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseMvc();
        }
    }
}
