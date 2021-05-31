using System.Collections.Generic;

namespace SharperMC.Core.Entities
{
    public class EntityHandler
    {
        public readonly List<Entity> Entities = new();

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }

        public int GenerateId()
        {
            return Entities.Count + 1;
        }
    }
}