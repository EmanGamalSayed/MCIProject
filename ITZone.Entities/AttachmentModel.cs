using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITZone.Entities
{
    [Table("Attachment")]
    public class AttachmentModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int FK_TypeID { get; set; }
        public int FK_ItemID { get; set; }
        public byte[] Content { get; set; }
        public int FK_CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
