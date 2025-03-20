using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using Swashbuckle.AspNetCore.Annotations;

namespace PosTech.Contacts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController(IMediator mediator, IMapper mapper) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ContactResponseDto))]
        public async Task<IActionResult> CreateContactAsync(CreateAndUpdateContactRequestDto createContactRequestDto)
        {
            var createContactCommand = _mapper.Map<CreateContactCommand>(createContactRequestDto);
            var result = await _mediator.Send(createContactCommand);

            return Ok(result);
        }

        [HttpPost("search")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<ContactResponseDto>))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SearchContactsAsync(SearchContactRequestDto searchContactRequestDto)
        {
            var searchContactCommand = _mapper.Map<SearchContactsCommand>(searchContactRequestDto);
            var contacts = await _mediator.Send(searchContactCommand);

            if (contacts is null || contacts.Count == 0) return NoContent();

            return Ok(contacts);
        }

        [HttpGet("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ContactResponseDto))]
        [SwaggerResponse(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetContactByIdAsync(Guid id)
        {
            var contact = await _mediator.Send(new GetContactByIdCommand { Id = id });

            if (contact is null) return NoContent();

            return Ok(contact);
        }

        [HttpPut("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ContactResponseDto))]
        [SwaggerResponse(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateContactAsync(CreateAndUpdateContactRequestDto updateContactRequestDto, Guid id)
        {
            var updateContactCommand = _mapper.Map<UpdateContactCommand>(updateContactRequestDto);
            updateContactCommand.Id = id;

            var result = await _mediator.Send(updateContactCommand);

            if (result is null) return UnprocessableEntity();

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ContactResponseDto))]
        public async Task<IActionResult> DeleteContactAsync(Guid id)
        {
            await _mediator.Send(new DeleteContactCommand { Id = id });

            return Ok();
        }
    }
}
