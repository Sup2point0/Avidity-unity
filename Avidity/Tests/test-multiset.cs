namespace tests;

using System.Linq;


[TestClass]
public class Test_MultiSet
{
    [TestMethod]
    public void Add()
    {
        MultiSet<string> set = new();

        set.Add("test");
        Assert.IsTrue(
            set.SequenceEqual(
                new Dictionary<string, uint>() {
                    ["test"] = 1,
                }
            )
        );

        set.Add("test");
        Assert.IsTrue(
            set.SequenceEqual(
                new Dictionary<string, uint>() {
                    ["test"] = 2,
                }
            )
        );

        set.Add("testing");
        Assert.IsTrue(
            set.SequenceEqual(
                new Dictionary<string, uint>() {
                    ["test"] = 2,
                    ["testing"] = 1,
                }
            )
        );
    }

    [TestMethod]
    public void SortedDescending()
    {
        MultiSet<string> set = new() {
            ["low"] = 1,
            ["mid"] = 10,
            ["high"] = 100,
        };

        Assert.IsTrue(
            set.SortedDescending()
            .SequenceEqual(new List<string>() {
                "high", "mid", "low"
            })
        );
    }
}
