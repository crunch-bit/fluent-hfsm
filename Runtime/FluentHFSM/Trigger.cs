namespace HashStudios.FluentHFSM
{
    public sealed class Trigger
    {
        private bool fired;

        public void Fire()
        {
            fired = true;
        }

        public bool Consume()
        {
            if (!fired) return false;
            fired = false;
            return true;
        }
    }
}