// ---------- Presentation/ViewModels/EntryViewModel.cs ----------
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grimoire.Domain.Models;
using Grimoire.Domain.UseCases;

namespace Grimoire.Presentation.ViewModels;

public partial class EntryViewModel : ObservableObject
{
    private readonly ISaveEntryUseCase _saveEntry;

    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _content = string.Empty;
    [ObservableProperty] private bool _isSaving;

    public string TomeId { get; }
    public string EntryId { get; }

    public EntryViewModel(ISaveEntryUseCase saveEntry, string tomeId, string entryId = "")
    {
        _saveEntry = saveEntry;
        TomeId = tomeId;
        EntryId = string.IsNullOrWhiteSpace(entryId) ? Guid.NewGuid().ToString("N") : entryId;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsSaving) return;
        IsSaving = true;
        try
        {
            var entry = new TomeEntry(EntryId, Title.Trim(), Content, DateTimeOffset.UtcNow);
            await _saveEntry.ExecuteAsync(new SaveEntryRequest(TomeId, entry));
        }
        finally { IsSaving = false; }
    }
}