
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdate
{
    public static class PutWikiPageUpdateCommand    
    {  
        public static async Task<PutWikiPageUpdateResponse> Execute(GrpcChannel channel, object args)
        {
            var client = new WikiPageUpdateServiceProto.WikiPageUpdateServiceProtoClient(channel);
            var entity = (Library.WikiPageUpdate)args;
            var entityRequest = new PutWikiPageUpdateRequest 
            { 
                Id = entity.WikiPageUpdateID,
                Data = entity        
            };
            var response = await client.PutWikiPageUpdateAsync(entityRequest);

            return response;
        }
    }
}