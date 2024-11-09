using Web.Data;
using Web.Data.Test.Answers;
using Web.Extensions;

namespace Web.Services;

public sealed partial class TestWarrior
{
    private async void CheckSqlQuery(TaskAnswer taskAnswer)
    {
        try
        {
            var conn = await TaskExtension.SetupConnection(taskAnswer.AnsweredTask!);
            if (conn == null)
            {
                MissingSqlTasks(taskAnswer);
                return;
            }

            var cmd = conn.CreateCommand();
            //TODO: check for not contains any USE or DROP operators
            cmd.CommandText = taskAnswer.StringAnswer;
            var reader = await cmd.ExecuteReaderAsync();
            var rows = TaskExtension.ExtractItemRows(reader);
            if (taskAnswer.AnsweredTask!.DataRows!.SequenceEqual(rows))
            {
                taskAnswer.IsSuccess = true;
                taskAnswer.IsFailedCheck = false;
                taskAnswer.TestAnswer.Score += taskAnswer.TestAnswer.TaskWeight;
                taskAnswer.Result = "Exit queries are completely identical";
                SaveSqlTask(taskAnswer);
                return;
            }

            var needRows = taskAnswer.AnsweredTask!.DataRows!.Except(rows);
            taskAnswer.IsSuccess = false;
            taskAnswer.IsFailedCheck = false;
            taskAnswer.Result = $"Queries are different: \n{string.Join(", ", needRows)}";
            SaveSqlTask(taskAnswer);
            return;
        }
        catch (Exception e)
        {
            FailedToCheckSqlTask(taskAnswer, e);
            return;
        }

        FailedToCheckSqlTask(taskAnswer, new Exception("Something wonderful was happened(all checks are crushed....)"));
    }

    private void SaveSqlTask(TaskAnswer taskAnswer)
    {
        using var scope = _serviceProvider.CreateScope();
        dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // dbContext.Entry(taskAnswer.AnsweredTask.Settings).State = EntityState.Unchanged;//TODO fix cringe State
        taskAnswer.IsCheckEnded = true;
        dbContext.Update(taskAnswer);
        dbContext.SaveChanges();
    }

    private void MissingSqlTasks(TaskAnswer taskAnswer)
    {
        var testAnswer = taskAnswer.TestAnswer;
        testAnswer.Score += testAnswer.TaskWeight;
        taskAnswer.StringAnswer = "SQL string on server are not filled or bad, check is skipped";
        taskAnswer.IsSuccess = true;
        taskAnswer.IsFailedCheck = true;

        SaveSqlTask(taskAnswer);
    }

    private void FailedToCheckSqlTask(TaskAnswer taskAnswer, Exception ex)
    {
        _logger.LogWarning(ex, "Error while try exec taskAnswer query");
        taskAnswer.Result = ex.Message;
        taskAnswer.IsFailedCheck = true;
        taskAnswer.IsSuccess = false;

        SaveSqlTask(taskAnswer);
    }
}

