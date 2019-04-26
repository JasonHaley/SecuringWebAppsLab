using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;
using SecureWebApp.Db;
using SecureWebApp.Models;
using SecureWebApp.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace SecureWebApp.Services
{
    public class DemoService : IDemoService
    {
        private readonly LabSettings _settings;
        private readonly LabDbContext _dbContext;
        
        public DemoService(
            IOptionsSnapshot<LabSettings> settings,
            LabDbContext dbContext)
        {
            _settings = settings.Value;
            _dbContext = dbContext;
        }

        public async Task<StorageViewModel> AccessStorage()
        {
            string accessToken = await GetAccessToken("https://storage.azure.com/");

            var tokenCredential = new TokenCredential(accessToken);
            var storageCredentials = new StorageCredentials(tokenCredential);
            var uri = new Uri($"https://{_settings.StorageAccountName}.blob.core.windows.net/{_settings.StorageContainerName}/{_settings.StorageBlobName}");
            var blob = new CloudBlockBlob(uri, storageCredentials);

            // We download the whole file here because we are going to show it on the Razor view
            // Usually when reading files from Storage you should use the Stream APIs, e.g. blob.OpenReadAsync()
            string content = await blob.DownloadTextAsync();

            return new StorageViewModel
            {
                FileContent = content
            };
        }

        public async Task<SqlDbViewModel> AccessAdoSqlDatabase()
        {
            string accessToken = await GetAccessToken("https://database.windows.net/");

            List<SqlRowModel> results = await GetSqlRowsWithEfCore(accessToken);

            return new SqlDbViewModel
            {
                Results = results
            };
        }

        public async Task<SqlDbViewModel> AccessEFDatabase()
        {
            string accessToken = await GetAccessToken("https://database.windows.net/");

            List<SqlRowModel> results = await GetSqlRowsWithAdoNet(accessToken);
            
            return new SqlDbViewModel
            {
                Results = results
            };
        }

        private async Task<List<SqlRowModel>> GetSqlRowsWithEfCore(string accessToken)
        {
            //Have to cast DbConnection to SqlConnection
            //AccessToken property does not exist on the base class
            var conn = (SqlConnection)_dbContext.Database.GetDbConnection();
            conn.AccessToken = accessToken;

            return await _dbContext
                .AzureGroups
                .Select(t => new SqlRowModel
                {
                    Id = t.Id,
                    GroupName = t.GroupName,
                    MeetupUrl = t.MeetupUrl
                })
                .ToListAsync();
        }

        private async Task<List<SqlRowModel>> GetSqlRowsWithAdoNet(string accessToken)
        {
            var results = new List<SqlRowModel>();

            using (var conn = new SqlConnection(_settings.SqlConnectionString))
            {
                conn.AccessToken = accessToken;

                await conn.OpenAsync();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT [Id], [GroupName], [MeetupUrl] FROM [dbo].[AzureGroups]";
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int id = (int)reader["Id"];
                        string name = (string)reader["GroupName"];
                        string url = (string)reader["MeetupUrl"];

                        results.Add(new SqlRowModel
                        {
                            Id = id,
                            GroupName = name,
                            MeetupUrl = url
                        });
                    }
                }

                reader.Close();
            }

            return results;
        }

        private async Task<string> GetAccessToken(string resource)
        {
            var authProvider = new AzureServiceTokenProvider();
            string tenantId = _settings.ManagedIdentityTenantId;

            if (tenantId != null && tenantId.Length == 0)
            {
                tenantId = null; 
            }

            return await authProvider.GetAccessTokenAsync(resource, tenantId);
        }
    }
}
