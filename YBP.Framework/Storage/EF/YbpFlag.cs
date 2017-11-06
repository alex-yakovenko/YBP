using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace YBP.Framework.Storage.EF
{
    public class YbpFlag
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public virtual int ProcessId { get; set; }

        [ForeignKey("ProcessId")]
        public virtual YbpProcess Process {get; set;}

        [StringLength(128)]
        public virtual string Key { get; set; }

        public virtual bool IsSet { get; set; }

        public virtual DateTime UpdatedUTC { get; set; }

        public virtual int UserId { get; set; }
    }
}
