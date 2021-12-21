using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Worms.Web.Behaviour;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<AbstractWormBehaviour, OptimizedWormBehaviour>();
var app = builder.Build();
app.MapControllers();
app.Run();