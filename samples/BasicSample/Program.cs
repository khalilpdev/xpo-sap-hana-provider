// =============================================================================
// BasicSample – demonstração de uso do XpoSapHana Provider
// BasicSample – usage demonstration of the XpoSapHana Provider
// =============================================================================
//
// Este exemplo mostra como:
//   1. Registrar o provider HANA no XPO
//   2. Criar um DataLayer apontando para um banco SAP HANA
//   3. Definir uma entidade XPO simples (Customer)
//   4. Realizar operações CRUD com UnitOfWork
//
// This example shows how to:
//   1. Register the HANA provider with XPO
//   2. Create a DataLayer pointing to a SAP HANA database
//   3. Define a simple XPO entity (Customer)
//   4. Perform basic CRUD operations using UnitOfWork
// =============================================================================

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using XpoSapHana;

// ---------------------------------------------------------------------------
// PASSO 1 / STEP 1
// O provider é registrado automaticamente ao carregar o assembly XpoSapHana,
// graças ao construtor estático de HanaConnectionProvider.
// The provider is automatically registered when the XpoSapHana assembly loads,
// thanks to the static constructor in HanaConnectionProvider.
// ---------------------------------------------------------------------------
_ = typeof(HanaConnectionProvider); // garante que o assembly seja carregado / ensures the assembly is loaded

// ---------------------------------------------------------------------------
// PASSO 2 / STEP 2
// Construa a connection string usando a factory de conveniência.
// Build the connection string using the convenience factory.
// ---------------------------------------------------------------------------
// ⚠️  Substitua pelos dados reais do seu servidor SAP HANA.
// ⚠️  Replace with your actual SAP HANA server details.
var connectionString = HanaDataStoreFactory.CreateConnectionString(
    host: "hana-server",
    port: 30015,
    user: "SYSTEM",
    password: "YourPassword123"
);

// ---------------------------------------------------------------------------
// PASSO 3 / STEP 3
// Crie o IDataStore e configure o DataLayer global do XPO.
// Create the IDataStore and configure XPO's global DataLayer.
// ---------------------------------------------------------------------------
var dataStore = HanaDataStoreFactory.Create(connectionString, AutoCreateOption.DatabaseAndSchema);
XpoDefault.DataLayer = XpoDefault.GetDataLayer(dataStore, AutoCreateOption.DatabaseAndSchema);

Console.WriteLine("Conexão com SAP HANA estabelecida. / SAP HANA connection established.");

// ---------------------------------------------------------------------------
// PASSO 4 / STEP 4 – CREATE
// Criar um novo cliente / Create a new customer
// ---------------------------------------------------------------------------
using (var uow = new UnitOfWork())
{
    var customer = new Customer(uow)
    {
        Name = "João Silva",
        Email = "joao.silva@exemplo.com.br"
    };

    // Persiste as alterações no banco / Persist the changes to the database
    await uow.CommitChangesAsync();
    Console.WriteLine($"Cliente criado com Oid={customer.Oid} / Customer created with Oid={customer.Oid}");
}

// ---------------------------------------------------------------------------
// PASSO 5 / STEP 5 – READ
// Buscar todos os clientes / Fetch all customers
// ---------------------------------------------------------------------------
using (var uow = new UnitOfWork())
{
    var customers = uow.Query<Customer>().ToList();
    Console.WriteLine($"\nTotal de clientes / Total customers: {customers.Count}");
    foreach (var c in customers)
        Console.WriteLine($"  [{c.Oid}] {c.Name} – {c.Email}");
}

// ---------------------------------------------------------------------------
// PASSO 6 / STEP 6 – UPDATE
// Atualizar o primeiro cliente encontrado / Update the first customer found
// ---------------------------------------------------------------------------
using (var uow = new UnitOfWork())
{
    var customer = uow.Query<Customer>().FirstOrDefault();
    if (customer != null)
    {
        customer.Email = "joao.novo@exemplo.com.br";
        await uow.CommitChangesAsync();
        Console.WriteLine($"\nE-mail atualizado / Email updated: {customer.Email}");
    }
}

// ---------------------------------------------------------------------------
// PASSO 7 / STEP 7 – DELETE
// Excluir o primeiro cliente encontrado / Delete the first customer found
// ---------------------------------------------------------------------------
using (var uow = new UnitOfWork())
{
    var customer = uow.Query<Customer>().FirstOrDefault();
    if (customer != null)
    {
        uow.Delete(customer);
        await uow.CommitChangesAsync();
        Console.WriteLine("\nCliente excluído / Customer deleted.");
    }
}

Console.WriteLine("\nDemo finalizada. / Demo finished.");

// =============================================================================
// Definição da entidade XPO / XPO entity definition
// =============================================================================

/// <summary>
/// Entidade XPO que representa um cliente no banco de dados SAP HANA.
/// XPO entity representing a customer in the SAP HANA database.
/// </summary>
[Persistent("CUSTOMERS")]
public class Customer : XPObject
{
    private string _name = string.Empty;
    private string _email = string.Empty;

    /// <summary>Inicializa uma nova instância de <see cref="Customer"/>.</summary>
    public Customer(Session session) : base(session) { }

    /// <summary>Nome completo do cliente / Customer full name.</summary>
    [Size(200)]
    public string Name
    {
        get => _name;
        set => SetPropertyValue(nameof(Name), ref _name, value);
    }

    /// <summary>Endereço de e-mail / E-mail address.</summary>
    [Size(320)]
    public string Email
    {
        get => _email;
        set => SetPropertyValue(nameof(Email), ref _email, value);
    }
}
