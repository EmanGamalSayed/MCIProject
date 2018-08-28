using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITZone.Entities
{
    [Table("User")]
    public class UserTable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    public class User : UserTable
    {
        public string Role { get; set; }
        public int RoleID { get; set; }
    }
    //[Table("User")]
    public class UserDB : UserTable
    {
        public byte[] photo { get; set; }
    }

    [Table("Fn_GetUserProfile")]
    public class UserModel : UserTable
    {
        public string Role { get; set; }
        public int RoleID { get; set; }
        public byte[] Photo { get; set; }
    }

    [Table("Fn_GetAssignees")]
    public class Assignee
    {
        public int ID { get; set; }
        public string EmpName { get; set; }
    }
    public class photoVM
    {
        public int ID { get; set; }
        public byte[] photo { get; set; }
    }
    
    //public class UserTasksModel
    //{
    //    public string sectionName { get; set; }
    //    public List<TaskModel> tasks { get; set; }
    //}
}
