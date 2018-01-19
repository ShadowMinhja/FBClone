using System;
using FBClone.Model;
using FBClone.Data.Infrastructure;

namespace FBClone.Data.Repositories
{
    public interface IStaffMemberRepository : IGenericRepository<StaffMember>
    {
    }

    public class StaffMemberRepository : GenericRepository<StaffMember>, IStaffMemberRepository
    {
        public StaffMemberRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}