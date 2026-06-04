using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class SceneEditorPage : ContentPage
{
	public SceneEditorPage(SceneEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
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