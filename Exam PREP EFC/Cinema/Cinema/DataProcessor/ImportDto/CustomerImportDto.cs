
using Cinema.Data.Models.Constant;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class CustomerImportDto
    {
        [XmlElement("FirstName")]
        [Required]
        [StringLength(GlobalConstants.MaxLenghtFistName, MinimumLength = GlobalConstants.MinLenghtFirstName)]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        [Required]
        [StringLength(GlobalConstants.MaxLenghtLastName, MinimumLength = GlobalConstants.MinLenghtLasttName)]
        public string LastName { get; set; }

        [XmlElement("Age")]
        [Required]
        [Range(GlobalConstants.MinAge, GlobalConstants.MaxAge)]
        public int Age { get; set; }

        [XmlElement("Balance")]
        [Required]
        [Range(GlobalConstants.MinBalanceValue, Double.MaxValue)]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public CustomerTicketstDto[] Tickets { get; set; }

    }
    [XmlType("Ticket")]
    public class CustomerTicketstDto
    {
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
