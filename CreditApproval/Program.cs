WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CreditApprovalDbContext>(opt =>
{
    opt.UseInMemoryDatabase("InMemoryDB");
});

// Validators
builder.Services.AddScoped<IValidator<SubmitCreditDTO>, CreditValidator>();
builder.Services.AddScoped<IValidator<ReviewCreditDTO>, ReviewCreditValidator>();

// Services
builder.Services.AddSingleton<CreditIdentifierGenerator>();
builder.Services.AddScoped<ICreditService, CreditService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IDataSeeder, DataSeeder>();

WebApplication app = builder.Build();

// Seed users
await app.Services.GetRequiredService<IDataSeeder>().SeedUsers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
