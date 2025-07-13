using Hub.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<SignalRService>();
// Radzen 및 SignalR 서비스 등록

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>(); 
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<TokenService>();
builder.Services.AddApiAuthorization();
builder.Services.AddScoped<FileSystemService>();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped<TtsApi>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();


// Blazor 앱 실행
await builder.Build().RunAsync().ContinueWith(task =>
{
    if (task.IsFaulted)
    {
        Console.WriteLine($"Blazor Load Failed: {task.Exception?.Message}");
    }
    else
    {
        Console.WriteLine("Blazor Loaded");
    }
});


