using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class EffectEditorPage : ContentPage
{
	public EffectEditorPage(EffectEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}