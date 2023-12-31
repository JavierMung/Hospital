using API.Context;
using API.Interfaces;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMedicosServices, MedicosServices>();
builder.Services.AddScoped<ITrabajadoresServices, TrabajadoresServices>();
builder.Services.AddScoped<IServiciosServices, ServiciosServices>();
builder.Services.AddScoped<ICitasServices, CitasServices>();
builder.Services.AddScoped<IPersonaServices, PersonaServices>();
builder.Services.AddDbContext<HospitalContext>();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
	builder.AllowAnyOrigin()
		   .AllowAnyMethod()
		   .AllowAnyHeader();
}));
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


/*
 
 */