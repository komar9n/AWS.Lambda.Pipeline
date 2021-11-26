namespace AWS.Lambda.Pipeline
{
    public class PipelineResult<TResult>
    {
        private PipelineResult(TResult result, bool isFailure, string error, int? errorCode = null)
        {
            Result = result;
            IsFailure = isFailure;
            Error = error;
            ErrorCode = errorCode;
        }

        public int? ErrorCode { get; }

        public TResult Result { get; }

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public string Error { get; }

        public static PipelineResult<TResult> Success(TResult result)
        {
            return new PipelineResult<TResult>(result, false, default);
        }

        public static PipelineResult<TResult> Fail(string error, int errorCode = 500)
        {
            return new PipelineResult<TResult>(default, true, error, errorCode);
        }
    }
}
