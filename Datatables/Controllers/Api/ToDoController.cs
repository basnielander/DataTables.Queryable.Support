using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMapper.Extensions.ExpressionMapping;
using Datatables.Sample.Domain.Interfaces;
using Datatables.Sample.Domain.Models;
using Datatables.Sample.Web.Models;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using DataTables.Queryable.Support;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Datatables.Sample.Web.Controllers.Api
{
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IDataTablesRequestProcessor requestProcessor;
        private readonly IToDoManager toDoManager;
        private readonly IMapper mapper;

        public ToDoController(IDataTablesRequestProcessor requestProcessor, IToDoManager toDoManager, IMapper mapper)
        {
            this.requestProcessor = requestProcessor;
            this.toDoManager = toDoManager;
            this.mapper = mapper;
        }

        [Route("api/todo")]
        public IActionResult GetToDos(IDataTablesRequest request)
        {
            var response = requestProcessor.CreateResponse<ToDoModel, ToDoViewModel>(request, toDoManager.SelectToDos);
            
            return new DataTablesJsonResult(response, true);
        }        

        
    }
}