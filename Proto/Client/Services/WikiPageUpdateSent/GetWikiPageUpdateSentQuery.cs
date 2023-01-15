
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdateSent
{
    public static class GetWikiPageUpdateSentQuery    
    {  
        public static async Task<GetWikiPageUpdateSentByIdResponse> Execute(GrpcChannel channel, Guid id)
        {
            var client = new WikiPageUpdateSentServiceProto.WikiPageUpdateSentServiceProtoClient(channel);

            var entityRequest = new GetWikiPageUpdateSentByIdRequest { Id = id.ToString() };
            var response = await client.GetWikiPageUpdateSentAsync(entityRequest);

            return response;
        }
    }
}