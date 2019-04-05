# DataTables.Queryable.Support
Package offers support for processing DataTables requests using IQueryable Expressions. Search, filter and sorting specified in DataTables request are (automatically) translated from Viewmodel Expressions in the UI layer to Domain Expressions using this library in combination with AutoMapper.

# How to start

1. Create QueryablesProcessor object:
   1. directly in code:
```
IDataTablesRequestProcessor requestProcessor = new QueryablesProcessor();
```
        ii. or via DI:
```
public static void Register(IServiceCollection services, IConfiguration configuration)
{
    // other registrations
    services.AddScoped<IDataTablesRequestProcessor, QueryablesProcessor>();
}
```
And in a controller
```
public ...Controller(IDataTablesRequestProcessor requestProcessor, ...)
{
    this.requestProcessor = requestProcessor;
    ...
}
```
   
2. Call CreateResponse() in a DataTables request action. For example:

```
[Route("api/todo")]
public IActionResult GetToDos(IDataTablesRequest request)
{
    var response = requestProcessor.CreateResponse<ToDoModel, ToDoViewModel>(request, toDoManager.SelectToDos);
            
    return new DataTablesJsonResult(response, true);
}
```