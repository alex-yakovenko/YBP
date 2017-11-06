using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace YBP.Framework.Storage.EF
{
    public class YbpFlagHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public virtual int FlagId { get; set; }

        [ForeignKey("FlagId")]
        public virtual YbpFlag Flag { get; set; }

        public virtual bool IsSet { get; set; }

        public virtual DateTime UpdatedUTC { get; set; }

        public virtual int UserId { get; set; }
    }
}
