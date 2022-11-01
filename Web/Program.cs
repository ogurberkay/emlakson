using Business.Service.Abstract;
using Business.Service.Concrete;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureUnitOfWork();

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();  
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();