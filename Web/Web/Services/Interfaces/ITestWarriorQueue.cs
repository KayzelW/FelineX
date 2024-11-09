using Web.Data.Test.Answers;

namespace Web.Services.Interfaces;

public interface ITestWarriorQueue
{
    void RegisterTestAnswer(TestAnswer test);
}