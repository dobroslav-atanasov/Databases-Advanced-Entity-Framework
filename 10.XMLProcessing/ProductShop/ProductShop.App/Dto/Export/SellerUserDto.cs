namespace ProductShop.App.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("user")]
    public class SellerUserDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public SoldProductDto[] Products { get; set; }
    }
}