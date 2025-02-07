using System.Collections.Concurrent;
using Utilities.Interfaces;

namespace Exam.ResultServices
{
    public class ResultService: IResultService
    {
        private ConcurrentDictionary<Guid, Tuple<DateTime, IServiceResult<object>>> _results;
        private IStatusService _statusService;

        public ResultService(IStatusService statusService)
        {
            _statusService = statusService;
            _results = new ConcurrentDictionary<Guid, Tuple<DateTime, IServiceResult<object>>>();
        }

        public Task AddResult(Guid id, IServiceResult<object> result)
        {
            return Task.Run(() =>
            {
                if (_results.TryAdd(id, Tuple.Create(DateTime.Now, result)))
                {
                    _statusService.UpdateStatus(id, Status.Done);
                }

                if (_results.Count > 1000) 
                {
                    ClearResults(); 
                }
            });
        }

        private void ClearResults() 
        {
            var old = _results.Where(r => (DateTime.Now - r.Value.Item1).TotalHours > 1).ToList();
            foreach (var r in old) 
            {
                if (_results.TryRemove(r.Key, out Tuple<DateTime,IServiceResult<object>> result)) 
                {
                    _statusService.RemoveStatus(r.Key);
                }
            }
        }

        public async Task<IServiceResult<object>> GetResult(Guid id)
        {
            if (_results.TryRemove(id, out Tuple<DateTime, IServiceResult<object>> result))
            {
                _statusService.RemoveStatus(id);
                return result.Item2;
            }
            return null;
        }
    }
}
