using Shared.DB.Test.Answers;

namespace BlazorServer.Services;

public interface ITestWarriorQueue
{
    void RegisterTestAnswer(TestAnswer test);
}