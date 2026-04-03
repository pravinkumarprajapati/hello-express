namespace TMS.Domain.Common;

/// <summary>
/// Base entity used for all aggregate roots and entities that need auditing and soft delete support.
/// </summary>
public abstract class BaseAuditableEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedOnUtc { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}
