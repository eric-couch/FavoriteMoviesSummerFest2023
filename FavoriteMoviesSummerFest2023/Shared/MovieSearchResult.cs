using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FavoriteMoviesSummerFest2023.Shared;

public class MovieSearchResult
{
    public MovieSearchResultItems[] Search { get; set; }
    public string totalResults { get; set; }
    public string Response { get; set; }
}
