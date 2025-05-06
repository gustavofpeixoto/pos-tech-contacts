using PosTech.Contacts.ApplicationCore.Entities;

namespace PosTech.Contacts.ApplicationCore.Messaging
{
    public class ContactUpdatedMessage
    {
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int DddCode { get; set; }
        public string DddState { get; set; }
        public string RegionName { get; set; }

        public static explicit operator ContactUpdatedMessage(Contact contact)
        {
            return new ContactUpdatedMessage
            {
                ContactEmail = contact.Email,
                ContactId = contact.Id,
                ContactName = contact.Name,
                ContactPhone = contact.Phone,
                ContactSurname = contact.Surname,
                DddCode = contact.Ddd.DddCode,
                DddState = contact.Ddd.State,
                RegionName = contact.Ddd.Region.RegionName,
            };
        }
    }
}
