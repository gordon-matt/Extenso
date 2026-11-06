namespace Extenso.Windows.Forms.Wizard;

/// <summary>
/// Represents the wizard page location changed event args.
/// </summary>
public class WizardPageLocationChangedEventArgs
{
    /// <summary>
    /// The page number of the current IWizardPage
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// The location of the current IWizardPage
    /// </summary>
    public WizardPageLocation PageLocation { get; set; }

    /// <summary>
    /// Gets or sets the previous page index.
    /// </summary>
    public int PreviousPageIndex { get; set; }
}