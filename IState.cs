public interface IState
{
    void Enter(Enemy parent);

    void Exit();

    void Update();

    
}