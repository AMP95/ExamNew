using Exam.BackgroundServices;
using Exam.Interfaces;
using System.Collections.Concurrent;

namespace Exam.ResultServices
{
    public class ResultService: IResultService
    {
        private ConcurrentDictionary<Guid, ServiceResult> _results;
        private IRequestStatusService _statusService;

        public ResultService(IRequestStatusService status)
        {
            _statusService = status;
            _results = new ConcurrentDictionary<Guid, ServiceResult>();
        }

        public Task AddResult(Guid id, ServiceResult result)
        {
            return Task.Run(() =>
            {
                if (_results.TryAdd(id, result))
                {
                    _statusService.UpdateStatus(id, RequestStatus.Done);
                }

                if (_results.Count > 1000) 
                {
                    ClearResults(); 
                }
            });
        }

        private void ClearResults() 
        {
            var old = _results.Where(r => (DateTime.Now - r.Value.CreateTime).TotalHours > 1).ToList();
            foreach (var r in old) 
            {
                if (_results.TryRemove(r.Key, out ServiceResult result)) 
                {
                    _statusService.RemoveStatus(r.Key);
                }
            }
        }

        public async Task<ServiceResult> GetResult(Guid id)
        {
            if (_results.TryRemove(id, out ServiceResult result))
            {
                _statusService.RemoveStatus(id);
                return result;
            }
            return null;
        }

    }
}
