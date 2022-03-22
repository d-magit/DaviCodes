using VRCModding.Business;
using VRCModding.Business.Repositories;
using VRCModding.Business.Services;
using VRCModding.Configuration;
using VRCModding.Entities;
using VRCModding.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

// Todo: After all is done, create API search endpoints to look stuff up (such as alts, old nicknames, etc.)
// Todo: Add i18n, ErrorResources, Webhook endpoints, RequirePermission attribute, Discord Notification Templates, possibly a Discord bot too, which includes studying API keys n shit, and also Authentication process
var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging((hostingContext, logging) => {
	logging.AddSerilog(((Func<LoggerConfiguration>)(() => {
			// Default for all
			var defaultConfig = new LoggerConfiguration().ReadFrom.Configuration(hostingContext.Configuration, "Serilog");
			
			// Custom cases
			return builder.Environment.IsDevelopment() ? defaultConfig
				.WriteTo.File(
					path: $"../Logs/{DateTime.Today:yyyy-M-d}-VRCModding.txt", 
					outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
				) : defaultConfig; // Allows for other Environments
		})).Invoke().CreateLogger()
	);
});

#region Services
// General Services
builder.Services.AddCors();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
	options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});
builder.Services.AddControllersWithViews(o => {
	o.Filters.Add<HandleExceptionFilter>();
	o.Filters.Add<ValidateModelStateAttribute>();
}).AddNewtonsoftJson();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    opt.UseMySql(connectionString, new MariaDbServerVersion(ServerVersion.AutoDetect(connectionString)));
});
var key = System.Text.Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("General:PrivateKey"));
builder.Services.AddSingleton(s =>
	new TokenGenerator(key)
);
builder.Services.AddAuthentication(a => {
	a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(j=> {
	j.RequireHttpsMetadata = false;
	j.SaveToken = true;
	j.TokenValidationParameters = new TokenValidationParameters {
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});
builder.Services.AddSingleton<ModelConverter>();
builder.Services.AddSingleton<ExceptionBuilder>();

// Business Repositories
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<HwidRepository>();
builder.Services.AddScoped<IpRepository>();
builder.Services.AddScoped<UserRepository>();

// Business Services
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<HwidService>();
builder.Services.AddScoped<IpService>();
builder.Services.AddScoped<UserService>();
#endregion

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(c => c
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
	endpoints.MapControllers();
});

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
