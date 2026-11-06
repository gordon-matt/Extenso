namespace Extenso.Windows.Forms;

/// <summary>
/// Provides extension methods for forms.
/// </summary>
public static class FormExtensions
{
    extension(Form form)
    {
        /// <summary>
        /// Performs the center to parent operation.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        public void CenterToParent(Form parentForm)
        {
            if (parentForm is not null)
            {
                CenterToParent(form, parentForm.Location, parentForm.Size);
            }
        }

        /// <summary>
        /// Performs the center to parent operation.
        /// </summary>
        /// <param name="parentLocation">The parent location.</param>
        /// <param name="parentSize">The parent size.</param>
        public void CenterToParent(Point parentLocation, Size parentSize) =>
            CenterToParent(form, parentLocation, new Point(parentSize));

        /// <summary>
        /// Performs the center to parent operation.
        /// </summary>
        /// <param name="parentLocation">The parent location.</param>
        /// <param name="parentSize">The parent size.</param>
        public void CenterToParent(Point parentLocation, Point parentSize)
        {
            if (parentLocation != default && parentSize != default)
            {
                int centerX = parentLocation.X + (parentSize.X / 2);
                int centerY = parentLocation.Y + (parentSize.Y / 2);

                int x = centerX - (form.Size.Width / 2);
                int y = centerY - (form.Size.Height / 2);

                form.Location = new Point(x, y);
            }
        }
    }
}