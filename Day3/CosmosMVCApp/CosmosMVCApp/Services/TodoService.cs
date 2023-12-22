using CosmosMVCApp.Models;
using Microsoft.Azure.Cosmos;

namespace CosmosMVCApp.Services
{
	public class TodoService
	{

		private readonly Microsoft.Azure.Cosmos.Container _container;
		private readonly string _containerName;
		public TodoService(string connectionString, string database, string containerName)
		{
			_container = new CosmosClient(connectionString)
				.GetDatabase(database)
				.GetContainer(containerName);
			_containerName = containerName;
		}

		public async Task<List<TodoVM>> GetTodosAsync()
		{
			string query = $@"SELECT * FROM {_containerName}";

			var iterator = _container.GetItemQueryIterator<TodoVM>(query);

			List<TodoVM> listOfTodos = new List<TodoVM>();
			while (iterator.HasMoreResults)
			{
				var next = await iterator.ReadNextAsync();
				listOfTodos.AddRange(next);
			}

			return listOfTodos;
		}

		public async Task<bool> AddTodo(TodoVM model)
		{

			model.Id = Guid.NewGuid().ToString();
			var itemResponse = await _container.CreateItemAsync<TodoVM>(model, new PartitionKey(model.Priority));
			if (itemResponse.StatusCode == System.Net.HttpStatusCode.Created)
			{
				return true;
			}
			else
			{

				return false;
			}

		}
	}
}
