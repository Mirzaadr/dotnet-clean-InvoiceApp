public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); // this method will save all the changes made to the database
    int SaveChanges();
}