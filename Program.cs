using Microsoft.EntityFrameworkCore;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);

//6.1.4 在Program.cs加入使用appsettings.json中的連線字串程式碼(這段必須寫在var builder這行後面)
builder.Services.AddDbContext<dbHouseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbHouseConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //超過10分鐘未動作，自動清除session(自動登出)
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//4.2.7 在Program.cs中註冊及啟用Session
//註冊session
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
