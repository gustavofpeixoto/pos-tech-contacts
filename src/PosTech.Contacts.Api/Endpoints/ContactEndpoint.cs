﻿using AutoMapper;
using FluentValidation;
using MediatR;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;

namespace PosTech.Contacts.Api.Endpoints
{
    public static class ContactEndpoint
    {
        public static async Task<IResult> CreateContactAsync(IMediator mediator, IValidator<CreateAndUpdateContactRequestDto> validator,
            IMapper mapper, CreateAndUpdateContactRequestDto createContactRequestDto)
        {
            var validationResult = await validator.ValidateAsync(createContactRequestDto);
            if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());

            var createContactCommand = mapper.Map<CreateContactCommand>(createContactRequestDto);

            var result = await mediator.Send(createContactCommand);

            return TypedResults.Ok(result);
        }

        public static async Task<IResult> SearchContactsAsync(IMediator mediator, IMapper mapper,
            IValidator<SearchContactRequestDto> validator, SearchContactRequestDto searchContactRequestDto)
        {
            var validationResult = await validator.ValidateAsync(searchContactRequestDto);

            if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());

            var searchContactCommand = mapper.Map<SearchContactsCommand>(searchContactRequestDto);
            var contacts = await mediator.Send(searchContactCommand);

            if (contacts is null || contacts.Count == 0) return TypedResults.NoContent();

            return TypedResults.Ok(contacts);
        }

        public static async Task<IResult> GetContactByIdAsync(IMediator mediator, Guid id)
        {
            var contact = await mediator.Send(new GetContactByIdCommand { Id = id });

            if (contact is null) return TypedResults.NoContent();

            return TypedResults.Ok(contact);
        }

        public static async Task<IResult> UpdateContactAsync(IMediator mediator, IValidator<CreateAndUpdateContactRequestDto> validator,
            IMapper mapper, CreateAndUpdateContactRequestDto updateContactRequestDto, Guid id)
        {
            var validationResult = await validator.ValidateAsync(updateContactRequestDto);
            if (!validationResult.IsValid) return TypedResults.ValidationProblem(validationResult.ToDictionary());

            var updateContactCommand = mapper.Map<UpdateContactCommand>(updateContactRequestDto);
            updateContactCommand.Id = id;

            var result = await mediator.Send(updateContactCommand);

            if (result is null) return TypedResults.UnprocessableEntity();

            return TypedResults.Ok(result);
        }

        public static async Task<IResult> DeleteContactAsync(IMediator mediator, Guid id)
        {
            await mediator.Send(new DeleteContactCommand { Id = id });

            return TypedResults.Ok();
        }
    }
}
