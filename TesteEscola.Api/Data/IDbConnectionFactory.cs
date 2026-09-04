using System.Data;

namespace TesteEscola.Api.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection Create();
    }
}
