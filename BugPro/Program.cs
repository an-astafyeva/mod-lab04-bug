using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triaged,
    Fixed,
    Closed,
    Reopened,
    Deferred,
    Rejected
}

public enum BugTrigger
{
    Triage,
    Fix,
    Close,
    Reopen,
    Defer,
    Reject
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public BugState State => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.New);

        _machine.Configure(BugState.New)
            .Permit(BugTrigger.Triage, BugState.Triaged);

        _machine.Configure(BugState.Triaged)
            .Permit(BugTrigger.Fix, BugState.Fixed)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Fix, BugState.Fixed);
    }

    public void Fire(BugTrigger trigger)
    {
        _machine.Fire(trigger);
    }
}

public class Program
{
    public static void Main()
    {
        var bug = new Bug();

        Console.WriteLine($"Initial State: {bug.State}");

        bug.Fire(BugTrigger.Triage);
        Console.WriteLine($"After Triage: {bug.State}");

        bug.Fire(BugTrigger.Fix);
        Console.WriteLine($"After Fix: {bug.State}");

        bug.Fire(BugTrigger.Close);
        Console.WriteLine($"After Close: {bug.State}");
    }
}
