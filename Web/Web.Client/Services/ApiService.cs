using Shared.Data.Test;
using Shared.Data.Test.Answers;
using Shared.Data.Test.Task;

namespace Web.Client.Services;

public class ApiService
{
    public async Task<Guid> SubmitTest(TestAnswer testAnswer)
    {
        return Guid.Empty;
        throw new NotImplementedException();
    }

    public async Task<UniqueTest> GetTest(string testLink)
    {
        return await Task.Run(() => new UniqueTest());
        throw new NotImplementedException();
    }
}