namespace Watchlist;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        languagePicker.SelectedItem = Preferences.Get("language", "en");
        languagePicker.SelectedIndexChanged += OnLanguageChanged;
        apiKeyEntry.Text = Preferences.Get("ApiKey", string.Empty);
    }

    private void OnLanguageChanged(object sender, EventArgs e)
    {
        Preferences.Set("language", (string)languagePicker.SelectedItem);
    }

    private void OnApiKeyChanged(object sender, TextChangedEventArgs e)
    {
        Preferences.Set("ApiKey", e.NewTextValue);
    }
}