using WikiPageApp.Proto.Library;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using System.Linq;
using Nelibur.ObjectMapper;
using WikiPageApp.Proto.Server.Services;
using WikiPageApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Model = WikiPageApp.Model;
using System.Linq.Expressions;

namespace WikiPageApp.Proto.Server.Services
{
    public class WikiPageUpdateServerService : WikiPageUpdateServiceProto.WikiPageUpdateServiceProtoBase, IWikiPageUpdateServerService
    {
        private readonly WikiPageUpdateDataContext _context;
        public WikiPageUpdateServerService(IConfiguration configuration)
        {
            var objName = nameof(WikiPageUpdate);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageUpdateDataContext(connectionString, EFProvider);
        }

        public async override Task<GetWikiPageUpdateByIdResponse> GetWikiPageUpdate(GetWikiPageUpdateByIdRequest request, ServerCallContext context)
        {
            var id = Guid.Parse(request.Id);

            var result = await _context.WikiPageUpdate.Where(entity => entity.WikiPageUpdateID == id).FirstOrDefaultAsync();
            var response = new GetWikiPageUpdateByIdResponse { Data = new WikiPageUpdate() };
            await MapEntityToProto(response.Data, result);
            return response;
        }

        private async Task MapEntityToProto(WikiPageUpdate proto, Model.WikiPageUpdate entity)
        {
            if (null == entity) return;
            TinyMapper.Bind<Model.WikiPageUpdate, Proto.Library.WikiPageUpdate>();            
            proto = TinyMapper.Map(entity, proto);
        proto.WikiPageUpdateInjestDatetime = await ToProtoTimestamp(entity.WikiPageUpdateInjestDatetime);
        proto.WikiPageUpdateTimestamp = await ToProtoTimestamp(entity.WikiPageUpdateTimestamp);
        
           

        }

        private async Task MapProtoToEntity(WikiPageUpdate proto, Model.WikiPageUpdate entity)
        {
            TinyMapper.Bind<Proto.Library.WikiPageUpdate, Model.WikiPageUpdate>();
            entity = TinyMapper.Map(proto, entity);            
                    entity.WikiPageUpdateInjestDatetime = proto.WikiPageUpdateInjestDatetime.ToDateTime();
        entity.WikiPageUpdateTimestamp = proto.WikiPageUpdateTimestamp.ToDateTime();
   
                  }

        private async Task<Google.Protobuf.WellKnownTypes.Timestamp> ToProtoTimestamp(DateTime prop)
        {
            var dtkPropertyDateTime = DateTime.SpecifyKind(prop, DateTimeKind.Utc);
            return Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dtkPropertyDateTime);
        }



        
     

        public override async Task<GetAllWikiPageUpdateResponse> GetAllWikiPageUpdate(GetAllWikiPageUpdateRequest request, ServerCallContext context)
        {
            var maxTake = 200;
            var result = await _context.WikiPageUpdate.TakeLast(maxTake).ToListAsync();
            var response = new GetAllWikiPageUpdateResponse();
            WikiPageUpdate proto = null;
            result.ForEach(async entity =>
            {
                proto = new WikiPageUpdate();
                await MapEntityToProto(proto, entity);
                response.Data.Add(proto);
            });

            return response;
        }

        public override async Task<PutWikiPageUpdateResponse> PutWikiPageUpdate(PutWikiPageUpdateRequest request, ServerCallContext context)
        {
            try
            {
                var inputEntity = new Model.WikiPageUpdate();
                Model.WikiPageUpdate outputEntity;

                await MapProtoToEntity(request.Data, inputEntity);
                
                var idGuid = Guid.Parse(request.Id);                
                var existing = await _context.WikiPageUpdate.FindAsync(idGuid);

                if (existing == null) _context.WikiPageUpdate.Add(inputEntity);
                else
                {
                    _context.WikiPageUpdate.Remove(existing);
                    _context.WikiPageUpdate.Add(inputEntity);
                }
                
                await _context.SaveChangesAsync();
                outputEntity = inputEntity;
                var outputProto = new WikiPageUpdate();
                await MapEntityToProto(outputProto, outputEntity);

                var response = new PutWikiPageUpdateResponse { Data = outputProto, Id = outputProto.WikiPageUpdateID };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}