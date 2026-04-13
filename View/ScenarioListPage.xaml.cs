using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ScenarioListPage : ContentPage
{
	public ScenarioListPage(ScenarioListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}