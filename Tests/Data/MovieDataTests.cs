using MvcTunnikontroll.Data;
using MvcTunnikontroll.Tests;

namespace MvcTunnikontroll.Tests.Data;

[TestClass] public class MovieDataTests: ClassTests<MovieData, EntityData<MovieData>> {
    
    [TestInitialize] public override void Initialize() {
        base.Initialize();
        if (obj == null) return;
        obj.Id = 1;
        obj.Title = "Title";
        obj.Price = 1.0;
        obj.ReleaseDate = DateTime.Today;
        obj.Genre = "Genre";
    }
    [TestMethod] public void CloneTest() {
        var d = obj?.Clone();
        notNull(d);
        equal(1, d?.Id);
        equal("Title", d?.Title);
        equal(1.0, d?.Price);
        equal(DateTime.Today, d?.ReleaseDate);
        equal("Genre", d?.Genre);
    }
}
