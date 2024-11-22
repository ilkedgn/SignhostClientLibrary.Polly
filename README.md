# SignhostClientLibrary.Polly

This repository contains the implementation for the Signhost API Retry Client. It provides functionality for interacting with the Signhost API, with automatic retrying on failures using a custom retry policy.

### Install
Get it on NuGet:

`PM> Install-Package SignhostClientLibrary.Polly`

### Example code
```c#
var settings = new SignHostApiClientSettings("YourAppKey");

var retryClient = new SignHostApiRetryClient(settings);

var response = await retryClient.CreateTransactionAsync(new Transaction());

var documentFile = new MemoryStream();
var fileUploadOptions = new FileUploadOptions();
var uploadResponse = await retryClient.AddOrReplaceFileToTransactionAsync(
    documentFile, "TransactionId", "FileId", fileUploadOptions
);

var transactionDetails = await retryClient.GetTransactionAsync("TransactionId");

```