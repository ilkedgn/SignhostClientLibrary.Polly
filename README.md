# SignhostClientLibrary.Polly
[![Nuget package](https://img.shields.io/nuget/v/EntrustSignhostClientLibrary.svg)](https://www.nuget.org/Packages/EntrustSignhostClientLibrary)

This repository contains the implementation for the Signhost API Retry Client. It provides functionality for interacting with the Signhost API, with automatic retrying on failures using a custom retry policy.

### Install
Get it on NuGet:

`PM> Install-Package SignhostClientLibrary.Polly`

### Example code

```c#
For detailed examples, visit: [https://github.com/Evidos/SignhostClientLibrary/blob/master/README.md](https://github.com/Evidos/SignhostClientLibrary/blob/master/README.md)
var settings = new  SignHostApiClientSettings("AppName appkey", "apikey or usertoken");

var retryClient = new SignHostApiRetryClient(settings);

```