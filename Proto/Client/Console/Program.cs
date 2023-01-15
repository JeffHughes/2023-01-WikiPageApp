





// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using WikiPageApp.Proto.Client.Services.WikiPage;
            
try
    {
    AppContext.SetSwitch(
        "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        // The port number(5000) must match the port of the gRPC server.
        using var channel = GrpcChannel.ForAddress("https://localhost:5001");
        var dtkPropertyDateTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
        var currentDateTIme = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dtkPropertyDateTime);
        var clientService = new WikiPageClientService();
        //var response = await clientService.GetAllWikiPage(channel, null);
        //var response = await clientService.GetWikiPage(channel, Guid.Parse("106FA7D7-16C1-4754-92F1-59FA6C83B851"));
                
        var response = await clientService.PutWikiPage(channel, new WikiPageApp.Proto.Library.WikiPage
        {
            WikiPageID = Guid.NewGuid().ToString(),            
            EntityDefinitionType = 1,
            WikiPageIsSoftDeleted = false,
            // rest of the property values go here...the required values must be set otherwise the proto to c# mapper will throw an exception
            // TODO: Decimal DataType implementation - currently decimals are implemented as string
            //date properties initialization
              
            //string properties initialization
              
           //numeric string properties initialization
              

        //numeric properties initialization
              
        WikiPageIDint = randomInt(),
        
         //boolean properties initialization
              
        //guid properties initialization
              

            WikiPageUpdatedByResourceID = Guid.NewGuid().ToString(),
            WikiPageUpdatedTimestampUTC = currentDateTIme
        });

        System.Console.WriteLine("Response: " + response.StatusMessage);
   }
catch (Exception ex)
   {
        System.Console.WriteLine(ex.ToString());
   }

System.Console.WriteLine("Press any key to exit...");
System.Console.ReadKey();


      static DateTime Next(DateTime start, DateTime? end = null)
        {
            Random random = new Random();
            end ??= new DateTime();
            int range = (end.Value - start).Days;
            return start.AddDays(random.Next(range));
        }

        static Google.Protobuf.WellKnownTypes.Timestamp RandomProtoTimeStamp()
        {
            var randomDateTime = Next(DateTime.Now, DateTime.Now.AddDays(10));
            var dtkPropertyDateTime = DateTime.SpecifyKind(randomDateTime, DateTimeKind.Utc);
            var propertyDateTime = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(dtkPropertyDateTime);
            return propertyDateTime;
        }
        
        static long randomLong()
        {
            Random random = new Random();
            byte[] bytes = new byte[8];
            random.NextBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

       static int randomInt()
        {
            Random random = new Random();
            byte[] bytes = new byte[8];
            random.NextBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
