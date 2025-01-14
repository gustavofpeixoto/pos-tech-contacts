using AutoMapper;
using FluentValidation;
using MediatR;
using PosTech.Contacts.ApplicationCore;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddApplicationCoreServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

RouteGroupBuilder contacts = app.MapGroup(RouteConst.Contacts);

contacts.MapPost("/", async (IMediator mediator, IMapper mapper,
    IValidator<CreateAndUpdateContactRequestDto> validator, CreateAndUpdateContactRequestDto contactDto)
    => await CreateContactAsync(mediator, validator, mapper, contactDto));

contacts.MapPost("/search", async (IMediator mediator, IMapper mapper,
    IValidator<SearchContactRequestDto> validator, SearchContactRequestDto searchContactRequestDto)
    => await SearchContactsAsync(mediator, mapper, validator, searchContactRequestDto));

contacts.MapGet("/{id:guid}", async (IMediator mediator, Guid id)
    => await GetContactByIdAsync(mediator, id));

contacts.MapPut("/{id:guid}", async (IMediator mediator, IMapper mapper,
    IValidator<CreateAndUpdateContactRequestDto> validator, CreateAndUpdateContactRequestDto contactDto, Guid id)
    => await UpdateContactAsync(mediator, validator, mapper, contactDto, id));

contacts.MapDelete("/{id:guid}", async (IMediator mediator, Guid id)
    => await DeleteContactAsync(mediator, id));

static async Task<IResult> CreateContactAsync(IMediator mediator, IValidator<CreateAndUpdateContactRequestDto> validator, IMapper mapper,
    CreateAndUpdateContactRequestDto createContactRequestDto)
{
    var validationResult = await validator.ValidateAsync(createContactRequestDto);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var createContactCommand = mapper.Map<CreateContactCommand>(createContactRequestDto);

    await mediator.Send(createContactCommand);

    return TypedResults.Created();
}

static async Task<IResult> SearchContactsAsync(IMediator mediator, IMapper mapper,
    IValidator<SearchContactRequestDto> validator, SearchContactRequestDto searchContactRequestDto)
{
    var validationResult = await validator.ValidateAsync(searchContactRequestDto);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var searchContactCommand = mapper.Map<SearchContactsCommand>(searchContactRequestDto);
    var contacts = await mediator.Send(searchContactCommand);

    if (contacts is null || contacts.Count == 0) return Results.NotFound();

    return Results.Ok(contacts);
}

static async Task<IResult> GetContactByIdAsync(IMediator mediator, Guid id)
{
    var contact = await mediator.Send(new GetContactByIdCommand { Id = id });

    if (contact is null) return Results.NotFound();

    return Results.Ok(contact);
}
static async Task<IResult> UpdateContactAsync(IMediator mediator, IValidator<CreateAndUpdateContactRequestDto> validator, IMapper mapper,
    CreateAndUpdateContactRequestDto updateContactRequestDto, Guid id)
{
    var validationResult = await validator.ValidateAsync(updateContactRequestDto);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());

    var updateContactCommand = mapper.Map<UpdateContactCommand>(updateContactRequestDto);
    updateContactCommand.Id = id;

    await mediator.Send(updateContactCommand);

    return Results.Ok();
}

static async Task<IResult> DeleteContactAsync(IMediator mediator, Guid id)
{
    await mediator.Send(new DeleteContactCommand { Id = id });

    return Results.Ok();
}

app.Run();
