using Shared.Data.Test.Answers;
using Web.Extensions;

namespace Web.Services;

public sealed partial class TestWarrior
{
    private async Task CheckSqlQueryAsync(TaskAnswer taskAnswer)
    {
        try
        {
            var conn = await TaskExtension.SetupConnection(taskAnswer.AnsweredTask!);
            if (conn == null)
            {
                await MissingSqlTasksAsync(taskAnswer);
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
                await SaveSqlTaskAsync(taskAnswer);
                return;
            }

            var needRows = taskAnswer.AnsweredTask!.DataRows!.Except(rows);
            taskAnswer.IsSuccess = false;
            taskAnswer.IsFailedCheck = false;
            taskAnswer.Result = $"Queries are different: \n{string.Join(", ", needRows)}";
            await SaveSqlTaskAsync(taskAnswer);
            return;
        }
        catch (Exception e)
        {
            await FailedToCheckSqlTaskAsync(taskAnswer, e);
            return;
        }

        await FailedToCheckSqlTaskAsync(taskAnswer, new Exception("Something wonderful was happened(all checks are crushed....)"));
    }

    private async Task SaveSqlTaskAsync(TaskAnswer taskAnswer)
    {
        // dbContext.Entry(taskAnswer.AnsweredTask.Settings).State = EntityState.Unchanged;//TODO fix cringe State
        taskAnswer.IsCheckEnded = true;
        _dbContext.Update(taskAnswer);
        await _dbContext.SaveChangesAsync();
    }

    private async Task MissingSqlTasksAsync(TaskAnswer taskAnswer)
    {
        var testAnswer = taskAnswer.TestAnswer;
        testAnswer.Score += testAnswer.TaskWeight;
        taskAnswer.StringAnswer = "SQL string on server are not filled or bad, check is skipped";
        taskAnswer.IsSuccess = true;
        taskAnswer.IsFailedCheck = true;

        await SaveSqlTaskAsync(taskAnswer);
    }

    private async Task FailedToCheckSqlTaskAsync(TaskAnswer taskAnswer, Exception ex)
    {
        _logger.LogWarning(ex, "Error while try exec taskAnswer query");
        taskAnswer.Result = ex.Message;
        taskAnswer.IsFailedCheck = true;
        taskAnswer.IsSuccess = false;

        await SaveSqlTaskAsync(taskAnswer);
    }
}