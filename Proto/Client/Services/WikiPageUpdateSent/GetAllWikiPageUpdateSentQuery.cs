
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdateSent
{
    public static class GetAllWikiPageUpdateSentQuery    
    {  
        public static async Task<GetAllWikiPageUpdateSentResponse> Execute(GrpcChannel channel, object args)
        {
            var client = new WikiPageUpdateSentServiceProto.WikiPageUpdateSentServiceProtoClient(channel);

            var entityRequest = new GetAllWikiPageUpdateSentRequest { QueryString = "$top=20" };
            var response = await client.GetAllWikiPageUpdateSentAsync(entityRequest);

            return response;
        }
    }
}