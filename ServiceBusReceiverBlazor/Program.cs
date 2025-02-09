using ServiceBusReceiverBlazor.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();


builder.Services.AddSingleton<IServiceBusReceiverService, ServiceBusReceiverService>();
builder.Services.AddSignalR();  

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var serviceBusReceiverService = app.Services.GetRequiredService<IServiceBusReceiverService>();
await serviceBusReceiverService.StartReceivingMessagesAsync();


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();
