using System;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Grimoire.Models
{
    public class Entry : INotifyPropertyChanged
    {
        private string? _filename;

        public string Filename {
            get => _filename ?? "Untitled.md";
            set
            {
                if (_filename != $"{Title}.md")
                {
                    _filename = $"{Title}.md";
                    OnPropertyChanged(nameof(Filename));
                }
            }
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Entry()
        {
            Title = "Untitled";
            Content = "Write your entry here";
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public async Task Save()
        {
            UpdatedAt = DateTime.Now;
            await File.WriteAllTextAsync(Path.Combine(FileSystem.Current.AppDataDirectory, Filename), Content);
        }

        public void Delete()
        {
            File.Delete(Path.Combine(FileSystem.Current.AppDataDirectory, Filename));
        }

        public static async Task<Entry> LoadAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentException($"File {filename} does not exist");
            }
            else {
                if (File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, filename)))
                {
                    return new Entry { Filename = filename, Content = await File.ReadAllTextAsync(Path.Combine(FileSystem.Current.AppDataDirectory, filename)) };
                }
                else
                {
                    return new Entry { Filename = filename, Content = "Write your entry here" };
                }
            }
        }

        public static async Task<List<Entry>> LoadAllAsync()
        {
            string appDataDirectory = FileSystem.Current.AppDataDirectory;
            var entries = await Task.WhenAll(Directory.EnumerateFiles(appDataDirectory, "*.md")
                .Select(file => LoadAsync(Path.GetFileName(file))));
            return entries.OrderByDescending(e => e.UpdatedAt).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
