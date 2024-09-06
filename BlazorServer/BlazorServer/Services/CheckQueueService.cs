using System.Collections.Concurrent;
using Shared.DB.Test.Answers;

namespace BlazorServer.Services;

public class CheckQueueService
{
    public readonly ConcurrentQueue<TestAnswer> TestAnswers = new ConcurrentQueue<TestAnswer>();
    public readonly ConcurrentQueue<TaskAnswer> SqlTasks = new ConcurrentQueue<TaskAnswer>();
}