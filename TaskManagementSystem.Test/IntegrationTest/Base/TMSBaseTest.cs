using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementSystem.Infrastructure.Data.Context;
using TaskManagementSystem.Test.Helper;
using TaskManagementSystem.Test.Infrastructure;

namespace TaskManagementSystem.Test.IntegrationTest.Base
{
    public abstract class TMSBaseTest<TController> : IDisposable where TController : ControllerBase
    {
        protected readonly TaskManagementSystemApplication<TController> _application;
        protected readonly HttpClient _client;
        protected TMSBaseTest()
        {
            _application = new TaskManagementSystemApplication<TController>();
            _client = _application.CreateClient(); 
        }
        public virtual void Dispose()
        {
            _client.Dispose();
            _application.Dispose();
        }
        
    }
}
