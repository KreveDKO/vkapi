namespace Core.Entity
{
    public class VkUserGroup
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int GroupType { get; set; }

        public long ExternalId { get; set; }

        public bool IsDeactivated { get; set; }
    }
}
