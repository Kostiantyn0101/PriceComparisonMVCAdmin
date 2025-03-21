namespace PriceComparisonMVCAdmin.Infrastructure.DependencyInjection
{
    public static class AddAutoMapperExtension
    {
        public static void AddAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));
        }
    }
}
