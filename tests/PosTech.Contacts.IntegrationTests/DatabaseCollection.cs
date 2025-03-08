using PosTech.Contacts.ApplicationCore.Constants;

namespace PosTech.Contacts.IntegrationTests
{
    [CollectionDefinition(CollectionConst.DatabaseCollection)]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // Class used only for Collection Definition
    }
}