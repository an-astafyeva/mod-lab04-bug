using Stateless;

namespace BugPro;
  
// Состояния бага
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

// Действия (триггеры)
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
    private StateMachine<BugState, BugTrigger> _machine;
    private BugState _currentState;
    
    public BugState CurrentState => _machine.State;
    
    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.Open);
        
        // Настройка автомата состояний
        ConfigureMachine();
    }
    
    private void ConfigureMachine()
    {
        // State: Open
        _machine.Configure(BugState.Open)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected);
        
        // State: Assigned
        _machine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.Reject, BugState.Rejected);
        
        // State: InProgress
        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Fix, BugState.Fixed);
        
        // State: Fixed
        _machine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Verify, BugState.Verified)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
        
        // State: Verified
        _machine.Configure(BugState.Verified)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
        
        // State: Closed
        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
        
        // State: Reopened
        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.Assigned);
        
        // State: Rejected
        _machine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
    }
    
    public void Fire(BugTrigger trigger)
    {
        if (_machine.CanFire(trigger))
        {
            _machine.Fire(trigger);
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Триггер: {trigger} -> Новое состояние: {_machine.State}");
        }
        else
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ОШИБКА: Нельзя выполнить {trigger} в состоянии {_machine.State}");
        }
    }
    
    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);
}

// Основная программа для демонстрации
class Program
{
    static void Main()
    {
        Console.WriteLine("=== Демонстрация WorkFlow бага ===\n");
        
        var bug = new Bug();
        Console.WriteLine($"Начальное состояние: {bug.CurrentState}\n");
        
        // Демонстрация корректного пути
        Console.WriteLine("--- Корректный сценарий ---");
        bug.Fire(BugTrigger.Assign);
        bug.Fire(BugTrigger.StartProgress);
        bug.Fire(BugTrigger.Fix);
        bug.Fire(BugTrigger.Verify);
        bug.Fire(BugTrigger.Close);
        
        Console.WriteLine("\n--- Сценарий с ошибкой ---");
        var bug2 = new Bug();
        Console.WriteLine($"Начальное состояние: {bug2.CurrentState}");
        bug2.Fire(BugTrigger.Fix); // Нельзя, так как баг ещё не взят в работу
        
        Console.WriteLine("\n--- Сценарий Reopen ---");
        var bug3 = new Bug();
        bug3.Fire(BugTrigger.Assign);
        bug3.Fire(BugTrigger.StartProgress);
        bug3.Fire(BugTrigger.Fix);
        bug3.Fire(BugTrigger.Reopen);
        Console.WriteLine($"Состояние после Reopen: {bug3.CurrentState}");
        
        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
