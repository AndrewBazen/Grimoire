using System.Threading.Tasks;
using Grimoire.Domain.Models;
using Grimoire.Domain.UseCases;
using Grimoire.Presentation.ViewModels;

namespace Grimoire.Presentation.Views;

public partial class ThemeSettingsPage : ContentPage
{
    public ThemeSettingsPage(ThemeSettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();        
        var vm = (ThemeSettingsViewModel)BindingContext;
    }

    private async void OnThemeRadioButtonChecked(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value) return;

        if (sender is RadioButton radioButton && radioButton.BindingContext is ThemeSettingsViewModel vm)
        {
            // Update selected state in the view models
            foreach (var theme in vm.AvailableThemes)
            {
                theme.IsSelected = theme.Name == vm.CurrentTheme.Name;
            }

            // Apply the theme
            if (vm.CurrentTheme.Name != null)
            {
                await vm.SelectThemeCommand.ExecuteAsync(vm.CurrentTheme.Name);
            }
        }
    }
}

