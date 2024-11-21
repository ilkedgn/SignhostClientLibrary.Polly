# SignhostClientLibrary.Polly
[![join gitter chat](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Evidos/signhost-api)
[![Build status](https://ci.appveyor.com/api/projects/status/696lddgivr6kkhsd/branch/master?svg=true)](https://ci.appveyor.com/project/MrJoe/signhostclientlibrary-xcr5f/branch/master)
[![Nuget package](https://img.shields.io/nuget/v/EntrustSignhostClientLibrary.svg)](https://www.nuget.org/Packages/EntrustSignhostClientLibrary)

This repository contains the implementation for the Signhost API Retry Client. It provides functionality for interacting with the Signhost API, with automatic retrying on failures using a custom retry policy.

### Install
Get it on NuGet:

`PM> Install-Package SignhostClientLibrary.Polly`

### Example code
```c#
var settings = new SignHostApiClientSettings("YourAppKey")
{
    Endpoint = "http://localhost/api/"
};

var retryClient = new SignHostApiRetryClient(
    settings,
    new HttpClient(new HttpClientHandler())
);

var response = await retryClient.CreateTransactionAsync(new Transaction());

var documentFile = new MemoryStream();
var fileUploadOptions = new FileUploadOptions();
var uploadResponse = await retryClient.AddOrReplaceFileToTransactionAsync(
    documentFile, "TransactionId", "FileId", fileUploadOptions
);

var transactionDetails = await retryClient.GetTransactionAsync("TransactionId");

```