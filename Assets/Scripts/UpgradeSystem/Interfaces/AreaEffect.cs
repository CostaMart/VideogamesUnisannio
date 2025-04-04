namespace UpgradeSystem.Interfaces
{
    public abstract class AreaEffect : AbstractEffect
    {
        public override float? Activate(AbstractStatus target)
        {
            throw new System.NotImplementedException();
        }

        public abstract SingleActivationEffect GetEffectToDeploy(PlayerEffectDispatcher pl);

    }
}