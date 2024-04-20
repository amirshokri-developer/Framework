using ASh.Framework.EventBus.RabbitMQ.Abstractions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace ASh.Framework.EventBus.RabbitMQ
{
    internal sealed class ConnectionProvider : IDisposable, IConnectionProvider
    {
        private readonly ILogger<ConnectionProvider> _logger;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;

        public ConnectionProvider(ILogger<ConnectionProvider> logger,
            ConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    _connection?.Close();
                    _connection?.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMq connection");
            }
        }

        public IConnection GetConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            return _connection;
        }
    }
}
