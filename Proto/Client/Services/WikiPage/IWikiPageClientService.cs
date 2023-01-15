
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPage
{
    public interface IWikiPageClientService
    {
        public Task<GetWikiPageByIdResponse> GetWikiPage(GrpcChannel channel, Guid id);

       
         public Task<GetAllWikiPageResponse> GetAllWikiPage(GrpcChannel channel, object args);       

        public Task<PutWikiPageResponse> PutWikiPage(GrpcChannel channel, object args);        
    }
    
}
