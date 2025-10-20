using Microsoft.JSInterop;

namespace Store.Services;

public class AgentFrameworkService
{
    private readonly IJSRuntime _jsRuntime;
    private string? _cachedFramework;

    public AgentFrameworkService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetSelectedFrameworkAsync()
    {
        if (_cachedFramework != null)
        {
            return _cachedFramework;
        }

        try
        {
            var framework = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "agentFramework");
            _cachedFramework = string.IsNullOrEmpty(framework) ? "SK" : framework;
            return _cachedFramework;
        }
        catch
        {
            // Default to SK if localStorage is not available
            _cachedFramework = "SK";
            return _cachedFramework;
        }
    }

    public async Task SetSelectedFrameworkAsync(string framework)
    {
        _cachedFramework = framework;
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "agentFramework", framework);
    }

    public void ClearCache()
    {
        _cachedFramework = null;
    }
}
