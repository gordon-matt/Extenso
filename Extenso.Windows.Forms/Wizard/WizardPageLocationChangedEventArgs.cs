namespace Extenso.Windows.Forms.Wizard;

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

    public int PreviousPageIndex { get; set; }
}