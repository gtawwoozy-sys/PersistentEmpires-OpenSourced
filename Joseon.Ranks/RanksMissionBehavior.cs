using PersistentEmpiresLib.PersistentEmpiresMission.MissionBehaviors; // InformationComponent
using TaleWorlds.MountAndBlade;                                        // MissionNetwork, NetworkCommunicator
using Joseon.Core;

namespace Joseon.Ranks
{
    // 신분 시스템 서버측 미션 비헤이비어 (MissionNetwork = 네트워크 연동 비헤이비어).
    // - 미션 시작: 신분 시스템 활성 로그
    // - 플레이어 접속: 기본 신분(상민) 부여 + 본인에게 신분 안내 메시지
    // 다음 단계: /신분 명령(PE 명령 시스템 연동), 신분별 장비/구역 게이팅, PE DB 영속화.
    public class RanksMissionBehavior : MissionNetwork
    {
        // 조선 금빛 (#C9A227) — 신분 안내 메시지 색
        private static readonly uint JoseonColor =
            TaleWorlds.Library.Color.ConvertStringToColor("#C9A227FF").ToUnsignedInteger();

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            string list = "";
            foreach (JoseonSinbun s in SinbunInfo.All)
            {
                list += SinbunInfo.KoreanName(s) + " ";
            }
            JoseonLog.Info("Joseon.Ranks", "신분 시스템 활성 — " + list.Trim());
        }

        public override void OnPlayerConnectedToServer(NetworkCommunicator networkPeer)
        {
            base.OnPlayerConnectedToServer(networkPeer);
            if (networkPeer == null || networkPeer.VirtualPlayer == null) return;

            string key = networkPeer.VirtualPlayer.Id.ToString();
            JoseonSinbun sinbun = SinbunManager.EnsureDefault(key);   // 없으면 상민 부여
            string name = SinbunInfo.KoreanName(sinbun);

            JoseonLog.Info("Joseon.Ranks",
                "접속: " + networkPeer.UserName + " → 신분 " + name + " (등록 " + SinbunManager.Count + "명)");

            // 접속자 본인에게 신분 안내
            try
            {
                if (InformationComponent.Instance != null)
                {
                    InformationComponent.Instance.SendMessage("[조선] 당신의 신분: " + name, JoseonColor, networkPeer);
                }
            }
            catch { /* 메시지 실패는 무시 */ }
        }
    }
}
