# fanduel-depthchart

## Depth Chart code implementation

This repository includes a very basic solution for the depth chart code challenge implementation.

The present implementation does not include database persistence functionality but stores data in a file. This solution is built in the form of a REST API that exposes four endpoints to cover the requirements.

The codebase consists of two projects, one for the web API and another one for unit tests with xUnit.


### How to scale

In order to make this implementation more scalable it would be required to implement a more robust storage mechanism, for example, a document database. A relational database could also be used but a document database would be preferable inthis case given the nature of the requirements in which the solution implies moving the order of persistent lists of players constantly which would suit better the purpose of a no-sql document database like MongoDb or similar. This way, all the teams of the NFL and many more sports could be easily supported. 


### How to run
From the `DepthChartBackend` directory run:

```$ dotnet run```

When the project runs it will display the base URL of the API endpoints, ie: `http://localhost:PORT`. You can access the Swagger documentation from the browser at `http://localhost:PORT/swagger` from which the endpoints can be tested.

### How to run unit tests

In order to run the test cases you need to run:

```$ dotnet test```

from the root directory of the project.
