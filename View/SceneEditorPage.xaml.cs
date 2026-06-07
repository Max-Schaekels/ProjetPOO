using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class SceneEditorPage : ContentPage
{
	public SceneEditorPage(SceneEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    private async void ButtonScrollToTop_Clicked(object sender, EventArgs e)
    {
        await SceneEditorScrollView.ScrollToAsync(0, 0, true);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SceneEditorViewModel sceneEditorViewModel)
        {
            sceneEditorViewModel.RefreshLoadedScene();
        }
    }
}