using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Oligopoly.Game;

public class Event
{
    private int effect;
    private string target;
    private string title;
    private string content;

    /// <summary>
    /// Gets or sets the effect of the event.
    /// That is, the value by which the price of the <see cref="Company.SharePrice"/> should change.
    /// The effect cannot be equal to zero.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the target of the event.
    /// That is, the company to which the <see cref="Event.Effect"/> will be applied.
    /// The target cannot be null or whitespace.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the title of the event.
    /// The title cannot be null or whitespace.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the content of the event.
    /// The content cannot be null or whitespace.
    /// </summary>
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