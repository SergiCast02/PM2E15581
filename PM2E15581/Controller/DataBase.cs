using PM2E15581.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2E15581.Controller
{
    public class DataBase
    {
        readonly SQLiteAsyncConnection dbase;
        /* Constructor de clase */
        public DataBase(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);
            dbase.CreateTableAsync<Sitios>();
        }

        #region Operaciones
        // Create
        public Task<int> SitioSave(Sitios sitio)
        {
            if (sitio.id != 0)
            {
                return dbase.UpdateAsync(sitio); // Update
            }
            else
            {
                return dbase.InsertAsync(sitio);
            }
        }

        // Read
        public Task<List<Sitios>> obtenerListaSitios()
        {
            return dbase.Table<Sitios>().ToListAsync();
        }

        // Read V2
        public Task<Sitios> obtenerSitio(int sid)
        {
            return dbase.Table<Sitios>()
                .Where(i => i.id == sid)
                .FirstOrDefaultAsync();
        }

        // Delete
        public Task<int> SitioDelete(Sitios sitio)
        {
            return dbase.DeleteAsync(sitio);
        }

        #endregion Operaciones
    }
}
