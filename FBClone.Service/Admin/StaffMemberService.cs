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
    public interface IStaffMemberService
    {
        StaffMember GetById(string Id);
        IEnumerable<StaffMember> GetAll();
        IEnumerable<StaffMember> GetMany(string userName);
        IQueryable<StaffMember> Query(Expression<Func<StaffMember, bool>> where);
        StaffMember Add(StaffMember staffMember);
        StaffMember Update(StaffMember staffMember);
        void Delete(string id);
    }
    public class StaffMemberService : IStaffMemberService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IStaffMemberRepository staffMemberRepository;
        public StaffMemberService(IUnitOfWork unitOfWork, IStaffMemberRepository staffMemberRepository)
        {
            this.unitOfWork = unitOfWork;
            this.staffMemberRepository = staffMemberRepository;
        }

        public StaffMember GetById(string id)
        {
            return staffMemberRepository.GetById(id);
        }

        public IEnumerable<StaffMember> GetAll()
        {
            return staffMemberRepository.GetAll();
        }

        public IEnumerable<StaffMember> GetMany(string name)
        {
            return staffMemberRepository.GetMany(x => x.Name == name);
        }

        public IQueryable<StaffMember> Query(Expression<Func<StaffMember, bool>> where)
        {
            return staffMemberRepository.Query(where);
        }

        public StaffMember Add(StaffMember staffMember)
        {
            staffMemberRepository.Add(staffMember);
            unitOfWork.SaveChanges();
            return staffMember;
        }

        public StaffMember Update(StaffMember staffMember)
        {
            staffMemberRepository.Update(staffMember);
            unitOfWork.SaveChanges();
            return staffMember;
        }

        public void Delete(string id)
        {
            var staffMember = staffMemberRepository.GetById(id);
            var StaffMembers = staffMemberRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in StaffMembers)
            {
                staffMemberRepository.Delete(item);
            }

            if (staffMember != null)
            {
                staffMemberRepository.Delete(staffMember);
                unitOfWork.SaveChanges();
            }
        }

    }
}
