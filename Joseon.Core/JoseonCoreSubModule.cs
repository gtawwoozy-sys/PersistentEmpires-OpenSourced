using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace Joseon.Core
{
    // 조선 커스텀 모듈의 공통 진입점 (Joseon.Core).
    // 독립 Bannerlord 서브모듈로 PE 서버에 로드된다. PE 코어는 건드리지 않는다.
    // 다음 단계: Joseon.Factions / Joseon.Ranks(신분·관직) / Joseon.Economy / Joseon.Events 가 이 토대 위에 추가됨.
    public class JoseonCoreSubModule : MBSubModuleBase
    {
        public const string ModuleName = "Joseon.Core";
        public const string Tag = "Joseon.Core";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            JoseonLog.Info(Tag, "서브모듈 로드됨 — 조선 RP 커스텀 모듈 토대 활성");
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            JoseonLog.Info(Tag, "게임 초기화 완료 — Joseon.Core 준비됨");
        }
    }
}
