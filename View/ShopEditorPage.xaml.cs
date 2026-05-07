using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ShopEditorPage : ContentPage
{
	public ShopEditorPage(ShopEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}