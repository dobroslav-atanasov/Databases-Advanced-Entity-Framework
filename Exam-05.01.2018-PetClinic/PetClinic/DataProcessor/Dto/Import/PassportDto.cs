namespace PetClinic.DataProcessor.Dto.Import
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class PassportDto
    {
        [RegularExpression("^([A-z]{7}[0-9]{3})$")]
        public string SerialNumber { get; set; }

        [RegularExpression(@"^(\+359|0)[0-9]{9}$")]
        public string OwnerPhoneNumber { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string OwnerName { get; set; }

        public string RegistrationDate { get; set; }
    }
}