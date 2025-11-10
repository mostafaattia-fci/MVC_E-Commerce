namespace DAL.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOnUtc { get; set; }
        string? DeletedById { get; set; }
    }
}
