﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PagoElectronico.Repositories;
using PagoElectronico.Entities;

namespace PagoElectronico.Services
{
    public class EmisorTarjetaService
    {
        public IEnumerable<EmisorTarjeta> GetAll()
        {
            EmisorTarjetaRepository emisorTarjetaRepo = new EmisorTarjetaRepository();
            return emisorTarjetaRepo.GetAll();
        }
    }
}
