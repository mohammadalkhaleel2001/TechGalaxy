using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechGalaxyProject.Data;
using TechGalaxyProject.Data.Models;
using TechGalaxyProject.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechGalaxyProject.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("myCon")));


builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNetlifyApp", policy =>
    {
        policy.WithOrigins("https://68311f7890065681810ad943--steady-rugelach-10f3fa.netlify.app")

              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


builder.Services.AddControllers()
                .AddNewtonsoftJson();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenJwtAuth();
builder.Services.AddCustomJwtAuth(builder.Configuration);
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowNetlifyApp");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
