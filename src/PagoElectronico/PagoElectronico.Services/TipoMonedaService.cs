﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagoElectronico.Repositories;
using PagoElectronico.Entities;

namespace PagoElectronico.Services
{
    public class TipoMonedaService
    {
        public IEnumerable<TipoMoneda> getAll()
        {
            TipoMonedaRepository tipoMonedaRepo = new TipoMonedaRepository();
            return tipoMonedaRepo.GetAll();
        }
    }
}