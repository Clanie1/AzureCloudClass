using System.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StudentDbContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/students", async (StudentDbContext db) =>
    {
        return await db.Student.ToListAsync();
    })
    .WithName("GetStudents")
    .WithOpenApi();

app.MapPost("/students", async (StudentDbContext db, Student student) =>
    {
        db.Student.Add(student);
        await db.SaveChangesAsync();
        return Results.Ok(student);
    })
    .WithName("AddStudent")
    .WithOpenApi();

app.MapDelete("/students", async (StudentDbContext db, int studentId) =>
    {
        var student = await db.Student.FindAsync(studentId);
        db.Student.Remove(student);       
        await db.SaveChangesAsync();
        return Results.Ok(student);
    })
    .WithName("RemoveStudent")
    .WithOpenApi();

app.MapPut("/students", async (StudentDbContext db, int studentId, Student updatedStudent) =>
    {
        if (studentId != updatedStudent.Student_id)
        {
            return Results.BadRequest("Product ID mismatch");
        }

        var existingProduct = await db.Student.FindAsync(studentId);
        if (existingProduct == null)
        {
            return Results.NotFound();
        }

        existingProduct.Fullname = updatedStudent.Fullname;

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        { 
            return Results.NotFound();
        }

        return Results.NoContent();
    })
    .WithName("EditStudent")
    .WithOpenApi();


var configuration = builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets(typeof(Program).Assembly)
    .Build();



// builder.Services.AddDbContext<DatabaseContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     try
//     {
//         var dbContext = services.GetRequiredService<DatabaseContext>();
//         dbContext.Database.Migrate();
//     }
//     catch (Exception ex)
//     {
//         // You can log the exception or handle it as needed
//         var logger = services.GetRequiredService<ILogger<Program>>();
//         logger.LogError(ex, "An error occurred while applying the database migrations.");
//     }
// }


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}