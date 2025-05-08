using Microsoft.EntityFrameworkCore;
using PosTech.Contacts.ApplicationCore.Constants;
using PosTech.Contacts.ApplicationCore.Entities;
using PosTech.Contacts.Infrastructure;
using PosTech.Contacts.Infrastructure.Repositories.Sql;

namespace PosTech.Contacts.IntegrationTests
{
    [CollectionDefinition(CollectionConst.DatabaseCollection)]
    public class ContactRepositoryTests : IClassFixture<DatabaseFixture>
    {
        private readonly ApplicationDbContext _context;
        private readonly ContactRepository _repository;

        public ContactRepositoryTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _repository = new ContactRepository(_context);
        }

        [Fact]
        public async Task Add_New_Contact_Should_Recover_It()
        {
            #region Arrange
            var regionId = Guid.NewGuid();
            var dddId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var region = new Region { Id = regionId, RegionName = "Região Sudeste" };
            var ddd = new Ddd() { DddCode = 31, Id = dddId, RegionId = region.Id, State = "MG" };
            var contact = new Contact(contactId) { DddId = ddd.Id, Name = "João", Surname = "da Silva", Phone = "998971312" };
            #endregion

            #region Act
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            await _context.Ddds.AddAsync(ddd);
            await _context.SaveChangesAsync();
            await _repository.AddContactAsync(contact);
            
            var storedContact = await _repository.GetByIdAsync(contact.Id);
            #endregion

            #region Assert
            Assert.NotNull(storedContact);
            Assert.Equal(contact.Id, storedContact.Id);
            #endregion
        }

        [Fact]
        public async Task Delete_Added_Contact_Should_Not_Be_Able_To_Recover_It()
        {
            #region Arrange
            var regionId = Guid.NewGuid();
            var dddId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var region = new Region { Id = regionId, RegionName = "Região Sudeste" };
            var ddd = new Ddd() { DddCode = 31, Id = dddId, RegionId = region.Id, State = "MG" };
            var contact = new Contact(contactId) { DddId = ddd.Id, Name = "João", Surname = "da Silva", Phone = "998971312" };
            #endregion

            #region Act
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            await _context.Ddds.AddAsync(ddd);
            await _context.SaveChangesAsync();
            await _repository.AddContactAsync(contact);
            
            _context.Entry(contact).State = EntityState.Detached;
            
            await _repository.DeleteContactAsync(contact.Id);
            
            var storedContact = await _repository.GetByIdAsync(contact.Id);
            #endregion

            #region Assert
            Assert.Null(storedContact);
            #endregion
        }

        [Fact]
        public async Task Update_Contact_Properties_Contact_Properties_Must_Be_Updated_When_Retrieving_It()
        {
            #region Arrange
            var regionId = Guid.NewGuid();
            var dddId = Guid.NewGuid();
            var contactId = Guid.NewGuid();
            var region = new Region { Id = regionId, RegionName = "Região Sudeste" };
            var ddd = new Ddd() { DddCode = 31, Id = dddId, RegionId = region.Id, State = "MG" };
            var contact = new Contact(contactId) { DddId = ddd.Id, Name = "João", Surname = "da Silva", Phone = "998971312" };
            #endregion

            #region Act
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            await _context.Ddds.AddAsync(ddd);
            await _context.SaveChangesAsync();
            await _repository.AddContactAsync(contact);

            var storedContact = await _repository.GetByIdAsync(contact.Id);
            var email = "joao.da.silva@emailprovider.com";

            storedContact.Email = email;

            await _repository.UpdateContactAsync(storedContact);

            storedContact = await _repository.GetByIdAsync(storedContact.Id);

            #endregion

            #region Assert
            Assert.NotNull(storedContact);
            Assert.Equal(contact.Id, storedContact.Id);
            Assert.Equal(email, storedContact.Email);
            #endregion
        }

        [Fact]
        public async Task Find_Added_Contact_Should_Recover_Added_Contacts_Using_Expression()
        {
            #region Arrange
            var regionId = Guid.NewGuid();
            var dddId = Guid.NewGuid();
            var JoaoContactId = Guid.NewGuid();
            var MariaContactId = Guid.NewGuid();
            var region = new Region { Id = regionId, RegionName = "Região Sudeste" };
            var ddd = new Ddd() { DddCode = 31, Id = dddId, RegionId = region.Id, State = "MG" };
            var JoaoContact = new Contact(JoaoContactId) { DddId = ddd.Id, Name = "João", Surname = "da Silva", Phone = "998971312" };
            var MariaContact = new Contact(MariaContactId) { DddId = ddd.Id, Name = "Maria", Surname = "Fernandes", Phone = "991923839" };
            #endregion

            #region Act
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
            await _context.Ddds.AddAsync(ddd);
            await _context.SaveChangesAsync();
            await _repository.AddContactAsync(JoaoContact);
            await _repository.AddContactAsync(MariaContact);

            var contactsByDddCode = await _repository.FindContactsAsync(contact => contact.Ddd.DddCode == ddd.DddCode);

            #endregion

            #region Assert
            Assert.NotNull(contactsByDddCode);
            Assert.NotEmpty(contactsByDddCode);
            Assert.Equal(2, contactsByDddCode.Count());
            #endregion
        }
    }
}
