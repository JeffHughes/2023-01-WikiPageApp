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
    public class WikiPageUpdateSentServerService : WikiPageUpdateSentServiceProto.WikiPageUpdateSentServiceProtoBase, IWikiPageUpdateSentServerService
    {
        private readonly WikiPageUpdateSentDataContext _context;
        public WikiPageUpdateSentServerService(IConfiguration configuration)
        {
            var objName = nameof(WikiPageUpdateSent);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageUpdateSentDataContext(connectionString, EFProvider);
        }

        public async override Task<GetWikiPageUpdateSentByIdResponse> GetWikiPageUpdateSent(GetWikiPageUpdateSentByIdRequest request, ServerCallContext context)
        {
            var id = Guid.Parse(request.Id);

            var result = await _context.WikiPageUpdateSent.Where(entity => entity.WikiPageUpdateSentID == id).FirstOrDefaultAsync();
            var response = new GetWikiPageUpdateSentByIdResponse { Data = new WikiPageUpdateSent() };
            await MapEntityToProto(response.Data, result);
            return response;
        }

        private async Task MapEntityToProto(WikiPageUpdateSent proto, Model.WikiPageUpdateSent entity)
        {
            if (null == entity) return;
            TinyMapper.Bind<Model.WikiPageUpdateSent, Proto.Library.WikiPageUpdateSent>();            
            proto = TinyMapper.Map(entity, proto);
        
           

        }

        private async Task MapProtoToEntity(WikiPageUpdateSent proto, Model.WikiPageUpdateSent entity)
        {
            TinyMapper.Bind<Proto.Library.WikiPageUpdateSent, Model.WikiPageUpdateSent>();
            entity = TinyMapper.Map(proto, entity);            
               
                  }

        private async Task<Google.Protobuf.WellKnownTypes.Timestamp> ToProtoTimestamp(DateTime prop)
        {
            var dtkPropertyDateTime = DateTime.SpecifyKind(prop, DateTimeKind.Utc);
            return Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dtkPropertyDateTime);
        }



        
     

        public override async Task<GetAllWikiPageUpdateSentResponse> GetAllWikiPageUpdateSent(GetAllWikiPageUpdateSentRequest request, ServerCallContext context)
        {
            var maxTake = 200;
            var result = await _context.WikiPageUpdateSent.TakeLast(maxTake).ToListAsync();
            var response = new GetAllWikiPageUpdateSentResponse();
            WikiPageUpdateSent proto = null;
            result.ForEach(async entity =>
            {
                proto = new WikiPageUpdateSent();
                await MapEntityToProto(proto, entity);
                response.Data.Add(proto);
            });

            return response;
        }

        public override async Task<PutWikiPageUpdateSentResponse> PutWikiPageUpdateSent(PutWikiPageUpdateSentRequest request, ServerCallContext context)
        {
            try
            {
                var inputEntity = new Model.WikiPageUpdateSent();
                Model.WikiPageUpdateSent outputEntity;

                await MapProtoToEntity(request.Data, inputEntity);
                
                var idGuid = Guid.Parse(request.Id);                
                var existing = await _context.WikiPageUpdateSent.FindAsync(idGuid);

                if (existing == null) _context.WikiPageUpdateSent.Add(inputEntity);
                else
                {
                    _context.WikiPageUpdateSent.Remove(existing);
                    _context.WikiPageUpdateSent.Add(inputEntity);
                }
                
                await _context.SaveChangesAsync();
                outputEntity = inputEntity;
                var outputProto = new WikiPageUpdateSent();
                await MapEntityToProto(outputProto, outputEntity);

                var response = new PutWikiPageUpdateSentResponse { Data = outputProto, Id = outputProto.WikiPageUpdateSentID };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}