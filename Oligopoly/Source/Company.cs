using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly
{
    public class Company
    {
        private string name;
        private string industry;
        private string description;
        private decimal sharePrice;
        private int numberOfShares;

        [XmlElement("Name")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Name cannot be null or whitespace!");
                }
                else
                {
                    name = value;
                }
            }
        }

        [XmlElement("Industry")]
        public string Industry
        {
            get
            {
                return industry;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Industry cannot be null or whitespace!");
                }
                else
                {
                    industry = value;
                }
            }
        }

        [XmlElement("Description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Description cannot be null or whitespace!");
                }
                else
                {
                    description = value;
                }
            }
        }

        [XmlElement("SharePrice")]
        public decimal SharePrice
        {
            get
            {
                return sharePrice;
            }
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Share Price cannot be less than or equal to zero!");
                }
                else
                {
                    sharePrice = value;
                }
            }
        }

        public int NumberShares
        {
            get
            {
                return numberOfShares;
            }
            set
            {
                if (value < 0)
                {
                    throw new Exception("Number of Shares cannot be less than zero!");
                }
                else
                {
                    numberOfShares = value;
                }
            }
        }
    }
}