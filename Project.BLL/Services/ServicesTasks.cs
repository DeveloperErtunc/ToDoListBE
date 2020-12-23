using Project.API.Mapper;
using Project.BLL.APIModels;
using Project.BLL.DTO;
using Project.DAL.Context;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Project.DAL.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Project.BLL.Services
{
    public static class ServicesTasks
    {
        #region Variables
        private readonly static MyContext dataBase = new MyContext();
        private static string header1 = "Authorization";
        private static string header2 = "Bearer 524c1d4f83960bdddf53b8a1025570beff34dde6";
        private static long Project_id = 2252415757;
        private static JsonSerializerOptions _jsonSerializerOptions
            = new JsonSerializerOptions();
        private static HttpClient _httpClient = new HttpClient();
        private static string url = "https://api.todoist.com/rest/v1/tasks";
        #endregion
        public static async Task<List<DTOTask>> GetByProjectID(IHttpClientFactory _clientFactory)
        {
            var request =
            new HttpRequestMessage(HttpMethod.Get, url + "?project_id=" + Project_id);
            request.Headers.Add(header1, header2);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var tasks = await System.Text.Json.JsonSerializer.DeserializeAsync
                    <List<ToDoTaskResponse>>(responseStream);
                return MapperTask.ListToDTO(tasks, dataBase.TaskDetails.ToList());
            }
            else
            {
                return null;
            }
        }

        public static async Task<Boolean> Add(DTOTask dtoTask, IHttpClientFactory _clientFactory)
        {
            ToDoTaskRequest toDoTaskRequest = new ToDoTaskRequest();

            toDoTaskRequest = MapperTask.ToRequest(dtoTask);
            var todoItemJson = new StringContent(
                      System.Text.Json.JsonSerializer.Serialize(toDoTaskRequest, _jsonSerializerOptions),
                      Encoding.UTF8,
                      "application/json");

            var request =
            new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add(header1, header2);
            request.Content = todoItemJson;
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var task = await System.Text.Json.JsonSerializer.DeserializeAsync
                <ToDoTaskResponse>(responseStream);
                dtoTask.id = task.id;
                dataBase.TaskDetails.Add(MapperTask.ToTaskDetail(dtoTask));
                dataBase.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }


        public static async Task<Boolean> Update(DTOTask dtoTask, IHttpClientFactory _clientFactory)
        {
            TaskDetail taskDetail = new TaskDetail();
            taskDetail = MapperTask.ToTaskDetail(dtoTask);
            taskDetail.ID = dataBase.TaskDetails.FirstOrDefault(x => x.IDTask == dtoTask.id).ID;
            dataBase.Entry(dataBase.TaskDetails.Find(taskDetail.ID)).CurrentValues.SetValues(taskDetail);
            dataBase.SaveChanges();

            var todoItemJson = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(MapperTask.ToRequest(dtoTask), _jsonSerializerOptions),
                Encoding.UTF8,
                "application/json");

            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add(header1, header2);
            using var httpResponse =
                await httpClient.PostAsync(url + dtoTask.id, todoItemJson);
            httpResponse.EnsureSuccessStatusCode();
            return true;
        }

        public static async Task<Task<List<DTOTask>>> Close(ToDoTaskResponse task, IHttpClientFactory _clientFactory)
        {

            var request =
              new HttpRequestMessage(HttpMethod.Post, url + "/" + +task.id + "/close");
            request.Headers.Add(header1, header2);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return GetByProjectID(_clientFactory);
            }
            else
            {
                return null;
            }
        }

        public static Task<List<DTOTask>> Status(long id, IHttpClientFactory _clientFactory)
        {
            TaskDetail taskDetail = new TaskDetail();
            taskDetail = dataBase.TaskDetails.FirstOrDefault(x => x.IDTask == id);
            taskDetail.Status = true;
            dataBase.Entry(dataBase.TaskDetails.Find(taskDetail.ID)).CurrentValues.SetValues(taskDetail);
            dataBase.SaveChanges();
            return GetByProjectID(_clientFactory);
        }
    }
}
