using TaleWorlds.MountAndBlade;
using Joseon.Core;

namespace Joseon.Ranks
{
    // 신분 시스템 서버측 미션 비헤이비어.
    // 현재: 미션 시작 시 신분 시스템 활성화 로그 + 데이터 모델 자가검증.
    // 다음 단계: 플레이어 접속 시 SinbunManager.EnsureDefault(기본 상민) 부여, /신분 조회 명령, 신분별 장비/구역 게이팅.
    public class RanksMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType
        {
            get { return MissionBehaviorType.Other; }
        }

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
    }
}
