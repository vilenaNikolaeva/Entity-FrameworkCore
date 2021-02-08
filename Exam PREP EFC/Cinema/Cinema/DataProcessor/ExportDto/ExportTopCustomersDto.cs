using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ExportDto
{
    [XmlType("Customer")]
    public class ExportTopCustomersDto
    {
        [XmlArray("Customer")]
        public ExportCustomersDTO[] Customers { get; set; }
    }

    public class ExportCustomersDTO
    {
        [XmlElement("SpendtMoney")]
        public string SpentMoney { get; set; }

        [XmlElement("SpentTime")]
        public string SpentTime { get; set; }


        [XmlAttribute("FistName")]
        public string FistName { get; set; }

        [XmlAttribute("LastName")]
        public string LastName { get; set; }

    }
}
