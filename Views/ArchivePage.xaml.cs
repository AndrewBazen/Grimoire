using Grimoire.ViewModels;
using Grimoire.Services;

namespace Grimoire.Views;

public partial class ArchivePage : ContentPage
{
    public ArchivePage()
    {
        InitializeComponent();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        // Get TomeService from dependency injection
        var tomeService = Handler?.MauiContext?.Services?.GetService<TomeService>();
        
        if (tomeService != null)
        {
            var viewModel = new ArchiveViewModel(tomeService);
            BindingContext = viewModel;
        }
        else
        {
            throw new Exception("TomeService is null");
        }
    }
}