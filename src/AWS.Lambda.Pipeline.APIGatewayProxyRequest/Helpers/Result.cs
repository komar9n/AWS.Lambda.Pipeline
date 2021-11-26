namespace AWS.Lambda.Pipeline.APIGatewayProxy.Helpers
{
    public readonly struct Result
    {
        private Result(bool isFailure, string error)
        {
            IsFailure = isFailure;
            Error = error;
        }

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public string Error { get; }

        public static Result Success()
        {
            return new Result(false, default);
        }

        public static Result Fail(string error)
        {
            return new Result(true, error);
        }
    }
}