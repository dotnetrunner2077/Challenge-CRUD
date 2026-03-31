using TestCrudWeb.DTOs;
using TestCrudWeb.Helpers;

namespace TestCrudWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IActionHelpers _actionHelpers;
        private readonly string apirl = "https://localhost:7280/api/";
        public UserService(IActionHelpers actionHelpers) 
        {
            _actionHelpers= actionHelpers;
        }

        public async Task<List<UsuariosDTO>> GetAll()
        {
            return await _actionHelpers.ExcecuteAsync<List<UsuariosDTO>>(apirl + "Users/List", "GET");
        }

        public async Task<UsuariosDTO> GetByID(int id)
        {
            return await _actionHelpers.ExcecuteAsync<UsuariosDTO>(apirl + "Users/"+ id.ToString(), "GET");
        }

        public async Task<UsuariosDTO> Update(UsuariosDTO usuario)
        {
            return await _actionHelpers.ExcecuteAsync<UsuariosDTO>(apirl + "Users", "PUT", usuario);
        }
        public async Task<UsuariosDTO> Login(string email, string password)
        {
            return await _actionHelpers.ExcecuteAsync<UsuariosDTO>(
                apirl + "Users/Login?email=" + email + "&password=" + password
                , "POST");
        }

        public async Task<UsuariosDTO> Create(UsuariosDTO usuario)
        {
            return await _actionHelpers.ExcecuteAsync<UsuariosDTO>(apirl + "Users", "POST", usuario);
        }

        public async Task<bool> Delete(int id)
        {
            return await _actionHelpers.ExcecuteAsync<bool>(apirl + "Users?id=" + id.ToString(), "DELETE");
        }
    }

    public interface IUserService
    {
        Task<List<UsuariosDTO>> GetAll();
        Task<UsuariosDTO> GetByID(int id);
        Task<UsuariosDTO> Update(UsuariosDTO usuario);
        Task<UsuariosDTO> Login(string email, string password);
        Task<UsuariosDTO> Create(UsuariosDTO usuario);
        Task<bool> Delete(int id);
    }
}
