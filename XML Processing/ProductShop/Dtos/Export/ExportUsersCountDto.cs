using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ExportUsersCountDto
    {
        [XmlElement("count")]

        public int Count { get; set; }

        [XmlElement("users")]

        public ExportUsersDto[] Users { get; set; }

    }
    [XmlType("User")]
    public class ExportUsersDto
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]

        public string LastName { get; set; }

        [XmlElement("age")]

        public int? Age { get; set; }

        [XmlElement("products")]

        public ExportProductCountDto SoldProduct { get; set; }

    }

    public class ExportProductCountDto
    {
        [XmlElement("count")]

        public int  Count { get; set; }

        [XmlArray("products")]

        public ExportProducDto[] Products { get; set; }

    }
    [XmlType("Product")]
    public class ExportProducDto
    {
        [XmlElement("name")]

        public string Name { get; set; }

        [XmlElement("price")]

        public decimal Price { get; set; }

    }
}
