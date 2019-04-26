using SecureWebApp.Models;
using System.Threading.Tasks;

namespace SecureWebApp.Services
{
    public interface IDemoService
    {
        Task<StorageViewModel> AccessStorage();

        Task<SqlDbViewModel> AccessAdoSqlDatabase();

        Task<SqlDbViewModel> AccessEFDatabase();
    }
}
