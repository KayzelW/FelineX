using System.Collections.Concurrent;
using Web.Data.Test.Answers;

namespace Web.Services;

public class CheckQueueService
{
    public readonly ConcurrentQueue<TestAnswer> TestAnswers = new ConcurrentQueue<TestAnswer>();
    public readonly ConcurrentQueue<TaskAnswer> SqlTasks = new ConcurrentQueue<TaskAnswer>();
}