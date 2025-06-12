using System.Threading.Tasks;
using Grimoire.Presentation.ViewModels;

namespace Grimoire.Presentation.Views;

public partial class StartPage : ContentPage
{
    public StartPage(StartViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = (StartViewModel)BindingContext;
        vm.InitializeCommand.Execute(null);
    }
}