# dotnet-sandbox

This is a personal playground, IE: Sandbox, to test and prototype code in an operating environment.  This is not meant to be production level code, but if some of the ideas in here, examples, or techniques; then use them as you wish!  

# Setup
#### Clone the repository:
``` 
git clone git@github.com:bsampica/dotnet-sandbox.git     
```
#### or
``` 
git clone https://github.com/bsampica/dotnet-sandbox.git
``` 

## Install the dependencies

``` 
dotnet restore
```

``` 
dotnet run
```

## URL Access
```
http://localhost:5000/swagger
```
```
http://localhost:5000/hangfire
```


This project depends on:
- Microsoft WebAPI
- .NET 8.0
- C# 12.0 with Advanced Features enabled
- git for windows (command line)
- HangFire
- Hangfire.Core
- Hangfire.InMemoryStorage

##### Note, these dependencies should install with dotnet restore.

## Devleopment
Its being developed in **[ Visual Studio Code ]**, and I've tested it with **[ Visual Studio 2022 ]**, and **[ JetBrains Rider ]**.

