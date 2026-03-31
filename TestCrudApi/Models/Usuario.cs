using System;
using System.Collections.Generic;

namespace TestCrudApi.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int IdTipoUsuario { get; set; }

    public virtual TipoUsuario IdTipoUsuarioNavigation { get; set; } = null!;
}
