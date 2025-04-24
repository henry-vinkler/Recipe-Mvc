using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcTunnikontroll.Data;
using MvcTunnikontroll.Domain;
using MvcTunnikontroll.Tests;

namespace MvcTunnikontroll.Tests.Domain;

[TestClass] public class MovieTests : BaseClassTests<Movie, Entity<MovieData>> {
    protected override Movie createObj() {
        var d = new MovieData {
            Id = 1,
            Title = "Title",
            ReleaseDate = DateTime.Today,
            Genre = "Genre",
            Rating = "Rating",
            Price = 1.0d
        };
        return new Movie(d);
    }
    [TestMethod] public void TitleTest() => equal("Title", obj?.Title);
    [TestMethod] public void ReleaseDateTest() => equal(DateTime.Today, obj?.ReleaseDate);
    [TestMethod] public void GenreTest() => equal("Genre", obj?.Genre);
    [TestMethod] public void PriceTest() => equal(1.0, obj?.Price);
    [TestMethod] public void RatingTest() => equal("Rating", obj?.Rating);
    [TestMethod] public void IdTest() => equal(1, obj?.Id); 
    [TestMethod] public void DataTest() => notNull(obj?.data); 
}
