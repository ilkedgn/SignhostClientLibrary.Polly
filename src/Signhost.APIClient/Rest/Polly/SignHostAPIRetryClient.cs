using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Signhost.APIClient.Rest.DataObjects;
using Signhost.APIClient.Rest.ErrorHandling;

namespace Signhost.APIClient.Rest.Polly
{
	/// <summary>
	/// Implements the <see cref="ISignHostApiClient"/> interface which provides
	/// a signhost api client implementation.
	/// </summary>
	public class SignHostApiRetryClient
		: ISignHostApiClient
		, IDisposable
	{
		private readonly SignHostApiClient client;
		private readonly AsyncPolicy retryPolicy;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignHostApiRetryClient"/> class.
		/// This client has a built-in retry mechanism.
		/// Set your usertoken and APPKey by creating a <see cref="SignHostApiClientSettings"/>.
		/// </summary>
		/// <param name="settings"><see cref="SignHostApiClientSettings"/>.</param>
		/// <param name="policy">An <see cref="AsyncPolicy"/> to use instead of the default.</param>
		public SignHostApiRetryClient(
			ISignHostApiClientSettings settings,
			AsyncPolicy policy = null)
			: this(settings, new HttpClient(), policy)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SignHostApiRetryClient"/> class.
		/// This client has a built-in retry mechanism.
		/// Set your usertoken and APPKey by creating a <see cref="SignHostApiClientSettings"/>.
		/// </summary>
		/// <param name="settings"><see cref="SignHostApiClientSettings"/>.</param>
		/// <param name="httpClient"><see cref="HttpClient"/> to use for all http calls.</param>
		/// <param name="policy">An <see cref="AsyncPolicy"/> to use instead of the default.</param>
		public SignHostApiRetryClient(
			ISignHostApiClientSettings settings,
			HttpClient httpClient,
			AsyncPolicy policy = null)
		{
			client = new SignHostApiClient(settings, httpClient);

			if (policy is null) {
				retryPolicy = GetDefaultPolicy();
			}
			else {
				retryPolicy = policy;
			}
		}

		/// <inheritdoc />
		public async Task AddOrReplaceFileMetaToTransactionAsync(
			FileMeta fileMeta,
			string transactionId,
			string fileId)
			=> await AddOrReplaceFileMetaToTransactionAsync(
				fileMeta,
				transactionId,
				fileId,
				default);

		/// <inheritdoc />
		public async Task AddOrReplaceFileMetaToTransactionAsync(
			FileMeta fileMeta,
			string transactionId,
			string fileId,
			CancellationToken cancellationToken = default)
		{
			await retryPolicy
				.ExecuteAsync(
					ct => client.AddOrReplaceFileMetaToTransactionAsync(
						fileMeta,
						transactionId,
						fileId,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task AddOrReplaceFileToTransactionAsync(
			Stream fileStream,
			string transactionId,
			string fileId,
			FileUploadOptions uploadOptions)
			=> await AddOrReplaceFileToTransactionAsync(
				fileStream,
				transactionId,
				fileId,
				uploadOptions,
				default);

		/// <inheritdoc />
		public async Task AddOrReplaceFileToTransactionAsync(
			Stream fileStream,
			string transactionId,
			string fileId,
			FileUploadOptions uploadOptions,
			CancellationToken cancellationToken = default)
		{
			await retryPolicy
				.ExecuteAsync(
					ct => client.AddOrReplaceFileToTransactionAsync(
						fileStream,
						transactionId,
						fileId,
						uploadOptions,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task AddOrReplaceFileToTransactionAsync(
			string filePath,
			string transactionId,
			string fileId,
			FileUploadOptions uploadOptions)
			=> await AddOrReplaceFileToTransactionAsync(
				filePath,
				transactionId,
				fileId,
				uploadOptions,
				default);

		/// <inheritdoc />
		public async Task AddOrReplaceFileToTransactionAsync(
			string filePath,
			string transactionId,
			string fileId,
			FileUploadOptions uploadOptions,
			CancellationToken cancellationToken = default)
		{
			await retryPolicy
				.ExecuteAsync(
					ct => client.AddOrReplaceFileToTransactionAsync(
						filePath,
						transactionId,
						fileId,
						uploadOptions,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Transaction> CreateTransactionAsync(
			Transaction transaction)
			=> await CreateTransactionAsync(transaction, default);

		/// <inheritdoc />
		public async Task<Transaction> CreateTransactionAsync(
			Transaction transaction,
			CancellationToken cancellationToken = default)
		{
			return await retryPolicy
				.ExecuteAsync(
					ct => client.CreateTransactionAsync(
						transaction,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task DeleteTransactionAsync(
			string transactionId,
			CancellationToken cancellationToken = default)
			=> await DeleteTransactionAsync(
				transactionId,
				default,
				cancellationToken);

		/// <inheritdoc />
		public async Task DeleteTransactionAsync(
			string transactionId,
			DeleteTransactionOptions options)
			=> await DeleteTransactionAsync(
				transactionId,
				options,
				default);

		/// <inheritdoc />
		public async Task DeleteTransactionAsync(
			string transactionId,
			DeleteTransactionOptions options = null,
			CancellationToken cancellationToken = default)
		{
			await retryPolicy
				.ExecuteAsync(
					ct => client.DeleteTransactionAsync(
						transactionId,
						options,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Stream> GetDocumentAsync(
			string transactionId,
			string fileId)
			=> await GetDocumentAsync(transactionId, fileId, default);

		/// <inheritdoc />
		public async Task<Stream> GetDocumentAsync(
			string transactionId,
			string fileId,
			CancellationToken cancellationToken = default)
		{
			return await retryPolicy
				.ExecuteAsync(
					ct => client.GetDocumentAsync(
						transactionId,
						fileId,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Stream> GetReceiptAsync(string transactionId)
			=> await GetReceiptAsync(transactionId, default);

		/// <inheritdoc />
		public async Task<Stream> GetReceiptAsync(
			string transactionId,
			CancellationToken cancellationToken = default)
		{
			return await retryPolicy
				.ExecuteAsync(
					ct => client.GetReceiptAsync(
						transactionId,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task<Transaction> GetTransactionAsync(string transactionId)
			=> await GetTransactionAsync(transactionId, default);

		/// <inheritdoc />
		public async Task<Transaction> GetTransactionAsync(
			string transactionId,
			CancellationToken cancellationToken = default)
		{
			return await retryPolicy
				.ExecuteAsync(
					ct => client.GetTransactionAsync(
						transactionId,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task<ApiResponse<Transaction>> GetTransactionResponseAsync(
			string transactionId)
			=> await GetTransactionResponseAsync(transactionId, default);

		/// <inheritdoc />
		public async Task<ApiResponse<Transaction>> GetTransactionResponseAsync(
			string transactionId,
			CancellationToken cancellationToken = default)
		{
			return await retryPolicy
				.ExecuteAsync(
					ct => client.GetTransactionResponseAsync(
						transactionId,
						ct),
					cancellationToken);
		}

		/// <inheritdoc />
		public async Task StartTransactionAsync(
			string transactionId)
			=> await StartTransactionAsync(transactionId, default);

		/// <inheritdoc />
		public async Task StartTransactionAsync(
			string transactionId,
			CancellationToken cancellationToken = default)
		{
			await retryPolicy
				.ExecuteAsync(
					ct => client.StartTransactionAsync(
						transactionId,
						ct),
					cancellationToken);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the instance.
		/// </summary>
		/// <param name="disposing">Is <see cref="Dispose"/> callled.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing) {
				client?.Dispose();
			}
		}

		private static AsyncPolicy GetDefaultPolicy()
		{
			return Policy
				.Handle<SignhostRestApiClientException>(ex =>
					!(ex is BadAuthorizationException) &&
					!(ex is BadRequestException) &&
					!(ex is NotFoundException))

				// When an HttpClient times out it doesn't throw a TimeoutException like you'd expect.
				// Instead it throws a TaskCanceledException, that's why we check the cancellation token.
				.Or<TaskCanceledException>(ex => !ex.CancellationToken.IsCancellationRequested)
				.WaitAndRetryAsync(
					3,
					retryAttempt =>
						TimeSpan.FromMilliseconds(Math.Pow(10, retryAttempt)));
		}
	}
}
