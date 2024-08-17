using Microsoft.OpenApi.Models;
using TodoApp.Application.Commands;
using TodoApp.Application.Queries;
using TodoApp.Application.Services.TodoApp.Application.Services;
using TodoApp.Infrastructure.EventStore;
using TodoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyEventSourcingApp.API", Version = "v1" });
});

// Register application services and handlers
builder.Services.AddTransient<ITodoCommandHandlers, TodoCommandHandlers>();
builder.Services.AddTransient<ITodoQueryHandlers, TodoQueryHandlers>();

// Register domain services
builder.Services.AddSingleton<InMemoryEventStore>();
builder.Services.AddTransient<ITodoRepository, TodoRepository>();
builder.Services.AddTransient<ITodoService, TodoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyEventSourcingApp.API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
