﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PagoElectronico.Entities
{
    public class TipoTransaccion
    {
        public long ID { get; set; }
        public string descripcion { get; set; }
        public double costo { get; set; }
    }
}
