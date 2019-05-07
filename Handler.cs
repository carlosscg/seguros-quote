using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Threading.Tasks;

[assembly:LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AwsDotnetCsharp
{
    public class Handler
    {
       public Response Hello(Request request)
       {
         var message = request.Key1 ?? $"Test at {DateTime.UtcNow.ToLongDateString()}";
         request.Key1 = message;
         var arn = "arn:aws:sns:us-east-1:150135223216:topicCSHarp";
         var publishRequest = new PublishRequest(arn, message);
         
         publishRequest.MessageAttributes
          .Add("ProviderName",new MessageAttributeValue(){ DataType = "String",StringValue = "Hotelbeds"  });

         
        //  var xx =  await snsClient.PublishAsync(publishRequest);
        var response =  SendSNS(publishRequest);
         System.Console.WriteLine("MessageId: ", response.Result.MessageId);

          return new Response("MessageId: " + response.Result.MessageId, request);
       }

       public async Task<PublishResponse> SendSNS(PublishRequest request) {
         var snsClient =  new AmazonSimpleNotificationServiceClient();
         var response = await snsClient.PublishAsync(request);
         Console.WriteLine("response " + response.MessageId);
         return response;
      }
    }

      

    public class Response
    {
      public string Message {get; set;}
      public Request Request {get; set;}

      public Response(string message, Request request){
        Message = message;
        Request = request;
      }
    }

    public class Request
    {
      public string Key1 {get; set;}
      public string Key2 {get; set;}
      public string Key3 {get; set;}

      public Request(string key1, string key2, string key3){
        Key1 = key1;
        Key2 = key2;
        Key3 = key3;
      }
    }
}
