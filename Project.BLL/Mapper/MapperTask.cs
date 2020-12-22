using Project.BLL.APIModels;
using Project.BLL.DTO;
using Project.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Project.API.Mapper
{
    public static class MapperTask
    {
        public static DTOTask ToDTO(ToDoTaskResponse responseTask, TaskDetail taskDetail)
        {
            DTOTask dTOTask = new DTOTask();
            dTOTask.content = responseTask.content;
            dTOTask.date = responseTask.due.date;
            dTOTask.due_date = responseTask.created;
            dTOTask.id = responseTask.id;
            dTOTask.Title = taskDetail.Title;
            dTOTask.Status = taskDetail.Status;
            dTOTask.assignee = responseTask.assignee;
            return dTOTask;
        }

        public static List<DTOTask> ListToDTO(List<ToDoTaskResponse> responseTask, List<TaskDetail> taskDetail)
        {
            List<DTOTask> ListDTOTask = new List<DTOTask>();
            foreach (ToDoTaskResponse response_task in responseTask)
            {
                foreach (TaskDetail detail in taskDetail)
                {
                    if (detail.IDTask == response_task.id)
                    {
                        ListDTOTask.Add(ToDTO(response_task, detail));
                    }
                }
            }
            return ListDTOTask;
        }


        public static ToDoTaskRequest ToRequest(DTOTask dtoTask)
        {
            ToDoTaskRequest request = new ToDoTaskRequest();
            request.content = dtoTask.content;
            request.due_date = dtoTask.due_date;
            request.id = dtoTask.id;
            request.assignee = dtoTask.assignee;
            request.project_id = 2252415757;
            return request;
        }

        public static TaskDetail ToTaskDetail(DTOTask dTOTask)
        {
            TaskDetail detail = new TaskDetail();
            detail.Title = dTOTask.Title;
            detail.Status = dTOTask.Status;
            detail.IDTask = dTOTask.id;
            return detail;
        }

    }
}
