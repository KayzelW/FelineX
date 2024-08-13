using System.Collections.Concurrent;
using Shared.DB.Test.Answers;

namespace APIServer.Services;

public class CheckQueueService
{
    public ConcurrentQueue<TestAnswer>? _testAnswers = new ConcurrentQueue<TestAnswer>();
    public ConcurrentQueue<TaskAnswer>? _sqlTasks = new ConcurrentQueue<TaskAnswer>();
}