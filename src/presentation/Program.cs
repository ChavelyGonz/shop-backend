#region Usings
using Application;
using Presentation;
using Infrastructure.Persistence;
using System.Text.Json.Serialization;
#endregion

var builder = WebApplication.CreateBuilder(args);

// --- Configure Services ---

// Allow any origin (⚠ dev only; restrict in prod)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Add Controllers only (no Views, if you have API only)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register App & Infra layers
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddApplicationLayer();

// Optional: NewtonsoftJson if you need it
// builder.Services.AddControllers().AddNewtonsoftJson(options =>
// {
//     options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
// });

var app = builder.Build();

// --- Configure Middleware Pipeline ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error"); // handle exceptions in production
    app.UseHsts();                     // use HTTP Strict Transport Security
}

app.UseCors();                        // enable CORS
app.UseHttpsRedirection();            // redirect HTTP -> HTTPS
app.UseStaticFiles();                 // serve wwwroot files (e.g., images)

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseErrorHandlingMiddleware();     // your custom global error handlerapp.UseRouting();                     // enable endpoint routing
app.UseAuthentication();              // enable authentication
app.UseAuthorization();               // enable authorization

// --- Map Endpoints ---
app.MapControllers(); // maps attribute-routed controllers

// Optional: fallback for SPA
// app.MapFallbackToFile("index.html");

app.Run();

// Optional: seed DB at startup
// async Task InitializeDatabase(WebApplication app)
// {
//     using var scope = app.Services.CreateScope();
//     var seedService = scope.ServiceProvider.GetRequiredService<Seed>();
//     await seedService.AddAsync();
// }

public partial class Program { }
