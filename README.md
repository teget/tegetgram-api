# tegetgram-api
tegetgram-api is a simple api that lets messaging between users.

### Setup

This project uses NuGet to manage external dependencies. To install them simply run

```sh
cd Tegetgram
dotnet restore
```

As data storage, this is set up to use an SQL Server. Plug in your favorite connection string into `Tegetgram.Api\appsettings.json` to configure the server you would like to utilize.

After that, run migrations using
```sh
dotnet ef database update --project Tegetgram.Data/ --startup-project Tegetgram.Api/
```

Now you are ready to roll. Simply execute the following to run the API
```sh
dotnet run --project Tegetgram.Api/
```

**Have Fun!**
