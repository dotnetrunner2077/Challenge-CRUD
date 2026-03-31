using TestCrudApi.Models;
using TestCrudApi.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace TestCrudApi.Services
{
    public class UserService : IUserService
    {
        private readonly TestCrudContext _context;

        public UserService(TestCrudContext context)
        {
            _context = context;
        }

        public async Task<UsuariosDTO> Create(UsuariosDTO usuario)
        {
            var result = new UsuariosDTO();

            try
            {
                var isUsuaruiExist = await _context.Usuarios
                    .Where(u => u.Email == usuario.Email)
                    .CountAsync();
                if (isUsuaruiExist == 0)
                {
                    var saveUsuario = new Usuario
                    {
                        Apellido = usuario.Apellido,
                        Email = usuario.Email,
                        FechaNacimiento = usuario.FechaNacimiento,
                        Nombre = usuario.Nombre,
                        Password = this.Hash(usuario.Password),
                        IdTipoUsuario = usuario.IdTipoUsuario
                    };

                    var usuarioSaved = _context.Add(saveUsuario);
                    await _context.SaveChangesAsync();

                    result.IdUsuario = usuarioSaved.Entity.IdUsuario;
                }
            }
            catch (Exception ex)
            {
                result.IdUsuario = 0;
                result.Email = ex.Message;
                return result;
            }
            return result;
        }

        public async Task<UsuariosDTO> Login(string email, string password)
        {
            var result = new UsuariosDTO();

            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.Email == email)
                    .Include(t => t.IdTipoUsuarioNavigation)
                    .FirstOrDefaultAsync();
                if (usuario != null)
                {
                    if (VerfyHashPassword(password, usuario.Password))
                    {
                        result.Apellido = usuario.Apellido;
                        result.Email = usuario.Email;
                        result.FechaNacimiento = usuario.FechaNacimiento;
                        result.IdTipoUsuario = usuario.IdTipoUsuario;
                        result.IdUsuario = usuario.IdUsuario;
                        result.Nombre = usuario.Nombre;
                        result.TipoUsuario = usuario.IdTipoUsuarioNavigation.Descripcion;
                    }    
                    else
                    {
                        result.IdUsuario = -2;
                        result.Email = "Usuario o contraseña incorrectos";
                    }
                }
                else
                {
                    result.IdUsuario = -1;
                    result.Email = "Mail no registrado";
                }
            }
            catch (Exception ex)
            {
                result.IdUsuario = 0;
                result.Email = ex.Message;
                return result;
            }
            return result;
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Where(u => u.IdUsuario == id)
                    .FirstOrDefaultAsync();

                if (usuario != null)
                {
                    _context.Remove(usuario);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<UsuariosDTO>> GetAll()
        {
            var listUsuario = new List<UsuariosDTO>();

            var usaurios = await _context.Usuarios
                .Include(t => t.IdTipoUsuarioNavigation)
                .ToListAsync();

            foreach (var usuario in usaurios)
            {
                var usuarioDTO = new UsuariosDTO
                {
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaNacimiento = usuario.FechaNacimiento,
                    IdTipoUsuario = usuario.IdTipoUsuario,
                    IdUsuario = usuario.IdUsuario,
                    Nombre = usuario.Nombre,
                    TipoUsuario = usuario.IdTipoUsuarioNavigation.Descripcion
                };

                listUsuario.Add(usuarioDTO);
            }

            return listUsuario;
        }

        public async Task<UsuariosDTO> GetById(int id)
        {
            var usuarioDTO = new UsuariosDTO();
            var usuario = await _context.Usuarios
                .Include(t => t.IdTipoUsuarioNavigation)
                .Where(u => u.IdUsuario == id)
                .FirstOrDefaultAsync();
            if (usuario != null)
            {

                usuarioDTO.Apellido = usuario.Apellido;
                usuarioDTO.Email = usuario.Email;
                usuarioDTO.IdUsuario = usuario.IdUsuario;
                usuarioDTO.FechaNacimiento = usuario.FechaNacimiento;
                usuarioDTO.IdTipoUsuario = usuario.IdTipoUsuario;
                usuarioDTO.Nombre = usuario.Nombre;
                usuarioDTO.TipoUsuario = usuario.IdTipoUsuarioNavigation.Descripcion;
            }

            return usuarioDTO;
        }

        public async Task<UsuariosDTO> Update(UsuariosDTO usuario)
        {
            var result = new UsuariosDTO();
            try
            {
                var usuarioDB = await _context.Usuarios
                        .Where(u => u.IdUsuario == usuario.IdUsuario)
                        .AsNoTracking()
                        .FirstOrDefaultAsync();
                if (usuarioDB != null)
                {

                    usuarioDB.Apellido = usuario.Apellido;
                    usuarioDB.Email = usuario.Email;
                    usuarioDB.FechaNacimiento = usuario.FechaNacimiento;
                    usuarioDB.Nombre = usuario.Nombre;
                    if (!string.IsNullOrEmpty(usuario.Password) && !VerfyHashPassword(usuario.Password, usuarioDB.Password))
                    {
                        usuarioDB.Password = this.Hash(usuario.Password);
                    }
                    usuarioDB.IdTipoUsuario = usuario.IdTipoUsuario;

                    _context.Update(usuarioDB);
                    await _context.SaveChangesAsync();

                    usuario.Password = "";

                    result = usuario;
                }
            }
            catch (Exception ex)
            {
                result.IdUsuario = 0;
                result.Email = ex.Message;
                return result;
            }
            return result;
        }

        private string Hash(string textPlain)
        {
            byte[] salt;
            byte[] buffer;
            if (string.IsNullOrEmpty(textPlain))
                throw new ArgumentNullException(nameof(textPlain));

            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(textPlain, 16, 64))
            {
                salt = bytes.Salt;
                buffer = bytes.GetBytes(32);
            }
            byte[] dst = new byte[64];
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }
        private bool VerfyHashPassword(string password, string hashPassword)
        {
            byte[] buffer4;
            if (string.IsNullOrEmpty(hashPassword))
                return false;
            if (string.IsNullOrEmpty(hashPassword))
                throw new ArgumentNullException(nameof(hashPassword));
            byte[] src = Convert.FromBase64String(hashPassword);
            if (src.Length != 64 || src[0] != 0)
                return false;
            byte[] dst = new byte[16];
            Buffer.BlockCopy(src, 1, dst, 0, 16);
            byte[] buffer3 = new byte[32];
            Buffer.BlockCopy(src, 17, buffer3, 0, 32);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 64))
            {
                buffer4 = bytes.GetBytes(32);
            }
            return buffer3.SequenceEqual(buffer4);
        }


    }

    public interface IUserService
    {
        Task<UsuariosDTO> Login(string email, string password);
        Task<List<UsuariosDTO>> GetAll();
        Task<UsuariosDTO> GetById(int id);
        Task<UsuariosDTO> Update(UsuariosDTO usuario);
        Task<bool> DeleteById(int id);
        Task<UsuariosDTO> Create(UsuariosDTO usuario);
    }
}
