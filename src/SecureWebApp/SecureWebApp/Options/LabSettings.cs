namespace SecureWebApp.Options
{
    public class LabSettings
    {
        public string ManagedIdentityTenantId { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageContainerName { get; set; }
        public string StorageBlobName { get; set; }
        public string SqlConnectionString { get; set; }
        public string KeyVaultSecret { get; set; }
    }
}
