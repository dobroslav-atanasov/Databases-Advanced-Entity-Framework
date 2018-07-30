namespace ProductShop.App.Dto.Export
{
    using System.Xml.Serialization;

    [XmlType("sold-products")]
    public class ProductSoldRootDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("product")]
        public ProductSoldDto[] ProductSoldDtos { get; set; }
    }
}