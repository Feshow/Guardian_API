using Guardian.Domain.Interfaces.IRepository.Guardian;
using Guardian.Domain.Interfaces.IRepository.TaskGuardian;
using Guardian.Data;
using Guardian.Data.Repository.Guardian;
using Guardian.Data.Repository.GuardianTask;
using Microsoft.EntityFrameworkCore;
using Guardian.Domain.Interfaces.IRepository.User;
using Guardian.Data.Repository.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Guardian.Domain.Models;

#region Containers / Builder
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>() //Default identity user (It is a identity library provided by .NET/ Add a few tables relatated to identity in database)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddResponseCaching(); //Enable caching for API requests
builder.Services.AddScoped<IGuardianRepository, GuardianRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGuardianTaskRepository, GuardianTaskRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Adding versioning for API
builder.Services.AddApiVersioning(options => 
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions= true; //Response header will show the supported API versions (Swagger documentation)
});

//When we have differents endpoint versions it is necessary to specify which one we want to use (Check for [MapToApiVersion("")] on controler) 
builder.Services.AddVersionedApiExplorer(options => 
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true; //The version is auto chosed in url
});

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
//Config Authentication for API
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("Default30", //Adding a default config for chaching (any controller can use it)
        new CacheProfile()
        {
            Duration = 30
        });
    //option.ReturnHttpNotAcceptable = true
;                       
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    //Adding security definition for swagger (name is bearer)
    //Describes how the API is protected throught the generated swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 1234abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "ouuth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
    //Adding differents versions on documentation
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Guardian",
        Description = "Guardian API to manage tasks",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Felippe Delesporte",
            Url = new Uri("https://www.linkedin.com/in/felippe-delesporte/")
        },
        License= new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Guardian v2",
        Description = "Guardian API to manage tasks",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Felippe Delesporte",
            Url = new Uri("https://www.linkedin.com/in/felippe-delesporte/")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

});

var app = builder.Build();
#endregion

#region Swagger
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //Adding differents versions on documentation
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Guadian_v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Guadian_v2");
    });
}
#endregion

app.UseHttpsRedirection();

app.UseAuthentication(); //Add for API Authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
