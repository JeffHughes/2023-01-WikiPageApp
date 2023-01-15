using WikiPageApp.Proto.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace WikiPageApp.Proto.Server.Services
{
    public interface IWikiPageServerService
    {
        public Task<GetWikiPageByIdResponse> GetWikiPage(GetWikiPageByIdRequest request, ServerCallContext context);

       
         public Task<GetAllWikiPageResponse> GetAllWikiPage(GetAllWikiPageRequest request, ServerCallContext context);

        public Task<PutWikiPageResponse> PutWikiPage(PutWikiPageRequest request, ServerCallContext context);
    }
    
}