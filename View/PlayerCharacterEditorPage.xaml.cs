using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class PlayerCharacterEditorPage : ContentPage
{
	public PlayerCharacterEditorPage(PlayerCharacterEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}