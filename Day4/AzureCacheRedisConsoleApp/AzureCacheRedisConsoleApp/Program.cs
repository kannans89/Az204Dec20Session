using StackExchange.Redis;

string connectionString = "day3.redis.cache.windows.net:6380,password=gy7TWdao9r0jrqOghasWymElIHzkEcJiYAzCaNfKSi4=,ssl=True,abortConnect=False";

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);

//SetCacheData();
GetCacheData();

void SetCacheData()
{
    IDatabase database = redis.GetDatabase();

    database.StringSet("message1", "Hello kannan");

    Console.WriteLine("Cache data set");
}

void GetCacheData()
{
    IDatabase database = redis.GetDatabase();
    if (database.KeyExists("message1"))
        Console.WriteLine(database.StringGet("message1"));
    else
        Console.WriteLine("key does not exist");

}