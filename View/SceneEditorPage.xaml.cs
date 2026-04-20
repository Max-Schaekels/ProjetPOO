using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class SceneEditorPage : ContentPage
{
	public SceneEditorPage(SceneEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}