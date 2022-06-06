using DisneyWebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

ProgramExtension.AddSwagger(builder.Services);

ProgramExtension.AddDbContexts(builder.Services, config);

ProgramExtension.AddIdentity(builder.Services);

ProgramExtension.AddAuthentication(builder.Services, config);

ProgramExtension.AddServicesImplementations(builder.Services);

builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Disney Web API", policy =>
    {
        policy.WithOrigins("https://localhost:7000",
            "https://localhost:5000");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Disney Web API V1"));
}

app.UseHttpsRedirection();

app.UseCors("Disney Web API");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
