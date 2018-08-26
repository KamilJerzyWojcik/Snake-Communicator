using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Models
{
    public class ChannelModel
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [ForeignKey("UserID")]
        public UserModel UserAuthor { get; set; }

        public ICollection<MessageModel> Messages { get; set; }
        public ICollection<UserChannelModel> UserChannel { get; set; }
    }
}
