namespace PetClinic.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Vet")]
    public class VetDto
    {
        [XmlElement("Name")]
        [StringLength(40, MinimumLength = 3)]
        public string Name { get; set; }

        [XmlElement("Profession")]
        [StringLength(50, MinimumLength = 3)]
        public string Profession { get; set; }

        [XmlElement("Age")]
        [Range(22, 65)]
        public int Age { get; set; }

        [XmlElement("PhoneNumber")]
        [RegularExpression(@"^(\+359|0)[0-9]{9}$")]
        public string PhoneNumber { get; set; }
    }
}