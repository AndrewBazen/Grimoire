using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Grimoire.Models;

public class Tome
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string Language { get; set; }
    public string Genre { get; set; }
    public string Theme { get; set; }
    public ObservableCollection<Entry> Entries { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Tome()
    {
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Entries = [];
        Name = "New Tome";
        Description = "Write your tome here";
        Author = "Anonymous";
        Language = "English";
        Genre = "Fiction";
        Theme = "Default";
    }
}
