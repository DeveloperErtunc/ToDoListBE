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
     [Authorize(Roles ="User,Admin")]
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        public TasksController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public Task<Boolean> Add(DTOTask dtoTask)
        {
            return ServicesTasks.Add(dtoTask, _clientFactory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("update")]
        public Task<Boolean> Update(DTOTask dtoTask)
        {
            return ServicesTasks.Update(dtoTask, _clientFactory);
        }

        [HttpGet]
        [Route("byid")]
        public Task<List<DTOTask>> GetByProjectID()
        {
            return ServicesTasks.GetByProjectID(_clientFactory);
        }

        [HttpGet]
        [Route("status/id")]
        public Task<List<DTOTask>> Status(long id)
        {
            return ServicesTasks.Status(id, _clientFactory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("close")]
        public async Task<List<DTOTask>> Close(ToDoTaskResponse task)
        {
              HttpClient _httpClient = new HttpClient();
            var  header1 = "Authorization";
            var  header2 = "Bearer 524c1d4f83960bdddf53b8a1025570beff34dde6";
            var url = "https://api.todoist.com/rest/v1/tasks";

         var request =
          new HttpRequestMessage(HttpMethod.Post, url + "/" + task.id + "/close");
            request.Headers.Add(header1, header2);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await ServicesTasks.GetByProjectID(_clientFactory);
            }
            else
            {
                return null;
            }
        }
    }
}