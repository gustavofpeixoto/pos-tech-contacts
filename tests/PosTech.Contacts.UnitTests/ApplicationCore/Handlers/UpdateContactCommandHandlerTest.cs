using AutoMapper;
using Moq;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories;

namespace PosTech.Contacts.UnitTests.ApplicationCore.Handlers
{
    public class UpdateContactCommandHandlerTest
    {
        private readonly Mock<IAddContactRepository> _repositoryMock;
        private readonly Mock<IDddRepository> _dddRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMessagingProducer> _messagingProducerMock;
        private readonly UpdateContactCommandHandler _updateContactCommandHandler;

        public UpdateContactCommandHandlerTest()
        {
            _repositoryMock = new Mock<IAddContactRepository>();
            _dddRepositoryMock = new Mock<IDddRepository>();
            _mapperMock = new Mock<IMapper>();
            _messagingProducerMock = new Mock<IMessagingProducer>();
            _updateContactCommandHandler = new UpdateContactCommandHandler(
                _repositoryMock.Object,
                _dddRepositoryMock.Object,
                _mapperMock.Object,
                _messagingProducerMock.Object);
        }

        [Fact]
        public async Task Update_Contact_Successfully()
        {
            //Arrange
            var ddd = new Ddd
            {
                DddCode = 31,
                Id = Guid.NewGuid(),
                State = "MG",
                Region = new Region { RegionName = "Região Sudeste" }
            };
            var storageContact = new Contact
            {
                Ddd = ddd,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };
            var contactResponse = new ContactResponseDto
            {
                Ddd = 31,
                Email = "newemail@gmail.com",
                Name = "NewValidName",
                Phone = "88888888",
                Surname = "NewValidSurname"
            };
            var command = new UpdateContactCommand
            {
                Ddd = 31,
                Email = "newemail@gmail.com",
                Name = "NewValidName",
                Phone = "88888888",
                Surname = "NewValidSurname"
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(storageContact);
            _mapperMock.Setup(x => x.Map<ContactResponseDto>(It.IsAny<Contact>())).Returns(contactResponse);

            //Act
            var result = await _updateContactCommandHandler.Handle(command, new CancellationToken());

            //Assert
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once());
            _repositoryMock.Verify(x => x.UpdateContactAsync(It.IsAny<Contact>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal(command.Email, result.Email);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Phone, result.Phone);
            Assert.Equal(command.Surname, result.Surname);

        }
    }
}
