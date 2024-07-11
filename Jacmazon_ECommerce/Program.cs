using Jacmazon_ECommerce.Interfaces;
using Jacmazon_ECommerce.Models;
using Jacmazon_ECommerce.Models.AdventureWorksLT2016Context;
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
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//��Ʈw
builder.Services.AddDbContext<AdventureWorksLt2016Context>(option =>
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
});
#endregion

builder.Services.AddScoped<ICRUDService<ProductCategory>, CRUDService<ProductCategory>>();
builder.Services.AddScoped<ICRUDService<Product>, CRUDService<Product>>();
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

#region swagger
builder.Services.AddSwaggerGen(options =>
{
    //���Y�y�z
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Jacmazon API",
        Description = "An ASP.NET Core Web API for managing Jacmazon",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    //xml���W�[����
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    //�W�[Token�������
    options.AddSecurityDefinition("Bearer",
    new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization"
    });

    options.AddSecurityRequirement(
       new OpenApiSecurityRequirement
       {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
       });

});
#endregion


//builder.Services.Configure
var app = builder.Build();

//antiforgeryToken
//�N��g�bapi�W���o�Y�i
//app.UseRequestLogging();

// �۩w�qJTW [Authorize] ���q�L��Response
app.UseCustomAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
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
    pattern: "{controller=home}/{action=index}/{id?}"
);

app.Run();
