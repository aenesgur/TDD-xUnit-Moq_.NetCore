using System;
using System.Collections.Generic;
using System.Text;

namespace StationaryStore.DAL.Abstractions
{
    public interface IMongoDbSettings
    {
        string ConnectionString { get; set; }
        string Database { get; set; }
        string Collection { get; set; }
    }
}
