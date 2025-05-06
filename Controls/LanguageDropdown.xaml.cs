using System.Windows.Input;
using Barangay_Office.Utilities;
using CommunityToolkit.Maui.Views;

namespace Barangay_Office.Controls;

public partial class LanguageDropdown : ContentView
{
    
    private readonly List<string> _languages =
    [
       "English",
       "Filipino",
       "Spanish",
       "Japanese",
       "Taiwanese",
       "Chinese"
    ];

    
    public static readonly BindableProperty SelectedLanguageProperty = BindableProperty.Create(nameof(SelectedLanguage),typeof(string),typeof(LanguageDropdown), default(string),BindingMode.TwoWay);

    public static readonly BindableProperty ArrowIconSourceProperty = BindableProperty.Create(
        nameof(ArrowIconSource),
        typeof(string),
        typeof(LanguageDropdown),
        default(string),
        BindingMode.OneWay);
    
    private string? _pendingSelection;

    public string SelectedLanguage
    {
        get => (string)GetValue(SelectedLanguageProperty);
        set => SetValue(SelectedLanguageProperty, value);
    }

    public string ArrowIconSource
    {
        get => (string)GetValue(ArrowIconSourceProperty);
        set => SetValue(ArrowIconSourceProperty, value);
    }
    
    public ICommand OkButton { get; }
    public ICommand CancelButton { get; }




    public LanguageDropdown()
    {



        OkButton = new RelayCommand(_ =>
        {
            //collapse the dropdown after selection
            SelectedLanguageLabel.Text = _pendingSelection;
            LanguageExpander.IsExpanded = false;

            return Task.CompletedTask;
        });
        
        CancelButton = new RelayCommand(_ =>
        {
            //collapse the dropdown after selection
            LanguageExpander.IsExpanded = false;

            return Task.CompletedTask;
        });
        InitializeComponent();

        BindingContext = this;

        //do not remove this
        LanguageCollectionView.ItemsSource = _languages;
    }


    private void Expander_ExpandedChanged(object sender, CommunityToolkit.Maui.Core.ExpandedChangedEventArgs e)
    {


        //SwitchIcon();
        if (Application.Current?.RequestedTheme == AppTheme.Dark)
        {
            ArrowIcon.Source = LanguageExpander.IsExpanded ? "arrow_down_light" : "arrow_right_light";
        }
        else
        {
            ArrowIcon.Source = LanguageExpander.IsExpanded ? "arrow_down_dark" : "arrow_right_dark";
        }
    }


    //private void SwitchIcon()
    //{

        
    //}


    private void HandleLanguageSelection(string selectedLanguage)
    {
        if (!string.IsNullOrEmpty(selectedLanguage))
        {
            //update the selected language
            SelectedLanguage = selectedLanguage;

            //update the label to display
            _pendingSelection = selectedLanguage;

            //update the selected item
            LanguageCollectionView.SelectedItem = selectedLanguage;

        }
    }

    private void OnLanguageSelected(object sender, TappedEventArgs e)
    {

        if (sender is Label label)
        {
            HandleLanguageSelection(label.Text);
        }

        // Refresh the CollectionView to apply the background color
        LanguageCollectionView.ItemsSource = null;
        LanguageCollectionView.ItemsSource = _languages;
        
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if(e.CurrentSelection.Count > 0 && e.CurrentSelection[0] is string selectedLanguage)
        {
            HandleLanguageSelection(selectedLanguage);
        }
    }
}
