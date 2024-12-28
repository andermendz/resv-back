using Microsoft.EntityFrameworkCore;
using SpaceReservation.Application.Services;
using SpaceReservation.Domain.Repositories;
using SpaceReservation.Infrastructure.Persistence;
using SpaceReservation.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure database connection
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
    builder.Configuration.GetConnectionString("DefaultConnection");

// Add PostgreSQL DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register repositories
builder.Services.AddScoped<ISpaceRepository, SpaceRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// Register application services
builder.Services.AddScoped<ISpaceService, SpaceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Apply migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
