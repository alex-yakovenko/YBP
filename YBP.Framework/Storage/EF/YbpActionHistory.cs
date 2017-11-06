using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace YBP.Framework.Storage.EF
{
    public class YbpActionHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public virtual int ProcessId { get; set; }

        [ForeignKey("ProcessId")]
        public virtual YbpProcess Process { get; set; }

        [StringLength(128)]
        public virtual string Name { get; set; }

        public virtual string Params { get; set; }
        public virtual string Results { get; set; }
        public virtual bool IsAuthorized { get; set; }
        public virtual bool Succeed { get; set; }

        public virtual DateTime StartedUTC { get; set; }
        public virtual DateTime FinishedUTC { get; set; }

        public virtual int UserId { get; set; }
    }
}
