using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ConditionEditorPage : ContentPage
{
	public ConditionEditorPage(ConditionEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}