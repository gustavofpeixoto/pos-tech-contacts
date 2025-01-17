using Moq;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Commands;

namespace Test.ApplicationCore.Handlers
{
    public class DeleteContactCommandHandlerTest
    {
        private readonly Mock<IContactRepository> _contactRepositoryMock;
        private readonly DeleteContactCommandHandler _deleteContactCommandHandler;

        public DeleteContactCommandHandlerTest()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _deleteContactCommandHandler = new DeleteContactCommandHandler(_contactRepositoryMock.Object);
        }

        [Fact]
        public async Task Delete_Contact_Successfully()
        {
            //Act
            await _deleteContactCommandHandler.Handle(new DeleteContactCommand(), new CancellationToken());

            //Assert
            _contactRepositoryMock.Verify(x => x.DeleteContactAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
