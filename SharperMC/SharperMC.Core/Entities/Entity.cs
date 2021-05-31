namespace SharperMC.Core.Entities
{
    public class Entity
    {
        public int Id { get; }
        public byte Dimension { get; set; }
        
        public Entity()
        {
            SharperMC.Instance.Server.EntityHandler.AddEntity(this);
            Id = SharperMC.Instance.Server.EntityHandler.GenerateId();
        }
    }
}