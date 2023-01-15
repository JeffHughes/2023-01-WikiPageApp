
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPage
{
    public static class PutWikiPageCommand    
    {  
        public static async Task<PutWikiPageResponse> Execute(GrpcChannel channel, object args)
        {
            var client = new WikiPageServiceProto.WikiPageServiceProtoClient(channel);
            var entity = (Library.WikiPage)args;
            var entityRequest = new PutWikiPageRequest 
            { 
                Id = entity.WikiPageID,
                Data = entity        
            };
            var response = await client.PutWikiPageAsync(entityRequest);

            return response;
        }
    }
}