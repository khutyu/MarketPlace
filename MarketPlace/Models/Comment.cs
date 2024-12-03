using System.ComponentModel;

namespace MarketPlace.Models
{
    public class Comment
    {
        public int CommentId {  get; set; }

        public string Remark { get; set; }
        public int UserCommented { get; set; }
        public User User { get; set; }

    }
}
