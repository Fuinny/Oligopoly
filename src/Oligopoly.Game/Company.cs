namespace Oligopoly.Game;

public class Company
{
    private string _name;
    private string _industry;
    private string _description;
    private decimal _sharePrice;
    private int _numberOfShares;

    /// <summary>Gets or sets the name of the company.</summary>
    /// <remarks>Name of the company must not be null or whitespace.</remarks>
    [XmlElement("Name")]
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Name of the company must not be null or whitespace!");
            }
            else
            {
                _name = value;
            }
        }
    }

    /// <summary>Gets or sets the industry of the company.</summary>
    /// <remarks>Industry of the company must not be null or whitespace.</remarks>
    [XmlElement("Industry")]
    public string Industry
    {
        get => _industry;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Industry of the company must not be null or whitespace!");
            }
            else
            {
                _industry = value;
            }
        }
    }

    /// <summary>Gets or sets the description of the company.</summary>
    /// <remarks>Description of the company must not be null or whitespace.</remarks>
    [XmlElement("Description")]
    public string Description
    {
        get => _description;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new Exception("Description of the company must not be null or whitespace!");
            }
            else
            {
                _description = value;
            }
        }
    }

    /// <summary>Gets or sets the share price of the company.</summary>
    /// <remarks>Share price of the company must be greater than zero.</remarks>
    [XmlElement("SharePrice")]
    public decimal SharePrice
    {
        get => _sharePrice;
        set
        {
            if (value <= 0)
            {
                throw new Exception("Share price of the company must be greater than zero!");
            }
            else
            {
                _sharePrice = value;
            }
        }
    }

    /// <summary>Gets or sets the number of shares of the company.</summary>
    /// <remarks>Number of shares of the company must not be less than zero.</remarks>
    [XmlElement("NumberOfShares")]
    public int NumberOfShares
    {
        get => _numberOfShares;
        set
        {
            if (value < 0)
            {
                throw new Exception("Number of shares of the company must not be less than zero!");
            }
            else
            {
                _numberOfShares = value;
            }
        }
    }
}