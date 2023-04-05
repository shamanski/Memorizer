namespace Memorizer.DbModel 
{ 
    public class WebAppState : BaseEntity
    {
        public int UserId { get; set; }
        public string CommandName { get; set; }
        public string StateData { get; set; }
    }
}
