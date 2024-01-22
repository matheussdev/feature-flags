using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
// Outros usings necessários...
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

try {

    builder.Configuration.AddAzureAppConfiguration(options => {
        options.Connect("Endpoint=https://digital-beneficiary-dev.azconfig.io;Id=42Kr;Secret=1+ga2EUbxR07obG5KOtImV35U3CBtctX2Kw1wlNFLEo=");
        options.UseFeatureFlags(
            featureFlagOptions => {
                featureFlagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(1);
                featureFlagOptions.Label = "flag 2 criado enabled";
            }
        );
    });
    logger.LogInformation("Azure App Configuration loaded successfully.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Azure App Configuration failed to load.");
}


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAzureAppConfiguration();

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