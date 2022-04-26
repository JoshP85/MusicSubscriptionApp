using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace MusicSubscriptionApp.Data
{

    public class CreateTables
    {
        public static async Task CreateMusicTableAsync(IAmazonDynamoDB client)
        {
            string tableName = "music";

            try
            {
                var response = await client.CreateTableAsync(new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition>()
                              {
                                  new AttributeDefinition
                                  {
                                      AttributeName = "Artist",
                                      AttributeType = "S"
                                  },
                                  new AttributeDefinition
                                  {
                                      AttributeName = "Title",
                                      AttributeType = "S"
                                  }
                              },
                    KeySchema = new List<KeySchemaElement>()
                              {
                                  new KeySchemaElement
                                  {
                                      AttributeName = "Artist",
                                      KeyType = "HASH"
                                  },
                                  new KeySchemaElement
                                  {
                                      AttributeName = "Title",
                                      KeyType = "RANGE"
                                  }
                              },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                });

                var tableDescription = response.TableDescription;

                string status = tableDescription.TableStatus;

                // Wait until table is created.
                while (status != "ACTIVE")
                {
                    Thread.Sleep(1000);
                    try
                    {
                        var res = await client.DescribeTableAsync(new DescribeTableRequest
                        {
                            TableName = tableName
                        });

                        status = res.Table.TableStatus;
                    }
                    // Try-catch to handle potential eventual-consistency issue.
                    catch (ResourceNotFoundException)
                    { }
                }
            }
            // Try-catch to handle table name already existing.
            catch (ResourceInUseException)
            {
                return;
            }
            await SeedData.SeedMusicTable(client);
        }

        public static async Task CreateLoginTableAsync(IAmazonDynamoDB client)
        {
            string tableName = "login";

            try
            {
                var response = await client.CreateTableAsync(new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = new List<AttributeDefinition>()
                              {
                                  new AttributeDefinition
                                  {
                                      AttributeName = "Email",
                                      AttributeType = "S"
                                  },
                              },
                    KeySchema = new List<KeySchemaElement>()
                              {
                                  new KeySchemaElement
                                  {
                                      AttributeName = "Email",
                                      KeyType = "HASH"
                                  },
                              },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 5
                    }
                });

                var tableDescription = response.TableDescription;

                string status = tableDescription.TableStatus;

                // Wait until table is created.
                while (status != "ACTIVE")
                {
                    Thread.Sleep(1000);
                    try
                    {
                        var res = await client.DescribeTableAsync(new DescribeTableRequest
                        {
                            TableName = tableName
                        });

                        status = res.Table.TableStatus;
                    }
                    // Try-catch to handle potential eventual-consistency issue.
                    catch (ResourceNotFoundException)
                    { }
                }
            }
            // Try-catch to handle table name already existing.
            catch (ResourceInUseException)
            {
                return;
            }
            await SeedData.SeedLoginTable(client);
        }
    }
}
