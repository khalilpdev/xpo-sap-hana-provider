using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using Xunit;
using XpoSapHana;

namespace XpoSapHana.Tests;

/// <summary>
/// Unit tests for the XpoSapHana provider components.
/// These tests do NOT require a live SAP HANA connection.
/// </summary>
public class HanaConnectionProviderTests
{
    // -----------------------------------------------------------------------
    // XpoProviderTypeString
    // -----------------------------------------------------------------------

    /// <summary>
    /// The XPO provider type string must be exactly "SapHana" so that XPO can
    /// resolve the correct provider from a connection string token.
    /// </summary>
    [Fact]
    public void XpoProviderTypeString_ShouldBe_SapHana()
    {
        Assert.Equal("SapHana", HanaConnectionProvider.XpoProviderTypeString);
    }

    // -----------------------------------------------------------------------
    // HanaDataStoreFactory.CreateConnectionString
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies that <see cref="HanaDataStoreFactory.CreateConnectionString"/> produces
    /// the expected "Server=host:port;UserName=user;Password=password;" format.
    /// </summary>
    [Fact]
    public void CreateConnectionString_ShouldReturnExpectedFormat()
    {
        var cs = HanaDataStoreFactory.CreateConnectionString("hana-host", 30015, "SYSTEM", "Passw0rd");
        Assert.Equal("Server=hana-host:30015;UserName=SYSTEM;Password=Passw0rd;", cs);
    }

    /// <summary>
    /// Verifies that <see cref="HanaDataStoreFactory.CreateConnectionString"/> includes
    /// the port number in the server token.
    /// </summary>
    [Fact]
    public void CreateConnectionString_ShouldContainPort()
    {
        var cs = HanaDataStoreFactory.CreateConnectionString("localhost", 39015, "ADMIN", "secret");
        Assert.Contains("39015", cs);
    }

    /// <summary>
    /// Passing a null or empty host should throw <see cref="ArgumentNullException"/>.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateConnectionString_NullOrEmptyHost_ShouldThrow(string? host)
    {
        Assert.Throws<ArgumentNullException>(() =>
            HanaDataStoreFactory.CreateConnectionString(host!, 30015, "USER", "PASS"));
    }

    // -----------------------------------------------------------------------
    // HanaSqlGenerator.GenerateCreateTable
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies that <see cref="HanaSqlGenerator.GenerateCreateTable"/> produces a valid
    /// CREATE TABLE DDL with HANA-specific types and double-quoted identifiers.
    /// </summary>
    [Fact]
    public void GenerateCreateTable_ShouldProduceValidDDL()
    {
        var table = new DBTable("CUSTOMERS");
        table.AddColumn(new DBColumn("OID", true, string.Empty, 0, DBColumnType.Int32));
        table.AddColumn(new DBColumn("NAME", false, string.Empty, 200, DBColumnType.String));
        table.AddColumn(new DBColumn("EMAIL", false, string.Empty, 320, DBColumnType.String));
        table.AddColumn(new DBColumn("CREATED_AT", false, string.Empty, 0, DBColumnType.DateTime));
        table.PrimaryKey = new DBPrimaryKey(new StringCollection { "OID" });

        var ddl = HanaSqlGenerator.GenerateCreateTable(table);

        Assert.Contains("CREATE TABLE \"CUSTOMERS\"", ddl);
        Assert.Contains("\"OID\"", ddl);
        Assert.Contains("INTEGER", ddl);
        Assert.Contains("\"NAME\" NVARCHAR(200)", ddl);
        Assert.Contains("\"EMAIL\" NVARCHAR(320)", ddl);
        Assert.Contains("\"CREATED_AT\" TIMESTAMP", ddl);
        Assert.Contains("PRIMARY KEY", ddl);
    }

    /// <summary>
    /// A column with size > 5000 should map to NCLOB.
    /// </summary>
    [Fact]
    public void GenerateCreateTable_LargeStringColumn_ShouldUseNClob()
    {
        var table = new DBTable("DOCS");
        table.AddColumn(new DBColumn("BODY", false, string.Empty, 10000, DBColumnType.String));

        var ddl = HanaSqlGenerator.GenerateCreateTable(table);

        Assert.Contains("NCLOB", ddl);
    }

    /// <summary>
    /// Passing a null table should throw <see cref="ArgumentNullException"/>.
    /// </summary>
    [Fact]
    public void GenerateCreateTable_NullTable_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => HanaSqlGenerator.GenerateCreateTable(null!));
    }

    // -----------------------------------------------------------------------
    // HanaSqlGenerator.GenerateCreateIndex
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies that a unique index generates the UNIQUE keyword.
    /// </summary>
    [Fact]
    public void GenerateCreateIndex_UniqueIndex_ShouldContainUnique()
    {
        var table = new DBTable("CUSTOMERS");
        var index = new DBIndex("IDX_EMAIL", new StringCollection { "EMAIL" }, true);

        var sql = HanaSqlGenerator.GenerateCreateIndex(table, index);

        Assert.Contains("CREATE UNIQUE INDEX", sql);
        Assert.Contains("\"IDX_EMAIL\"", sql);
        Assert.Contains("\"CUSTOMERS\"", sql);
        Assert.Contains("\"EMAIL\"", sql);
    }

    /// <summary>
    /// Verifies that a non-unique index does NOT contain the UNIQUE keyword.
    /// </summary>
    [Fact]
    public void GenerateCreateIndex_NonUniqueIndex_ShouldNotContainUnique()
    {
        var table = new DBTable("CUSTOMERS");
        var index = new DBIndex("IDX_NAME", new StringCollection { "NAME" }, false);

        var sql = HanaSqlGenerator.GenerateCreateIndex(table, index);

        Assert.DoesNotContain("UNIQUE", sql);
        Assert.Contains("CREATE INDEX", sql);
    }

    // -----------------------------------------------------------------------
    // HanaSqlGenerator.GenerateAddForeignKey
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies that foreign key SQL includes REFERENCES and double-quoted identifiers.
    /// </summary>
    [Fact]
    public void GenerateAddForeignKey_ShouldProduceValidSQL()
    {
        var table = new DBTable("ORDERS");
        var fk = new DBForeignKey(
            new StringCollection { "CUSTOMER_OID" },
            "CUSTOMERS",
            new StringCollection { "OID" })
        {
            Name = "FK_ORDERS_CUSTOMERS"
        };

        var sql = HanaSqlGenerator.GenerateAddForeignKey(table, fk);

        Assert.Contains("ALTER TABLE \"ORDERS\"", sql);
        Assert.Contains("ADD CONSTRAINT \"FK_ORDERS_CUSTOMERS\"", sql);
        Assert.Contains("FOREIGN KEY (\"CUSTOMER_OID\")", sql);
        Assert.Contains("REFERENCES \"CUSTOMERS\" (\"OID\")", sql);
    }

    // -----------------------------------------------------------------------
    // HanaSchemaProvider.MapHanaTypeToDB
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies the type mapping from HANA SQL type strings to XPO DBColumnType.
    /// </summary>
    [Theory]
    [InlineData("INTEGER", DBColumnType.Int32)]
    [InlineData("BIGINT", DBColumnType.Int64)]
    [InlineData("SMALLINT", DBColumnType.Short)]
    [InlineData("TINYINT", DBColumnType.Byte)]
    [InlineData("DOUBLE", DBColumnType.Double)]
    [InlineData("FLOAT", DBColumnType.Single)]
    [InlineData("DECIMAL", DBColumnType.Decimal)]
    [InlineData("NVARCHAR", DBColumnType.String)]
    [InlineData("NCLOB", DBColumnType.String)]
    [InlineData("BLOB", DBColumnType.ByteArray)]
    [InlineData("TIMESTAMP", DBColumnType.DateTime)]
    [InlineData("BOOLEAN", DBColumnType.Boolean)]
    public void MapHanaTypeToDB_ShouldReturnExpectedType(string hanaType, DBColumnType expected)
    {
        var result = HanaSchemaProvider.MapHanaTypeToDB(hanaType);
        Assert.Equal(expected, result);
    }
}
