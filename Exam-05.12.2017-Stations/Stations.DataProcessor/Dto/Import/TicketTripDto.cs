namespace Stations.DataProcessor.Dto.Import
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Trip")]
    public class TicketTripDto
    {
        [Required]
        [XmlElement("OriginStation")]
        public string OriginStation { get; set; }

        [Required]
        [XmlElement("DestinationStation")]
        public string DestinationStation { get; set; }

        [Required]
        [XmlElement("DepartureTime")]
        public string DepartureTime { get; set; }
    }
}