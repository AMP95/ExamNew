using Exam.Interfaces;
using System.Collections.Concurrent;

namespace Exam.ResultServices
{
    public class RequestStatusService : IRequestStatusService
    {
        private ConcurrentDictionary<Guid, RequestStatus> _statuses;

        public RequestStatusService()
        {
            _statuses = new ConcurrentDictionary<Guid, RequestStatus>();
        }

        public Task UpdateStatus(Guid id, RequestStatus status)
        {
            return Task.Run(() =>
            {
                if (_statuses.ContainsKey(id))
                {
                    _statuses[id] = status;
                }
                else
                {
                    _statuses.TryAdd(id, status);
                }
            });
        }
        public Task RemoveStatus(Guid id)
        {
            return Task.Run(() => { _statuses.TryRemove(id, out var status); });
        }

        public async Task<RequestStatus> GetStatus(Guid id)
        {
            if (_statuses.ContainsKey(id))
            {
                return _statuses[id];
            }
            return RequestStatus.Unknown;
        }
    }
}
