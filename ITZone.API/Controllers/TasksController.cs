using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ITZone.API.Data;
using ITZone.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ITZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private Context db;
        private enum sectionIDs {   Zero, Urgent, SME, Regional_Sectorial, ThisWeek, ThisMonth, Remaining, empty, empty2, empty3, empty4, empty5, empty6, empty7, empty8,
            education, customerService, Employee }

        public TasksController(Context context)
        {
            db = context;
            
        }

        [HttpGet("GetTaskById")]
        public ActionResult GetTaskById(int taskId)
        {
            try
            {
                TaskModel task = db.GetTask.FromSql($"SELECT * FROM [Fn_GetTaskById] ({taskId})").FirstOrDefault();
                List<Comment> comment = db.Comment.Where(c => c.FK_ItemID == taskId).ToList();
                List<AttachmentModel> attachs = db.Attachments.Where(a => a.FK_ItemID == taskId).ToList();

                return Ok(new DetailTask { Task = task, Comments = comment, Attachments = attachs });
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetUserTasks")]
        public ActionResult GetUserTasks(int userId,bool isUrgent)
        {
            try
            {
                List<TimeLineTasks> tasksList = new List<TimeLineTasks>();
                var tasks = db.Tasks.FromSql($"select* from [Fn_GetTimeLineWithUrgent] ({userId},{isUrgent})").ToList();
                var urgentTask = tasks.Where(t => t.flag_id == (int)sectionIDs.Urgent).ToList();
                tasksList.Add(new TimeLineTasks { sectionName = "urgent", tasks = urgentTask });
                var thisWeekTask = tasks.Where(t => t.flag_id == (int)sectionIDs.ThisWeek).ToList();
                tasksList.Add(new TimeLineTasks { sectionName = "thisweek", tasks = thisWeekTask });
                var thisMonthTask = tasks.Where(t => t.flag_id == (int)sectionIDs.ThisMonth).ToList();
                tasksList.Add(new TimeLineTasks { sectionName = "thismonth", tasks = thisMonthTask });
                var remainingTask = tasks.Where(t => t.flag_id == (int)sectionIDs.Remaining).ToList();
                tasksList.Add(new TimeLineTasks { sectionName = "remaining", tasks = remainingTask });
                return Ok(tasksList);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Get_SectorsView")]
        public ActionResult Get_SectorsView(int userId)
        {
            //try
            //{
                List<SectionTasks> tasksList = new List<SectionTasks>();
                var tasks = db.Tasks.FromSql($"select* from [itzonegl_MC].[Fn_GetSectorsView] ({userId})").ToList();
                var SMESupportTask = tasks.Where(t => t.flag_id == (int)sectionIDs.SME).ToList();
                tasksList.Add(new SectionTasks { sectionName = "SME Support", tasks = SMESupportTask });
                var RegionalAndSectorialTask = tasks.Where(t => t.flag_id == (int)sectionIDs.Regional_Sectorial).ToList();
                tasksList.Add(new SectionTasks { sectionName = "Regional and Sectorial", tasks = RegionalAndSectorialTask });
                var educationTask = tasks.Where(t => t.flag_id == (int)sectionIDs.education).ToList();
                tasksList.Add(new SectionTasks { sectionName = "education", tasks = educationTask });
                var customerServiceTask = tasks.Where(t => t.flag_id == (int)sectionIDs.customerService).ToList();
                tasksList.Add(new SectionTasks { sectionName = "customer service", tasks = customerServiceTask });
                return Ok(tasksList);
            //}
            //catch
            //{
            //    return BadRequest();
            //}
        }

        [HttpGet("GetTaskComments"), Authorize]
        public ActionResult GetTaskComments(int taskId)
        {
            try
            {
                var comment = db.Comments.FromSql($"SELECT * FROM [Fn_GetTaskComments] ({taskId})").ToList();
                return Ok(comment);
                }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("AddTask")]
        public ActionResult AddTask(TaskAttachment taskAttachment)
        {
            try
            {
                TaskTable task = taskAttachment.Task;
                    db.AddTask.Add(task);
                List<int> attachmentIDs = new List<int>();
                bool result = db.SaveChanges() > 0 ? true : false;
                if (result)
                {
                    foreach (var item in taskAttachment.Attachment)
                    {
                        db.Attachments.Add(new AttachmentModel
                        {
                            FK_TypeID = item.FK_TypeID,
                            FK_ItemID = task.ID,
                            Title = item.Title,
                            Content = item.Content,
                            CreationDate = item.CreationDate,
                            FK_CreatedBy = item.FK_TypeID,
                        });
                        bool result2 = db.SaveChanges() > 0 ? true : false;
                    }
                }
                return Ok( new{ id = task.ID/*, AttachmentIDs = taskAttachment.Attachment*/ });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("AddTaskComment"), Authorize]
        public ActionResult AddTaskComment(Comment comment)
        {
            try
            {
                db.Comment.Add(comment);
                db.SaveChanges();
                return Ok(new { id = comment.ID });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateTaskUrgent"), Authorize]
        public ActionResult UpdateTaskUrgent(UrgentTask task)
        {
            try
            {
                var obj = db.AddTask.Find(task.ID);
                if (obj == null)
                    return NotFound();
                if (obj != null)
                    obj.FK_UrgentSupportID = task.FK_UrgentSupportID;
                db.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateTaskReturned"), Authorize]
        public ActionResult UpdateTaskReturned(int taskId)
        {
            try
            {
                var obj = db.AddTask.Find(taskId);
                if (obj == null)
                    return NotFound();
                if (obj != null)
                    obj.FK_StatusID = 12;
                db.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateTaskClosed"), Authorize]
        public ActionResult UpdateTaskClosed(int taskId)
        {
            try
            {
                var obj = db.AddTask.Find(taskId);
                if (obj == null)
                    return NotFound();
                if (obj != null)
                    obj.FK_StatusID = 11;
                db.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("UpdateTask"), Authorize]
        public ActionResult UpdateTask(TaskAttachment taskAttachment)
        {
            try
            {
                var obj = db.AddTask.Find(taskAttachment.Task.ID);
                if (obj == null)
                    return NotFound();
                if (obj != null)
                {
                    List<AttachmentModel> attachs = db.Attachments.Where(a => a.FK_ItemID == taskAttachment.Task.ID).ToList();
                    foreach (var item in attachs)
                    {
                        db.Attachments.Remove(item);
                    }
                    db.SaveChanges();
                    foreach (var item in taskAttachment.Attachment)
                    {
                        db.Attachments.Add(new AttachmentModel
                        {
                            FK_TypeID = item.FK_TypeID,
                            FK_ItemID = obj.ID,
                            Title = item.Title,
                            Content = item.Content,
                            CreationDate = item.CreationDate,
                            FK_CreatedBy = item.FK_TypeID,
                        });
                        bool result2 = db.SaveChanges() > 0 ? true : false;
                    }
                    db.SaveChanges();

                }

                db.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("DeleteTask"), Authorize]
        public ActionResult DeleteTask(int taskId)
        {
            try
            {
                var obj = db.AddTask.Find(taskId);
                if (obj == null)
                    return NotFound();
                else if (obj != null)
                    obj.IsActive = false;
                db.SaveChanges();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }




    }
}
//[HttpGet("GetUserTasks2"), Authorize]
//public ActionResult GetUserTasks2(int userId)
//{
//    try
//    {
//        List<TasksModel> tasks = new List<TasksModel>();
//        var urgentTask = db.Tasks.FromSql($"SELECT * FROM [Fn_GetUserUrgentTasks] ({userId})").ToList();
//        if (urgentTask != null)
//            tasks.Add(new TasksModel { sectionName = "urgent", tasks = urgentTask });

//        var completedTask = db.Tasks.FromSql($"SELECT * FROM [Fn_GetUserCompletedTasks] ({userId})").ToList();
//        if (completedTask != null)
//            tasks.Add(new TasksModel { sectionName = "completed", tasks = completedTask });

//        var thisWeekTask = db.Tasks.FromSql($"SELECT * FROM [Fn_GetUserThisWeekTasks] ({userId})").ToList();
//        if (thisWeekTask != null)
//            tasks.Add(new TasksModel { sectionName = "thisweek", tasks = thisWeekTask });

//        var thisMonthTask = db.Tasks.FromSql($"SELECT * FROM [Fn_GetUserThisMonthTasks] ({userId})").ToList();
//        if (thisMonthTask != null)
//            tasks.Add(new TasksModel { sectionName = "thismonth", tasks = thisMonthTask });

//        var remainingTask = db.Tasks.FromSql($"SELECT * FROM [Fn_GetUserRemainingTasks] ({userId})").ToList();
//        if (remainingTask != null)
//            tasks.Add(new TasksModel { sectionName = "remaining", tasks = remainingTask });

//        return Ok(tasks);
//    }
//    catch
//    {
//        return BadRequest();
//    }
//}