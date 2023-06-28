using FavoriteMoviesSummerFest2023.Client.HttpRepository;
using RichardSzalay.MockHttp;

namespace FavoriteMoviesSummerFest2023.Client.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test_GetMovies_ReturnListOMDBMovies()
    {
        var mockHttp = new MockHttpMessageHandler();
        string testUserResponse = """
            {
              "id": "62cd0eb8-7bbd-4d7b-b710-245e6aa61b16",
              "userName": "eric.couch@example.com",
              "favoriteMovies": [
                {
                  "id": 1,
                  "imdbId": "tt0482571"
                },
                {
                  "id": 2,
                  "imdbId": "tt0499549"
                }
              ]
            }
            """;
        string testOMDBPrestigeResponse = """
            {
              "Title": "The Prestige",
              "Year": "2006",
              "Rated": "PG-13",
              "Released": "20 Oct 2006",
              "Runtime": "130 min",
              "Genre": "Drama, Mystery, Sci-Fi",
              "Director": "Christopher Nolan",
              "Writer": "Jonathan Nolan, Christopher Nolan, Christopher Priest",
              "Actors": "Christian Bale, Hugh Jackman, Scarlett Johansson",
              "Plot": "After a tragic accident, two stage magicians in 1890s London engage in a battle to create the ultimate illusion while sacrificing everything they have to outwit each other.",
              "Language": "English",
              "Country": "United Kingdom, United States",
              "Awards": "Nominated for 2 Oscars. 6 wins & 45 nominations total",
              "Poster": "https://m.media-amazon.com/images/M/MV5BMjA4NDI0MTIxNF5BMl5BanBnXkFtZTYwNTM0MzY2._V1_SX300.jpg",
              "Ratings": [
                {
                  "Source": "Internet Movie Database",
                  "Value": "8.5/10"
                },
                {
                  "Source": "Rotten Tomatoes",
                  "Value": "76%"
                },
                {
                  "Source": "Metacritic",
                  "Value": "66/100"
                }
              ],
              "Metascore": "66",
              "imdbRating": "8.5",
              "imdbVotes": "1,363,633",
              "imdbID": "tt0482571",
              "Type": "movie",
              "DVD": "13 Feb 2007",
              "BoxOffice": "$53,089,891",
              "Production": "N/A",
              "Website": "N/A",
              "Response": "True"
            }
            """;
        string testOMDBAvatarResponse = """
            {
              "Title": "Avatar",
              "Year": "2009",
              "Rated": "PG-13",
              "Released": "18 Dec 2009",
              "Runtime": "162 min",
              "Genre": "Action, Adventure, Fantasy",
              "Director": "James Cameron",
              "Writer": "James Cameron",
              "Actors": "Sam Worthington, Zoe Saldana, Sigourney Weaver",
              "Plot": "A paraplegic Marine dispatched to the moon Pandora on a unique mission becomes torn between following his orders and protecting the world he feels is his home.",
              "Language": "English, Spanish",
              "Country": "United States",
              "Awards": "Won 3 Oscars. 89 wins & 131 nominations total",
              "Poster": "https://m.media-amazon.com/images/M/MV5BZDA0OGQxNTItMDZkMC00N2UyLTg3MzMtYTJmNjg3Nzk5MzRiXkEyXkFqcGdeQXVyMjUzOTY1NTc@._V1_SX300.jpg",
              "Ratings": [
                {
                  "Source": "Internet Movie Database",
                  "Value": "7.9/10"
                },
                {
                  "Source": "Rotten Tomatoes",
                  "Value": "82%"
                },
                {
                  "Source": "Metacritic",
                  "Value": "83/100"
                }
              ],
              "Metascore": "83",
              "imdbRating": "7.9",
              "imdbVotes": "1,342,345",
              "imdbID": "tt0499549",
              "Type": "movie",
              "DVD": "22 Apr 2010",
              "BoxOffice": "$785,221,649",
              "Production": "N/A",
              "Website": "N/A",
              "Response": "True"
            }
            """;
        mockHttp.When("http://localhost:7139/api/get-movies")
            .Respond("application/json", testUserResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0482571")
            .Respond("application/json", testOMDBPrestigeResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt0499549")
            .Respond("application/json", testOMDBAvatarResponse);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("http://localhost:7139/");
        var userHttpRepository = new UserHttpRepository(client);

        var dataResponse = await userHttpRepository.GetMovies();
        var response = dataResponse.Data;

        Assert.That(response.Count(), Is.EqualTo(2));
        Assert.That(response[0].Title, Is.EqualTo("The Prestige"));
        Assert.That(response[0].Year, Is.EqualTo("2006"));
        Assert.That(response[1].Title, Is.EqualTo("Avatar"));
        Assert.That(response[1].Year, Is.EqualTo("2009"));
    }
}