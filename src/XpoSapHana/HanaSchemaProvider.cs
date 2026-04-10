using System.Data;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;

namespace XpoSapHana;

/// <summary>
/// Reads SAP HANA schema information from system views and maps HANA SQL types
/// back to XPO <see cref="DBColumnType"/> values.
/// </summary>
public static class HanaSchemaProvider
{
    /// <summary>
    /// Queries <c>SYS.TABLES</c> and returns the names of all user tables in the current schema.
    /// </summary>
    /// <param name="connection">An open SAP HANA ADO.NET connection.</param>
    /// <returns>A list of table names.</returns>
    public static IReadOnlyList<string> GetExistingTables(IDbConnection connection)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));

        const string sql =
            "SELECT TABLE_NAME FROM SYS.TABLES WHERE SCHEMA_NAME = CURRENT_SCHEMA ORDER BY TABLE_NAME";

        using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        var tables = new List<string>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            tables.Add(reader.GetString(0));

        return tables;
    }

    /// <summary>
    /// Queries <c>SYS.TABLE_COLUMNS</c> and returns column metadata for the specified table.
    /// </summary>
    /// <param name="connection">An open SAP HANA ADO.NET connection.</param>
    /// <param name="tableName">The name of the table to inspect.</param>
    /// <returns>A list of <see cref="DBColumn"/> objects representing the table's columns.</returns>
    public static IReadOnlyList<DBColumn> GetTableColumns(IDbConnection connection, string tableName)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));
        if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException(nameof(tableName));

        const string sql =
            "SELECT COLUMN_NAME, DATA_TYPE_NAME, LENGTH, IS_NULLABLE " +
            "FROM SYS.TABLE_COLUMNS " +
            "WHERE SCHEMA_NAME = CURRENT_SCHEMA AND TABLE_NAME = :p0 " +
            "ORDER BY POSITION";

        using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        var p = cmd.CreateParameter();
        p.ParameterName = ":p0";
        p.Value = tableName;
        cmd.Parameters.Add(p);

        var columns = new List<DBColumn>();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var name = reader.GetString(0);
            var hanaType = reader.GetString(1);
            var size = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
            var dbType = MapHanaTypeToDB(hanaType);
            var col = new DBColumn(name, false, string.Empty, size, dbType);
            columns.Add(col);
        }

        return columns;
    }

    /// <summary>
    /// Queries <c>SYS.INDEXES</c> and returns index metadata for the specified table.
    /// </summary>
    /// <param name="connection">An open SAP HANA ADO.NET connection.</param>
    /// <param name="tableName">The name of the table to inspect.</param>
    /// <returns>A list of <see cref="DBIndex"/> objects for the table.</returns>
    public static IReadOnlyList<DBIndex> GetTableIndexes(IDbConnection connection, string tableName)
    {
        if (connection == null) throw new ArgumentNullException(nameof(connection));
        if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException(nameof(tableName));

        const string sql =
            "SELECT I.INDEX_NAME, IC.COLUMN_NAME, I.CONSTRAINT " +
            "FROM SYS.INDEXES I " +
            "JOIN SYS.INDEX_COLUMNS IC " +
            "  ON I.SCHEMA_NAME = IC.SCHEMA_NAME AND I.TABLE_NAME = IC.TABLE_NAME AND I.INDEX_NAME = IC.INDEX_NAME " +
            "WHERE I.SCHEMA_NAME = CURRENT_SCHEMA AND I.TABLE_NAME = :p0 " +
            "ORDER BY I.INDEX_NAME, IC.POSITION";

        using var cmd = connection.CreateCommand();
        cmd.CommandText = sql;

        var p = cmd.CreateParameter();
        p.ParameterName = ":p0";
        p.Value = tableName;
        cmd.Parameters.Add(p);

        var indexDict = new Dictionary<string, (bool isUnique, List<string> cols)>(StringComparer.OrdinalIgnoreCase);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var idxName = reader.GetString(0);
            var colName = reader.GetString(1);
            var constraint = reader.IsDBNull(2) ? string.Empty : reader.GetString(2);
            var isUnique = constraint.Equals("UNIQUE", StringComparison.OrdinalIgnoreCase)
                        || constraint.Equals("PRIMARY KEY", StringComparison.OrdinalIgnoreCase);

            if (!indexDict.TryGetValue(idxName, out var entry))
            {
                entry = (isUnique, new List<string>());
                indexDict[idxName] = entry;
            }

            entry.cols.Add(colName);
        }

        return indexDict
            .Select(kv =>
            {
                var columns = new System.Collections.Specialized.StringCollection();
                foreach (var column in kv.Value.cols)
                    columns.Add(column);

                return new DBIndex(kv.Key, columns, kv.Value.isUnique);
            })
            .ToList();
    }

    /// <summary>
    /// Maps a SAP HANA SQL data type name to an XPO <see cref="DBColumnType"/>.
    /// </summary>
    /// <param name="hanaType">The HANA type name (e.g., "NVARCHAR", "INTEGER").</param>
    /// <returns>The corresponding <see cref="DBColumnType"/>.</returns>
    public static DBColumnType MapHanaTypeToDB(string hanaType)
    {
        return hanaType.ToUpperInvariant() switch
        {
            "BOOLEAN" => DBColumnType.Boolean,
            "TINYINT" => DBColumnType.Byte,
            "SMALLINT" => DBColumnType.Int16,
            "INTEGER" or "INT" => DBColumnType.Int32,
            "BIGINT" => DBColumnType.Int64,
            "FLOAT" or "REAL" => DBColumnType.Single,
            "DOUBLE" or "DOUBLE PRECISION" => DBColumnType.Double,
            "DECIMAL" or "NUMERIC" => DBColumnType.Decimal,
            "NVARCHAR" or "VARCHAR" or "ALPHANUM" or "SHORTTEXT" => DBColumnType.String,
            "NCLOB" or "CLOB" or "TEXT" => DBColumnType.String,
            "BLOB" or "VARBINARY" or "BINARY" => DBColumnType.ByteArray,
            "DATE" or "TIME" or "SECONDDATE" or "TIMESTAMP" or "LONGDATE" => DBColumnType.DateTime,
            _ => DBColumnType.Unknown
        };
    }
}
