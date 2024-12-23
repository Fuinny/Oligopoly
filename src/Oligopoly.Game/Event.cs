namespace Oligopoly.Game;

public class Event
{
    private int _effect;
    private string _target;
    private string _title;
    private string _content;

    /// <summary>Gets or sets the effect of the event, represented as a percentage.</summary>
    /// <remarks>Effect of the event must be a non-zero integer.</remarks>
    [XmlElement("Effect")]
    public int Effect
    {
        get => _effect;
        set
        {
            if (value == 0)
            {
                throw new Exception("Effect of the event must be a non-zero integer!");
            }
            else
            {
                _effect = value;
            }
        }
    }

    /// <summary>Gets or sets the target company of the event.</summary>
    /// <remarks>Target company of the event must not be null or whitespace.</remarks>
    [XmlElement("Target")]
    public string Target
    {
        get => _target;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Target company of the event must not be null or whitespace!");
            }
            else
            {
                _target = value;
            }
        }
    }

    /// <summary>Gets or sets the title of the event.</summary>
    /// <remarks>Title of the event must not be null or whitespace.</remarks>
    [XmlElement("Title")]
    public string Title
    {
        get => _title;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Title of the event must not be null or whitespace!");
            }
            else
            {
                _title = value;
            }
        }
    }

    /// <summary>Gets or sets the content of the event.</summary>
    /// <remarks>Content of the event must not be null or whitespace.</remarks>
    [XmlElement("Content")]
    public string Content
    {
        get => _content;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Content of the event must not be null or whitespace!");
            }
            else
            {
                _content = value;
            }
        }
    }
}