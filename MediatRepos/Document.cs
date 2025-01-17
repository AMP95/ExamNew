using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using System.Linq.Expressions;

namespace MediatorServices
{
    public static class DocumentConverter
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
                dto = DocumentConverter.Convert(document);
            }
            return dto;
        }
    }

    public class GetFilterDocumentService : GetFilterModelService<Document>
    {
        public GetFilterDocumentService(IRepository repository, ILogger<GetFilterModelService<Document>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Expression<Func<Document, bool>> filter)
        {
            IEnumerable<Document> documents = await _repository.Get<Document>(filter);
            List<DocumentDto> dtos = new List<DocumentDto>();

            foreach (var document in documents)
            {
                dtos.Add(DocumentConverter.Convert(document));
            }

            return dtos;
        }

        protected override Expression<Func<Document, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Document, bool>> filter = null;
            switch (property) 
            {
                case nameof(DocumentDto.ContractId):
                    Guid guid = (Guid)parameters[0];
                    filter = d => d.ContractId == guid;
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

            return filter;
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
            return await _repository.Update(DocumentConverter.Convert(dto));
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
