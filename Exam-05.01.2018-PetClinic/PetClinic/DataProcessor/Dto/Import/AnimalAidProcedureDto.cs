namespace PetClinic.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("AnimalAid")]
    public class AnimalAidProcedureDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }
    }
}