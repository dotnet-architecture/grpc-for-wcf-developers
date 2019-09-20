using System;

namespace TraderSys.Portfolios
{
    public interface IUserLookup
    {
        bool TryGetId(string name, out Guid guid);
    }
}