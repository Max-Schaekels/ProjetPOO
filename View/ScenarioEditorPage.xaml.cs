using CommunityToolkit.Mvvm.Input;
using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ScenarioEditorPage : ContentPage
{
	public ScenarioEditorPage(ScenarioEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}