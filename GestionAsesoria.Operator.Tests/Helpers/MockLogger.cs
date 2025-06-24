using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace GestionAsesoria.Operator.Tests.Helpers
{
    public class MockLogger<T> : ILogger<T>
    {
        private readonly List<string> _logMessages = new();
        private readonly List<LogLevel> _logLevels = new();
        private readonly List<Exception> _logExceptions = new();

        public IReadOnlyList<string> LogMessages => _logMessages;
        public IReadOnlyList<LogLevel> LogLevels => _logLevels;
        public IReadOnlyList<Exception> LogExceptions => _logExceptions;

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _logMessages.Add(formatter(state, exception));
            _logLevels.Add(logLevel);
            if (exception != null)
            {
                _logExceptions.Add(exception);
            }
        }

        public void Clear()
        {
            _logMessages.Clear();
            _logLevels.Clear();
            _logExceptions.Clear();
        }
    }
} 