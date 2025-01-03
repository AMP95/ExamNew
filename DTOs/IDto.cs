namespace DTOs
{
    public interface IDto
    {
        bool HasValidationError { get; }
        string ValidationError { get; }
    }
}
