using ProjetPOO.ViewModel;

namespace ProjetPOO.View;

public partial class ChoiceEditorPage : ContentPage
{
	public ChoiceEditorPage(ChoiceEditorViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ChoiceEditorViewModel choiceEditorViewModel)
        {
            choiceEditorViewModel.RefreshLoadedChoice();
        }
    }
}