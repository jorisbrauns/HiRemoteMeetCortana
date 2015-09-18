using WebService.Infrastructure.DataAccess;

namespace WebService.Infrastructure.Repository
{
    public class DeleteManager : IDeleteManager
    {
        public void Delete(Entity entity)
        {
            if (entity is ISoftDeletable)
            {
                entity.State = State.Modified;
                (entity as ISoftDeletable).IsDeleted = true;
            }
            else
            {
                entity.State = State.Deleted;
            }
        }
    }
}