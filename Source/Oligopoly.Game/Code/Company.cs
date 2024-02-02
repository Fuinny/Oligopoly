namespace Oligopoly.Game;

public class Company
{
    private string name;
    private string industry;
    private string description;
    private decimal sharePrice;
    private int numberOfShares;

    /// <summary>Gets or sets the name of the company.</summary>
    /// <remarks>Name of the company must not be null or whitespace.</remarks>
    [XmlElement("Name")]
    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Name of the company must not be null or whitespace!");
            }
            else
            {
                name = value;
            }
        }
    }

    /// <summary>Gets or sets the industry of the company.</summary>
    /// <remarks>Industry of the company must not be null or whitespace.</remarks>
    [XmlElement("Industry")]
    public string Industry
    {
        get => industry;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Industry of the company must not be null or whitespace!");
            }
            else
            {
                industry = value;
            }
        }
    }

    /// <summary>Gets or sets the description of the company.</summary>
    /// <remarks>Description of the company must not be null or whitespace.</remarks>
    [XmlElement("Description")]
    public string Description
    {
        get => description;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Description of the company must not be null or whitespace!");
            }
            else
            {
                description = value;
            }
        }
    }

    /// <summary>Gets or sets the share price of the company.</summary>
    /// <remarks>Share price of the company must be greater than zero.</remarks>
    [XmlElement("SharePrice")]
    public decimal SharePrice
    {
        get => sharePrice;
        set
        {
            if (value <= 0) 
            {
                throw new Exception("Share price of the company must be greater than zero!");
            }
            else
            {
                sharePrice = value;
            }
        }
    }

    /// <summary>Gets or sets the number of shares of the company.</summary>
    /// <remarks>Number of shares of the company must not be less than zero.</remarks>
    [XmlElement("NumberOfShares")]
    public int NumberOfShares
    {
        get => numberOfShares;
        set
        {
            if (value < 0)
            {
                throw new Exception("Number of shares of the company must not be less than zero!");
            }
            else
            {
                numberOfShares = value;
            }
        }
    }
}