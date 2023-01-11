namespace Pirina.Providers.Databases.AzureCosmosDatabase
{
    public interface IDocumentDbConfiguration
    {
        string DatabaseId { get; }
        string EndPointUri { get; }
        string AuthKey { get; }
        string PrimaryKey { get; }
    }
}
