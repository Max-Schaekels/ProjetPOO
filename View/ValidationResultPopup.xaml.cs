using CommunityToolkit.Maui.Views;

namespace ProjetPOO.View;

public partial class ValidationResultPopup : Popup
{
    public ValidationResultPopup(string title, List<string> errors)
    {
        InitializeComponent();
        TitleLabel.Text = title;

        if (errors == null || errors.Count == 0)
        {
            SummaryLabel.Text = "Aucun problème détecté.";

            Label successLabel = new Label
            {
                Text = "Le scénario semble cohérent et jouable.",
                FontSize = 15,
                TextColor = Color.FromArgb("#111111"),
                HorizontalTextAlignment = TextAlignment.Center
            };

            ErrorsLayout.Children.Add(successLabel);

            return;
        }

        SummaryLabel.Text = $"{errors.Count} problème(s) détecté(s).";

        for (int i = 0; i < errors.Count; i++)
        {
            Label errorLabel = new Label
            {
                Text = $"• {errors[i]}",
                FontSize = 14,
                TextColor = Color.FromArgb("#111111"),
                LineBreakMode = LineBreakMode.WordWrap
            };

            ErrorsLayout.Children.Add(errorLabel);
        }
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
