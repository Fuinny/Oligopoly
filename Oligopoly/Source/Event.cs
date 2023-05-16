using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly
{
    public class Event
    {
        private int effect;
        private string target;
        private string title;
        private string content;

        [XmlElement("Effect")]
        public int Effect
        {
            get
            {
                return effect;
            }
            set
            {
                if (value == 0)
                {
                    throw new Exception("Effect cannot be equal to zero!");
                }
                else
                {
                    effect = value;
                }
            }
        }

        [XmlElement("Target")]
        public string Target
        {
            get
            {
                return target;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Target cannot be null or whitespace!");
                }
                else
                {
                    target = value;
                }
            }
        }

        [XmlElement("Title")]
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Title cannot be null or whitespace!");
                }
                else
                {
                    title = value;
                }
            }
        }

        [XmlElement("Content")]
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Content cannot be null or whitespace!");
                }
                else
                {
                    content = value;
                }
            }
        }
    }
}
