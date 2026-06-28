using Microsoft.VisualStudio.TestTools.UnitTesting;
using BugPro;

namespace BugTests;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void InitialState_ShouldBeNew()
    {
        var bug = new Bug();

        Assert.AreEqual(BugState.New, bug.State);
    }

    [TestMethod]
    public void Triage_ShouldMoveToTriaged()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);

        Assert.AreEqual(BugState.Triaged, bug.State);
    }

    [TestMethod]
    public void Fix_ShouldMoveToFixed()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.Fix);

        Assert.AreEqual(BugState.Fixed, bug.State);
    }

    [TestMethod]
    public void Close_ShouldMoveToClosed()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Close);

        Assert.AreEqual(BugState.Closed, bug.State);
    }

    [TestMethod]
    public void Reopen_ShouldMoveToReopened()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Reopen);

        Assert.AreEqual(BugState.Reopened, bug.State);
    }

    [TestMethod]
    public void Deferred_ShouldMoveToDeferred()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.Defer);

        Assert.AreEqual(BugState.Deferred, bug.State);
    }

    [TestMethod]
    public void Reject_ShouldMoveToRejected()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.Reject);

        Assert.AreEqual(BugState.Rejected, bug.State);
    }

    [TestMethod]
    public void ReopenedBug_CanBeFixedAgain()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Triage);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.Fix);

        Assert.AreEqual(BugState.Fixed, bug.State);
    }

    [TestMethod]
    [ExpectedException(typeof(System.InvalidOperationException))]
    public void CannotFixWithoutTriaged()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Fix);
    }

    [TestMethod]
    [ExpectedException(typeof(System.InvalidOperationException))]
    public void CannotCloseWithoutFix()
    {
        var bug = new Bug();

        bug.Fire(BugTrigger.Close);
    }
}
