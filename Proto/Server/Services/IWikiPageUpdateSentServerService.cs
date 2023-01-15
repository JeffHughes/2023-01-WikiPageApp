using WikiPageApp.Proto.Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;

namespace WikiPageApp.Proto.Server.Services
{
    public interface IWikiPageUpdateSentServerService
    {
        public Task<GetWikiPageUpdateSentByIdResponse> GetWikiPageUpdateSent(GetWikiPageUpdateSentByIdRequest request, ServerCallContext context);

       
         public Task<GetAllWikiPageUpdateSentResponse> GetAllWikiPageUpdateSent(GetAllWikiPageUpdateSentRequest request, ServerCallContext context);

        public Task<PutWikiPageUpdateSentResponse> PutWikiPageUpdateSent(PutWikiPageUpdateSentRequest request, ServerCallContext context);
    }
    
}