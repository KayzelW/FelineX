using Shared.DB.Test.Answers;

namespace APIServer.Services;

public interface ITestWarriorQueue
{
    void RegisterTestAnswer(TestAnswer test);
}