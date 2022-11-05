using Microsoft.EntityFrameworkCore;
using Tictactoe.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
builder.Services.AddEntityFrameworkSqlite();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(connectionString ?? "Test");
});


const string corsRuleName = "TicTacToeCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsRuleName, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Fix Docker migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DatabaseContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.UseDefaultFiles();
//app.UseStaticFiles();

// app.UseHttpsRedirection();

app.UseCors(corsRuleName);

//app.UseAuthorization();

app.MapControllers();

app.Run();
