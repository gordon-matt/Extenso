using System.ComponentModel;

namespace Extenso.Windows.Forms.Wizard;

/// <summary>
/// Represents the wizard host.
/// </summary>
public partial class WizardHost : Form
{
    private const string VALIDATION_MESSAGE = "Current page is not valid. Please fill in required information";

    #region Properties

    private bool navigationEnabled = true;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is navigation enabled.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool NavigationEnabled
    {
        get => navigationEnabled;
        set
        {
            btnFirst.Enabled = value;
            btnPrevious.Enabled = value;
            btnNext.Enabled = value;
            btnLast.Enabled = value;
            navigationEnabled = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is show first button.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowFirstButton
    {
        get => btnFirst.Visible;
        set => btnFirst.Visible = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is show last button.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public bool ShowLastButton
    {
        get => btnLast.Visible;
        set => btnLast.Visible = value;
    }

    /// <summary>
    /// Gets or sets the wizard pages.
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public WizardPageCollection WizardPages { get; set; }

    #endregion Properties

    #region Delegates & Events

    /// <summary>
    /// Represents the method that handles wizard completed event handler.
    /// </summary>
    public delegate void WizardCompletedEventHandler();

    /// <summary>
    /// Occurs when wizard completed.
    /// </summary>
    public event WizardCompletedEventHandler WizardCompleted;

    #endregion Delegates & Events

    #region Constructor & Window Event Handlers

    /// <summary>
    /// Initializes a new instance of the WizardHost class.
    /// </summary>
    public WizardHost()
    {
        InitializeComponent();
        WizardPages = [];
        WizardPages.WizardPageLocationChanged += new WizardPageCollection.WizardPageLocationChangedEventHanlder(WizardPages_WizardPageLocationChanged);
    }

    private void WizardPages_WizardPageLocationChanged(WizardPageLocationChangedEventArgs e) => LoadNextPage(e.PageIndex, e.PreviousPageIndex, true);

    #endregion Constructor & Window Event Handlers

    #region Private Methods

    /// <summary>
    /// Updates the navigation.
    /// </summary>
    public void UpdateNavigation()
    {
        #region Reset

        btnNext.Enabled = true;
        btnNext.Visible = true;

        btnLast.Text = "Last >>";
        btnLast.Enabled = ShowLastButton;

        #endregion Reset

        bool canMoveNext = WizardPages.CanMoveNext;
        bool canMovePrevious = WizardPages.CanMovePrevious;

        btnPrevious.Enabled = canMovePrevious;
        btnFirst.Enabled = canMovePrevious;

        if (canMoveNext)
        {
            btnNext.Text = "Next >";
            btnNext.Enabled = true;

            if (ShowLastButton)
            {
                btnLast.Enabled = true;
            }
        }
        else
        {
            if (ShowLastButton)
            {
                btnLast.Text = "Finish";
                btnNext.Visible = false;
            }
            else
            {
                btnNext.Text = "Finish";
                btnNext.Visible = true;
            }
        }
    }

    private bool CheckPageIsValid()
    {
        if (!WizardPages.CurrentPage.PageValid)
        {
            MessageBox.Show(
                string.Concat(VALIDATION_MESSAGE, Environment.NewLine, Environment.NewLine, WizardPages.CurrentPage.ValidationMessage),
                "Details Required",
                MessageBoxButtons.OK,
                MessageBoxIcon.Exclamation);
            return false;
        }
        return true;
    }

    private void NotifyWizardCompleted()
    {
        if (WizardCompleted != null)
        {
            OnWizardCompleted();
            WizardCompleted();
        }
    }

    private void OnWizardCompleted()
    {
        WizardPages.LastPage.Save();
        WizardPages.Reset();
        this.DialogResult = DialogResult.OK;
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// Loads the next page.
    /// </summary>
    /// <param name="pageIndex">The page index.</param>
    /// <param name="previousPageIndex">The previous page index.</param>
    /// <param name="savePreviousPage">The save previous page.</param>
    public void LoadNextPage(int pageIndex, int previousPageIndex, bool savePreviousPage)
    {
        if (pageIndex != -1)
        {
            contentPanel.Controls.Clear();
            contentPanel.Controls.Add(WizardPages[pageIndex].Content);
            if (savePreviousPage && previousPageIndex != -1)
            {
                WizardPages[previousPageIndex].Save();
            }
            WizardPages[pageIndex].Load();
            UpdateNavigation();
        }
    }

    /// <summary>
    /// Loads the wizard.
    /// </summary>
    public void LoadWizard() => WizardPages.MovePageFirst();

    #endregion Public Methods

    #region Event Handlers

    private void btnFirst_Click(object sender, EventArgs e) =>
        //if (!CheckPageIsValid()) //Maybe doesn't matter if move back; only matters if move forward
        //{ return; }

        WizardPages.MovePageFirst();

    private void btnLast_Click(object sender, EventArgs e)
    {
        if (!CheckPageIsValid())
        { return; }

        if (WizardPages.CanMoveNext)
        {
            WizardPages.MovePageLast();
        }
        else
        {
            //This is the finish button and it has been clicked
            NotifyWizardCompleted();
        }
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        if (!CheckPageIsValid())
        { return; }

        if (WizardPages.CanMoveNext)
        {
            WizardPages.MovePageNext();
        }
        else
        {
            //This is the finish button and it has been clicked
            NotifyWizardCompleted();
        }
    }

    private void btnPrevious_Click(object sender, EventArgs e) =>
        //if (!CheckPageIsValid()) //Maybe doesn't matter if move back; only matters if move forward
        //{ return; }

        WizardPages.MovePagePrevious();

    #endregion Event Handlers
}