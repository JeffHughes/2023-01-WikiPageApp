using WikiPageApp.Proto.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace WikiPageApp.Proto.Server.Services
{
    public interface IWikiPageUpdateServerService
    {
        public Task<GetWikiPageUpdateByIdResponse> GetWikiPageUpdate(GetWikiPageUpdateByIdRequest request, ServerCallContext context);

       
         public Task<GetAllWikiPageUpdateResponse> GetAllWikiPageUpdate(GetAllWikiPageUpdateRequest request, ServerCallContext context);

        public Task<PutWikiPageUpdateResponse> PutWikiPageUpdate(PutWikiPageUpdateRequest request, ServerCallContext context);
    }
    
}