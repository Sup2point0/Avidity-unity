namespace tests;


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
    public void AddMaybe()
    {
        Dictionary<string, int> dict = [];

        Assert.AreEqual( Utils.AddMaybe(dict, "test", null), dict );
        Assert.AreEqual( Utils.AddMaybe(dict, "test", 0), new() { ["test"] = 0 } );
    }
}
