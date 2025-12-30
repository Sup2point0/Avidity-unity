namespace tests;

using System.Linq;

using Avidity;


[TestClass]
public class Test_Utils_Paths
{
    public Track track = new();

    [TestMethod]
    public void JoinPaths()
    {
        Assert.AreEqual( Utils.JoinPaths([]), "" );
        Assert.AreEqual( Utils.JoinPaths([null, null]), "" );
        Assert.AreEqual( Utils.JoinPaths(["parent", "child"]), "parent/child" );
        Assert.AreEqual( Utils.JoinPaths(["parent", null, "child"]), "parent/child" );
    }
}

[TestClass]
public class Test_Utils_Collections
{
    [TestMethod]
    public void Chunks()
    {
        List<int> list = [1, 2, 3, 4, 5];

        var chunks = list.Chunked(3).ToList();
        Assert.IsTrue(chunks[0].SequenceEqual([1, 2, 3]));
        Assert.IsTrue(chunks[1].SequenceEqual([4, 5]));
    }

    [TestMethod]
    public void AddMaybe()
    {
        Dictionary<string, string> dict = [];

        Assert.IsTrue(
            Utils.AddMaybe(dict, "test", null)
            .SequenceEqual(dict)
        );
        Assert.IsTrue(
            Utils.AddMaybe(dict, "test", "t")
            .SequenceEqual(new Dictionary<string, string>() { ["test"] = "t" })
        );
    }
}
