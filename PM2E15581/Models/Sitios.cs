using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E15581.Models
{
    public class Sitios
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public Byte[] imagen { get; set; }
        public String latitud { get; set; }
        public String longitud { get; set; }
        public String descripcion { get; set; }
    }
}
