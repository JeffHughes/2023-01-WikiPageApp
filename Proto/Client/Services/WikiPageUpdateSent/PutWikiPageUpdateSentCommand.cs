
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdateSent
{
    public static class PutWikiPageUpdateSentCommand    
    {  
        public static async Task<PutWikiPageUpdateSentResponse> Execute(GrpcChannel channel, object args)
        {
            var client = new WikiPageUpdateSentServiceProto.WikiPageUpdateSentServiceProtoClient(channel);
            var entity = (Library.WikiPageUpdateSent)args;
            var entityRequest = new PutWikiPageUpdateSentRequest 
            { 
                Id = entity.WikiPageUpdateSentID,
                Data = entity        
            };
            var response = await client.PutWikiPageUpdateSentAsync(entityRequest);

            return response;
        }
    }
}