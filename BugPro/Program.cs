using Stateless;

namespace BugPro;

public enum BugState
{
    Open,
    Assigned,
    InProgress,
    Fixed,
    Verified,
    Closed,
    Reopened,
    Rejected
}

public enum BugTrigger
{
    Assign,
    StartProgress,
    Fix,
    Verify,
    Close,
    Reopen,
    Reject
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public BugState CurrentState => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.Open);

        _machine.Configure(BugState.Open)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Fix, BugState.Fixed);

        _machine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Verify, BugState.Verified)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Verified)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.Assigned);

        _machine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
    }

    public void Fire(BugTrigger trigger)
    {
        if (_machine.CanFire(trigger))
            _machine.Fire(trigger);
    }

    public bool CanFire(BugTrigger trigger)
    {
        return _machine.CanFire(trigger);
    }
}

public class Program
{
    public static void Main()
    {
        var bug = new Bug();

        Console.WriteLine($"Initial State: {bug.CurrentState}");

        bug.Fire(BugTrigger.Assign);
        Console.WriteLine($"After Assign: {bug.CurrentState}");

        bug.Fire(BugTrigger.StartProgress);
        Console.WriteLine($"After StartProgress: {bug.CurrentState}");

        bug.Fire(BugTrigger.Fix);
        Console.WriteLine($"After Fix: {bug.CurrentState}");

        bug.Fire(BugTrigger.Verify);
        Console.WriteLine($"After Verify: {bug.CurrentState}");

        bug.Fire(BugTrigger.Close);
        Console.WriteLine($"After Close: {bug.CurrentState}");
    }
}
