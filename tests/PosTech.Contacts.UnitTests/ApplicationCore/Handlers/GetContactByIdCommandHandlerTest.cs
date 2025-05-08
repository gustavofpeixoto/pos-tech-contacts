using AutoMapper;
using Moq;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Services;

namespace PosTech.Contacts.UnitTests.ApplicationCore.Handlers
{
    public class GetContactByIdCommandHandlerTest
    {
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IAddContactRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetContactByIdCommandHandler _getContactByIdCommandHandler;

        public GetContactByIdCommandHandlerTest()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _repositoryMock = new Mock<IAddContactRepository>();
            _mapperMock = new Mock<IMapper>();
            _getContactByIdCommandHandler = new GetContactByIdCommandHandler(_cacheServiceMock.Object,
                _repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Get_Contact_By_Id_Without_Access_Data_Base()
        {
            //Arrange
            var ddd = new Ddd { DddCode = 31, Id = Guid.NewGuid(), State = "MG" };
            var contactResponse = new ContactResponseDto
            {
                Ddd = ddd.DddCode,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };

            _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<string>(), out contactResponse)).Returns(true);

            //Act
            var result = await _getContactByIdCommandHandler.Handle(new GetContactByIdCommand(), new CancellationToken());

            //Assert
            _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<ContactResponseDto>.IsAny), Times.Once);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never());
            Assert.NotNull(result);
            Assert.Equal(contactResponse, result);
        }

        [Fact]
        public async Task Get_Contact_By_Id_Accessing_Data_Base()
        {
            //Arrange
            var ddd = new Ddd { DddCode = 31, Id = Guid.NewGuid(), State = "MG" };
            var contact = new Contact
            {
                Ddd = ddd,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };
            var contactResponse = new ContactResponseDto
            {
                Ddd = ddd.DddCode,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };

            _repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(contact);
            _mapperMock.Setup(x => x.Map<ContactResponseDto>(It.IsAny<Contact>())).Returns(contactResponse);

            //Act
            var result = await _getContactByIdCommandHandler.Handle(new GetContactByIdCommand(), new CancellationToken());

            //Assert
            _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<ContactResponseDto>.IsAny), Times.Once);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once());
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<ContactResponseDto>(), It.IsAny<TimeSpan>()), Times.Once);
            Assert.Equal(contact.Ddd.DddCode, result.Ddd);
            Assert.Equal(contact.Email, result.Email);
            Assert.Equal(contact.Name, result.Name);
            Assert.Equal(contact.Phone, result.Phone);
            Assert.Equal(contact.Surname, result.Surname);
        }
    }
}
