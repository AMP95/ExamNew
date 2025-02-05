using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    internal static class DocumentConverter
    {
        public static DocumentDto Convert(Document document)
        {
            return new DocumentDto()
            {
                Id = document.Id,
                ContractId = document.ContractId,
                CreationDate = document.CreationDate,
                Direction = (DocumentDirection)document.DocumentDirection,
                Number = document.Number,
                RecieveType = (RecievingType)document.RecieveType,
                RecievingDate = document.RecievingDate,
                Summ = document.Summ,
                Type = (DocumentType)document.DocumentType
            };
        }

        public static Document Convert(DocumentDto dto)
        {
            return new Document()
            {
                ContractId = dto.ContractId,
                CreationDate = dto.CreationDate,
                DocumentType = (short)dto.Type,
                Summ = dto.Summ,
                DocumentDirection = (short)dto.Direction,
                Number = dto.Number,
                RecieveType = (short)dto.RecieveType,
                RecievingDate = dto.RecievingDate,
            };
        }
    }


    internal static class StatusUpdater 
    {
        public async static Task<bool> UpdateContractStatus(Guid contractId, IRepository repository) 
        {
            IEnumerable<Contract> contracts = await repository.Get<Contract>(contract => contract.Id == contractId, null, "Documents,Payments");

            Contract contract = contracts.FirstOrDefault();

            if (contract.Status == (short)ContractStatus.Failed) 
            { 
                return false;
            }

            contract.Status = (short)ContractStatus.Created;


            IEnumerable<Document> incomeDoc = contract.Documents.Where(d => d.DocumentDirection == (short)DocumentDirection.Income).ToList();
            IEnumerable<Document> outcomeDoc = contract.Documents.Where(d => d.DocumentDirection == (short)DocumentDirection.Outcome).ToList();

            IEnumerable<Payment> incomePay = contract.Payments.Where(d => d.DocumentDirection == (short)DocumentDirection.Outcome);

            if (incomeDoc.Any() && (incomeDoc.All(d => d.DocumentType == (short)DocumentType.Bill ||
                                   d.DocumentType == (short)DocumentType.UDT ||
                                   d.DocumentType == (short)DocumentType.TTN) || incomeDoc.All(d => d.DocumentType == (short)DocumentType.Bill ||
                                                                                                    d.DocumentType == (short)DocumentType.Act ||
                                                                                                    d.DocumentType == (short)DocumentType.Invoice ||
                                                                                                    d.DocumentType == (short)DocumentType.TTN)))
            {
                var bills = incomeDoc.Where(d => d.DocumentType == (short)DocumentType.Bill);

                double summ = bills.Sum(b => b.Summ);

                if (summ >= contract.CarrierPayment)
                {
                    contract.Status = (short)ContractStatus.DocumentsRecieved;
                }
            }

            if (outcomeDoc.Any() && (outcomeDoc.All(d => d.DocumentType == (short)DocumentType.Bill ||
                                                        d.DocumentType == (short)DocumentType.UDT ||
                                                        d.DocumentType == (short)DocumentType.TTN) || outcomeDoc.All(d => d.DocumentType == (short)DocumentType.Bill ||
                                                                                                                          d.DocumentType == (short)DocumentType.Act ||
                                                                                                                          d.DocumentType == (short)DocumentType.Invoice ||
                                                                                                                          d.DocumentType == (short)DocumentType.TTN)))
            {
                contract.Status = (short)ContractStatus.DocumentSended;
            }

            if (incomePay.Sum(p => p.Summ) >= contract.ClientPayment) 
            {
                contract.Status = (short)ContractStatus.Closed;
            }

            return await repository.Update(contract);
        }
    }


    public class GetIdDocumentService : GetIdModelService<DocumentDto>
    {
        public GetIdDocumentService(IRepository repository, ILogger<GetIdModelService<DocumentDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Document document = await _repository.GetById<Document>(id);
            DocumentDto dto = null;

            if (document != null)
            {
                dto = DocumentConverter.Convert(document);
            }
            return dto;
        }
    }

    public class GetFilterDocumentService : IRequestHandler<GetFilter<DocumentDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilterDocumentService> _logger;

        public GetFilterDocumentService(IRepository repository, ILogger<GetFilterDocumentService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<DocumentDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Document, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Document, bool>> filter)
        {
            IEnumerable<Document> documents = await _repository.Get<Document>(filter);
            List<DocumentDto> dtos = new List<DocumentDto>();

            foreach (var document in documents)
            {
                dtos.Add(DocumentConverter.Convert(document));
            }

            return dtos;
        }

        protected Expression<Func<Document, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Document, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(DocumentDto.ContractId):
                        if (Guid.TryParse(parameters[0].ToString(), out Guid id)) 
                        {
                            filter = d => d.ContractId == id;
                        }
                        break;
                    case nameof(DocumentDto.Type):
                        DocumentType documentType = (DocumentType)parameters[0];
                        filter = d => d.DocumentType == (short)documentType;
                        break;
                    case nameof(DocumentDto.Direction):
                        DocumentDirection direction = (DocumentDirection)parameters[0];
                        filter = d => d.DocumentDirection == (short)direction;
                        break;
                    case nameof(DocumentDto.RecieveType):
                        RecievingType recieve = (RecievingType)parameters[0];
                        filter = d => d.RecieveType == (short)recieve;
                        break;
                }
            }
            catch (Exception ex)
            {
                filter = c => false;
                _logger.LogError(ex, ex.Message);
            }

            return filter;
        }
    }

    public class DeleteDocumentService : IRequestHandler<Delete<DocumentDto>, bool>
    {
        private IRepository _repository;

        public DeleteDocumentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(Delete<DocumentDto> request, CancellationToken cancellationToken)
        {
            bool result = false;

            try
            {
                Document document = await _repository.GetById<Document>(request.Id);

                result = await _repository.Remove<Document>(document.Id);

                await StatusUpdater.UpdateContractStatus(document.ContractId, _repository);
            }
            catch (Exception ex) 
            {
            
            }
            return result;
        }
    }

    public class AddDocumentService : AddModelService<DocumentDto>
    {
        public AddDocumentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<Guid> Add(DocumentDto dto)
        {
            Guid docId = Guid.Empty;

            try
            {
                docId = await _repository.Add(DocumentConverter.Convert(dto));

                await StatusUpdater.UpdateContractStatus(dto.ContractId, _repository);
                
            }
            catch (Exception ex) 
            { 
                
            }

            return docId;
        }
    }

    public class UpdateDocumentService : UpdateModelService<DocumentDto>
    {
        public UpdateDocumentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(DocumentDto dto)
        {
            bool result = false;

            try
            {
                Document document = await _repository.GetById<Document>(dto.Id);

                document.Id = dto.Id;
                document.CreationDate = dto.CreationDate;
                document.DocumentDirection = (short)dto.Direction;
                document.Number = dto.Number;
                document.RecieveType = (short)dto.RecieveType;
                document.RecievingDate = dto.RecievingDate;
                document.Summ = dto.Summ;
                document.DocumentType = (short)dto.Type;

                result = await _repository.Update(document);

                await StatusUpdater.UpdateContractStatus(dto.ContractId, _repository);
            }
            catch (Exception ex) 
            {
                
            }

            return result;
        }
    }
}
