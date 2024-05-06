# SignhostClientLibrary.Polly
[![join gitter chat](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/Evidos/signhost-api)
[![Build status](https://ci.appveyor.com/api/projects/status/696lddgivr6kkhsd/branch/master?svg=true)](https://ci.appveyor.com/project/generateui/signhostclientlibrary-polly)
[![Nuget package](https://img.shields.io/nuget/v/EntrustSignhostClientLibrary.svg)](https://www.nuget.org/Packages/EntrustSignhostClientLibrary.Polly)

This is a client library in c# to demonstrate the usage of the [Signhost API](https://api.signhost.com/) using .NET. You will need a valid APPKey. You can request an APPKey [here](https://portal.signhost.com/signup/api-aanvraag).

### Install
Get it on NuGet:

`PM> Install-Package EntrustSignhostClientLibrary.Polly`

### Example code
The following code is an example of how to create SignHostApiRetryClient and call the GetDocumentAsync.
First you need a EntrsutSignhostClientLibrary package to use SignHostApiClientSettings and other interfaces such as Transaction or Signer. After configuring these settings, you can begin using the EntrustSignhostClientLibrary with a retry mechanism and bearer token. You need to have HttpClient and HttpContextAccessor objects to use this example.
```c#

		SignHostApiClientSettings settings = new("APPKey") {
			Endpoint = "http://localhost/api/",
		};

		var context = httpContextAccessor.HttpContext;

		string? accessToken = await context.GetUserAccessTokenAsync(
			cancellationToken: cancellationToken);

		if (!string.IsNullOrEmpty(accessToken)) {
			httpClient.SetBearerToken(accessToken);
		}

		var client = SignHostApiRetryClient(settings, httpClient);

		client.GetDocumentAsync(
			"Transaction Id",
			"File Id");

```

