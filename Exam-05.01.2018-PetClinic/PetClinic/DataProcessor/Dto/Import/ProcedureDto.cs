namespace PetClinic.DataProcessor.Dto.Import
{
    using System.Xml.Serialization;

    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [XmlElement("Vet")]
        public string Vet { get; set; }

        [XmlElement("Animal")]
        public string Animal { get; set; }

        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public AnimalAidProcedureDto[] AnimalAids { get; set; }
    }
}