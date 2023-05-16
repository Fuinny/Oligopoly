using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly
{
    public class Program
    {
        private static List<Company> Companies = new List<Company>();
        private static List<Event> Events = new List<Event>();

        public static void Main(string[] args)
        {

        }

        private static void LoadEmbeddedResources()
        {
            try
            {
                XDocument document;
                {
                    document = XDocument.Load("Data\\Companies.xml");
                    foreach (XElement companyElement in document.Root.Elements("Company"))
                    {
                        Company currentCompany = new Company
                        {
                            Name = companyElement.Element("Name").Value,
                            Industry = companyElement.Element("Industry").Value,
                            SharePrice = decimal.Parse(companyElement.Element("SharePrice").Value),
                            Description = companyElement.Element("Description").Value
                        };

                        Companies.Add(currentCompany);
                    }
                }
                {
                    document = XDocument.Load("Data\\Events.xml");
                    foreach (XElement eventElement in document.Root.Elements("Event"))
                    {
                        Event currentEvent = new Event
                        {
                            Effect = int.Parse(eventElement.Element("Effect").Value),
                            Target = eventElement.Element("Target").Value,
                            Title = eventElement.Element("Title").Value,
                            Content = eventElement.Element("Content").Value
                        };

                        Events.Add(currentEvent);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error! \nDetails: {ex.Message}");
                Console.WriteLine("Press any key to exit the game...");
                Console.ReadKey(true);
            }
        }
    }
}