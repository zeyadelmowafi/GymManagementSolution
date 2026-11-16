using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.BLL.Results;

public sealed class Result<T>
{
    private Result(bool success, T? value, string? error, ResultCode code) { IsSuccess = success; Value = value; Error = error; Code = code; }
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? Error { get; }
    public ResultCode Code { get; }
    public static Result<T> Ok(T value) => new(true, value, null, ResultCode.None);
    public static Result<T> Fail(string error, ResultCode code = ResultCode.Invalid) => new(false, default, error, code);
}
