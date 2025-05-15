namespace InvoiceApp.Domain.Invoices;

public enum InvoiceStatusType
{
    Pending = 0,
    Paid = 1,
    Overdue = 2,
    Cancelled = 3
}
public class InvoiceStatus
{
    public InvoiceStatusType Status { get; private set; }

    public InvoiceStatus(InvoiceStatusType status)
    {
        Status = status;
    }

    public bool IsValidStatus(InvoiceStatusType status)
    {
        return Enum.IsDefined(typeof(InvoiceStatusType), status);
    }

    public InvoiceStatus ChangeStatus(InvoiceStatusType newStatus)
    {
        if (this.Status == InvoiceStatusType.Paid && newStatus == InvoiceStatusType.Pending)
        {
            throw new InvalidOperationException("Cannot revert a paid invoice to pending");
        }

        return new InvoiceStatus(newStatus);
    }

    public bool IsOverdue(DateTime currentDate)
    {
        return Status == InvoiceStatusType.Overdue && currentDate > DateTime.Now;
    }
    
}