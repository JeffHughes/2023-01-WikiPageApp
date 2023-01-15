
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdateSent
{
    public interface IWikiPageUpdateSentClientService
    {
        public Task<GetWikiPageUpdateSentByIdResponse> GetWikiPageUpdateSent(GrpcChannel channel, Guid id);

       
         public Task<GetAllWikiPageUpdateSentResponse> GetAllWikiPageUpdateSent(GrpcChannel channel, object args);       

        public Task<PutWikiPageUpdateSentResponse> PutWikiPageUpdateSent(GrpcChannel channel, object args);        
    }
    
}
