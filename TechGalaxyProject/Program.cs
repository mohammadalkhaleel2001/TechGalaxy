using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechGalaxyProject.Data;
using TechGalaxyProject.Data.Models;
using TechGalaxyProject.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 🔹 إعداد الاتصال بقاعدة البيانات
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("myCon")));

// 🔹 إعداد الهوية (Identity)
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

// 🔹 إعداد CORS ليقبل أي Origin (مناسب لجافا سكريبت عادي أو ملف HTML مفتوح مباشرة)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 🔹 إعداد Controllers و JSON
builder.Services.AddControllers()
                .AddNewtonsoftJson();

// 🔹 إعداد Swagger مع دعم JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth(); // ← موجود في ملف Extentions
builder.Services.AddCustomJwtAuth(builder.Configuration); // ← إعدادات JWT

var app = builder.Build();

// 🔹 تفعيل Swagger فقط في بيئة التطوير
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔹 تفعيل CORS قبل Middleware المصادقة
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
