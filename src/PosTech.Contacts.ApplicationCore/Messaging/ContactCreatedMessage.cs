﻿namespace PosTech.Contacts.ApplicationCore.Messaging
{
    public class ContactCreatedMessage
    {
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactSurname { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public int DddCode { get; set; }
        public string DddState { get; set; }
        public string RegionName { get; set; }

        public static explicit operator ContactCreatedMessage(Entities.Command.Contact contact)
        {
            return new ContactCreatedMessage
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

        public static explicit operator Entities.Query.Contact(ContactCreatedMessage contactCreatedMessage)
        {
            return new Entities.Query.Contact(contactCreatedMessage.ContactId)
            {
                DddCode = contactCreatedMessage.DddCode,
                DddState = contactCreatedMessage.DddState,
                Email = contactCreatedMessage.ContactEmail,
                Name = contactCreatedMessage.ContactName,
                Phone = contactCreatedMessage.ContactPhone,
                RegionName = contactCreatedMessage.RegionName,
                Surname = contactCreatedMessage.ContactSurname,
            };
        }
    }
}
