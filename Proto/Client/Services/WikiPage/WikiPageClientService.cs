
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPage
{    
    public class WikiPageClientService : IWikiPageClientService
    {        
        public async Task<GetWikiPageByIdResponse> GetWikiPage(GrpcChannel channel, Guid id)
        {
            var output = await GetWikiPageQuery.Execute(channel, id);
            return output;
        }


       
         
        public async Task<GetAllWikiPageResponse> GetAllWikiPage(GrpcChannel channel, object args)
        {
            var output = await GetAllWikiPageQuery.Execute(channel, args);
            return output;
        }
        
        public async Task<PutWikiPageResponse> PutWikiPage(GrpcChannel channel, object args)
        {
            var output = await PutWikiPageCommand.Execute(channel, args);
            return output;
        }
    }
    
}
