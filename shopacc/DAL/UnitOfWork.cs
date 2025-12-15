using shopacc.Models;
using System;

namespace shopacc.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly DataEntities _context = new DataEntities();

        private GenericRepository<Admin> _adminRepository;
        private GenericRepository<Banner> _bannerRepository;
        private GenericRepository<ConfigSite> _configRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<Table> _tableRepository;

        public GenericRepository<Admin> AdminRepository =>
            _adminRepository ?? (_adminRepository = new GenericRepository<Admin>(_context));
        public GenericRepository<Banner> BannerRepository =>
            _bannerRepository ?? (_bannerRepository = new GenericRepository<Banner>(_context));
        public GenericRepository<ConfigSite> ConfigSiteRepository =>
            _configRepository ?? (_configRepository = new GenericRepository<ConfigSite>(_context));

        public GenericRepository<Product> ProductRepository =>
            _productRepository ?? (_productRepository = new GenericRepository<Product>(_context));
        public GenericRepository<Table> TableRepository =>
             _tableRepository ?? (_tableRepository = new GenericRepository<Table>(_context));
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
