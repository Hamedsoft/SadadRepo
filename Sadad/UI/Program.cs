using Autofac;
using Autofac.Extensions.DependencyInjection;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
var builder = WebApplication.CreateBuilder(args);

// تغییر IoC به Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    // ثبت HttpClient به عنوان Singleton
    builder.Register(c =>
    {
        var client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7064/api/")
        };
        return client;
    }).SingleInstance();
});

// ثبت سرویس‌های MVC
builder.Services.AddControllersWithViews();

// toset
builder.Services.AddNotyf(config => { config.DurationInSeconds = 3; config.IsDismissable = true; config.Position = NotyfPosition.BottomLeft; });

var app = builder.Build();

// تنظیمات مربوط به درخواست‌های HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseNotyf();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Products}/{id?}");

app.Run();