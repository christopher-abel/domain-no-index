using DomainNoIndex;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Route Robots.txt
app.MapControllerRoute(
    name: "RobotsText",
    pattern: "robots.txt",
    defaults: new { controller = "Home", action = "Robots" }
    ); 

// Route all other requests to the Home/Index action
app.MapControllerRoute(
    name: "CatchAll",
    pattern: "{*url}",
    defaults: new { controller = "Home", action = "Index" }
    );

// Add Serilog request logging
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestScheme}://{RequestHost}{RequestPath}{RequestQueryString} responded {StatusCode} in {Elapsed:0.0000} ms, ip: {RemoteIp}, UserAgent: {UserAgent}";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host);
        diagnosticContext.Set("RequestProtocol", httpContext.Request.Protocol);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RequestPath", httpContext.Request.Path);
        diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
        diagnosticContext.Set("RequestHeaders", httpContext.Request.Headers);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
        diagnosticContext.Set("RemoteIp", httpContext.Connection.RemoteIpAddress);

    };
});

app.Run();
