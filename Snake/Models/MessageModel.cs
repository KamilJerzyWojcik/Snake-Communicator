using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Models
{
    public class MessageModel
    {
        public int ID { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public ChannelModel Channel { get; set; }
        public UserModel User { get; set; }
    }
}
