using System.Xml;
using System.Xml.Serialization;

namespace Oligopoly
{
    [XmlRoot("Company")]
    public class Company
    {
        // Create a class fields.
        public string? name;
        public string? ticker;
        public string? industry;
        public string? description;
        public double sharePrice;

        [XmlElement("Name")]
        public string? Name
        {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Name cannot be null or whitespace.");
                }
                else
                {
                    name = value;
                }
            }
        }

        [XmlElement("Ticker")]
        public string? Ticker
        {
            get
            {
                return ticker;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException("Ticker cannot be null or whitespace.");
                }
                else
                {
                    ticker = value;
                }
            }
        }

        [XmlElement("Industry")]
        public string? Industry
        {
            get
            {
                return industry;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException("Industry cannot be null of whitespace.");
                }
                else
                {
                    industry = value;
                }
            }
        }

        [XmlElement("SharePrice")]
        public double SharePrice
        {
            get
            {
                return sharePrice;
            }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidOperationException("Share price cannot be less or equal to zero.");
                }
                else
                {
                    sharePrice = value;
                }
            }
        }

        [XmlElement("Description")]
        public string? Description
        {
            get
            {
                return description;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException("Description cannot be null or whitespace.");
                }
                else
                {
                    description = value;
                }
            }
        }
    }
}