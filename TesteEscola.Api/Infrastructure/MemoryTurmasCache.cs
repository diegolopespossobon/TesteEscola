using System;
using System.Collections.Generic;
using TesteEscola.Api.Dtos;

namespace TesteEscola.Api.Infrastructure
{
    public interface ITurmasCache
    {
        IEnumerable<TurmaResponse> Get();
        void Set(IEnumerable<TurmaResponse> turmas);
        void Invalidate();
    }

    public class MemoryTurmasCache : ITurmasCache
    {
        private readonly object _lock = new object();
        private IEnumerable<TurmaResponse> _turmas;
        private DateTime _expiresAtUtc;

        public IEnumerable<TurmaResponse> Get()
        {
            lock (_lock)
            {
                if (_turmas == null || DateTime.UtcNow >= _expiresAtUtc)
                {
                    return null;
                }

                return _turmas;
            }
        }

        public void Set(IEnumerable<TurmaResponse> turmas)
        {
            lock (_lock)
            {
                _turmas = turmas;
                _expiresAtUtc = DateTime.UtcNow.AddMinutes(5);
            }
        }

        public void Invalidate()
        {
            lock (_lock)
            {
                _turmas = null;
                _expiresAtUtc = DateTime.MinValue;
            }
        }
    }
}
