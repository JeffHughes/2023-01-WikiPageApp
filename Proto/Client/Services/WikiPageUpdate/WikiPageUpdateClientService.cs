
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdate
{    
    public class WikiPageUpdateClientService : IWikiPageUpdateClientService
    {        
        public async Task<GetWikiPageUpdateByIdResponse> GetWikiPageUpdate(GrpcChannel channel, Guid id)
        {
            var output = await GetWikiPageUpdateQuery.Execute(channel, id);
            return output;
        }


       
         
        public async Task<GetAllWikiPageUpdateResponse> GetAllWikiPageUpdate(GrpcChannel channel, object args)
        {
            var output = await GetAllWikiPageUpdateQuery.Execute(channel, args);
            return output;
        }
        
        public async Task<PutWikiPageUpdateResponse> PutWikiPageUpdate(GrpcChannel channel, object args)
        {
            var output = await PutWikiPageUpdateCommand.Execute(channel, args);
            return output;
        }
    }
    
}
