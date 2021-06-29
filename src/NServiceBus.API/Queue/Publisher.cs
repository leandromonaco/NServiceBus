using Microsoft.Data.SqlClient;
using NServiceBus.Transport.SqlServer;
using System.Threading.Tasks;

namespace NServiceBus.API.Queue
{
    public class Publisher : IPublisher
    {
        EndpointConfiguration _endpointConfiguration;
        string _connection;
        TransportExtensions<SqlServerTransport> _transport;
        SubscriptionSettings _subscriptions;

        public Publisher()
        {
            
        }

        public async Task PublishAsync(OrderSubmitted orderSubmitted)
        {
            ConfigureSender();

            var endpointInstance = await Endpoint.Start(_endpointConfiguration)
                                     .ConfigureAwait(false);

            await endpointInstance.Publish(orderSubmitted)
                                  .ConfigureAwait(false);

            //_logger.LogInformation("Published OrderSubmitted message");
        }

        private void ConfigureSender()
        {
            _endpointConfiguration = new EndpointConfiguration("Samples.Sql.Sender");
            _endpointConfiguration.SendFailedMessagesTo("error");
            _endpointConfiguration.EnableInstallers();

            //Sender Configuration Begins

            _connection = @"Data Source=host.docker.internal,1433;Initial Catalog=NServiceBusDB;Persist Security Info=True;User ID=sa;Password=NS3rv1c3Bus";

            _transport = _endpointConfiguration.UseTransport<SqlServerTransport>();
            _transport.ConnectionString(_connection);
            _transport.DefaultSchema("sender");
            _transport.UseSchemaForQueue("error", "dbo");
            _transport.UseSchemaForQueue("audit", "dbo");

            var persistence = _endpointConfiguration.UsePersistence<SqlPersistence>();
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("sender");
            persistence.ConnectionBuilder(() => new SqlConnection(_connection));
            persistence.TablePrefix("");

            _subscriptions = _transport.SubscriptionSettings();
            _subscriptions.SubscriptionTableName(
                tableName: "Subscriptions",
                schemaName: "dbo");

            //Sender Configuration Finishes
        }
    }
}
