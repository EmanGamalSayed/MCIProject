using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITZone.Entities
{
    [Table("Task")]
    public class TaskDB
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public double Progress { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    [Table("Task")]
    public class TaskTable : TaskDB
    {
        public DateTime CreationDate { get; set; }
        public int FK_CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public int FK_ResponsibleID { get; set; }
        public int FK_RefrenceID { get; set; }
        public int FK_PriorityID { get; set; }
        public int FK_SectorID { get; set; }
        public int FK_StatusID { get; set; }
        public int FK_UrgentSupportID { get; set; }

    }
    [Table("Fn_GetTaskById")]
    public class TaskModel : TaskDB
    {
        public string Assignee { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Sector { get; set; }
        public string Reference { get; set; }
        public string UrgentSupport { get; set; }
    }
    [Table("Fn_GetTimeLineWithUrgent")]
    public class TimeLineTask : TaskDB
    {
        public int Status_Id { get; set; }
        public string Status { get; set; }
        public int Priority_Id { get; set; }
        public string Priority { get; set; }
        public int AssignedTo_ID { get; set; }
        public string AssignedTo { get; set; }
        public string Sector { get; set; }
        public int? FK_UrgentSupportID { get; set; }
        public double Average_progress { get; set; }
        public int flag_id { get; set; }
        public string flag { get; set; }
    }
    [Table("Fn_GetTasksBy")]
    public class TaskSearch : TaskDB
    {
        public string Assignee { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Sector { get; set; }
        public string Reference { get; set; }
        public string UrgentSupport { get; set; }
    }
    public class SectionTask : TaskDB
    {
        public int Status_Id { get; set; }
        public string Status { get; set; }
        public int Priority_Id { get; set; }
        public string Priority { get; set; }
        public int AssignedTo_ID { get; set; }
        public string AssignedTo { get; set; }
        public string Sector { get; set; }
        public int Sector_ID { get; set; }
        public int? FK_UrgentSupportID { get; set; }
        public double Average_progress { get; set; }
        public int flag_id { get; set; }
        public string flag { get; set; }
    }
    //[Table("Task")]
    public class UrgentTask
    {
        public int ID { get; set; }
        public int FK_UrgentSupportID { get; set; }
    }

    public class TimeLineTasks
    {
        public string sectionName { get; set; }
        public List<TimeLineTask> tasks { get; set; }
    }
    public class SectionTasks
    {
        public string sectionName { get; set; }
        public List<TimeLineTask> tasks { get; set; }
    }
    public class TaskAttachment
    {
        public TaskTable Task { get; set; }
        public List<AttachmentModel> Attachment { get; set; }
    }

    public class DetailTask
    {
        public TaskModel Task { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AttachmentModel> Attachments { get; set; }
    }

    //public class DetailsTask
    //{
    //    public int ID { get; set; }
    //    public string Title { get; set; }
    //    public string Description { get; set; }
    //    public double Progress { get; set; }
    //    //public int Status_Id { get; set; }
    //    public string Status { get; set; }
    //    //public int Priority_Id { get; set; }
    //    public string Priority { get; set; }
    //    //public int AssignedTo_ID { get; set; }
    //    public string AssignedTo { get; set; }
    //    public string Sector { get; set; }
    //    //public int FK_RefrenceID { get; set; }
    //    public string Reference { get; set; }
    //    //public int FK_UrgentSupportID { get; set; }
    //    public string UrgentSupport { get; set; }
    //    public DateTime StartDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //    //public int sectionName_id { get; set; }
    //    //public string sectionName { get; set; }
    //}
}
