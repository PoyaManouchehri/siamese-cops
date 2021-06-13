namespace Assets.Scripts
{
    public interface IHealthOwner
    {
        public bool CanReceiveHealth();
        public float HealthSpeedMultiplier();
    }
}