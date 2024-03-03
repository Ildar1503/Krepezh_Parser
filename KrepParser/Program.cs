using KrepParser.Application;
using KrepParser.Infrastruction;

var builder = WebApplication.CreateBuilder(args);

#region Добавление сервисов.

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();

#endregion

var app = builder.Build();

app.UseStaticFiles();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomeIndex}");

app.Run();
