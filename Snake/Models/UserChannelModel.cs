using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snake.Models
{
    public class UserChannelModel
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public UserModel User { get; set; }
        public int ChannelID { get; set; }
        [ForeignKey("ChannelID")]
        public ChannelModel Channel { get; set; }

    }
}
