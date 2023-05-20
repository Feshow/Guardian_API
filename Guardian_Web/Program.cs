using Guardian_Web;
using Guardian_Web.Services;
using Guardian_Web.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));

//Register Http cliente on service and then register the service
builder.Services.AddHttpClient<IGuardianService, GuardianService>();
builder.Services.AddScoped<IGuardianService, GuardianService>(); //Registering GuardianService to dependency injection --> It will have one object of service even if it is required many time, it will use the same object

builder.Services.AddHttpClient<IGuardianTaskService, GuardianTaskService>();
builder.Services.AddScoped<IGuardianTaskService, GuardianTaskService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //To access http informetion in _Layout.cshtml

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Momory cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout= TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
