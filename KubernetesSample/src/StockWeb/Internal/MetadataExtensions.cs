using System;
using System.Linq;
using Grpc.Core;

namespace StockWeb.Internal
{
    public static class MetadataExtensions
    {
        public static string GetString(this Metadata metadata, string key, string defaultValue = "UNKNOWN")
        {
            return metadata.FirstOrDefault(e => e.Key.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value ?? defaultValue;
        }
    }
}