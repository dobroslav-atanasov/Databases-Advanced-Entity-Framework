namespace Stations.DataProcessor.Dto.Export
{
    using System.Xml.Serialization;
    using Models.Enums;

    [XmlType("Card")]
    public class CardDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public CardType CardType { get; set; }

        [XmlArray("Tickets")]
        public TicketDto[] Tickets { get; set; }
    }
}