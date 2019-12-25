using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class Message
    {

        public long Id { get; set; }
        /// <summary>
        /// Идентификатор диалога
        /// </summary>
        public long DialogId { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual User User { get; set; }
        /// <summary>
        /// Внешний идентификатор сообщения
        /// </summary>
        public long ExternalId { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public DateTime DateTime { get; set; }





    }
}
