using System.Xml;
using System.Xml.Serialization;

namespace Oligopoly
{
    [XmlRoot("Events")]
    public class Event
    {
        [XmlElement("Effect")]
        public int Effect { get; set; }

        [XmlElement("Target")]
        public string? Target { get; set; }

        [XmlElement("Type")]
        public string? Type { get; set; }

        [XmlElement("Title")]
        public string? Title { get; set; }

        [XmlElement("Content")]
        public string? Content { get; set; }
    }
}