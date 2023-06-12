namespace Extenso.Windows.Forms.Wizard;

public class WizardPageCollection : Dictionary<int, IWizardPage>
{
    #region Properties

    /// <summary>
    /// <para>Determines whether the wizard is able to move to the next page.</para>
    /// <para>Will return false if Page Location is currently the last page.</para>
    /// <para>Otherwise, true.</para>
    /// </summary>
    public bool CanMoveNext
    {
        get
        {
            if (Count == 1)
            { return false; }

            if (Count > 0 && PageLocation != WizardPageLocation.End)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// <para>Determines whether the wizard is able to move to the previous page.</para>
    /// <para>Will return false if Page Location is currently the first page.</para>
    /// <para>Otherwise, true.</para>
    /// </summary>
    public bool CanMovePrevious
    {
        get
        {
            if (Count == 1)
            { return false; }

            if (Count > 0 && PageLocation != WizardPageLocation.Start)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// The current IWizardPage
    /// </summary>
    public IWizardPage CurrentPage { get; private set; }

    /// <summary>
    /// The first page in the collection
    /// </summary>
    public IWizardPage FirstPage => this[this.Min(x => x.Key)];

    /// <summary>
    /// The last page in the collection
    /// </summary>
    public IWizardPage LastPage => this[this.Max(x => x.Key)];

    /// <summary>
    /// The location of the current IWizardPage
    /// </summary>
    public WizardPageLocation PageLocation { get; private set; }

    #endregion Properties

    #region Constructor

    public WizardPageCollection()
    {
        PageLocation = WizardPageLocation.Start;
    }

    #endregion Constructor

    #region Delegates & Events

    public delegate void WizardPageLocationChangedEventHanlder(WizardPageLocationChangedEventArgs e);

    public event WizardPageLocationChangedEventHanlder WizardPageLocationChanged;

    #endregion Delegates & Events

    #region Public Methods

    /// <summary>
    /// Find the page number of the current page
    /// </summary>
    /// <param name="wizardPage">The IWiwardPage whose page number to retrieve.</param>
    /// <returns>Page number for the given IWizardPage</returns>
    public int IndexOf(IWizardPage wizardPage)
    {
        foreach (var kv in this)
        {
            if (kv.Value.Equals(wizardPage))
            {
                return kv.Key;
            }
        }
        return -1;
    }

    /// <summary>
    /// Moves to the first page in the collection
    /// </summary>
    /// <returns>First page as IWizard</returns>
    public IWizardPage MovePageFirst()
    {
        int previousPageIndex = IndexOf(CurrentPage);

        PageLocation = WizardPageLocation.Start;
        // Find the index of the first page
        int firstPageIndex = this.Min(x => x.Key);

        // Set the current page to be the first page
        CurrentPage = this[firstPageIndex];

        NotifyPageChanged(previousPageIndex);

        return CurrentPage;
    }

    /// <summary>
    /// Moves to the last page in the collection
    /// </summary>
    /// <returns>Last page as IWizard</returns>
    public IWizardPage MovePageLast()
    {
        int previousPageIndex = IndexOf(CurrentPage);

        PageLocation = WizardPageLocation.End;
        // Find the index of the last page
        int lastPageIndex = this.Max(x => x.Key);

        // Set the current page to be the last page
        CurrentPage = this[lastPageIndex];

        NotifyPageChanged(previousPageIndex);

        return CurrentPage;
    }

    /// <summary>
    /// Moves to the next page in the collection
    /// </summary>
    /// <returns>Next page as IWizard</returns>
    public IWizardPage MovePageNext()
    {
        int previousPageIndex = IndexOf(CurrentPage);

        if (PageLocation != WizardPageLocation.End &&
            CurrentPage != null)
        {
            // Find the index of the next page
            int nextPageIndex = this.Where(x => x.Key > IndexOf(CurrentPage)).Min(x => x.Key);

            // Find the index of the last page
            int lastPageIndex = this.Max(x => x.Key);

            // If the next page is the last page
            if (nextPageIndex == lastPageIndex)
            {
                PageLocation = WizardPageLocation.End;
            }
            else { PageLocation = WizardPageLocation.Middle; }

            // Set the current page to be the next page
            CurrentPage = this[nextPageIndex];
            NotifyPageChanged(previousPageIndex);

            return CurrentPage;
        }
        return null;
    }

    /// <summary>
    /// Moves to the previous page in the collection
    /// </summary>
    /// <returns>Previous page as IWizard</returns>
    public IWizardPage MovePagePrevious()
    {
        int prevPageIndex = IndexOf(CurrentPage);

        if (PageLocation != WizardPageLocation.Start && CurrentPage != null)
        {
            // Find the index of the previous page
            int previousPageIndex = this.Where(x => x.Key < IndexOf(CurrentPage)).Max(x => x.Key);

            // Find the index of the first page
            int firstPageIndex = this.Min(x => x.Key);

            // If the previous page is the first page
            if (previousPageIndex == firstPageIndex)
            {
                PageLocation = WizardPageLocation.Start;
            }
            else { PageLocation = WizardPageLocation.Middle; }

            CurrentPage = this[previousPageIndex];

            NotifyPageChanged(prevPageIndex);

            return CurrentPage;
        }
        return null;
    }

    public void Reset()
    {
        CurrentPage = null;
        PageLocation = WizardPageLocation.Start;
    }

    #endregion Public Methods

    #region private Methods

    private void NotifyPageChanged(int previousPageIndex)
    {
        if (WizardPageLocationChanged != null)
        {
            var e = new WizardPageLocationChangedEventArgs();
            e.PageLocation = PageLocation;
            e.PageIndex = IndexOf(CurrentPage);
            e.PreviousPageIndex = previousPageIndex;
            WizardPageLocationChanged(e);
        }
    }

    #endregion private Methods
}