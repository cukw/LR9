using System;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;

namespace Library.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private string _name = "";
    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _phone = "";
    public string Phone
    {
        get => _phone;
        set => this.RaiseAndSetIfChanged(ref _phone, value);
    }

    public ObservableCollection<PhoneEntry> Items { get; } = new();

    private PhoneEntry? _selectedItem;
    public PhoneEntry? SelectedItem
    {
        get => _selectedItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedItem, value);

            // при выборе строки подтягиваем в поля
            if (value != null)
            {
                Name = value.Name;
                Phone = value.Phone;
            }
        }
    }

    public string TotalText => $"Всего: {Items.Count}";

    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteCommand { get; }
    public ReactiveCommand<Unit, Unit> FindCommand { get; }

    public MainWindowViewModel()
    {
        // начальные данные как на картинке
        Items.Add(new PhoneEntry { Name = "Валентина", Phone = "333-33-33" });
        Items.Add(new PhoneEntry { Name = "Василий",   Phone = "222-22-22" });
        Items.Add(new PhoneEntry { Name = "Ирина",     Phone = "555-55-55" });
        Items.Add(new PhoneEntry { Name = "Максим",    Phone = "111-11-11" });

        // чтобы "Всего: N" обновлялось при добавлении/удалении
        Items.CollectionChanged += (_, _) => this.RaisePropertyChanged(nameof(TotalText));

        var canDelete = this.WhenAnyValue(x => x.SelectedItem).Select(x => x != null);

        AddCommand = ReactiveCommand.Create(Add);
        DeleteCommand = ReactiveCommand.Create(Delete, canDelete);
        FindCommand = ReactiveCommand.Create(Find);
    }

    private void Add()
    {
        var n = (Name ?? "").Trim();
        var p = (Phone ?? "").Trim();

        if (string.IsNullOrWhiteSpace(n) || string.IsNullOrWhiteSpace(p))
            return;

        // если имя уже есть — обновим телефон, иначе добавим
        var existing = Items.FirstOrDefault(x => string.Equals(x.Name, n, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
        {
            existing.Phone = p;
            // обновим отображение (проще: “перекинуть” элемент)
            var idx = Items.IndexOf(existing);
            Items[idx] = new PhoneEntry { Name = existing.Name, Phone = existing.Phone };
            SelectedItem = Items[idx];
            return;
        }

        var entry = new PhoneEntry { Name = n, Phone = p };
        Items.Add(entry);
        SelectedItem = entry;
    }

    private void Delete()
    {
        if (SelectedItem == null) return;

        var toRemove = SelectedItem;
        SelectedItem = null;
        Items.Remove(toRemove);
    }

    private void Find()
    {
        var n = (Name ?? "").Trim();
        if (string.IsNullOrWhiteSpace(n))
            return;

        var found = Items.FirstOrDefault(x => string.Equals(x.Name, n, StringComparison.OrdinalIgnoreCase));
        if (found != null)
            SelectedItem = found;
    }
}
