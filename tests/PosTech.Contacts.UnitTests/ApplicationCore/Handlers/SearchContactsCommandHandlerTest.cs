using AutoMapper;
using Moq;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.DTOs.Responses;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Services;
using System.Linq.Expressions;

namespace PosTech.Contacts.UnitTests.ApplicationCore.Handlers
{
    public class SearchContactsCommandHandlerTest
    {
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IAddContactRepository> _repositoryMock;
        private readonly Mock<IDddRepository> _dddRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SearchContactsCommandHandler _searchContactsCommandHandler;

        public SearchContactsCommandHandlerTest()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _repositoryMock = new Mock<IAddContactRepository>();
            _dddRepositoryMock = new Mock<IDddRepository>();
            _mapperMock = new Mock<IMapper>();
            _searchContactsCommandHandler = new SearchContactsCommandHandler(_cacheServiceMock.Object, _repositoryMock.Object,
                _dddRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Get_Contact_List_Without_Access_Data_Base()
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
            var contactResponseList = new List<ContactResponseDto> { contactResponse };

            _cacheServiceMock.Setup(x => x.TryGetValue(It.IsAny<string>(), out contactResponseList)).Returns(true);

            //Act
            var result = await _searchContactsCommandHandler.Handle(new SearchContactsCommand(), new CancellationToken());

            _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<List<ContactResponseDto>>.IsAny), Times.Once);
            _repositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never());
            Assert.NotNull(result);
            Assert.Equal(contactResponseList, result);
        }

        [Fact]
        public async Task Get_Contact_List_Accessing_Data_Base()
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
            var contactList = new List<Contact> { contact };
            var contactResponse = new ContactResponseDto
            {
                Ddd = ddd.DddCode,
                Email = "validemail@email.com",
                Name = "ValidName",
                Phone = "99999999",
                Surname = "ValidSurname"
            };
            var contactResponseList = new List<ContactResponseDto> { contactResponse };
            var command = new SearchContactsCommand
            {
                DddCode = ddd.DddCode,
                Email = "validemail@email.com"
            };

            _repositoryMock.Setup(x => x.FindContactsAsync(It.IsAny<Expression<Func<Contact, bool>>>()))
                .ReturnsAsync(contactList);
            _mapperMock.Setup(x => x.Map<List<ContactResponseDto>>(It.IsAny<List<Contact>>())).Returns(contactResponseList);
            _dddRepositoryMock.Setup(x => x.GetByDddCodeAsync(It.IsAny<int>())).ReturnsAsync(ddd);

            //Act
            var result = await _searchContactsCommandHandler.Handle(command, new CancellationToken());

            //Assert
            _cacheServiceMock.Verify(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<List<ContactResponseDto>>.IsAny), Times.Once);
            _repositoryMock.Verify(x => x.FindContactsAsync(It.IsAny<Expression<Func<Contact, bool>>>()), Times.Once());
            _cacheServiceMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<List<ContactResponseDto>>(), It.IsAny<TimeSpan>()), Times.Once);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
