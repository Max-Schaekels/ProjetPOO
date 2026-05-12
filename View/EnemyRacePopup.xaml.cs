using CommunityToolkit.Maui.Views;
using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class EnemyRacePopup : Popup
{
    private readonly EnemyEditorViewModel viewModel;
    public EnemyRacePopup(EnemyEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private async void CreateButton_Clicked(object sender, EventArgs e)
    {
        bool isCreated = await viewModel.SaveNewEnemyRace();

        if (isCreated)
        {
            Close();
        }
    }
}