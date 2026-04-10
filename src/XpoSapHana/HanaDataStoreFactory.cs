using DevExpress.Xpo.DB;

namespace XpoSapHana;

/// <summary>
/// Convenience factory for creating SAP HANA XPO data store instances and connection strings.
/// </summary>
public static class HanaDataStoreFactory
{
    /// <summary>
    /// Creates an <see cref="IDataStore"/> backed by a SAP HANA database.
    /// </summary>
    /// <param name="connectionString">SAP HANA ADO.NET connection string.</param>
    /// <param name="autoCreateOption">Determines how XPO handles schema creation.</param>
    /// <returns>A configured <see cref="IDataStore"/> instance.</returns>
    public static IDataStore Create(string connectionString, AutoCreateOption autoCreateOption)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        return HanaConnectionProvider.CreateProviderFromString(
            connectionString,
            autoCreateOption,
            out _);
    }

    /// <summary>
    /// Builds a SAP HANA ADO.NET connection string from individual components.
    /// </summary>
    /// <param name="host">Server host name or IP address.</param>
    /// <param name="port">SAP HANA SQL port (default: 30015).</param>
    /// <param name="user">Database user name.</param>
    /// <param name="password">Database user password.</param>
    /// <returns>A connection string in the format <c>Server=host:port;UserName=user;Password=password;</c></returns>
    public static string CreateConnectionString(string host, int port, string user, string password)
    {
        if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
        if (string.IsNullOrWhiteSpace(user)) throw new ArgumentNullException(nameof(user));
        if (password == null) throw new ArgumentNullException(nameof(password));

        return $"Server={host}:{port};UserName={user};Password={password};";
    }
}
