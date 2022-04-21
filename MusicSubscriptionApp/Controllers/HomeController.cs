using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using MusicSubscriptionApp.Models;
using System.Diagnostics;

namespace MusicSubscriptionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDynamoDBContext dynamoDBContext;
        private readonly IAmazonDynamoDB client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IDynamoDBContext dynamoDBContext, IAmazonDynamoDB client, ILogger<HomeController> logger)
        {
            this.dynamoDBContext = dynamoDBContext;
            this.client = client;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //AmazonDynamoDBClient client = dynamoDBContext.;
            string tableName = "music";

            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
              {
                new AttributeDefinition
                {
                  AttributeName = "Artist",
                  AttributeType = "S"
                }
              },
                KeySchema = new List<KeySchemaElement>()
              {
                new KeySchemaElement
                {
                  AttributeName = "Id",
                  KeyType = "HASH"  //Partition key
                }
              },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 10,
                    WriteCapacityUnits = 5
                }
            };

            var response = client.CreateTableAsync(request);
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}