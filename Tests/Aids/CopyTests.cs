using MvcTunnikontroll.Aids;
using MvcTunnikontroll.Tests;

namespace MvcTunnikontroll.Tests.Aids;

internal class CopyTestClass1 { 
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? ValidFrom { get; set; }
}
internal class CopyTestClass2 {
    public string? Id { get; set; }
    public string? Name { get; set; }
 }
[TestClass] public sealed class CopyTests : BaseTests {
    private CopyTestClass2? x;
    [TestInitialize] public void Initialize() {
        x = Copy.Members(new CopyTestClass1 { Id = 1, Name = "A", ValidFrom = DateTime.Now },
            new CopyTestClass2());  
    }
    [TestMethod] public void MembersTest() => equal("A", x?.Name);
    [TestMethod] public void IdTest() => equal(null, x?.Id);
}