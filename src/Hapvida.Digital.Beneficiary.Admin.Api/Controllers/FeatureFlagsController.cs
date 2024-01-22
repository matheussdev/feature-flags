namespace Hapvida.Digital.Beneficiary.Admin.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
[ApiController]
[Route("[controller]")]
public class FeatureFlagsController : ControllerBase
{
    private readonly IFeatureManager _featureManager;

    public FeatureFlagsController(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var featureFlags = new List<string>();
        await foreach (var feature in _featureManager.GetFeatureNamesAsync())
        {
            featureFlags.Add(feature);
        }

        return Ok(featureFlags);
    }

    [HttpGet("check-feature")]
    public async Task<IActionResult> CheckFeature()
    {
        bool featureEnabled = await _featureManager.IsEnabledAsync("FLAG_HAPVIDA_TESTE2");

        if (featureEnabled)
        {
            // A Feature Flag está habilitada, execute a lógica correspondente
            return Ok("A Feature Flag está habilitada.");
        }
        else
        {
            // A Feature Flag está desabilitada, execute outra lógica ou retorne uma mensagem
            return Ok("A Feature Flag está desabilitada.");
        }
    }
    [HttpGet("list-features")]
    public async Task<IActionResult> ListFeatures()
    {
        var features = new List<object>();
        Console.WriteLine(_featureManager.GetFeatureNamesAsync());
        await foreach (var feature in _featureManager.GetFeatureNamesAsync())
        {
            bool isEnabled = await _featureManager.IsEnabledAsync(feature);
            features.Add(new
            {
                Feature = feature,
                Enabled = isEnabled,
            });
        }

        return Ok(features);
    }

    [ApiController]
    [Route("[controller]")]
    public class FeatureFlagController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;

        public FeatureFlagController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet("{featureName}")]
        public async Task<IActionResult> GetFeatureFlagStatus(string featureName)

        {
            Console.WriteLine(await _featureManager.IsEnabledAsync(featureName));
            bool isEnabled = await _featureManager.IsEnabledAsync(featureName);
            return Ok(new { Feature = featureName, IsEnabled = isEnabled });
        }
    }
}