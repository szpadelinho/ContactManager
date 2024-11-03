using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;

namespace ContactManager;

public partial class MainWindow : Window
{
    private List<Contact> _contacts = new();
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddContact_OnClick(object? sender, RoutedEventArgs e)
    {
        var newContact = new Contact
        {
            Name = Name.Text,
            Surname = Surname.Text,
            Phone = Phone.Text,
            Email = Mail.Text,
            IsFavorite = false
        };

        if (newContact.Name == null || newContact.Surname == null || newContact.Phone == null ||
            newContact.Email == null)
        {
            var messageBox = new Window
            {
                Title = "Błąd",
                Content = "Wypełnij wszystkie pola!",
            };
            
            messageBox.Show();

            return;
        }
        
        _contacts.Add(newContact);
        
        RefreshContactList();
        
        Name.Clear();
        Surname.Clear();
        Phone.Clear();
        Mail.Clear();
    }

    private void RefreshContactList()
    {
        ContactList.Items.Clear();

        string selectedFavFilter = (ComboBoxFav.SelectedItem as ComboBoxItem)?.Content as string ?? "Wszystko";
        string selectedSort = (ComboBoxFilter.SelectedItem as ComboBoxItem)?.Content as string ?? "Domyslnie";

        var filteredContacts = _contacts.Where(contact =>
            (selectedFavFilter == "Wszystko" || (selectedFavFilter == "Ulubione" && contact.IsFavorite))).ToList();

        if (selectedSort == "Imie")
        {
            filteredContacts = filteredContacts.OrderBy(contact => contact.Name).ToList();
        }
        else if (selectedSort == "Nazwisko")
        {
            filteredContacts = filteredContacts.OrderBy(contact => contact.Surname).ToList();
        }

        foreach (var contact in filteredContacts)
        {
            var contactElement = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10)
            };

            var nameBlock = new TextBlock
            {
                Text = $"{contact.Name} {contact.Surname}",
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var phoneBlock = new TextBlock
            {
                Text = contact.Phone,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var mailBlock = new TextBlock
            {
                Text = contact.Email,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            var favCheckBox = new CheckBox
            {
                IsChecked = contact.IsFavorite,
                Content = "Ulubiony",
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            favCheckBox.Checked += (_, __) => ToggleFav(contact);
            favCheckBox.Unchecked += (_, __) => ToggleFav(contact);
            
            contactElement.Children.Add(nameBlock);
            contactElement.Children.Add(phoneBlock);
            contactElement.Children.Add(mailBlock);
            contactElement.Children.Add(favCheckBox);
            
            ContactList.Items.Add(contactElement);
        }
    }
    
    public class Contact
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsFavorite { get; set; }
    }

    private void ComboBoxFilter_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        RefreshContactList();
    }

    private void ComboBoxFav_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        RefreshContactList();
    }

    private void ToggleFav(Contact contact)
    {
        contact.IsFavorite = !contact.IsFavorite;
        RefreshContactList();
    }
}