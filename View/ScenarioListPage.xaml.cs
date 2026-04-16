using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ScenarioListPage : ContentPage
{
	public ScenarioListPage(ScenarioListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private void ButtonScrollToTop_Clicked(object sender, EventArgs e)
    {
        ScenarioCollectionView.ScrollTo(0, position: ScrollToPosition.Start, animate: true);
    }
}