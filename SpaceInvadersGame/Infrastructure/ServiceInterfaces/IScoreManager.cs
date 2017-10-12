namespace Infrastructure.ServiceInterfaces
{
    public interface IScoreManager
    {
        void AddObjectToMonitor(ICollidable i_ScoringSprite);
        void RemoveObjectFromMonitor(ICollidable i_ScoringSprite);
        int Points(ICollidable i_ScoringSprite);
    }
}
