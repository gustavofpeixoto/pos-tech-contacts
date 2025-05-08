using Moq;
using PosTech.Contacts.ApplicationCore.Repositories;
using PosTech.Contacts.ApplicationCore.Handlers;
using PosTech.Contacts.ApplicationCore.Commands;
using PosTech.Contacts.ApplicationCore.Messaging;

namespace PosTech.Contacts.UnitTests.ApplicationCore.Handlers
{
    public class DeleteContactCommandHandlerTest
    {
        private readonly Mock<IAddContactRepository> _repositoryMock;
        private readonly Mock<IMessagingProducer> _messagingProducerMock;
        private readonly DeleteContactCommandHandler _deleteContactCommandHandler;

        public DeleteContactCommandHandlerTest()
        {
            _repositoryMock = new Mock<IAddContactRepository>();
            _messagingProducerMock = new Mock<IMessagingProducer>();
            _deleteContactCommandHandler = new DeleteContactCommandHandler(_repositoryMock.Object, _messagingProducerMock.Object);
        }

        [Fact]
        public async Task Delete_Contact_Successfully()
        {
            //Act
            await _deleteContactCommandHandler.Handle(new DeleteContactCommand(), new CancellationToken());

            //Assert
            _repositoryMock.Verify(x => x.DeleteContactAsync(It.IsAny<Guid>()), Times.Once);
        }
    }
}
