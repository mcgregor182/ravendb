using System.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;
using Xunit;

namespace Raven.Tests.MailingList
{
	public class Dummy
	{
		public string Id { get; set; }
		public string Name { get; set; }
	}

	public class AsyncTest : RavenTest
	{
		[Fact]
		public void SyncQuery()
		{
			using(GetNewServer())
			using(var store = new DocumentStore
			{
				Url = "http://localhost:8079"
			}.Initialize())
			using (var session = store.OpenSession())
			{
				var results = session.Query<Dummy>().ToList();
				Assert.Equal(0, results.Count);
				results = session.Query<Dummy>().ToList();
				Assert.Equal(0, results.Count);
			}
		}

		[Fact]
		public void AsyncQuery()
		{
			using (GetNewServer())
			using (var store = new DocumentStore
			{
				Url = "http://localhost:8079"
			}.Initialize())
			using (var session = store.OpenAsyncSession())
			{
				var results = session.Query<Dummy>().ToListAsync();
				results.Wait();
				Assert.Equal(0, results.Result.Count);
				var results2 = session.Query<Dummy>().ToListAsync();
				results2.Wait();
				Assert.Equal(0, results2.Result.Count);
			}
		}
	}
}