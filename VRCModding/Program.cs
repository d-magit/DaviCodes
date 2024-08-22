using System.Configuration;
using VRCModding.Business.Repositories;
using VRCModding.Business.Services;
using VRCModding.Configuration;
using VRCModding.Entities;
using VRCModding.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging((hostingContext, logging) => {
	logging.AddSerilog(
		new LoggerConfiguration()
			.ReadFrom.Configuration(hostingContext.Configuration, "Serilog")
			.CreateLogger()
	);
});

#region Services
// General Services
builder.Services.AddControllersWithViews(o => {
	o.Filters.Add<HandleExceptionFilter>();
	o.Filters.Add<ValidateModelStateAttribute>();
}).AddNewtonsoftJson();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseMySql(connectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString)));
});
builder.Services.AddScoped<ModelConverter>();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
//});

// Business Repositories
builder.Services.AddScoped<AccountInfoRepository>();
builder.Services.AddScoped<HwidRepository>();
builder.Services.AddScoped<IpRepository>();

// Business Services
builder.Services.AddScoped<AccountInfoService>();
builder.Services.AddScoped<HwidService>();
builder.Services.AddScoped<IpService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

//app.UseAuthorization();

//app.UseEndpoints(endpoints => {
//	endpoints.MapControllers();
//});
app.MapControllers(); // Todo: Debug initial state of code. Everything that was added on this commit.
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope()) {
	var services = scope.ServiceProvider;
	var logger = services.GetRequiredService<ILogger<Program>>();
	logger.LogInformation("Initializing DB auto-update routine!");
	try {
		var context = services.GetRequiredService<AppDbContext>();
		context.Database.Migrate();
	} catch (Exception ex) {
		logger.LogError(ex, "An error occurred migrating the DB.");
	}
}

app.Run();
