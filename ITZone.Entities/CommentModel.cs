using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITZone.Entities
{
    public class CommentModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        //public DateTime ModificationDate { get; set; }
        public int TaskID { get; set; }
        public string TaskTitle { get; set; }
        public string CreatedBy { get; set; }
        public string ClassificationType { get; set; }
    }
    [Table("Comment")]
    public class Comment
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        //public DateTime ModificationDate { get; set; }
        public int FK_ItemID { get; set; }
        public int FK_CreatedBy { get; set; }
        public int FK_TypeID { get; set; }
    }
}
