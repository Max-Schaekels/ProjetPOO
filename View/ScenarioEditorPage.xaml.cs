using CommunityToolkit.Mvvm.Input;
using ProjetPOO.Model.Story;
using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ScenarioEditorPage : ContentPage
{
    private readonly ScenarioEditorViewModel viewModel;
    public ScenarioEditorPage(ScenarioEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    public void LoadScenario(Scenario scenario)
    {
        viewModel.LoadScenario(scenario);
    }

    public void PrepareNewScenario()
    {
        viewModel.PrepareNewScenario();
    }

}