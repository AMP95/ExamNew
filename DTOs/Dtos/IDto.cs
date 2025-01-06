using System.ComponentModel;

namespace DTOs.Dtos
{
    public interface IDto : IDataErrorInfo
    {
        public Guid Id { get; }
    }
}
