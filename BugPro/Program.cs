using Stateless;

namespace BugPro;

public enum State
{
    NewDefect,
    Analysis,
    Resolution,
    Returned,
    Closed,
    Reopened,
    NeedMoreInfo,
    Review,
    OnHold
}

public enum Trigger
{
    Analyze,
    Reject,
    AskInfo,
    ProvideInfo,
    StartFix,
    VerifySuccess,
    VerifyFailure,
    ReportCannotReproduce,
    ReturnForInfo,
    ContinueFix,
    ConfirmNotRepro,
    ConfirmBugExists,
    Reopen,
    AnalyzeAgain,
    Hold,
    Resume
}

public class Bug
{
    private readonly StateMachine<State, Trigger> _machine;
    
    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(State.NewDefect);
        
        _machine.Configure(State.NewDefect)
            .Permit(Trigger.Analyze, State.Analysis)
            .Permit(Trigger.Hold, State.OnHold);
        
        _machine.Configure(State.Analysis)
            .Permit(Trigger.Reject, State.Returned)
            .Permit(Trigger.AskInfo, State.NeedMoreInfo)
            .Permit(Trigger.StartFix, State.Resolution)
            .Permit(Trigger.Hold, State.OnHold);
        
        _machine.Configure(State.NeedMoreInfo)
            .Permit(Trigger.ProvideInfo, State.Analysis)
            .Permit(Trigger.ContinueFix, State.Resolution)
            .Permit(Trigger.Reject, State.Returned);
        
        _machine.Configure(State.Resolution)
            .Permit(Trigger.VerifySuccess, State.Closed)
            .Permit(Trigger.VerifyFailure, State.Returned)
            .Permit(Trigger.ReportCannotReproduce, State.Review)
            .Permit(Trigger.ReturnForInfo, State.NeedMoreInfo)
            .Permit(Trigger.Hold, State.OnHold);
        
        _machine.Configure(State.Review)
            .Permit(Trigger.ConfirmNotRepro, State.Closed)
            .Permit(Trigger.ConfirmBugExists, State.Returned);
        
        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);
        
        _machine.Configure(State.Reopened)
            .Permit(Trigger.AnalyzeAgain, State.Analysis);
        
        _machine.Configure(State.Returned)
            .Permit(Trigger.Analyze, State.Analysis);
        
        _machine.Configure(State.OnHold)
            .Permit(Trigger.Resume, State.Analysis);
    }
    
    public void Analyze() => _machine.Fire(Trigger.Analyze);
    public void Reject() => _machine.Fire(Trigger.Reject);
    public void AskInfo() => _machine.Fire(Trigger.AskInfo);
    public void ProvideInfo() => _machine.Fire(Trigger.ProvideInfo);
    public void StartFix() => _machine.Fire(Trigger.StartFix);
    public void VerifySuccess() => _machine.Fire(Trigger.VerifySuccess);
    public void VerifyFailure() => _machine.Fire(Trigger.VerifyFailure);
    public void ReportCannotReproduce() => _machine.Fire(Trigger.ReportCannotReproduce);
    public void ReturnForInfo() => _machine.Fire(Trigger.ReturnForInfo);
    public void ContinueFix() => _machine.Fire(Trigger.ContinueFix);
    public void ConfirmNotRepro() => _machine.Fire(Trigger.ConfirmNotRepro);
    public void ConfirmBugExists() => _machine.Fire(Trigger.ConfirmBugExists);
    public void Reopen() => _machine.Fire(Trigger.Reopen);
    public void AnalyzeAgain() => _machine.Fire(Trigger.AnalyzeAgain);
    public void Hold() => _machine.Fire(Trigger.Hold);
    public void Resume() => _machine.Fire(Trigger.Resume);
    
    public State CurrentState => _machine.State;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Демонстрация Workflow бага ===\n");
        var bug = new Bug();
        Console.WriteLine($"1. Новый дефект: {bug.CurrentState}");
        bug.Analyze();
        Console.WriteLine($"2. После анализа: {bug.CurrentState}");
        bug.StartFix();
        Console.WriteLine($"3. Начато исправление: {bug.CurrentState}");
        bug.VerifySuccess();
        Console.WriteLine($"4. Проверка успешна: {bug.CurrentState}");
        Console.WriteLine("\n=== Дефект успешно исправлен ===");
    }
}