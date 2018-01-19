using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;
using FBClone.Service.Common;

namespace FBClone.Service
{
    public interface IAspNetUserService
    {
        IEnumerable<AspNetUser> GetAllByNameFuzzyMatch(string name);
        AspNetUser GetByNameExactMatch(string name);
        AspNetUser GetByName(string name);
        AspNetUser GetByUserId(Guid guid);
        AspNetUser GetByEmail(string email);
        Actor ConvertToActor(AspNetUser user);
    }
    public class AspNetUserService : IAspNetUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAspNetUserRepository aspNetUserRepository;
        public AspNetUserService(IUnitOfWork unitOfWork, IAspNetUserRepository aspNetUserRepository)
        {
            this.unitOfWork = unitOfWork;
            this.aspNetUserRepository = aspNetUserRepository;
        }

        public IEnumerable<AspNetUser> GetAllByNameFuzzyMatch(string name)
        {
            return aspNetUserRepository.GetMany(x => (String.Equals(x.UserName, name, StringComparison.OrdinalIgnoreCase)));
        }

        public AspNetUser GetByNameExactMatch(string name)
        {
            return aspNetUserRepository.GetAll().Where(x => String.Equals(x.UserName, name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
        }

        public AspNetUser GetByName(string name)
        {
            return aspNetUserRepository.Query(x => x.UserName == name).SingleOrDefault();
        }

        public AspNetUser GetByUserId(Guid guid)
        {
            return aspNetUserRepository.Query(x => x.Id == guid.ToString()).SingleOrDefault();
        }

        public AspNetUser GetByEmail(string email)
        {
            return aspNetUserRepository.GetAll().Where(x => String.Equals(x.Email, email, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
        }

        public Actor ConvertToActor(AspNetUser user)
        {
            return new Actor
            {
                Id = GuidEncoder.Encode(user.Id),
                Email_md5 = Utils.GetMD5Hash(user.Email.ToLower()).Replace("-", "").ToLower(),
                First_name = user.FirstName,
                Last_name = user.LastName,
                User_name = user.UserName
            };
        }

    }
}
