//Room ID 와 Client ID
public class ClientInfo 
{
    //readonly 값이 한번 할당되면 읽기 전용
    //const는 선언과 동시에 할당을 해줘야 하는 차이점
    public readonly string roomId;
    public readonly string clientId;

    public ClientInfo(string roomId, string clientId)
    {
        this.roomId = roomId;
        this.clientId = clientId;
    }
}
