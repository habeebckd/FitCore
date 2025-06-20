using Application.Interface;
using Application.Interface.Reppo.Attantance;
using Application.Interface.Reppo.membership;
using Application.Interface.Reppo.Rating;
using Application.Interface.Reppo.Trainer;

//using Application.Interface.Reppo.Trainer;
using Application.Interface.Reppo.Workout;
using Application.Interface.Serv.Attantance;
using Application.Interface.Serv.membership;
using Application.Interface.Serv.Rating;
using Application.Interface.Serv.Trainer;

//using Application.Interface.Serv.Trainer;
using Application.Interface.Serv.Workout;
using Application.Services;
using AutoMapper;
using E_Commerce.CustomMiddleweare;
using infrastructure.Context;
using infrastructure.Mapper;
using infrastructure.SignalR;
using infrastructure.Repository;
using infrastructure.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Application.Interface.Reppo.NotificationRepo;
using Application.Interface.Serv.Notifications;

namespace FitCore_Manager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService,AuthService>();

            builder.Services.AddScoped<IMembershipPlanRepository, MembershipPlanRepository>();
            builder.Services.AddScoped<IAdminMembershipService, AdminMembershipService>();

            builder.Services.AddScoped < IUserMembershipRepository,UserMembershipRepository>();
            builder.Services.AddScoped<IUserMembershipService,UserMembershipService>();

            builder.Services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
            builder.Services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();

            builder.Services.AddScoped<IUserWorkoutPlanRepository,UserWorkoutPlanRepository>();
            builder.Services.AddScoped<IUserWorkoutPlanService,UserWorkoutPlanService>();

            builder.Services.AddScoped<IWorkoutPlanDayDetailsRepository,WorkoutPlanDayDetailsRepository>();
            builder.Services.AddScoped<IWorkoutPlanDayDetailsService,WorkoutPlanDayDetailsService>();

            //builder.Services.AddScoped<ITrainerBookingRepository,TrainerBookingRepository>();
            //builder.Services.AddScoped<ITrainerBookingService,TrainerBookingService>();
            builder.Services.AddScoped<ITrainerBookingRepository,TrainerBookingRepository>();
            builder.Services.AddScoped<ITrainerBookingService,TrainerBookingService>();

            builder.Services.AddScoped<ITrainerPunchRepository,TrainerPunchRepository>();
            builder.Services.AddScoped<ITrainerPunchService,TrainerPunchService>();

            builder.Services.AddScoped<IGymRatingRepository,GymRatingRepository>();
            builder.Services.AddScoped<IGymRatingService, GymRatingService>();

            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            builder.Services.AddScoped<ICloudinaryServices, CloudinaryService>();

            builder.Services.AddAutoMapper(typeof(ProfileMapper));

            builder.Services.AddSignalR();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            // Swagger Configuration
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your JWT token."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Add CORS Policy (optional)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            // JWT Authentication Configuration 
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
                o.RequireHttpsMetadata = false;  // Use true in production for security
                o.SaveToken = true;
            });



            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
                options.AddPolicy("TrainerOnly", policy => policy.RequireRole("Trainer"));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication(); // Added for JWT
            app.UseAuthorization();
            app.UseMiddleware<UserIdMiddleware>();
            app.MapHub<NotificationHub>("/notificationHub");

            app.MapControllers();

            app.Run();
        }
    }
}
