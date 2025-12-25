using AIRPORT.Data;           // Hasnain's Database Context
using AIRPORT.Services;       // All Logic (Hamza, Bilal, Jawwad, Awais)
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==============================================================
// 1. REGISTER SERVICES (Hiring the Staff)
// ==============================================================

// A. ENABLE CORS (Essential: Allows your HTML/JS to talk to this API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// B. REGISTER DATABASE (Hasnain's Module)
// NOTE: Make sure your appsettings.json has the "DefaultConnection" string!
builder.Services.AddDbContext<AirlineContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// C. REGISTER LOGIC SERVICES (The "Brain" of the App)
builder.Services.AddSingleton<FlightGraphService>(); // Hamza (Dijkstra)
builder.Services.AddSingleton<TspService>();         // Bilal (TSP Multi-City)
builder.Services.AddSingleton<PricingService>();     // Jawwad (Knapsack Pricing)
builder.Services.AddSingleton<GateService>();        // Awais (Graph Coloring Gates)

// D. REGISTER API CONTROLLERS (The "Mouth" of the App)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==============================================================
// 2. CONFIGURE PIPELINE (Opening the Airport)
// ==============================================================

// Show the Swagger Dashboard
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// E. ACTIVATE CORS (Must be before Authorization!)
app.UseCors("AllowAll");

app.UseAuthorization();

// F. MAP ENDPOINTS (Links the URLs to the Controllers)
app.MapControllers();

app.Run();