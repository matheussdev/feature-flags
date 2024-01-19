using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
// Outros usings necessários...

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

try {
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect("")
            .Select(".appconfig.featureflag/*");

        options.UseFeatureFlags();
    });
    logger.LogInformation("Azure App Configuration loaded successfully.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Azure App Configuration failed to load.");
}


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();
builder.Services.AddControllers();


builder.Services.Configure<Settings>(builder.Configuration.GetSection("digital-beneficiary-dev:Settings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers(); // Mapeia os Controllers
});

app.Run();


public class Settings
{
    public string MySetting { get; set; }
    // Outras propriedades que correspondem às chaves no seu Azure App Configuration
}