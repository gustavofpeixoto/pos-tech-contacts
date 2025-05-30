﻿using System.Text.RegularExpressions;

namespace PosTech.Contacts.ApplicationCore.Validators
{
    public static class CommonValidators
    {
        private static readonly int[] Ddds = { 11, 12, 13, 14, 15, 16, 17, 18, 19, 21, 22, 24, 27, 28, 31, 32, 33, 34, 35,
            37, 38, 41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 61, 62, 63, 64, 65, 66, 67, 68, 69,
            71, 73, 74, 75, 77, 79, 81, 82, 83, 84, 85, 86, 87, 88, 89, 91, 92, 93, 94, 95, 96, 97, 98, 99 };

        public static bool ValidateDdd(int ddd) => Ddds.Contains(ddd);

        public static bool ValidatePhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            var phonepattern = @"^(9\d{8}|\d{8})$";
            var regex = new Regex(phonepattern);

            return regex.IsMatch(phone);
        }
    }
}
