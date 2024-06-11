namespace Watchlist;

public partial class MovieDetailPage : ContentPage
{
    private readonly Movie _movie;

    public MovieDetailPage(Movie movie)
    {
        InitializeComponent();
        _movie = movie;
        titleLabel.Text = movie.Title;
        overviewLabel.Text = movie.Overview;
        posterImage.Source = movie.PosterPath;
        voteAverageLabel.Text = $"Średnia ocen: {movie.VoteAverage}";

        UpdateWatchlistButton();
    }

    private void OnAddToWatchlistClicked(object sender, EventArgs e)
    {
        if (MovieService.IsInWatchlist(_movie))
        {
            MovieService.RemoveFromWatchlist(_movie);
        }
        else
        {
            MovieService.AddToWatchlist(_movie);
        }

        UpdateWatchlistButton();
    }

    private void UpdateWatchlistButton()
    {
        if (MovieService.IsInWatchlist(_movie))
        {
            addToWatchlistButton.Text = "Usuń z Watchlist";
        }
        else
        {
            addToWatchlistButton.Text = "Dodaj do Watchlist";
        }
    }
}