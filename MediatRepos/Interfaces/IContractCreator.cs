using DTOs;

namespace MediatorServices.Abstract
{
    public interface IContractCreator
    {
        string CreateContractDocument(ContractDto contract, string templatePathWithoutRoot);
    }
}
