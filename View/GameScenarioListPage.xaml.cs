using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class GameScenarioListPage : ContentPage
{
	public GameScenarioListPage(GameScenarioListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is GameScenarioListViewModel viewModel)
        {
            viewModel.RefreshScenarios();
        }
    }
}