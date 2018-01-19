using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;
using System.Data.SqlClient;
using System.Data.Entity.Core;
using System.Configuration;

namespace FBClone.Service
{
    public interface ILocationService
    {
        Location GetById(string Id);
        IEnumerable<Location> GetAll();
        IEnumerable<Location> GetMany(string userName);
        IQueryable<Location> Query(Expression<Func<Location, bool>> where);
        IQueryable<Location> AllIncluding(Expression<Func<Location, bool>> where);

        Location Add(Location location);
        Location Update(Location location);
        Location AddOrUpdate(Location location, string userId);

        void Delete(string id);
        void UpdateMenuAssociation(string updatedLocationId, string updatedMenuId);
        void DeleteMenuAssociation(string locationIdToRemoveMenuFrom, string updatedMenuId);
    }
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILocationRepository locationRepository;
        private readonly IEntityDBService entityDBService;

        public LocationService(IUnitOfWork unitOfWork, ILocationRepository locationRepository, IEntityDBService entityDBService)
        {
            this.unitOfWork = unitOfWork;
            this.locationRepository = locationRepository;
            this.entityDBService = entityDBService;
        }

        public Location GetById(string id)
        {
            return locationRepository.GetById(id);
        }

        public IEnumerable<Location> GetAll()
        {
            return locationRepository.GetAll();
        }

        public IEnumerable<Location> GetMany(string name)
        {
            return locationRepository.GetMany(x => x.Name == name);
        }

        public IQueryable<Location> Query(Expression<Func<Location, bool>> where)
        {
            return locationRepository.Query(where);
        }

        public IQueryable<Location> AllIncluding(Expression<Func<Location, bool>> where)
        {
            return locationRepository.AllIncluding(
                s => s.Menus
            ).Where(where);
        }

        public Location Add(Location location)
        {
            locationRepository.Add(location);
            unitOfWork.SaveChanges();
            return location;
        }

        public Location Update(Location location)
        {
            locationRepository.Update(location);
            unitOfWork.SaveChanges();
            return location;
        }

        public Location AddOrUpdate(Location location, string userId)
        {
            var foundLocation = locationRepository.GetAll().Where(x => x.PlaceId == location.PlaceId).SingleOrDefault();
            if (foundLocation == null) {  //Add
                location = locationRepository.Add(location);
                unitOfWork.SaveChanges();
                return location;
            }
            else //Update / Claim for User
            {
                foundLocation.UserId = userId;
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@locationId", new Guid(foundLocation.Id)),
                    new SqlParameter("@userId", new Guid(userId))
                };
                entityDBService.ExecuteStoredProc("EXEC spUserClaimLocationAndMenus @locationId, @userId", sqlParams.ToArray());

                locationRepository.Refresh(foundLocation);
                locationRepository.Update(foundLocation);
                try
                {
                    unitOfWork.SaveChanges();
                }
                catch (OptimisticConcurrencyException)
                {
                }
                return foundLocation;
            }
        }

        public void Delete(string id)
        {
            var Location = locationRepository.GetById(id);
            var Locations = locationRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships
            string fbCloneUserId = ConfigurationManager.AppSettings["fbCloneUserId"];
            foreach (var item in Locations)
            {
                //locationRepository.Delete(item);
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@locationId", new Guid(item.Id)),
                    new SqlParameter("@userId", new Guid(fbCloneUserId))
                };
                entityDBService.ExecuteStoredProc("EXEC spUserClaimLocationAndMenus @locationId, @userId", sqlParams.ToArray());
            }

            if (Location != null)
            {
                //locationRepository.Delete(Location);
                List<SqlParameter> sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@locationId", new Guid(Location.Id)),
                    new SqlParameter("@userId", new Guid(fbCloneUserId))
                };
                entityDBService.ExecuteStoredProc("EXEC spUserClaimLocationAndMenus @locationId, @userId", sqlParams.ToArray());
            }
        }

        public void UpdateMenuAssociation(string updatedLocationId, string updatedMenuId)
        {
            using(var dbContext = new FBCloneContext())
            {
                var location = dbContext.Locations.Find(updatedLocationId);
                var menu = dbContext.Menus.Find(updatedMenuId);
                if (!location.Menus.Contains(menu))
                {
                    location.Menus.Add(menu);
                    dbContext.SaveChanges();
                }
            }
        }

        public void DeleteMenuAssociation(string locationIdToRemoveMenuFrom, string updatedMenuId)
        {
            using (var dbContext = new FBCloneContext())
            {
                var location = dbContext.Locations.Find(locationIdToRemoveMenuFrom);
                var menu = dbContext.Menus.Find(updatedMenuId);
                if (location.Menus.Contains(menu))
                {
                    location.Menus.Remove(menu);
                    dbContext.SaveChanges();
                }
            }
        }

    }
}
