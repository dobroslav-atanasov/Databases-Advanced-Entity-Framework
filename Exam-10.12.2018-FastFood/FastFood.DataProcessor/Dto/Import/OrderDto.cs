namespace FastFood.DataProcessor.Dto.Import
{
    using System;
    using System.Xml.Serialization;

    [XmlType("Order")]
    public class OrderDto
    {
        [XmlElement("Customer")]
        public string Customer { get; set; }

        [XmlElement("Employee")]
        public string Employee { get; set; }

        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlElement("Type")]
        public string Type { get; set; }

        [XmlArray("Items")]
        public ItemXmlDto[] Items { get; set; }
    }
}