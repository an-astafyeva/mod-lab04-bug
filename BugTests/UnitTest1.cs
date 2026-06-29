using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BugTests;

[TestClass]
public class BugTests
{
    [TestMethod]
    public void Test1_InitialState_ShouldBeOpen()
    {
        var bug = new Bug();
        Assert.AreEqual(BugState.Open, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test2_Assign_ShouldChangeStateToAssigned()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        Assert.AreEqual(BugState.Assigned, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test3_StartProgress_ShouldChangeStateToInProgress()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        Assert.AreEqual(BugState.InProgress, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test4_Fix_ShouldChangeStateToFixed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        Assert.AreEqual(BugState.Fixed, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test5_Verify_ShouldChangeStateToVerified()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        Assert.AreEqual(BugState.Verified, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test6_Close_ShouldChangeStateToClosed()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        bug.Fire(BugTrigger.Close);
        Assert.AreEqual(BugState.Closed, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test7_ReopenFromFixed_ShouldChangeStateToReopened()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test8_RejectFromOpen_ShouldChangeStateToRejected()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Reject);
        Assert.AreEqual(BugState.Rejected, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test9_InvalidTransition_ShouldNotChangeState()
    {
        var bug = new Bug();
        Assert.IsFalse(bug.CanFire(BugTrigger.Fix));
        bug.Fire(BugTrigger.Fix);
        Assert.AreEqual(BugState.Open, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test10_CanFire_ShouldReturnCorrectValues()
    {
        var bug = new Bug();
        Assert.IsTrue(bug.CanFire(BugTrigger.Assign));
        Assert.IsTrue(bug.CanFire(BugTrigger.Reject));
        Assert.IsFalse(bug.CanFire(BugTrigger.Fix));
        Assert.IsFalse(bug.CanFire(BugTrigger.StartProgress));
        Assert.IsFalse(bug.CanFire(BugTrigger.Close));
    }
    
    [TestMethod]
    public void Test11_ReopenAfterClosed_ShouldWork()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        bug.Fire(BugTrigger.Close);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test12_ReassignAfterReopen_ShouldWork()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Reopen);
        bug.Fire(BugTrigger.Assign);
        Assert.AreEqual(BugState.Assigned, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test13_RejectFromAssigned_ShouldChangeStateToRejected()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.Reject);
        Assert.AreEqual(BugState.Rejected, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test14_ReopenFromRejected_ShouldChangeStateToReopened()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Reject);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test15_ReopenFromVerified_ShouldChangeStateToReopened()
    {
        var bug = new Bug();
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        bug.Fire(BugTrigger.Reopen);
        Assert.AreEqual(BugState.Reopened, bug.CurrentState);
    }
}
