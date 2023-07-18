using AmlApiMvc.Interfaces;
using AmlApiMvc.Services;

namespace AmlApiMvc.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddAmlService(this IServiceCollection services)
        {
            return services.AddScoped<IAmlService, AmlService>();
        }
    }
}
