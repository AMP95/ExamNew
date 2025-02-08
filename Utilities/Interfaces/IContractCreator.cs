namespace Utilities.Interfaces
{
    public interface IContractCreator<Tcontract, Tcompany>
    {
        string CreateContractDocument(Tcontract contract, Tcompany company);
        string CreateContractPdf(string docPath, string stampPath);
    }
}
