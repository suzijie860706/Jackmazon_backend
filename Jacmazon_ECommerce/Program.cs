using Jacmazon_ECommerce.Interfaces;
using Jacmazon_ECommerce.Models;
using Jacmazon_ECommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Jacmazon_ECommerce.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Jacmazon_ECommerce.JWTServices;
using Microsoft.IdentityModel.Tokens;
using Jacmazon_ECommerce.��iddlewares;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//��Ʈw
builder.Services.AddDbContext<AdventureWorksLt2019Context>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorksLT2016")));
builder.Services.AddDbContext<LoginContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("Login")));

builder.Services.AddScoped<ICRUDRepository<ProductCategory>, ProductCategoryRepository>();
builder.Services.AddScoped<ICRUDRepository<Product>, ProductRepository>();

#region JWT�]�w
var key = Encoding.UTF8.GetBytes(Settings.Secret);
//�[�J����
builder.Services.AddAuthentication(x =>
{
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //�w�]���Ҥ�� JWT ���
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //��Y�ӽШD���q�L���ҮɡA�t�η|�ͦ��@��Challenge�A�o�Ӥ�״N�O�ΥH�B�z�o��Challenge
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    // �����ҥ��ѮɡA�^�����Y�|�]�t WWW-Authenticate ���Y�A�o�̷|��ܥ��Ѫ��Բӿ��~��]
    x.IncludeErrorDetails = true; // �w�]�Ȭ� true�A���ɷ|�S�O����

    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        //��٦w�����_
        IssuerSigningKey = new SymmetricSecurityKey(key),
        //�ݭn�קﰵ���ҡA�קK����H�i�Ӵ���Token
        ValidateIssuer = false, //�O�_���ҽֵ֮o��?
        ValidateAudience = false, //���ǫȤ�i�H�ϥ�? 
        ValidIssuer = Settings.Issuer
    };
}).
AddCookie(option =>
{
    option.AccessDeniedPath = "/Shop/AccessDeny"; //�ڵ�
    option.LoginPath = "/Shop/Login"; //�n�J��
}); ;
#endregion

builder.Services.AddScoped<ICRUDService<ProductCategory>,CRUDService<ProductCategory>>();
builder.Services.AddScoped<ICRUDService<Product>,CRUDService<Product>>();
//�^���֨�
builder.Services.AddResponseCaching();

//�s�WCORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30",
        new CacheProfile()
        {
            Duration = 30
        });
});

// �K�[ Antiforgery �A��
builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

//builder.Services.Configure
var app = builder.Build();

//antiforgeryToken
//�N��g�bapi�W���o�Y�i
//app.UseRequestLogging();

// �۩w�qJTW [Authorize] ���q�L��Response
app.UseCustomAuthorization();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//�[�JCORS
app.UseCors("AllowAllOrigins");

//�^���֨�
app.UseResponseCaching();
//�[�J����
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Shop}/{action=login}/{id?}");

app.Run();
