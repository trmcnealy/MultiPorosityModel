using Engineering.UI.Controls;

namespace MultiPorosity.Presentation.Models
{
    public sealed class APIMaskedTextBox : MaskedTextBox
    {
        public APIMaskedTextBox()
        {
            Mask = "00-000-00000-0000";
        }
    }
}
