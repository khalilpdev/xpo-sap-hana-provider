using System.Data;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using Sap.Data.Hana;

namespace XpoSapHana;

/// <summary>
/// DevExpress XPO custom data store provider for SAP HANA.
/// Inherits from <see cref="ConnectionProviderSql"/> and adapts SQL generation
/// to SAP HANA syntax (double-quoted identifiers, :pN parameters, LIMIT pagination).
/// </summary>
public class HanaConnectionProvider : ConnectionProviderSql
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

    /// <inheritdoc/>
    public override string GetParameterPrefix() => ":";

    /// <inheritdoc/>
    protected override string GetParameterName(OperandValue parameter, int index, ref bool createParameter)
    {
        createParameter = true;
        return $":p{index}";
    }

    /// <inheritdoc/>
    protected override string FormatTable(string schemaName, string tableName)
    {
        if (!string.IsNullOrEmpty(schemaName))
            return $"\"{schemaName}\".\"{tableName}\"";
        return $"\"{tableName}\"";
    }

    /// <inheritdoc/>
    protected override string FormatTable(string schemaName, string tableName, string tableAlias)
    {
        if (!string.IsNullOrEmpty(schemaName))
            return $"\"{schemaName}\".\"{tableName}\" \"{tableAlias}\"";
        return $"\"{tableName}\" \"{tableAlias}\"";
    }

    /// <inheritdoc/>
    protected override string FormatColumn(string columnName) => $"\"{columnName}\"";

    /// <inheritdoc/>
    protected override string FormatColumn(string columnName, string tableAlias) =>
        $"\"{tableAlias}\".\"{columnName}\"";

    /// <inheritdoc/>
    protected override string FormatConstraint(string constraintName) => $"\"{constraintName}\"";

    /// <inheritdoc/>
    protected override string GetSqlCreateColumnType(DBTable table, DBColumn column)
    {
        return column.ColumnType switch
        {
            DBColumnType.Boolean => "BOOLEAN",
            DBColumnType.Byte => "TINYINT",
            DBColumnType.SByte => "SMALLINT",
            DBColumnType.Short => "SMALLINT",
            DBColumnType.UShort => "INTEGER",
            DBColumnType.Int32 => "INTEGER",
            DBColumnType.UInt32 => "BIGINT",
            DBColumnType.Int64 => "BIGINT",
            DBColumnType.UInt64 => "DECIMAL(20,0)",
            DBColumnType.Single => "FLOAT",
            DBColumnType.Double => "DOUBLE",
            DBColumnType.Decimal => "DECIMAL(28,4)",
            DBColumnType.DateTime => "TIMESTAMP",
            DBColumnType.Guid => "NVARCHAR(36)",
            DBColumnType.String => column.Size <= 0 || column.Size > 5000
                ? "NCLOB"
                : $"NVARCHAR({column.Size})",
            DBColumnType.ByteArray => "BLOB",
            DBColumnType.Char => "NVARCHAR(1)",
            _ => "NVARCHAR(256)"
        };
    }

    /// <inheritdoc/>
    protected override string FormatSelect(string selectedPropertiesSql, string fromSql, string whereSql,
        string orderBySql, string groupBySql, string havingSql, int? skipSelectedRecords, int? topSelectedRecords)
    {
        var query = $"SELECT {selectedPropertiesSql} FROM {fromSql}";

        if (!string.IsNullOrEmpty(whereSql))
            query += $" WHERE {whereSql}";
        if (!string.IsNullOrEmpty(groupBySql))
            query += $" GROUP BY {groupBySql}";
        if (!string.IsNullOrEmpty(havingSql))
            query += $" HAVING {havingSql}";
        if (!string.IsNullOrEmpty(orderBySql))
            query += $" ORDER BY {orderBySql}";
        if (topSelectedRecords.HasValue)
            query += $" LIMIT {topSelectedRecords.Value}";
        if (skipSelectedRecords.HasValue)
            query += $" OFFSET {skipSelectedRecords.Value}";

        return query;
    }
}
