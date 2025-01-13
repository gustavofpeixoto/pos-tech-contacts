using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

RouteGroupBuilder contacts = app.MapGroup(RouteConst.Contacts);

contacts.MapPost("/", async () => await CreateContactAsync());
static async Task<IResult> CreateContactAsync()
{
    return TypedResults.Ok();
}

app.Run();
