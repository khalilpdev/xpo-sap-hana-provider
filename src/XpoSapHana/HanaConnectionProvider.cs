using System.Data;
using DevExpress.Xpo.DB;
using Sap.Data.Hana;
using DxHanaConnectionProvider = DevExpress.Xpo.DB.HanaConnectionProvider;

namespace XpoSapHana;

/// <summary>
/// Thin wrapper over DevExpress's built-in SAP HANA XPO provider.
/// Registers the custom provider token <c>SapHana</c> for backward compatibility.
/// </summary>
public class HanaConnectionProvider : DxHanaConnectionProvider
{
    /// <summary>
    /// The provider type string used to register and identify this provider with XPO.
    /// </summary>
    public const string XpoProviderTypeString = "SapHana";

    static HanaConnectionProvider()
    {
        DataStoreBase.RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="HanaConnectionProvider"/>.
    /// </summary>
    /// <param name="connection">An open or closed SAP HANA ADO.NET connection.</param>
    /// <param name="autoCreateOption">Determines how XPO handles schema creation.</param>
    public HanaConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption)
        : base(connection, autoCreateOption)
    {
    }

    /// <summary>
    /// Factory method used by XPO to create a provider from a connection string.
    /// </summary>
    /// <param name="connectionString">SAP HANA ADO.NET connection string.</param>
    /// <param name="autoCreateOption">Determines how XPO handles schema creation.</param>
    /// <param name="objectsToDisposeOnDisconnect">Objects that should be disposed when the connection is closed.</param>
    /// <returns>A configured <see cref="HanaConnectionProvider"/> instance.</returns>
    public static IDataStore CreateProviderFromString(
        string connectionString,
        AutoCreateOption autoCreateOption,
        out IDisposable[] objectsToDisposeOnDisconnect)
    {
        var connection = new HanaConnection(connectionString);
        objectsToDisposeOnDisconnect = new IDisposable[] { connection };
        return new HanaConnectionProvider(connection, autoCreateOption);
    }
}
