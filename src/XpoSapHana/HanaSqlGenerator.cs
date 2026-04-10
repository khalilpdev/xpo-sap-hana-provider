using System.Linq;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;

namespace XpoSapHana;

/// <summary>
/// Generates SAP HANA-specific DDL and DML SQL statements for use with XPO schema management.
/// Uses double-quoted identifiers throughout for SAP HANA compatibility.
/// </summary>
public static class HanaSqlGenerator
{
    /// <summary>
    /// Generates a <c>CREATE TABLE</c> DDL statement for the given <see cref="DBTable"/>.
    /// </summary>
    /// <param name="table">The XPO table descriptor.</param>
    /// <returns>A SQL string for creating the table in SAP HANA.</returns>
    public static string GenerateCreateTable(DBTable table)
    {
        if (table == null) throw new ArgumentNullException(nameof(table));

        var columns = table.Columns
            .Select(c => $"    \"{c.Name}\" {GetHanaType(c)}{GetNullability(table, c)}")
            .ToList();

        var pk = table.PrimaryKey;
        if (pk != null && pk.Columns.Count > 0)
        {
            var pkCols = string.Join(", ", pk.Columns.Cast<string>().Select(col => $"\"{col}\""));
            columns.Add($"    CONSTRAINT \"{pk.Name}\" PRIMARY KEY ({pkCols})");
        }

        var columnDefs = string.Join(",\n", columns);
        return $"CREATE TABLE \"{table.Name}\" (\n{columnDefs}\n)";
    }

    /// <summary>
    /// Generates a <c>CREATE INDEX</c> statement for the given index on a table.
    /// </summary>
    /// <param name="table">The table that owns the index.</param>
    /// <param name="index">The index descriptor.</param>
    /// <returns>A SQL string for creating the index in SAP HANA.</returns>
    public static string GenerateCreateIndex(DBTable table, DBIndex index)
    {
        if (table == null) throw new ArgumentNullException(nameof(table));
        if (index == null) throw new ArgumentNullException(nameof(index));

        var unique = index.IsUnique ? "UNIQUE " : string.Empty;
        var cols = string.Join(", ", index.Columns.Cast<string>().Select(c => $"\"{c}\""));
        return $"CREATE {unique}INDEX \"{index.Name}\" ON \"{table.Name}\" ({cols})";
    }

    /// <summary>
    /// Generates an <c>ALTER TABLE ... ADD CONSTRAINT ... FOREIGN KEY</c> statement.
    /// </summary>
    /// <param name="table">The table that owns the foreign key.</param>
    /// <param name="fk">The foreign key descriptor.</param>
    /// <returns>A SQL string for adding the foreign key constraint in SAP HANA.</returns>
    public static string GenerateAddForeignKey(DBTable table, DBForeignKey fk)
    {
        if (table == null) throw new ArgumentNullException(nameof(table));
        if (fk == null) throw new ArgumentNullException(nameof(fk));

        var localCols = string.Join(", ", fk.Columns.Cast<string>().Select(c => $"\"{c}\""));
        var refCols = string.Join(", ", fk.PrimaryKeyTableKeyColumns.Cast<string>().Select(c => $"\"{c}\""));
        return $"ALTER TABLE \"{table.Name}\" ADD CONSTRAINT \"{fk.Name}\" " +
               $"FOREIGN KEY ({localCols}) REFERENCES \"{fk.PrimaryKeyTable}\" ({refCols})";
    }

    private static string GetHanaType(DBColumn column)
    {
        return column.ColumnType switch
        {
            DBColumnType.Boolean => "BOOLEAN",
            DBColumnType.Byte => "TINYINT",
            DBColumnType.SByte => "SMALLINT",
            DBColumnType.Int16 => "SMALLINT",
            DBColumnType.UInt16 => "INTEGER",
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

    private static string GetNullability(DBTable table, DBColumn column)
    {
        var pk = table.PrimaryKey;
        if (pk != null && pk.Columns.Contains(column.Name))
            return " NOT NULL";
        return string.Empty;
    }
}
