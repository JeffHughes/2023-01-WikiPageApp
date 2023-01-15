
using WikiPageApp.Proto.Library;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WikiPageApp.Proto.Client.Services.WikiPageUpdate
{
    public interface IWikiPageUpdateClientService
    {
        public Task<GetWikiPageUpdateByIdResponse> GetWikiPageUpdate(GrpcChannel channel, Guid id);

       
         public Task<GetAllWikiPageUpdateResponse> GetAllWikiPageUpdate(GrpcChannel channel, object args);       

        public Task<PutWikiPageUpdateResponse> PutWikiPageUpdate(GrpcChannel channel, object args);        
    }
    
}
