using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using System.Globalization;

namespace WordleBlazor.Extensions
{
    public static class WebAssemblyHostExtension
    {
        public async static Task SetDefaultCulture(this WebAssemblyHost host)
        {
            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();

            var storedCulture = await localStorage.GetItemAsync<string>("CurrentCulture");

            CultureInfo culture;

            if (storedCulture != null)
            {
                if (storedCulture.StartsWith("es"))
                    culture = new CultureInfo("es-ES");
                else
                    culture = new CultureInfo("en-US");
            }
            else
            {
                var browserCulture = CultureInfo.CurrentCulture.Name;

                if (browserCulture.StartsWith("es"))
                {
                    await localStorage.SetItemAsync("CurrentCulture", "es-ES");
                    culture = new CultureInfo("es-ES");
                }
                else
                {
                    await localStorage.SetItemAsync("CurrentCulture", "en-US");
                    culture = new CultureInfo("en-US");
                }
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
