using System.Text.Json;

namespace Watchlist;

public static class MovieService
{
    private static string ApiKey => Preferences.Get("ApiKey", string.Empty);
    private const string BaseUrl = "https://api.themoviedb.org/3/search";

    public static List<Movie> Watchlist
    {
        get
        {
            var json = Preferences.Get("Watchlist", string.Empty);
            return string.IsNullOrEmpty(json) ? new List<Movie>() : JsonSerializer.Deserialize<List<Movie>>(json);
        }
    }

    public static async Task<IEnumerable<Movie>> SearchMoviesAsync(string query, string mediaType)
    {
        if (mediaType != "movie" && mediaType != "tv")
        {
            throw new ArgumentException("Invalid media type. Must be either 'movie' or 'tv'.", nameof(mediaType));
        }

        var language = Preferences.Get("language", "en");

        using var client = new HttpClient();
        var url = $"{BaseUrl}/{mediaType}?api_key={ApiKey}&query={Uri.EscapeDataString(query)}&language={language}";
        var response = await client.GetStringAsync(url);
        var movies = ParseMovies(response, mediaType);

        return movies;
    }

    public static void AddToWatchlist(Movie movie)
    {
        if (!Watchlist.Any(m => m.Id == movie.Id))
        {
            var watchlist = Watchlist;
            watchlist.Add(movie);
            SaveWatchlist(watchlist);
        }
    }

    private static void SaveWatchlist(List<Movie> watchlist)
    {
        var json = JsonSerializer.Serialize(watchlist);
        Preferences.Set("Watchlist", json);
    }

    public static void RemoveFromWatchlist(Movie movie)
    {
        var watchlist = Watchlist;
        var item = watchlist.FirstOrDefault(m => m.Id == movie.Id);
        if (item != null)
        {
            watchlist.Remove(item);
            SaveWatchlist(watchlist);
        }
    }

    public static bool IsInWatchlist(Movie movie)
    {
        return Watchlist.Any(m => m.Id == movie.Id);
    }

    private static IEnumerable<Movie> ParseMovies(string json, string mediaType)
    {
        var jsonObject = JsonDocument.Parse(json);
        var results = jsonObject.RootElement.GetProperty("results");
        return results.EnumerateArray().Select(movie =>
        {
            var titleProperty = mediaType == "movie" ? "title" : "name";
            return new Movie
            {
                Id = movie.GetProperty("id").GetInt32(),
                Title = movie.GetProperty(titleProperty).GetString(),
                Overview = movie.GetProperty("overview").GetString(),
                PosterPath = $"https://image.tmdb.org/t/p/w500{movie.GetProperty("poster_path").GetString()}",
                VoteAverage = movie.GetProperty("vote_average").GetDouble()
            };
        });
    }
}