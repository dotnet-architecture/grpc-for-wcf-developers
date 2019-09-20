using System;
using System.Collections.Generic;

namespace TraderSys.Portfolios
{
    public class UserLookup : IUserLookup
    {
        private static readonly Dictionary<string, Guid> Guids = new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase)
        {
            ["Alice"] = new Guid("68CB16F7-42BD-4330-A191-FA5904D2E5A0"),
            ["Bob"] = new Guid("7CD3AFA4-0F11-4947-84E5-FE8194DB5C3D"),
        };

        public bool TryGetId(string name, out Guid guid) => Guids.TryGetValue(name, out guid);
    }
}