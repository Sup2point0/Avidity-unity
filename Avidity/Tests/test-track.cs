namespace tests;

using Avidity;


[TestClass]
public class Test_Track
{
    public Track track = new();

    [TestMethod]
    public void TestEquality()
    {
        var t1 = new Track() { weak_shard = "test" };
        var t2 = new Track() { weak_shard = "test" };
        Assert.AreEqual(t1, t2);

        var t3 = new Track() { weak_shard = "testing" };
        Assert.AreNotEqual(t1, t3);
    }

    [TestMethod]
    public void DisplayDuration()
    {
        track.duration = null;
        Assert.AreEqual( "--:--", track.DisplayDuration() );

        track.duration = 0;
        Assert.AreEqual( "0:00", track.DisplayDuration() );

        track.duration = 1;
        Assert.AreEqual( "0:01", track.DisplayDuration() );

        track.duration = 60;
        Assert.AreEqual( "1:00", track.DisplayDuration() );

        track.duration = 61;
        Assert.AreEqual( "1:01", track.DisplayDuration() );

        track.duration = 60 * 59;
        Assert.AreEqual( "59:00", track.DisplayDuration() );

        track.duration = 60 * 59 + 59;
        Assert.AreEqual( "59:59", track.DisplayDuration() );

        track.duration = 60 * 60;
        Assert.AreEqual( "60:00", track.DisplayDuration() );
    }
}
