namespace PhotoShare.Models
{
    using System.Collections.Generic;
    using Validation;

    public class Tag
    {
        public Tag()
        {
            this.AlbumTags = new HashSet<AlbumTag>();
        }

        public int Id { get; set; }

        [Tag]
        public string Name { get; set; }

        public virtual ICollection<AlbumTag> AlbumTags { get; set; }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}