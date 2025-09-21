
public interface IGameState
{
    void Enter(); //상태 진입하면 초기화
    void Execute(); //프레임, 이벤트 처리
    void Exit(); //상태 벗어날 때 정리리
}
