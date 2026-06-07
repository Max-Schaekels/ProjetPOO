using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class SaveGameListPage : ContentPage
{
	public SaveGameListPage(SaveGameListViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SaveGameListViewModel viewModel)
        {
            viewModel.RefreshSaveGames();
        }
    }
}