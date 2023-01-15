
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdate
{
    public static class GetAllWikiPageUpdateQuery    
    {  
        public static async Task<GetAllWikiPageUpdateResponse> Execute(GrpcChannel channel, object args)
        {
            var client = new WikiPageUpdateServiceProto.WikiPageUpdateServiceProtoClient(channel);

            var entityRequest = new GetAllWikiPageUpdateRequest { QueryString = "$top=20" };
            var response = await client.GetAllWikiPageUpdateAsync(entityRequest);

            return response;
        }
    }
}