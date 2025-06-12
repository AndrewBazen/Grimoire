using System.Threading.Tasks;
using Grimoire.Presentation.ViewModels;

namespace Grimoire.Presentation.Views;

public partial class ArchivePage : ContentPage
{
    public ArchivePage(WorkspaceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var vm = (WorkspaceViewModel)BindingContext;
        vm.ArchiveViewModel.InitializeCommand.Execute(null);
    }
}