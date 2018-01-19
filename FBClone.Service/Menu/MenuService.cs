using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;

namespace FBClone.Service
{
    public interface IMenuService
    {
        Menu GetById(string id);
        Menu GetByRole(string roleCode);
        IEnumerable<Menu> GetAll();
        //IEnumerable<Menu> GetMany(string userName);
        IQueryable<Menu> Query(Expression<Func<Menu, bool>> where);
        IQueryable<Menu> AllIncluding(Expression<Func<Menu, bool>> where);
        Menu Add(Menu menu);
        Menu Update(Menu menu);
        void Delete(string id);
        MenuSection GetMenuSectionById(string id);
        MenuSection AddMenuSection(MenuSection menuSection);
        MenuSection UpdateMenuSection(MenuSection menuSection);
        void DeleteMenuSection(string id);
        MenuItem GetMenuItemById(string id);
        MenuItem AddMenuItem(MenuItem menuItem);
        MenuItem UpdateMenuItem(MenuItem updatedMenuItem);
        void DeleteMenuItem(string id);
    }
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMenuRepository menuRepository;
        private readonly IMenuSectionRepository menuSectionRepository;
        private readonly IMenuItemRepository menuItemRepository;
        public MenuService(IUnitOfWork unitOfWork, IMenuRepository menuRepository, IMenuSectionRepository menuSectionRepository, IMenuItemRepository menuItemRepository)
        {
            this.unitOfWork = unitOfWork;
            this.menuRepository = menuRepository;
            this.menuSectionRepository = menuSectionRepository;
            this.menuItemRepository = menuItemRepository;
        }

        public Menu GetById(string id)
        {
            var menu = menuRepository.AllIncluding(t => t.MenuSections.Select(s => s.MenuItems))
                                     .Where(t => t.Id == id).FirstOrDefault();
            return menu;
        }

        public Menu GetByRole(string roleCode)
        {
            var menu = menuRepository.AllIncluding(t => t.MenuSections.Select(s => s.MenuItems)).FirstOrDefault();
            return menu;
        }

        public IEnumerable<Menu> GetAll()
        {
            return menuRepository.GetAll();
        }

        public IQueryable<Menu> Query(Expression<Func<Menu, bool>> where)
        {
            return menuRepository.Query(where);
        }

        public IQueryable<Menu> AllIncluding(Expression<Func<Menu, bool>> where)
        {
            return menuRepository.AllIncluding(
                s => s.Locations,
                s => s.MenuSections,
                s => s.MenuSections.Select(m => m.MenuItems)
            ).Where(where);
        }

        public Menu Add(Menu menu)
        {
            menuRepository.Add(menu);
            unitOfWork.SaveChanges();
            return menu;
        }

        public Menu Update(Menu menu)
        {
            menuRepository.Update(menu);
            unitOfWork.SaveChanges();
            return menu;
            //Menu retrievedMenu = null;
            //using (var dbContext = new FBCloneContext())
            //{
            //    retrievedMenu = dbContext.Menus.Find(menu.Id);
            //    if(retrievedMenu != null)
            //    {
            //        retrievedMenu.Description = menu.Description;
            //    }
            //    dbContext.SaveChanges();
            //}
            //return retrievedMenu;
        }

        public void Delete(string id)
        {
            var menu = menuRepository.GetById(id);

            menuRepository.Delete(menu);
            unitOfWork.SaveChanges();
        }

        public MenuSection GetMenuSectionById(string id)
        {
            return menuSectionRepository.GetById(id);
        }

        public MenuSection AddMenuSection(MenuSection menuSection)
        {
            menuSectionRepository.Add(menuSection);
            unitOfWork.SaveChanges();
            return menuSection;
        }

        public MenuSection UpdateMenuSection(MenuSection menuSection)
        {
            menuSectionRepository.Update(menuSection);
            unitOfWork.SaveChanges();
            return menuSection;
        }

        public void DeleteMenuSection(string id)
        {
            var menuSection = menuSectionRepository.GetById(id);

            menuSectionRepository.Delete(menuSection);
            unitOfWork.SaveChanges();
        }
        public MenuItem GetMenuItemById(string id)
        {
            return menuItemRepository.GetById(id);
        }

        public MenuItem AddMenuItem(MenuItem menuItem)
        {
            menuItemRepository.Add(menuItem);
            unitOfWork.SaveChanges();
            return menuItem;
        }

        public MenuItem UpdateMenuItem(MenuItem updatedMenuItem)
        {
            MenuItem menuItem = updatedMenuItem;
            using (var dbContext = new FBCloneContext())
            {
                menuItem = dbContext.MenuItems.Find(updatedMenuItem.Id);
                menuItem.MenuSectionId = updatedMenuItem.MenuSectionId;
                menuItem.ItemType = updatedMenuItem.ItemType;
                menuItem.Required = updatedMenuItem.Required;
                menuItem.ItemText = updatedMenuItem.ItemText;
                menuItem.Price = updatedMenuItem.Price;
                menuItem.Active = updatedMenuItem.Active;
                menuItem.DaysOfWeek = updatedMenuItem.DaysOfWeek;
                menuItem.Sequence = updatedMenuItem.Sequence;
                menuItem.ItemImageUrl = updatedMenuItem.ItemImageUrl;
                menuItem.FoodTotalReviews = updatedMenuItem.FoodTotalReviews;
                menuItem.FoodTotalScore = updatedMenuItem.FoodTotalScore;
                menuItem.UserId = updatedMenuItem.UserId;
                menuItem.UpdatedBy = updatedMenuItem.UpdatedBy;
                dbContext.SaveChanges();
            }
            return menuItem;
        }

        public void DeleteMenuItem(string id)
        {
            var menuItem = menuItemRepository.GetById(id);

            menuItemRepository.Delete(menuItem);
            unitOfWork.SaveChanges();
        }

    }
}
