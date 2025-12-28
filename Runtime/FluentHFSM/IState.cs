namespace HashStudios.FluentHFSM
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Tick(float deltaTime);
    } 
}