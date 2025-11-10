namespace DAL.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedOnUtc { get; set; }
        string? CreatedById { get; set; }
        DateTime? ModifiedOnUtc { get; set; }
        string? ModifiedById { get; set; }
    }
}
