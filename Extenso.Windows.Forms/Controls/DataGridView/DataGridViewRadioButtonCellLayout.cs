﻿//Source: http://msdn2.microsoft.com/en-us/library/aa730882(VS.80).aspx

namespace Extenso.Windows.Forms.Controls;

/// <summary>
/// Internal class that represents the layout of a DataGridViewRadioButtonCell cell.
/// It tracks the first displayed item index, the number of displayed items,
/// scrolling information and various location/size information.
/// </summary>
internal class DataGridViewRadioButtonCellLayout
{
    private Rectangle contentBounds;
    private int displayedItemsCount;
    private Point downButtonLocation;
    private int firstDisplayedItemIndex;
    private Point firstDisplayedItemLocation;
    private Size radioButtonsSize;
    private Size scrollButtonsSize;
    private bool scrollingNeeded;
    private int totallyDisplayedItemsCount;
    private Point upButtonLocation;

    public DataGridViewRadioButtonCellLayout()
    {
    }

    /// <summary>
    /// Boundaries of the cell content defined as the radio buttons of the displayed items.
    /// </summary>
    public Rectangle ContentBounds
    {
        get
        {
            return this.contentBounds;
        }
        set
        {
            this.contentBounds = value;
        }
    }

    /// <summary>
    /// Number of displayed items (includes potential partially displayed one).
    /// </summary>
    public int DisplayedItemsCount
    {
        get
        {
            return this.displayedItemsCount;
        }
        set
        {
            this.displayedItemsCount = value;
        }
    }

    /// <summary>
    /// Location of the Down scroll button.
    /// </summary>
    public Point DownButtonLocation
    {
        get
        {
            return this.downButtonLocation;
        }
        set
        {
            this.downButtonLocation = value;
        }
    }

    /// <summary>
    /// Index of the first displayed item.
    /// </summary>
    public int FirstDisplayedItemIndex
    {
        get
        {
            return this.firstDisplayedItemIndex;
        }
        set
        {
            this.firstDisplayedItemIndex = value;
        }
    }

    /// <summary>
    /// Location of the top most displayed item.
    /// </summary>
    public Point FirstDisplayedItemLocation
    {
        get
        {
            return this.firstDisplayedItemLocation;
        }
        set
        {
            this.firstDisplayedItemLocation = value;
        }
    }

    /// <summary>
    /// Size of the radio button glyphs.
    /// </summary>
    public Size RadioButtonsSize
    {
        get
        {
            return this.radioButtonsSize;
        }
        set
        {
            this.radioButtonsSize = value;
        }
    }

    /// <summary>
    /// Size of the scroll buttons.
    /// </summary>
    public Size ScrollButtonsSize
    {
        get
        {
            return this.scrollButtonsSize;
        }
        set
        {
            this.scrollButtonsSize = value;
        }
    }

    /// <summary>
    /// Indicates whether the scroll buttons need to be shown or not.
    /// </summary>
    public bool ScrollingNeeded
    {
        get
        {
            return this.scrollingNeeded;
        }
        set
        {
            this.scrollingNeeded = value;
        }
    }

    /// <summary>
    /// Number of totally displayed items.
    /// </summary>
    public int TotallyDisplayedItemsCount
    {
        get
        {
            return this.totallyDisplayedItemsCount;
        }
        set
        {
            this.totallyDisplayedItemsCount = value;
        }
    }

    /// <summary>
    /// Location of the Up scroll button.
    /// </summary>
    public Point UpButtonLocation
    {
        get
        {
            return this.upButtonLocation;
        }
        set
        {
            this.upButtonLocation = value;
        }
    }
}