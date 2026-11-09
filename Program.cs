using progPart2Web.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Add Session services
// 1. Add Session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session expiration time

    // Fix: Nest cookie-specific settings inside the Cookie object
    options.Cookie.HttpOnly = true;                 // Keeps cookie secure against XSS
    options.Cookie.IsEssential = true;              // Mandatory for app operation
});

// Register your ApiService here if you haven't already
builder.Services.AddHttpClient<ApiService>(client =>
{
    // Ensure this port matches your backend Web API launch settings exactly!
    client.BaseAddress = new Uri("https://localhost:7221/");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 2. Add Session Middleware (MUST be before UseAuthorization)
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
