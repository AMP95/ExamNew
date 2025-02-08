﻿using System.Collections.Concurrent;
using Utilities.Interfaces;

namespace Exam.ResultServices
{
    public class StatusServiceResult : IServiceResult<Status>
    {
        public bool IsSuccess { get; set ; }
        public string ErrorMessage { get ; set ; }
        public Status Result { get ; set ; }
        public StatusServiceResult()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
            Result = Status.InProgress;
        }
    }

    public class StatusService : IStatusService
    {
        private ConcurrentDictionary<Guid, Status> _statuses;

        public StatusService()
        {
            _statuses = new ConcurrentDictionary<Guid, Status>();
        }

        public Task UpdateStatus(Guid id, Status status)
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

        public async Task<IServiceResult<Status>> GetStatus(Guid id)
        {
            if (_statuses.ContainsKey(id))
            {
                return new StatusServiceResult()
                {
                    IsSuccess = true,
                    Result = _statuses[id]
                };
            }
            else 
            {
                return new StatusServiceResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Статус не найден"
                };
            }
        }
    }
}
