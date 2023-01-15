





using Microsoft.AspNetCore.Builder;
using WikiPageApp.Proto.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Server.App
{
    public class ProtoServerAppServiceRegistrar : IProtoServerServiceRegistrar
    {
        private Microsoft.AspNetCore.Routing.IEndpointRouteBuilder _endpoints;
        public ProtoServerAppServiceRegistrar(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder endpoints)
        {
            _endpoints = endpoints;
        }
        public void MapGrpcService<TService>()
            where TService : class
        {
            _endpoints.MapGrpcService<TService>();
        }
    }
}
