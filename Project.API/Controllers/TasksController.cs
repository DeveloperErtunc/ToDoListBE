using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.DTO;
using Project.BLL.Services;
using Project.BLL.APIModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Project.API.Controllers
{
    /* [Authorize(Roles ="User,Admin")]*/
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactor;
        public TasksController(IHttpClientFactory clientFactor)
        {
            _clientFactor = clientFactor;
        }

        /* [Authorize(Roles = "Admin")]*/
        [HttpPost]
        [Route("")]
        public Task<Boolean> Add(DTOTask dtoTask)
        {

            return ServicesTasks.Add(dtoTask, _clientFactor);
        }

        /*  [Authorize(Roles = "Admin")]*/
        [HttpPost]
        [Route("update")]
        public Task<Boolean> Update(DTOTask dtoTask)
        {
            return ServicesTasks.Update(dtoTask, _clientFactor);
        }

        [HttpGet]
        [Route("byid")]
        public Task<List<DTOTask>> GetByProjectID()
        {
            return ServicesTasks.GetByProjectID(_clientFactor);
        }

        [HttpGet]
        [Route("status/id")]
        public Task<List<DTOTask>> Status(long id)
        {
            return ServicesTasks.Status(id, _clientFactor);
        }

     /*   [Authorize(Roles = "Admin")] */
        [HttpPost]
        [Route("close")]
        public Task<Task<List<DTOTask>>> Close(ToDoTaskResponse task)
        {
            return ServicesTasks.Close(task, _clientFactor);
        }
    }
}