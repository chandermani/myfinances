# My Finances
Personal Finance API that integrates with you bank

# Install and run
- Build using .Net Core 2.2 this requires .Net Core 2.2 to be installed.
- Unzip the file. Navigate the `myfinances` folder
- run `dotnet restore`
- run `dotnet test`
- There are three end to end tests that verify the API endpoints using an in memory host
- Else from command line
- run dotnet run -p MyFinances.Api\MyFinances.Api.csproj
- The API endpoints are available at http://localhost:5000 and http://localhost:5001
- To establish a connection
  - From TrueLayer build an auth link and add redirect url http://localhost:5000/integration/truelayer/callback.
  - Also since we do not allow registering new user. The way to associate this redirect to existing user is to pass user identifier (currently email) in state querystring such as `&state=john@doe.com`.
  - Example url https://auth.truelayer-sandbox.com/?response_type=code&client_id=sandbox-chandermani-982283&scope=info%20accounts%20balance%20cards%20transactions%20direct_debits%20standing_orders%20offline_access&redirect_uri=http://localhost:5000/integration/truelayer/callback&providers=uk-ob-all%20uk-oauth-all%20uk-cs-mock&state=john@doe.com
  - The users currently in the system are john@doe.com, john1@doe1.com and john2@doe2.com
- Once a connection is established you can import transaction using endpoint http://localhost:5000/users/{useridentifier}/import
  - Use `john@doe.com` for `useridentifier` here too
- The endpoint to get summary is http://localhost:5000/users/{useridentifier}/transactions/categorysummary. This method can import transactions if the connection is already establed. 
  - Use user `john@doe.com`

# Implementation Notes
This implementation is inspired from [Hexagonal Architecture](https://en.wikipedia.org/wiki/Hexagonal_architecture_(software))
- MyFinance.Core - Is the core business domain
- MyFinance.DataStore - Library to manage internal/memory storage
- MyFinance.Integration.TrueLayer - All true layer integration go here.
- MyFinance.Api - Api endpoint
- MyFinance.Core.Tests - Unit tests for core.
- MyFinance.Tests.E2E - End to End tests for the application.

All components depend upon Core.

This implemenation currently lacks a number of features including
- Error/Failure handling
- Handling negative flow
- Managing performance

And hence is incomplete. 

Some of the major areas that need improvement are:
- Overall timezone handled is missing.
- Multi currency handling is not there
- There is a batch method on the TrueLayer API, that i have not explored.
- Tests only cover a limited scenarios, more test coverage is required.
