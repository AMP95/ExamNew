using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;

namespace MediatorServices
{
    public class GetIdDocumentService : GetIdModelService<Document>
    {
        public GetIdDocumentService(IRepository repository, ILogger<GetIdModelService<Document>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            Document document = await _repository.GetById<Document>(id);
            DocumentDto dto = null;

            if (document != null)
            {
                dto = new DocumentDto()
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
            return dto;
        }
    }

    public class GetMainIdDocumentService : GetMainIdModelService<Document>
    {
        public GetMainIdDocumentService(IRepository repository, ILogger<GetMainIdModelService<Document>> logger) : base(repository, logger)
        {
        }

        protected async override Task<object> Get(Guid id)
        {
            IEnumerable<Document> documents = await _repository.Get<Document>(d => d.ContractId == id);
            List<DocumentDto> dtos = new List<DocumentDto>();

            foreach (var document in documents)
            {
                dtos.Add(new DocumentDto()
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
                });
            }

            return dtos;
        }
    }

    public class DeleteDocumentService : DeleteModelService<Document>
    {
        public DeleteDocumentService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddDocumentService : AddModelService<DocumentDto>
    {
        public AddDocumentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(DocumentDto dto)
        {
            Document document = new Document()
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

            return await _repository.Update(document);
        }
    }

    public class UpdateDocumentService : UpdateModelService<DocumentDto>
    {
        public UpdateDocumentService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(DocumentDto dto)
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

            return await _repository.Update(document);
        }
    }
}
