using Microsoft.EntityFrameworkCore;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);

//6.1.4 �bProgram.cs�[�J�ϥ�appsettings.json�����s�u�r��{���X(�o�q�����g�bvar builder�o��᭱)
builder.Services.AddDbContext<dbHouseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("dbHouseConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//4.2.7 �bProgram.cs�����U�αҥ�Session
//���Usession
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    //�W�L10�������ʧ@�A�۰ʲM��session(�۰ʵn�X)
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

//4.2.7 �bProgram.cs�����U�αҥ�Session
//���Usession
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
