using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;

namespace PersistentEmpiresLib.GameModes
{
    public class PersistentEmpiresGameMode : MultiplayerGameMode
    {
        public delegate void StartMultiplayerGameDelegate(string scene);
        public static StartMultiplayerGameDelegate OnStartMultiplayerGame;
        public PersistentEmpiresGameMode(string name) : base(name)
        {

        }

        public override void JoinCustomGame(JoinGameData joinGameData)
        {
            // [호스트 자기테스트 전용] 같은 PC에서 자기 서버에 접속할 때는 공인 IP로는 NAT 루프백
            // 때문에 로비 핸드셰이크가 안 끝난다. selftest 파일에 적힌 서버 공인 IP를 만나면
            // 127.0.0.1(localhost)로 바꿔 접속한다. 파일이 없으면(=실배포/실플레이어) 아무 동작도
            // 하지 않으므로 공인 IP로 정상 접속된다. 실배포 클라에는 이 파일이 없어야 한다.
            string addr = joinGameData.GameServerProperties.Address;
            try
            {
                const string selfTestFile = @"C:\dev\joseon-logs\selftest-ip.txt";
                if (System.IO.File.Exists(selfTestFile))
                {
                    string selfIp = System.IO.File.ReadAllText(selfTestFile).Trim();
                    if (!string.IsNullOrEmpty(selfIp) && addr == selfIp) addr = "127.0.0.1";
                }
            }
            catch { }
            LobbyGameStateCustomGameClient lobbyGameStateCustomGameClient = Game.Current.GameStateManager.CreateState<LobbyGameStateCustomGameClient>();
            lobbyGameStateCustomGameClient.SetStartingParameters(NetworkMain.GameClient, addr, joinGameData.GameServerProperties.Port, joinGameData.PeerIndex, joinGameData.SessionKey);
            Game.Current.GameStateManager.PushState(lobbyGameStateCustomGameClient, 0);
        }

        public override void StartMultiplayerGame(string scene)
        {
            if (OnStartMultiplayerGame != null)
            {
                OnStartMultiplayerGame(scene);
            }
        }
    }

}
