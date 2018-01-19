using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IMenuQrCodeRepository : IGenericRepository<MenuQrCode>
    {
    }

    public class MenuQrCodeRepository : GenericRepository<MenuQrCode>, IMenuQrCodeRepository
    {
        public MenuQrCodeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}