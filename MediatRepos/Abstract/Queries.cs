using DTOs.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MediatRepos
{
    public class Update<TDto> : IRequest<bool> where TDto : IDto
    {
        public TDto Value { get; set; }

        public Update(TDto value)
        {
            Value = value;
        }
    }

    public class Add<TDto> : IRequest<Guid> where TDto : IDto
    {
        public TDto Value { get; set; }

        public Add(TDto value)
        {
            Value = value;
        }
    }

    public class Delete<TDto> : IRequest<bool> where TDto : IDto
    {
        public Guid Id { get; set; }

        public Delete(Guid id)
        {
            Id = id;
        }
    }

    public class Patch<TDto> : IRequest<bool> where TDto : IDto
    {
        public Guid Id { get; set; }
        public KeyValuePair<string, object>[] Updates { get; set; }

        public Patch(Guid id, KeyValuePair<string, object>[] updates)
        {
            Id = id;
            Updates = updates;
        }
    }

    public class GetId<TDto> : IRequest<object> where TDto : IDto
    {
        public Guid Id { get; set; }

        public GetId(Guid id)
        {
            Id = id;
        }
    }

    public class GetFilter<TDto> : IRequest<object> where TDto : IDto
    { 
        public string PropertyName { get; set; }

        public object[] Params { get; set; }

        public GetFilter(string propertyName, params string[] parameters)
        {
            PropertyName = propertyName;
            Params = parameters;
        }
    }

    public class GetRange<TDto> : IRequest<object> where TDto : IDto
    {
        public int Start { get; set; }
        public int End { get; set; }

        public GetRange(int start, int end)
        {
            Start = start;
            End = end;
        }
    }

    public class AddFile : IRequest<Guid> 
    { 
        public IFormFile FormFile { get; set; }
        public FileDto FileDto { get; set; }

        public AddFile(FileData fileData)
        {
            FormFile = fileData.File;
            FileDto = fileData.FileDto;
        }
    }

    public class GetFile : IRequest<FileData> 
    {
        public Guid Id { get; set; }

        public GetFile(Guid id)
        {
            Id = id;
        }
    }
}
