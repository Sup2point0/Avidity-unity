namespace tests;


[TestClass]
public class Test_Utils
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
