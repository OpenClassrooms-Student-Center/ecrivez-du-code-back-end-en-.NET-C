using Hamac.Front;
using Hamac.Front.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<Texts>();
builder.Services.AddScoped<ILeaveApi, MockLeaveApi>();

await builder.Build().RunAsync();
