namespace SharperMC.Core.Events
{
    public interface ICancellable
    {
        public bool Cancelled { get; set; }

        public virtual void Cancel()
        {
            Cancelled = true;
        }
    }
}