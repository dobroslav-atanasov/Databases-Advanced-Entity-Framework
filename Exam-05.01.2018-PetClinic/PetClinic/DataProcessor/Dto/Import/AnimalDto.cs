namespace PetClinic.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;

    public class AnimalDto
    {
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Type { get; set; }

        [Range(1, 100)]
        public int Age { get; set; }

        public PassportDto Passport { get; set; }
    }
}