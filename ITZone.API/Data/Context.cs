using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITZone.Entities;


namespace ITZone.API.Data
{
    public class Context : DbContext
    {
        public Context ( DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserDB> UserTokenDB { get; set; }
        public DbSet<Assignee> Assignees { get; set; }
        //public DbSet<TaskTable> Task { get; set; }
        public DbSet<TaskModel> GetTask { get; set; }
        public DbSet<TimeLineTask> Tasks { get; set; }

        //public DbSet<DetailsTask> Tasks { get; set; }
        public DbSet<TaskTable> AddTask { get; set; }

        public DbSet<AttachmentModel> Attachments { get; set; }

        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<Comment> Comment { get; set; }

        public DbSet<SectorModel> Sectors { get; set; }
        public DbSet<PriorityModel> Priorities { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<StatusModel> Status { get; set; }


    }
}
