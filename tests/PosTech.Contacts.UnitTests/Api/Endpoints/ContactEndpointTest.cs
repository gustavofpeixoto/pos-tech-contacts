using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using PosTech.Contacts.Api.Endpoints;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Requests;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Validators;

namespace PosTech.Contacts.UnitTests.Api.Endpoints
{
    public class ContactEndpointTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidator<CreateAndUpdateContactRequestDto>> _createAndUpdateContactRequestValidatorMock;
        private readonly Mock<IValidator<SearchContactRequestDto>> _searchContactRequestValidatorMock;

        public ContactEndpointTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _createAndUpdateContactRequestValidatorMock = new Mock<IValidator<CreateAndUpdateContactRequestDto>>();
            _searchContactRequestValidatorMock = new Mock<IValidator<SearchContactRequestDto>>();
        }

        [Theory]
        [InlineData(11, "carlos@validservice.com", "Carlos", "988888888", "Silva")]
        [InlineData(62, "antonio@validservice.com", "Antônio", "88888888", "Antunes")]
        [InlineData(37, "jose@validservice.com", "José", "977777777", "Fernandes")]
        public async Task Create_Contact_Successfully(int ddd, string email, string name, string phone, string surname)
        {
            //Arrange
            var createContactRequest = new CreateAndUpdateContactRequestDto
            {
                Ddd = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };
            var createContactCommand = new CreateContactCommand
            {
                Ddd = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };

            _mapperMock.Setup(x => x.Map<CreateContactCommand>(createContactRequest)).Returns(createContactCommand);
            var validator = await new CreateAndUpdateContactValidator().ValidateAsync(createContactRequest);

            _createAndUpdateContactRequestValidatorMock.Setup(x => x.ValidateAsync(createContactRequest, It.IsAny<CancellationToken>())).ReturnsAsync(validator);

            //Act

            var result = await ContactEndpoint.CreateContactAsync(_mediatorMock.Object,
                _createAndUpdateContactRequestValidatorMock.Object, _mapperMock.Object, createContactRequest);

            //Assert
            var okResult = (Ok<ContactResponseDto>)result;
            Assert.IsType<Ok<ContactResponseDto>>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Theory]
        [InlineData(25, "carlos@validservice.com", "Carlos", "988888888", "Silva")]
        [InlineData(62, "antonio@validservice.com", "Antônio", "7777777", "Antunes")]
        [InlineData(37, "jose@validservice.com", "J", "977777777", "Fernandes")]
        public async Task Return_Validation_Problem_When_Data_Is_Invalid(int ddd, string email, string name, string phone, string surname)
        {
            var createContactRequest = new CreateAndUpdateContactRequestDto
            {
                Ddd = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };
            var createContactCommand = new CreateContactCommand
            {
                Ddd = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };

            _mapperMock.Setup(x => x.Map<CreateContactCommand>(createContactRequest)).Returns(createContactCommand);
            var validator = await new CreateAndUpdateContactValidator().ValidateAsync(createContactRequest);

            _createAndUpdateContactRequestValidatorMock.Setup(x => x.ValidateAsync(createContactRequest, It.IsAny<CancellationToken>())).ReturnsAsync(validator);

            //Act

            var result = await ContactEndpoint.CreateContactAsync(_mediatorMock.Object,
                _createAndUpdateContactRequestValidatorMock.Object, _mapperMock.Object, createContactRequest);

            //Assert
            var errorResult = (ValidationProblem)result;
            Assert.IsType<ValidationProblem>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, errorResult.StatusCode);
        }

        [Theory]
        [InlineData(11, "carlos@validservice.com", "Carlos", "988888888", "Silva")]
        [InlineData(62, "antonio@validservice.com", "Antônio", "88888888", "Antunes")]
        [InlineData(37, "jose@validservice.com", "José", "977777777", "Fernandes")]
        public async Task Search_Contact_Successfully_And_Returns_Concatcs(int ddd, string email, string name, string phone, string surname)
        {
            //Arrange
            var searchContactRequest = new SearchContactRequestDto
            {
                Ddd = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };
            var searchContactsCommand = new SearchContactsCommand
            {
                DddCode = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };

            _mapperMock.Setup(x => x.Map<SearchContactsCommand>(searchContactRequest)).Returns(searchContactsCommand);
            var validator = await new SearchContactValidator().ValidateAsync(searchContactRequest);

            _searchContactRequestValidatorMock.Setup(x => x.ValidateAsync(searchContactRequest, It.IsAny<CancellationToken>())).ReturnsAsync(validator);
            _mediatorMock.Setup(x => x.Send(It.IsAny<SearchContactsCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync([new()]);

            //Act

            var result = await ContactEndpoint.SearchContactsAsync(_mediatorMock.Object, _mapperMock.Object,
                _searchContactRequestValidatorMock.Object, searchContactRequest);

            //Assert
            Assert.IsType<Ok<List<ContactResponseDto>>>(result);
            var okResult = (Ok<List<ContactResponseDto>>)result;
            Assert.IsType<Ok<List<ContactResponseDto>>>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Theory]
        [InlineData(11, "carlos@validservice.com", "Carlos", "988888888", "Silva")]
        [InlineData(62, "antonio@validservice.com", "Antônio", "88888888", "Antunes")]
        [InlineData(37, "jose@validservice.com", "José", "977777777", "Fernandes")]
        public async Task Search_Contact_Successfully_And_Dont_Return_Concatcs(int ddd, string email, string name, string phone, string surname)
        {
            //Arrange
            var searchContactRequest = new SearchContactRequestDto
            {
                Ddd = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };
            var searchContactsCommand = new SearchContactsCommand
            {
                DddCode = ddd,
                Email = email,
                Name = name,
                Phone = phone,
                Surname = surname
            };

            _mapperMock.Setup(x => x.Map<SearchContactsCommand>(searchContactRequest)).Returns(searchContactsCommand);
            var validator = await new SearchContactValidator().ValidateAsync(searchContactRequest);

            _searchContactRequestValidatorMock.Setup(x => x.ValidateAsync(searchContactRequest, It.IsAny<CancellationToken>())).ReturnsAsync(validator);

            //Act

            var result = await ContactEndpoint.SearchContactsAsync(_mediatorMock.Object, _mapperMock.Object,
                _searchContactRequestValidatorMock.Object, searchContactRequest);

            //Assert
            Assert.IsType<NoContent>(result);
            var okResult = (NoContent)result;
            Assert.IsType<NoContent>(result);
            Assert.Equal(StatusCodes.Status204NoContent, okResult.StatusCode);
        }

        [Fact]
        public async Task Get_Contact_By_Id_Successfully()
        {
            //Arrange
            var contactResponse = new ContactResponseDto
            {
                Ddd = 37,
                Email = "jose@provedor.com",
                Id = Guid.NewGuid(),
                Name = "José",
                Phone = "99999999",
                Surname = "da Silva"
            };
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetContactByIdCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(contactResponse);
            //Act

            var result = await ContactEndpoint.GetContactByIdAsync(_mediatorMock.Object, Guid.NewGuid());

            //Assert
            var okResult = (Ok<ContactResponseDto>)result;
            Assert.IsType<Ok<ContactResponseDto>>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(contactResponse.Ddd, okResult.Value?.Ddd);
            Assert.Equal(contactResponse.Email, okResult.Value?.Email);
            Assert.Equal(contactResponse.Id, okResult.Value?.Id);
            Assert.Equal(contactResponse.Name, okResult.Value?.Name);
            Assert.Equal(contactResponse.Phone, okResult.Value?.Phone);
            Assert.Equal(contactResponse.Surname, okResult.Value?.Surname);
        }

        [Fact]
        public async Task Get_Contact_By_Id_Returns_No_Content_When_Contact_Not_Found()
        {
            //Act

            var result = await ContactEndpoint.GetContactByIdAsync(_mediatorMock.Object, Guid.NewGuid());

            //Assert
            var okResult = (NoContent)result;
            Assert.IsType<NoContent>(result);
            Assert.Equal(StatusCodes.Status204NoContent, okResult.StatusCode);
        }

        [Fact]
        public async Task Delete_Contact_Sucessfully()
        {
            //Act

            var result = await ContactEndpoint.DeleteContactAsync(_mediatorMock.Object, Guid.NewGuid());

            //Assert
            var okResult = (Ok)result;
            Assert.IsType<Ok>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
    }
}
