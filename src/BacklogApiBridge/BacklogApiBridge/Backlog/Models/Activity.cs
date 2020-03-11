namespace BacklogApiBridge.Backlog.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Project Project { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Content Content { get; set; }
    }
}
