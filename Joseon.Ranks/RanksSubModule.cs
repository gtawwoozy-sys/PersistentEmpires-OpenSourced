using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using Joseon.Core;

namespace Joseon.Ranks
{
    // Joseon.Ranks — 신분·관직 시스템 모듈. Joseon.Core 토대 위에 올라간다.
    // PE 코어는 수정하지 않고, 미션에 자체 비헤이비어를 주입한다.
    public class RanksSubModule : MBSubModuleBase
    {
        public const string Tag = "Joseon.Ranks";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            JoseonLog.Info(Tag, "서브모듈 로드됨 — 신분제(양반/중인/상민/천민)");
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            JoseonLog.Info(Tag, "게임 초기화 완료 — 신분 시스템 준비됨");
        }

        // 미션이 초기화될 때 신분 비헤이비어를 주입 (PE 미션에도 적용됨).
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new RanksMissionBehavior());
            JoseonLog.Info(Tag, "RanksMissionBehavior 미션에 주입됨");
        }
    }
}
