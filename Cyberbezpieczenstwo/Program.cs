using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Cyberbezpieczenstwo.Areas.Identity;
using Cyberbezpieczenstwo.Data;
using Cyberbezpieczenstwo;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Cyberbezpieczenstwo.Authentication;
using Cyberbezpieczenstwo.Data.Interfaces;
using Cyberbezpieczenstwo.Data.Services;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Ardalis.ListStartupServices;
using Cyberbezpieczenstwo.SharedKernel;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddDbContext();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<HashProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddAuthenticationCore();

builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton<UserController>();
builder.Services.AddSingleton<LogHistoryController>();
builder.Services.AddSingleton<PasswordLimitationController>();
builder.Services.AddSingleton<SecuritySettingsController>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddTransient<GooglereCaptchaService>();

builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);
    config.Path = "/listservices";
});
builder.Services.AddControllersWithViews().AddNewtonsoftJson();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.EnableAnnotations();
});
builder.Services.AddHttpClient("api", cfg =>
{
    cfg.BaseAddress = new Uri("http://localhost:28334/");
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseShowAllServicesMiddleware();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
builder.Services.AddHttpClient();


app.Run();

