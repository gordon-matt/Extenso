namespace Extenso.Windows.Forms.Wizard;

/// <summary>
/// Defines the contract for the wizard page.
/// </summary>
public interface IWizardPage
{
    /// <summary>
    /// Gets the content.
    /// </summary>
    UserControl Content { get; }

    /// <summary>
    /// Loads the value.
    /// </summary>
    void Load();

    /// <summary>
    /// Saves the value.
    /// </summary>
    void Save();

    /// <summary>
    /// Cancels the value.
    /// </summary>
    void Cancel();

    /// <summary>
    /// Gets a value indicating whether this instance is busy.
    /// </summary>
    bool IsBusy { get; }

    /// <summary>
    /// Gets a value indicating whether this instance is page valid.
    /// </summary>
    bool PageValid { get; }

    /// <summary>
    /// Gets the validation message.
    /// </summary>
    string ValidationMessage { get; }
}