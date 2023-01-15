
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdateSent
{    
    public class WikiPageUpdateSentClientService : IWikiPageUpdateSentClientService
    {        
        public async Task<GetWikiPageUpdateSentByIdResponse> GetWikiPageUpdateSent(GrpcChannel channel, Guid id)
        {
            var output = await GetWikiPageUpdateSentQuery.Execute(channel, id);
            return output;
        }


       
         
        public async Task<GetAllWikiPageUpdateSentResponse> GetAllWikiPageUpdateSent(GrpcChannel channel, object args)
        {
            var output = await GetAllWikiPageUpdateSentQuery.Execute(channel, args);
            return output;
        }
        
        public async Task<PutWikiPageUpdateSentResponse> PutWikiPageUpdateSent(GrpcChannel channel, object args)
        {
            var output = await PutWikiPageUpdateSentCommand.Execute(channel, args);
            return output;
        }
    }
    
}
