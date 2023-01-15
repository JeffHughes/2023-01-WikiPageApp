
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdate
{
    public static class GetWikiPageUpdateQuery    
    {  
        public static async Task<GetWikiPageUpdateByIdResponse> Execute(GrpcChannel channel, Guid id)
        {
            var client = new WikiPageUpdateServiceProto.WikiPageUpdateServiceProtoClient(channel);

            var entityRequest = new GetWikiPageUpdateByIdRequest { Id = id.ToString() };
            var response = await client.GetWikiPageUpdateAsync(entityRequest);

            return response;
        }
    }
}