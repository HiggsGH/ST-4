using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triage,
    OnHold,
    Rejected,
    Fixing,
    CannotReproduce,
    NeedCheck,
    Returned,
    Closed,
    Reopened
}

public enum BugAction
{
    StartTriage,
    Postpone,
    SeparateIssue,
    NeedInfo,
    NotABug,
    Duplicate,
    StartFix,
    CannotFix,
    MarkFixed,
    MarkNotFixed,
    ConfirmOk,
    ConfirmNotOk,
    Close,
    Reopen
}

public class Bug
{
    private readonly StateMachine<BugState, BugAction> _machine;
    
    public Bug()
    {
        _machine = new StateMachine<BugState, BugAction>(BugState.New);
        
        _machine.Configure(BugState.New)
            .Permit(BugAction.StartTriage, BugState.Triage);
        
        _machine.Configure(BugState.Triage)
            .Permit(BugAction.Postpone, BugState.OnHold)
            .Permit(BugAction.SeparateIssue, BugState.OnHold)
            .Permit(BugAction.NeedInfo, BugState.OnHold)
            .Permit(BugAction.NotABug, BugState.Rejected)
            .Permit(BugAction.Duplicate, BugState.Rejected)
            .Permit(BugAction.StartFix, BugState.Fixing);
        
        _machine.Configure(BugState.OnHold)
            .Permit(BugAction.StartTriage, BugState.Triage);
        
        _machine.Configure(BugState.Fixing)
            .Permit(BugAction.Postpone, BugState.OnHold)
            .Permit(BugAction.SeparateIssue, BugState.OnHold)
            .Permit(BugAction.NeedInfo, BugState.OnHold)
            .Permit(BugAction.CannotFix, BugState.CannotReproduce)
            .Permit(BugAction.MarkFixed, BugState.NeedCheck);
        
        _machine.Configure(BugState.CannotReproduce)
            .Permit(BugAction.ConfirmOk, BugState.Closed)
            .Permit(BugAction.ConfirmNotOk, BugState.Returned);
        
        _machine.Configure(BugState.NeedCheck)
            .Permit(BugAction.Close, BugState.Closed)
            .Permit(BugAction.MarkNotFixed, BugState.Returned);
        
        _machine.Configure(BugState.Returned)
            .Permit(BugAction.StartTriage, BugState.Triage);
        
        _machine.Configure(BugState.Closed)
            .Permit(BugAction.Reopen, BugState.Reopened);
        
        _machine.Configure(BugState.Reopened)
            .Permit(BugAction.StartTriage, BugState.Triage);
    }
    
    public void StartTriage() => _machine.Fire(BugAction.StartTriage);
    public void Postpone() => _machine.Fire(BugAction.Postpone);
    public void SeparateIssue() => _machine.Fire(BugAction.SeparateIssue);
    public void NeedInfo() => _machine.Fire(BugAction.NeedInfo);
    public void NotABug() => _machine.Fire(BugAction.NotABug);
    public void Duplicate() => _machine.Fire(BugAction.Duplicate);
    public void StartFix() => _machine.Fire(BugAction.StartFix);
    public void CannotFix() => _machine.Fire(BugAction.CannotFix);
    public void MarkFixed() => _machine.Fire(BugAction.MarkFixed);
    public void MarkNotFixed() => _machine.Fire(BugAction.MarkNotFixed);
    public void ConfirmOk() => _machine.Fire(BugAction.ConfirmOk);
    public void ConfirmNotOk() => _machine.Fire(BugAction.ConfirmNotOk);
    public void Close() => _machine.Fire(BugAction.Close);
    public void Reopen() => _machine.Fire(BugAction.Reopen);
    
    public BugState CurrentState => _machine.State;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Bug Workflow Demo ===\n");
        var bug = new Bug();
        
        Console.WriteLine($"1. Initial: {bug.CurrentState}");
        bug.StartTriage();
        Console.WriteLine($"2. After triage: {bug.CurrentState}");
        bug.StartFix();
        Console.WriteLine($"3. After fix assigned: {bug.CurrentState}");
        bug.MarkFixed();
        Console.WriteLine($"4. After fix marked: {bug.CurrentState}");
        bug.Close();
        Console.WriteLine($"5. After close: {bug.CurrentState}");
        
        Console.WriteLine("\n=== Workflow completed ===");
    }
}