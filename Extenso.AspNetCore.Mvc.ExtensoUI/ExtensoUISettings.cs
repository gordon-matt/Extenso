using System.Collections.Generic;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public static class ExtensoUISettings
    {
        public static void Init(
            IExtensoUIProvider defaultAdminProvider,
            IExtensoUIProvider defaultFrontendProvider)
        {
            AreaUIProviders = new Dictionary<string, IExtensoUIProvider>();
            DefaultAdminProvider = defaultAdminProvider;
            DefaultFrontendProvider = defaultFrontendProvider;
        }

        public static Dictionary<string, IExtensoUIProvider> AreaUIProviders { get; private set; }

        public static IExtensoUIProvider DefaultAdminProvider { get; private set; }

        public static IExtensoUIProvider DefaultFrontendProvider { get; private set; }

        public static void RegisterAreaUIProvider(string area, IExtensoUIProvider provider)
        {
            if (!AreaUIProviders.ContainsKey(area))
            {
                AreaUIProviders.Add(area, provider);
            }
        }
    }
}