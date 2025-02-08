using Utilities.Interfaces;

namespace MediatorServices
{
    public class MediatorServiceResult : IServiceResult<object>
    {
        public bool IsSuccess { get ; set ; }
        public string ErrorMessage { get ; set ; }
        public object Result { get ; set ; }

        public MediatorServiceResult()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
            Result = default;
        }
    }
}
