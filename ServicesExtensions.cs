using Microsoft.Extensions.DependencyInjection;
using Shopping_API.Repositories.IRepositories;
using Shopping_API.Repositories.Repositories;
using Shopping_API.Services.IServices;
using Shopping_API.Services.Services;

namespace Shopping_API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddCustomRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRedisRepository, RedisRepository>();
            
        }
    }
}