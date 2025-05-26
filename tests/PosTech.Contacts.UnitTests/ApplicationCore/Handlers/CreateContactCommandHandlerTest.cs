using AutoMapper;
using Moq;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Entities.Command;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Messaging;
using PosTech.Contacts.ApplicationCore.Repositories.Command;

namespace PosTech.Contacts.UnitTests.ApplicationCore.Handlers
{
    public class CreateContactCommandHandlerTest
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly Mock<IDddRepository> _dddRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMessagingProducer> _messagingProducerMock;
        private readonly CreateContactCommandHandler _createContactCommandHandler;

        public CreateContactCommandHandlerTest()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _dddRepositoryMock = new Mock<IDddRepository>();
            _mapperMock = new Mock<IMapper>();
            _messagingProducerMock = new Mock<IMessagingProducer>();

            _createContactCommandHandler = new CreateContactCommandHandler(
                _contactRepositoryMock.Object,
                _dddRepositoryMock.Object, 
                _mapperMock.Object,
                _messagingProducerMock.Object);
        }

        [Fact]
        public async Task Create_Contact_Successfully()
        {
            //Arrange
            var ddd = new Ddd
            {
                DddCode = 31,
                Id = Guid.NewGuid(),
                State = "MG",
                Region = new Region { RegionName = "Região Sudeste" }
            };
            var contactResponse = new ContactResponseDto
            {
                Ddd = ddd.DddCode,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };

            _dddRepositoryMock.Setup(x => x.GetByDddCodeAsync(It.IsAny<int>())).ReturnsAsync(ddd);
            _mapperMock.Setup(x => x.Map<ContactResponseDto>(It.IsAny<Contact>())).Returns(contactResponse);

            //Act

            var result = await _createContactCommandHandler.Handle(new CreateContactCommand(), new CancellationToken());

            //Assert
            _dddRepositoryMock.Verify(x => x.GetByDddCodeAsync(It.IsAny<int>()), Times.Once());
            _contactRepositoryMock.Verify(x => x.AddContactAsync(It.IsAny<Contact>()), Times.Once());
            Assert.NotNull(result);
            Assert.Equal(contactResponse.Ddd, result.Ddd);
            Assert.Equal(contactResponse.Email, result.Email);
            Assert.Equal(contactResponse.Name, result.Name);
            Assert.Equal(contactResponse.Phone, result.Phone);
            Assert.Equal(contactResponse.Surname, result.Surname);
        }
    }
}
