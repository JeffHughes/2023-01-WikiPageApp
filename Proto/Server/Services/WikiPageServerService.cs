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
    public class WikiPageServerService : WikiPageServiceProto.WikiPageServiceProtoBase, IWikiPageServerService
    {
        private readonly WikiPageDataContext _context;
        public WikiPageServerService(IConfiguration configuration)
        {
            var objName = nameof(WikiPage);
            var (connectionString, EFProvider) = ConfigUtils.GetConfig(objName);
            _context = new WikiPageDataContext(connectionString, EFProvider);
        }

        public async override Task<GetWikiPageByIdResponse> GetWikiPage(GetWikiPageByIdRequest request, ServerCallContext context)
        {
            var id = Guid.Parse(request.Id);

            var result = await _context.WikiPage.Where(entity => entity.WikiPageID == id).FirstOrDefaultAsync();
            var response = new GetWikiPageByIdResponse { Data = new WikiPage() };
            await MapEntityToProto(response.Data, result);
            return response;
        }

        private async Task MapEntityToProto(WikiPage proto, Model.WikiPage entity)
        {
            if (null == entity) return;
            TinyMapper.Bind<Model.WikiPage, Proto.Library.WikiPage>();            
            proto = TinyMapper.Map(entity, proto);
        
           

        }

        private async Task MapProtoToEntity(WikiPage proto, Model.WikiPage entity)
        {
            TinyMapper.Bind<Proto.Library.WikiPage, Model.WikiPage>();
            entity = TinyMapper.Map(proto, entity);            
               
                  }

        private async Task<Google.Protobuf.WellKnownTypes.Timestamp> ToProtoTimestamp(DateTime prop)
        {
            var dtkPropertyDateTime = DateTime.SpecifyKind(prop, DateTimeKind.Utc);
            return Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dtkPropertyDateTime);
        }



        
     

        public override async Task<GetAllWikiPageResponse> GetAllWikiPage(GetAllWikiPageRequest request, ServerCallContext context)
        {
            var maxTake = 200;
            var result = await _context.WikiPage.TakeLast(maxTake).ToListAsync();
            var response = new GetAllWikiPageResponse();
            WikiPage proto = null;
            result.ForEach(async entity =>
            {
                proto = new WikiPage();
                await MapEntityToProto(proto, entity);
                response.Data.Add(proto);
            });

            return response;
        }

        public override async Task<PutWikiPageResponse> PutWikiPage(PutWikiPageRequest request, ServerCallContext context)
        {
            try
            {
                var inputEntity = new Model.WikiPage();
                Model.WikiPage outputEntity;

                await MapProtoToEntity(request.Data, inputEntity);
                
                var idGuid = Guid.Parse(request.Id);                
                var existing = await _context.WikiPage.FindAsync(idGuid);

                if (existing == null) _context.WikiPage.Add(inputEntity);
                else
                {
                    _context.WikiPage.Remove(existing);
                    _context.WikiPage.Add(inputEntity);
                }
                
                await _context.SaveChangesAsync();
                outputEntity = inputEntity;
                var outputProto = new WikiPage();
                await MapEntityToProto(outputProto, outputEntity);

                var response = new PutWikiPageResponse { Data = outputProto, Id = outputProto.WikiPageID };
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}