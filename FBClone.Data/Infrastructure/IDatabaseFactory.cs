using System;
using FBClone.Data;
using FBClone.Model;

namespace FBClone.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        FBCloneContext Get();
    }
}
