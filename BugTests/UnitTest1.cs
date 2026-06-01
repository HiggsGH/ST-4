using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    private Bug _bug = null!;
    
    [TestInitialize]
    public void Setup()
    {
        _bug = new Bug();
    }
    
    [TestMethod]
    public void Test01_InitialState_ShouldBeNew()
    {
        Assert.AreEqual(BugState.New, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test02_StartTriage_ShouldChangeToTriage()
    {
        _bug.StartTriage();
        Assert.AreEqual(BugState.Triage, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test03_Postpone_FromTriage_ShouldChangeToOnHold()
    {
        _bug.StartTriage();
        _bug.Postpone();
        Assert.AreEqual(BugState.OnHold, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test04_SeparateIssue_FromTriage_ShouldChangeToOnHold()
    {
        _bug.StartTriage();
        _bug.SeparateIssue();
        Assert.AreEqual(BugState.OnHold, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test05_NeedInfo_FromTriage_ShouldChangeToOnHold()
    {
        _bug.StartTriage();
        _bug.NeedInfo();
        Assert.AreEqual(BugState.OnHold, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test06_StartTriage_FromOnHold_ShouldReturnToTriage()
    {
        _bug.StartTriage();
        _bug.Postpone();
        _bug.StartTriage();
        Assert.AreEqual(BugState.Triage, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test07_NotABug_ShouldChangeToRejected()
    {
        _bug.StartTriage();
        _bug.NotABug();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test08_Duplicate_ShouldChangeToRejected()
    {
        _bug.StartTriage();
        _bug.Duplicate();
        Assert.AreEqual(BugState.Rejected, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test09_StartFix_ShouldChangeToFixing()
    {
        _bug.StartTriage();
        _bug.StartFix();
        Assert.AreEqual(BugState.Fixing, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test10_Postpone_FromFixing_ShouldChangeToOnHold()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.Postpone();
        Assert.AreEqual(BugState.OnHold, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test11_CannotFix_ShouldChangeToCannotReproduce()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.CannotFix();
        Assert.AreEqual(BugState.CannotReproduce, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test12_ConfirmOk_FromCannotReproduce_ShouldChangeToClosed()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.CannotFix();
        _bug.ConfirmOk();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test13_ConfirmNotOk_FromCannotReproduce_ShouldChangeToReturned()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.CannotFix();
        _bug.ConfirmNotOk();
        Assert.AreEqual(BugState.Returned, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test14_MarkFixed_ShouldChangeToNeedCheck()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        Assert.AreEqual(BugState.NeedCheck, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test15_Close_FromNeedCheck_ShouldChangeToClosed()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test16_MarkNotFixed_ShouldChangeToReturned()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.MarkNotFixed();
        Assert.AreEqual(BugState.Returned, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test17_StartTriage_FromReturned_ShouldReturnToTriage()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.MarkNotFixed();
        _bug.StartTriage();
        Assert.AreEqual(BugState.Triage, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test18_Reopen_FromClosed_ShouldChangeToReopened()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        _bug.Reopen();
        Assert.AreEqual(BugState.Reopened, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test19_StartTriage_FromReopened_ShouldReturnToTriage()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        _bug.Reopen();
        _bug.StartTriage();
        Assert.AreEqual(BugState.Triage, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test20_FullHappyPathWorkflow()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test21_FullWorkflowWithReturn()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.MarkNotFixed();
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test22_FullWorkflowWithCannotReproduce()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.CannotFix();
        _bug.ConfirmOk();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test23_FullWorkflowWithCannotReproduceAndReturn()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.CannotFix();
        _bug.ConfirmNotOk();
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    public void Test24_FullWorkflowWithReopen()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        _bug.Reopen();
        _bug.StartTriage();
        _bug.StartFix();
        _bug.MarkFixed();
        _bug.Close();
        Assert.AreEqual(BugState.Closed, _bug.CurrentState);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test25_StartFixFromNew_ShouldThrow()
    {
        _bug.StartFix();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test26_CloseFromFixing_ShouldThrow()
    {
        _bug.StartTriage();
        _bug.StartFix();
        _bug.Close();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test27_ReopenFromTriage_ShouldThrow()
    {
        _bug.StartTriage();
        _bug.Reopen();
    }
}