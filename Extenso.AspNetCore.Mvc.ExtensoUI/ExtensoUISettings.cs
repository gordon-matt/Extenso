using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Provides extenso UI settings.
/// </summary>
public static class ExtensoUISettings
{
    /// <summary>
    /// Performs the init operation.
    /// </summary>
    /// <param name="defaultAdminProvider">The default admin provider.</param>
    /// <param name="defaultFrontendProvider">The default frontend provider.</param>
    public static void Init(
        IExtensoUIProvider defaultAdminProvider,
        IExtensoUIProvider defaultFrontendProvider)
    {
        AreaUIProviders = [];
        DefaultAdminProvider = defaultAdminProvider;
        DefaultFrontendProvider = defaultFrontendProvider;
    }

    /// <summary>
    /// Gets or sets the area UI providers.
    /// </summary>
    public static Dictionary<string, IExtensoUIProvider> AreaUIProviders { get; private set; }

    /// <summary>
    /// Gets or sets the default admin provider.
    /// </summary>
    public static IExtensoUIProvider DefaultAdminProvider { get; private set; }

    /// <summary>
    /// Gets or sets the default frontend provider.
    /// </summary>
    public static IExtensoUIProvider DefaultFrontendProvider { get; private set; }

    /// <summary>
    /// Registers the area UI provider.
    /// </summary>
    /// <param name="area">The area.</param>
    /// <param name="provider">The provider.</param>
    public static void RegisterAreaUIProvider(string area, IExtensoUIProvider provider)
    {
        if (!AreaUIProviders.ContainsKey(area))
        {
            AreaUIProviders.Add(area, provider);
        }
    }
}