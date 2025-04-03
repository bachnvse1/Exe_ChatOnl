using EXEChatOnl.Models;
using EXEChatOnl.Repository;
using EXEChatOnl.Service;
using EXEChatOnl.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Container.DependencyInjection.Mapper;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Container.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection InfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
            services.AddControllers().AddXmlSerializerFormatters()
                .AddOData(options =>
            {
                options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
            });

    
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Category>("Categories");
            return builder.GetEdmModel();
        }
    }
}
