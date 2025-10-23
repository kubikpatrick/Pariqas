namespace Pariqas.Api.Executors;

public static class ResultExecutor
{
    public static async Task<OperationResult> ExecuteAsync(Func<Task> func)
    {
        try
        {
            await func();

            return OperationResult.Success();
        }
        catch (Exception ex)
        {
            return OperationResult.Fail(ex.Message);
        }
    }
}