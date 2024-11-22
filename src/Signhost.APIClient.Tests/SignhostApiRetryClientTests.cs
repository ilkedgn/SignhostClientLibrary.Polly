using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Signhost.APIClient.Rest;
using Signhost.APIClient.Rest.DataObjects;
using Signhost.APIClient.Rest.ErrorHandling;
using Signhost.APIClient.Rest.Polly;
using Xunit;

namespace SignhostAPIRetryClient.Tests
{
	/// <summary>
	/// Tests the SignHostApiRetryClient.
	/// </summary>
	public class SignhostApiRetryClientTests
		: IDisposable
	{
		private readonly SignHostApiClientSettings settings;

		private readonly SignHostApiRetryClient firstCallSucceedsClient;
		private readonly SignHostApiRetryClient thirdCallSucceedsClient;
		private readonly SignHostApiRetryClient allCallsFailClient;

		private readonly MemoryStream documentFile;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignhostApiRetryClientTests"/> class.
		/// Sets up the mocked handlers for the HttpClient.
		/// </summary>
		public SignhostApiRetryClientTests()
		{
			settings = new SignHostApiClientSettings(
				"AppKey",
				"AuthKey") {
				Endpoint = "http://localhost/api/",
			};

			firstCallSucceedsClient = new SignHostApiRetryClient(
				settings,
				new HttpClient(
					SetupMockedHandler(
						HttpStatusCode.OK,
						HttpStatusCode.InternalServerError,
						HttpStatusCode.InternalServerError,
						HttpStatusCode.InternalServerError)));

			thirdCallSucceedsClient = new SignHostApiRetryClient(
				settings,
				new HttpClient(
					SetupMockedHandler(
						HttpStatusCode.InternalServerError,
						HttpStatusCode.InternalServerError,
						HttpStatusCode.OK,
						HttpStatusCode.InternalServerError)));

			allCallsFailClient = new SignHostApiRetryClient(
				settings,
				new HttpClient(
					SetupMockedHandler(
						HttpStatusCode.InternalServerError,
						HttpStatusCode.InternalServerError,
						HttpStatusCode.InternalServerError,
						HttpStatusCode.InternalServerError)));

			documentFile = new MemoryStream();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_AddOrReplaceFileMetaToTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.AddOrReplaceFileMetaToTransactionAsync(
					new FileMeta(),
					"Transaction Id",
					"File Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_AddOrReplaceFileToTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.AddOrReplaceFileToTransactionAsync(
					documentFile,
					"Transaction Id",
					"File Id",
					new FileUploadOptions());
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_CreateTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.CreateTransactionAsync(new Transaction());
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_DeleteTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.DeleteTransactionAsync(
					"Transaction Id",
					new DeleteTransactionOptions());
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_GetDocumentAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.GetDocumentAsync(
					"Transaction Id",
					"File Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_GetReceiptAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.GetReceiptAsync("Transaction Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_GetTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.GetTransactionAsync("transaction Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the first time around.
		/// So the retry policy isn't used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_First_Call_Succeeds_GetTransactionResponseAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				firstCallSucceedsClient.GetTransactionResponseAsync("Transaction Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_AddOrReplaceFileMetaToTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = ()
				=> thirdCallSucceedsClient.AddOrReplaceFileMetaToTransactionAsync(
					new FileMeta(),
					"Transaction Id",
					"File Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_AddOrReplaceFileToTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				thirdCallSucceedsClient.AddOrReplaceFileToTransactionAsync(
					documentFile,
					"Transaction Id",
					"File Id",
					new FileUploadOptions());
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_CreateTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				thirdCallSucceedsClient.CreateTransactionAsync(new Transaction());
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_DeleteTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				thirdCallSucceedsClient.DeleteTransactionAsync(
					"Transaction Id",
					new DeleteTransactionOptions());
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_GetDocumentAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = ()	=>
				thirdCallSucceedsClient.GetDocumentAsync(
					"Transaction Id",
					"File Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_GetReceiptAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				thirdCallSucceedsClient.GetReceiptAsync("Transaction Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_GetTransactionAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				thirdCallSucceedsClient.GetTransactionAsync("transaction Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call works as expected the third time around.
		/// So the retry policy is used and no exception is thrown.
		/// </summary>
		[Fact]
		public void When_Third_Call_Succeeds_GetTransactionResponseAsync_Should_Not_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				thirdCallSucceedsClient.GetTransactionResponseAsync("Transaction Id");
			apiCall.Should().NotThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact]
		public void When_All_Calls_Fail_AddOrReplaceFileMetaToTransactionAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.AddOrReplaceFileMetaToTransactionAsync(
					new FileMeta(),
					"Transaction Id",
					"File Id");
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact]
		public void When_All_Calls_Fail_AddOrReplaceFileToTransactionAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.AddOrReplaceFileToTransactionAsync(
					new MemoryStream(),
					"Transaction Id",
					"File Id",
					new FileUploadOptions());
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact]
		public void When_All_Calls_Fail_CreateTransactionAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.CreateTransactionAsync(new Transaction());
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact]
		public void When_All_Calls_Fail_DeleteTransactionAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.DeleteTransactionAsync(
					"Transaction Id",
					new DeleteTransactionOptions());
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact(Skip = "Current method of the base library hasn't implemented SignhostRestApiClientException yet.")]
		public void When_All_Calls_Fails_GetDocumentAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.GetDocumentAsync(
					"Transaction Id",
					"File Id");
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact(Skip = "Current method of the base library hasn't implemented SignhostRestApiClientException yet.")]
		public void When_All_Calls_Fail_GetReceiptAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.GetReceiptAsync("Transaction Id");
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact]
		public void When_All_Calls_Fail_GetTransactionAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.GetTransactionAsync("transaction Id");
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <summary>
		/// Tests if the api call fails as expected the fourth time around.
		/// So the retry policy is used and a exception is thrown.
		/// </summary>
		[Fact]
		public void When_All_Calls_Fail_GetTransactionResponseAsync_Should_Throw_Exception()
		{
			Func<Task> apiCall = () =>
				allCallsFailClient.GetTransactionResponseAsync("Transaction Id");
			apiCall.Should().ThrowAsync<SignhostRestApiClientException>();
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes of the objects in the <see cref="SignhostApiRetryClientTests"/> class,
		/// that implement IDisposable.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> then the object is being disposed from a call to Dispose();
		/// <c>false</c> if it is from a finalizer / destructor.</param>
		protected virtual void Dispose(bool disposing)
		{
			firstCallSucceedsClient.Dispose();
			thirdCallSucceedsClient.Dispose();
			allCallsFailClient.Dispose();

			documentFile.Dispose();
		}

		private HttpMessageHandler SetupMockedHandler(
			HttpStatusCode firstStatus,
			HttpStatusCode secondStatus,
			HttpStatusCode thirdStatus,
			HttpStatusCode fourthStatus)
		{
			var mockHandler = new Mock<HttpMessageHandler>();
			mockHandler
				.Protected()
				.SetupSequence<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage(statusCode: firstStatus))
				.ReturnsAsync(new HttpResponseMessage(statusCode: secondStatus))
				.ReturnsAsync(new HttpResponseMessage(statusCode: thirdStatus))
				.ReturnsAsync(new HttpResponseMessage(statusCode: fourthStatus));

			return mockHandler.Object;
		}
	}
}
