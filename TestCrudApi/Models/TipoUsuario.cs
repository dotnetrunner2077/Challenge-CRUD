using System;
using System.Collections.Generic;

namespace TestCrudApi.Models;

public partial class TipoUsuario
{
    public int IdTipoUsuario { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; } = new List<Usuario>();
}
