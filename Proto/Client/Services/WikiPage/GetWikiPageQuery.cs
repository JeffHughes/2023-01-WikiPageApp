
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPage
{
    public static class GetWikiPageQuery    
    {  
        public static async Task<GetWikiPageByIdResponse> Execute(GrpcChannel channel, Guid id)
        {
            var client = new WikiPageServiceProto.WikiPageServiceProtoClient(channel);

            var entityRequest = new GetWikiPageByIdRequest { Id = id.ToString() };
            var response = await client.GetWikiPageAsync(entityRequest);

            return response;
        }
    }
}