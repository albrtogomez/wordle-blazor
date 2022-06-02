using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WordleBlazor;
using WordleBlazor.Extensions;
using WordleBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GameManagerService>();
builder.Services.AddScoped<ToastNotificationService>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<BrowserLocalStorageService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLocalization();

var host = builder.Build();

await host.SetDefaultCulture();

var gameManager = host.Services.GetRequiredService<GameManagerService>();
await gameManager.InitializeAndLoadData();

await host.RunAsync();