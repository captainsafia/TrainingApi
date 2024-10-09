# TrainingApi

TrainingApi is a demo application showcasing the new features in minimal APIs for .NET 9.

## Run Application

To run the API, navigate to the `TrainingApi` directory and execute `dotnet run`.

```
$ cd TrainingApi
$ dotnet run
Building...
info: Microsoft.EntityFrameworkCore.Update[30100]
      Saved 7 entities to in-memory store.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5198
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /Users/captainsafia/github.com/TrainingApi/TrainingApi
```

Navigate to http://localhost:5198/ to view the Scalar UI for interacting with the application.

### Run Tests

Unit and integration tests are located under the `Tests` subdirectory.

```
$ cd Tests
$ dotnet test
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)