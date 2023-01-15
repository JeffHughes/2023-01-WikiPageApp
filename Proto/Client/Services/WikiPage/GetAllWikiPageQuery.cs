
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPage
{
    public static class GetAllWikiPageQuery    
    {  
        public static async Task<GetAllWikiPageResponse> Execute(GrpcChannel channel, object args)
        {
            var client = new WikiPageServiceProto.WikiPageServiceProtoClient(channel);

            var entityRequest = new GetAllWikiPageRequest { QueryString = "$top=20" };
            var response = await client.GetAllWikiPageAsync(entityRequest);

            return response;
        }
    }
}