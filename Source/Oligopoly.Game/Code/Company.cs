namespace Oligopoly.Game;

public class Company
{
    private string name;
    private string industry;
    private string description;
    private decimal sharePrice;
    private int numberOfShares;

    /// <summary>
    /// Gets or sets the name of the company.
    /// The name cannot be null or whitespace.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the industry of the company.
    /// The industry cannot be null or whitespace.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the description of the company.
    /// The description cannot be null or whitespace.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the share price of the company.
    /// The share price cannot less than or equal to zero.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the number of shares of the company.
    /// The number of share cannot be less than or equal to zero.
    /// </summary>
    public int NumberOfShares
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