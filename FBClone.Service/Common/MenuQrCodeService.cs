using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FBClone.Service
{
    public interface IMenuQrCodeService
    {
        IEnumerable<MenuQrCode> GetAll();
        IQueryable<MenuQrCode> Query(Expression<Func<MenuQrCode, bool>> where);
        IQueryable<MenuQrCode> AllIncluding(Expression<Func<MenuQrCode, bool>> where);
        MenuQrCode GetById(int menuQrCodeId);

        MenuQrCode Add(MenuQrCode menuQrCode);
        MenuQrCode Update(MenuQrCode menuQrCode);

        void Delete(MenuQrCode previousMenuQRCode);
    }

    public class MenuQrCodeService : IMenuQrCodeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMenuQrCodeRepository MenuQrCodeRepository;

        public MenuQrCodeService(IUnitOfWork unitOfWork, IMenuQrCodeRepository MenuQrCodeRepository)
        {
            this.unitOfWork = unitOfWork;
            this.MenuQrCodeRepository = MenuQrCodeRepository;
        }
        public IEnumerable<MenuQrCode> GetAll()
        {
            return MenuQrCodeRepository.GetAll();
        }

        public IQueryable<MenuQrCode> Query(Expression<Func<MenuQrCode, bool>> where)
        {
            return MenuQrCodeRepository.Query(where);
        }

        public IQueryable<MenuQrCode> AllIncluding(Expression<Func<MenuQrCode, bool>> where)
        {
            return MenuQrCodeRepository.AllIncluding(
                s => s.Location
            ).Where(where);
        }

        MenuQrCode IMenuQrCodeService.GetById(int menuQrCodeId)
        {
            return MenuQrCodeRepository.GetAll().Where(x => x.Id == menuQrCodeId).FirstOrDefault();
        }

        public MenuQrCode Add(MenuQrCode menuQrCode)
        {
            MenuQrCodeRepository.Add(menuQrCode);
            unitOfWork.SaveChanges();
            return menuQrCode;
        }

        public MenuQrCode Update(MenuQrCode menuQrCode)
        {
            MenuQrCodeRepository.Update(menuQrCode);
            unitOfWork.SaveChanges();
            return menuQrCode;
        }
        public void Delete(MenuQrCode previousMenuQRCode)
        {
            MenuQrCodeRepository.Delete(previousMenuQRCode);
        }

    }
}
