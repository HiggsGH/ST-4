using BugPro;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BugTests;

[TestClass]
public class BugWorkflowTests
{
    [TestMethod] 
    public void Test01_InitialState_NewDefect()
    {
        var bug = new Bug();
        Assert.AreEqual(State.NewDefect, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test02_Analyze_ToAnalysis()
    {
        var bug = new Bug();
        bug.Analyze();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test03_Reject_ToReturned()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.Reject();
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test04_AskInfo_ToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.AskInfo();
        Assert.AreEqual(State.NeedMoreInfo, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test05_ProvideInfo_ToAnalysis()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.AskInfo();
        bug.ProvideInfo();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test06_StartFix_ToResolution()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        Assert.AreEqual(State.Resolution, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test07_VerifySuccess_ToClosed()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test08_VerifyFailure_ToReturned()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifyFailure();
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test09_ReportCannotReproduce_ToReview()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        Assert.AreEqual(State.Review, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test10_ConfirmNotRepro_ToClosed()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        bug.ConfirmNotRepro();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test11_ConfirmBugExists_ToReturned()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        bug.ConfirmBugExists();
        Assert.AreEqual(State.Returned, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test12_ReturnForInfo_ToNeedMoreInfo()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReturnForInfo();
        Assert.AreEqual(State.NeedMoreInfo, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test13_ContinueFix_ToResolution()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReturnForInfo();
        bug.ContinueFix();
        Assert.AreEqual(State.Resolution, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test14_Reopen_ToReopened()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        bug.Reopen();
        Assert.AreEqual(State.Reopened, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test15_AnalyzeAgain_ToAnalysis()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        bug.Reopen();
        bug.AnalyzeAgain();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test16_FullPositiveWorkflow()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test17_InfoRequestWorkflow()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.AskInfo();
        bug.ProvideInfo();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test18_ReturnForInfoWorkflow()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReturnForInfo();
        bug.ContinueFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test19_NotReproducibleWorkflow()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.ReportCannotReproduce();
        bug.ConfirmNotRepro();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod] 
    public void Test20_ReopenWorkflow()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.VerifySuccess();
        bug.Reopen();
        bug.AnalyzeAgain();
        bug.StartFix();
        bug.VerifySuccess();
        Assert.AreEqual(State.Closed, bug.CurrentState);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test21_StartFixFromNewDefect_Throws() => new Bug().StartFix();
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test22_VerifySuccessFromAnalysis_Throws()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.VerifySuccess();
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Test23_ReopenFromResolution_Throws()
    {
        var bug = new Bug();
        bug.Analyze();
        bug.StartFix();
        bug.Reopen();
    }
    
    [TestMethod]
    public void Test24_Hold_ToOnHold()
    {
        var bug = new Bug();
        bug.Hold();
        Assert.AreEqual(State.OnHold, bug.CurrentState);
    }
    
    [TestMethod]
    public void Test25_Resume_ToAnalysis()
    {
        var bug = new Bug();
        bug.Hold();
        bug.Resume();
        Assert.AreEqual(State.Analysis, bug.CurrentState);
    }
}