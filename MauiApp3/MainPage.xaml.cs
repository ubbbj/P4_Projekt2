namespace Watchlist;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnSearchClicked(object sender, EventArgs e)
    {
        var searchTerm = searchEntry.Text;
        var mediaType = (string)mediaTypePicker.SelectedItem == "Film" ? "movie" : "tv";
        var movies = await MovieService.SearchMoviesAsync(searchTerm, mediaType);
        moviesCollectionView.ItemsSource = movies;
    }

    private void OnSettingsClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SettingsPage());
    }

    private void OnMovieSelected(object sender, EventArgs e)
    {
        var image = (Image)sender;
        var movie = (Movie)image.BindingContext;
        Navigation.PushAsync(new MovieDetailPage(movie));
    }

    private void OnWatchlistClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new WatchlistPage());
    }
}