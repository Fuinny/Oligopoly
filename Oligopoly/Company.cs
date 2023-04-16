using System.Xml;
using System.Xml.Serialization;

namespace Oligopoly
{
    [XmlRoot("Company")]
    public class Company
    {
        [XmlElement("Name")]
        public string? Name { get; set; }

        [XmlElement("Ticker")]
        public string? Ticker { get; set; }

        [XmlElement("Industry")]
        public string? Industry { get; set; }

        [XmlElement("SharePrice")]
        public int SharePrice { get; set; }

        [XmlElement("Description")]
        public string? Description { get; set; }
    }
}