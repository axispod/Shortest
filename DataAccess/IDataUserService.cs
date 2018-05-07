using DataAccess.Models;

namespace DataAccess
{
    public interface IDataUserService
    {
        User Register(string username, string password);
        User Get(string username, string password);
    }
}