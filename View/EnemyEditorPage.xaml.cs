using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class EnemyEditorPage : ContentPage
{
	public EnemyEditorPage(EnemyEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    private async void ButtonScrollToTop_Clicked(object sender, EventArgs e)
    {
        await EnemyEditorScrollView.ScrollToAsync(0, 0, true);
    }
}