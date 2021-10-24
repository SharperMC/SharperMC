using System;
using SharperMC.Core.Utils.World;

namespace SharperMC.Core.Entities
{
    public class Entity
    {
        public string Name;
        public int Id { get; }
        public byte Dimension { get; set; }
        
        public Location Location { get; set; }
        
        public Entity()
        {
            SharperMC.Instance.Server.EntityHandler.AddEntity(this);
            Id = SharperMC.Instance.Server.EntityHandler.GenerateId();
        }

        public virtual void Spawn()
        {
            
        }
    }
}