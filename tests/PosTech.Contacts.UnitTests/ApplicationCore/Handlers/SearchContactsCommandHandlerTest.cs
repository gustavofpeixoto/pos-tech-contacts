using Moq;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities.Query;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Repositories.Query;
using PosTech.Contacts.ApplicationCore.Services;
using System.Linq.Expressions;

namespace PosTech.Contacts.UnitTests.ApplicationCore.Handlers
{
    public class SearchContactsCommandHandlerTest
    {
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly SearchContactsCommandHandler _searchContactsCommandHandler;

        public SearchContactsCommandHandlerTest()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _contactRepositoryMock = new Mock<IContactRepository>();
            _searchContactsCommandHandler = new SearchContactsCommandHandler(_cacheServiceMock.Object, _contactRepositoryMock.Object);
        }

        [Fact]
        public async Task Get_Contact_List_Without_Access_Data_Base()
        {
            //Arrange
            var contactResponse = new ContactResponseDto
            {
                Ddd = 31,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };
            var contactResponseList = new List<ContactResponseDto> { contactResponse };

            _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<string>(), out contactResponseList)).Returns(true);

            //Act
            var result = await _searchContactsCommandHandler.Handle(new SearchContactsCommand(), new CancellationToken());

            _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<List<ContactResponseDto>>.IsAny), Times.Once);
            _contactRepositoryMock.Verify(x => x.GetAsync(It.IsAny<Guid>()), Times.Never());

            //Assert

            Assert.NotNull(result);
            Assert.Equal(contactResponseList, result);
        }

        [Fact]
        public async Task Get_Contact_List_Accessing_Data_Base()
        {
            //Arrange
            var contactId = Guid.NewGuid();
            var contact = new Contact(contactId)
            {
                DddCode = 31,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };
            var contactList = new List<Contact> { contact };
            var contactResponse = new ContactResponseDto
            {
                Ddd = contact.DddCode,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };
            var contactResponseList = new List<ContactResponseDto> { contactResponse };
            var command = new SearchContactsCommand
            {
                DddCode = 31,
                Email = "validemail@email.com"
            };

            _contactRepositoryMock.Setup(x => x.FindContactsAsync(It.IsAny<Expression<Func<Contact, bool>>>()))
                .ReturnsAsync(contactList);

            //Act
            var result = await _searchContactsCommandHandler.Handle(command, new CancellationToken());

            //Assert
            _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<List<ContactResponseDto>>.IsAny), Times.Once);
            _contactRepositoryMock.Verify(x => x.FindContactsAsync(It.IsAny<Expression<Func<Contact, bool>>>()), Times.Once());
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<List<ContactResponseDto>>(), It.IsAny<TimeSpan>()), Times.Once);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
