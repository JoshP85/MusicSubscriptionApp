using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace MusicSubscriptionApp.Models
{
    public class Query
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Year { get; set; }
        public string ImgUrl { get; set; }
        public string WebUrl { get; set; }
        public string SongID { get; set; }

        public static async Task<List<Query>> CreateQueryFromInputAsync(IAmazonDynamoDB client, Query newQuery)
        {
            ScanRequest request;
            ScanResponse response;
            string expression;
            string value;
            string value2;
            string yearValue;

            // If all fields are passed in, year is made null
            // because having artist and title is unique enough
            // with the current data in the DB
            if (newQuery.Artist != null && newQuery.Title != null)
                newQuery.Year = null;

            if (newQuery.Artist != null)
            {
                expression = "Artist = :val";
                value = newQuery.Artist;

                if (newQuery.Title != null)
                {
                    expression += " AND Title = :val2";
                    value2 = newQuery.Title;
                    request = QueryTwoFields(value, value2, expression);
                    response = await client.ScanAsync(request);
                    return createResultList(response);
                }
                else if (newQuery.Year != null)
                {
                    expression += " AND #y = :val2";
                    yearValue = newQuery.Year;
                    request = QueryOneFieldWithYear(value, yearValue, expression);
                    response = await client.ScanAsync(request);
                    return createResultList(response);
                }
                request = QueryOneField(value, expression);
                response = await client.ScanAsync(request);
                return createResultList(response);
            }

            if (newQuery.Title != null)
            {
                expression = "Title = :val";
                value = newQuery.Title;

                if (newQuery.Year != null)
                {
                    expression += " AND #y = :val2";
                    yearValue = newQuery.Year;
                    request = QueryOneFieldWithYear(value, yearValue, expression);
                    response = await client.ScanAsync(request);
                    return createResultList(response);
                }
                request = QueryOneField(value, expression);
                response = await client.ScanAsync(request);
                return createResultList(response);
            }

            if (newQuery.Year != null)
            {
                expression = "#y = :val";
                yearValue = newQuery.Year;

                request = QueryYear(yearValue, expression);

                response = await client.ScanAsync(request);

                return createResultList(response);
            }

            return null;
        }

        public static List<Query> createResultList(ScanResponse response)
        {
            var items = response.Items;
            var resultList = new List<Query>();
            foreach (Dictionary<string, AttributeValue> item in items)
            {
                Query result = new Query()
                {
                    Artist = item["Artist"].S.ToString(),
                    Title = item["Title"].S.ToString(),
                    Year = item["Year"].S.ToString(),
                    ImgUrl = item["img_url"].S.ToString(),
                    WebUrl = item["web_url"].S.ToString(),
                    SongID = item["SongID"].S.ToString(),

                };
                resultList.Add(result);
            }
            return resultList;
        }

        public static ScanRequest QueryOneField(string value, string expression)
        {
            var request = new ScanRequest
            {
                TableName = "music",
                // Optional parameters.
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue { S = value }}
                },
                FilterExpression = expression,
            };
            return request;
        }
        public static ScanRequest QueryTwoFields(string value, string value2, string expression)
        {
            var request = new ScanRequest
            {
                TableName = "music",
                // Optional parameters.
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue { S = value }},
                    {":val2", new AttributeValue { S = value2 }}
                },
                FilterExpression = expression,
            };
            return request;
        }
        public static ScanRequest QueryOneFieldWithYear(string value, string yearValue, string expression)
        {
            var request = new ScanRequest
            {
                TableName = "music",
                // Optional parameters.
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue { S = value }},
                    {":val2", new AttributeValue { S = yearValue }}
                },
                FilterExpression = expression,
                ExpressionAttributeNames = new Dictionary<string, string> {
                    { "#y" , "Year"} },
            };
            return request;
        }
        public static ScanRequest QueryYear(string yearValue, string expression)
        {
            var request = new ScanRequest
            {
                TableName = "music",
                // Optional parameters.
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":val", new AttributeValue { S = yearValue }}
                },
                FilterExpression = expression,
                ExpressionAttributeNames = new Dictionary<string, string> {
                    { "#y" , "Year"} },
            };
            return request;
        }
    }


}
