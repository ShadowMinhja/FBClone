using FBClone.Data;
using FBClone.Model;

namespace FBClone.Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private FBCloneContext dataContext;
    public FBCloneContext Get()
    {
        return dataContext ?? (dataContext = new FBCloneContext());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
