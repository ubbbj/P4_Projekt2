namespace Watchlist;

public partial class WatchlistPage : ContentPage
{
    public WatchlistPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        watchlistCollectionView.ItemsSource = MovieService.Watchlist;
    }

    private void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        var movie = (Movie)e.CurrentSelection.FirstOrDefault();
        if (movie != null)
        {
            Navigation.PushAsync(new MovieDetailPage(movie));
        }
    }
}