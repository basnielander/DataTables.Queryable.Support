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

Where 'ToDoModel' is a model from, for example, a domain layer and 'ToDoViewModel' the viewmodel from the UI layer. 
requestProcessor.CreateResponse() takes two parameters, the Datatables request and a delegate for the method that returns an IQueryable\<ToDoModel> object.
The CreateResponse() takes care of running the delegate and translating the DataTables request (search, column filters and sorting) from ViewModel Expressions to Domain model Expressions to the appropriate Where().OrderBy().Skip().Take() query.

# Customizations
QueryablesProcessor class by default only needs an IMapper instance. For customizing the way DataTables search and/or column filters are translated to Where() Expressions it is possible to define your own, so called, PropertyExpressionCreators. 
A PropertyExpressionCreator (class derived from IPropertyExpressionCreator) translates a Search and/or ColumnFilter datatables query (ISearch) to an Lambda Expression.
For example, the StringContainsExpressionCreator class translates the query 'Test' to item.property.Contains("Test").