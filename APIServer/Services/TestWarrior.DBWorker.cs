using System.Data;
using APIServer.Extensions;
using Microsoft.Data.Sqlite;
using Shared.DB.Test.Answers;
using Shared.Types;
using OriginalTask = Shared.DB.Test.Task.Task;

namespace APIServer.Services;

public sealed partial class TestWarrior
{
    private async void CheckSqlQuery(SqlTaskPare taskPare)
    {
        await TaskExtension.SetupConnection(taskPare.OriginalTask);
    }

    private void SaveSqlTask(TaskAnswer taskAnswer)
    {
        dbContext.Update(taskAnswer);
        dbContext.SaveChanges();
    }
    
    private void MissingSqlTasks(TaskAnswer taskAnswer)
    {
        var testAnswer = taskAnswer.TestAnswer;
        testAnswer.Score += testAnswer.TaskWeight;
        taskAnswer.StringAnswer = "SQL string on server are not filled or bad, check is skipped";

        SaveSqlTask(taskAnswer);
    }
    
}

public class SqlTaskPare
{
    public required TaskAnswer TaskAnswer;
    public required OriginalTask OriginalTask;
}
