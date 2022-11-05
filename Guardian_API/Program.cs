using Guardian_API.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Configuring and applying SeriLog (File Log)
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/GuardianLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();

builder.Services.AddControllers(option =>
{
    //return has to be acceptable by our API
    //option.ReturnHttpNotAcceptable = true
;                       //Make possible to accept XML file at API return
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add costom service to the container. In this case it is a log
builder.Services.AddSingleton<ILogging, Logging>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
