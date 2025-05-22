using Models;
using Services.Repository.Implementacion;
using Services.Repository.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ? Registrar tu servicio AQUÍ (antes del Build)
builder.Services.AddScoped<ICompañiaRepository<Compania>, CompañiaRepository>();
builder.Services.AddScoped<ITipoDocumentoRepository<TipoDocumento>, TipoDocumentoRepository>();
builder.Services.AddScoped<ITipoMovimientoRepository<TipoMovimiento>, TipoMovimientoRepository>();
builder.Services.AddScoped<ICentroCostoRepository<CentroCosto>, CentroCostoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
