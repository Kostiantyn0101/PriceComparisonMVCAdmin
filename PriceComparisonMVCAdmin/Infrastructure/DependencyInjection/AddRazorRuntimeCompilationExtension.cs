namespace PriceComparisonMVCAdmin.Infrastructure.DependencyInjection
{
    public static class AddRazorRuntimeCompilationExtension
    {
        public static void AddRazorRuntimeCompilation(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        }
    }
}
