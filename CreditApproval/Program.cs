WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CreditApprovalDbContext>(opt =>
{
    opt.UseInMemoryDatabase("InMemoryDB");
});

builder.Services.AddValidatorsFromAssemblyContaining<CreditValidator>(ServiceLifetime.Scoped);
builder.Services.AddSingleton<CreditIdentifierGenerator>();
builder.Services.AddScoped<ICreditService, CreditService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
