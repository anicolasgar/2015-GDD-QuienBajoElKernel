﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagoElectronico.Entities;

namespace PagoElectronico.Services.Interfaces
{
    public interface IRolService
    {
         int crearRol(Rol rol);

         IList<Funcionalidad> Getfunciones();

        List<Rol> getRoles(String nombre,bool activo);

    }

    
}
