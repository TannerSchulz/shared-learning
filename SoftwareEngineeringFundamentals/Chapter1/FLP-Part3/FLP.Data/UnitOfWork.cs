using FLP.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLP.Data
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public GenericRepository<ApplicationUser> _ApplicationUser;

        public GenericRepository<ApplicationUser> ApplicationUser
        {
            get
            {
                if(_ApplicationUser == null)
                {
                    _ApplicationUser = new GenericRepository<ApplicationUser>(_context);
                }
                return _ApplicationUser;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
