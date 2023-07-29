
using Blazored.Toast;
using FamilyPlanner.UI.Services;
using FamilyPlanner.UI.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);
var familyPlannerApiUriString = builder.Configuration.GetValue<string>("FamilyPlanner.Api");

if (string.IsNullOrWhiteSpace(familyPlannerApiUriString))
{
    throw new ArgumentNullException(nameof(familyPlannerApiUriString), "FamilyPlannerApi is not configured. Check appsettings.json file.");
}

if (!Uri.TryCreate(familyPlannerApiUriString, UriKind.Absolute, out var familyPlannerApiUri))
{
    throw new FormatException($"\"{familyPlannerApiUriString}\" is not a valid URI for FamilyPlannerApiUri. Check appsettings.json file.");
}

// Add services to the container.
builder.Services
    .AddTransient<IMealService, MealService>()
    .AddBlazoredToast()
    .AddHttpClient("FamilyPlanner.Api", c => c.BaseAddress = familyPlannerApiUri);



builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting();
    
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();