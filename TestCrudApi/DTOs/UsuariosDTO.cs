namespace TestCrudApi.DTOs
{
    public class UsuariosDTO
    {
        public int IdUsuario { get; set; } 
        public int IdTipoUsuario { get; set; }
        public string TipoUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; } 
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }       
    }
}
