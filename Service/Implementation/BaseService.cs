using Data;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Services.Implementation
{
    public class BaseService : IBaseService
    {
        public BaseService(DatabaseConnectService databaseConnectService)
        {
            this.DatabaseConnectService = databaseConnectService;
        }

        public DatabaseConnectService DatabaseConnectService
        {
            get; set;
        }
    }
}