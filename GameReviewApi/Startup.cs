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
            services.AddDbContext<ReviewContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("A server error happened. Please try again later!");
                    });
                });
            }

            //Add a seed menthod for data

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Review, Models.ReviewDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                $"{src.Author}"))
                .ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src =>
                src.DatePosted.ToString("dd/MM/yyyy")));

                cfg.CreateMap<Entities.Game, Models.GameDto>()
                .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate.ToShortDateString()));

                cfg.CreateMap<Entities.Comment, Models.CommentDto>()
                .ForMember(dest => dest.DatePosted, opt => opt.MapFrom(src => src.DatePosted.ToShortDateString()));

                cfg.CreateMap<Entities.Game, Models.GameDto>();

                cfg.CreateMap<Models.ReviewForCreationDto, Entities.Review>();

                cfg.CreateMap<Models.ReviewForCreationDto, Entities.Game>();

                cfg.CreateMap<Models.GameForUpdateDto, Entities.Game>();
            });

            app.UseMvc();
        }
    }
}
