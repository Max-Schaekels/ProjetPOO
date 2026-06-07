using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class GamePage : ContentPage
{
	public GamePage(GameViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}