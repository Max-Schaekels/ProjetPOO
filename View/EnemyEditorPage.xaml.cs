using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class EnemyEditorPage : ContentPage
{
	public EnemyEditorPage(EnemyEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}