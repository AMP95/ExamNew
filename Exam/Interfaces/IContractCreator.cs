using DTOs;
using DTOs.Dtos;

namespace Exam.Interfaces
{
    public interface IContractCreator
    {
        string CreateContractDocument(ContractDto contract, string templatePathWithoutRoot);
    }
}
