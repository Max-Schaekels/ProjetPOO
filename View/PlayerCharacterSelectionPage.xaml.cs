using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class PlayerCharacterSelectionPage : ContentPage
{
	public PlayerCharacterSelectionPage(PlayerCharacterSelectionViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}