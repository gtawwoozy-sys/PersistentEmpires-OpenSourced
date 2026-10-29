using System.Linq;
using PersistentEmpiresLib.PersistentEmpiresMission.MissionBehaviors; // SpawnFrameSelectionBehavior
using PersistentEmpiresLib.SceneScripts;                             // PE_SpawnFrame
using TaleWorlds.Engine;                                             // GameEntity, Scene
using TaleWorlds.Library;                                            // MatrixFrame, Mat3, Vec2, Vec3
using TaleWorlds.MountAndBlade;                                      // MissionNetwork, GameNetwork
using Joseon.Core;

namespace Joseon.Ranks
{
    // [맵 호환 브릿지 — 정식 조선 v1.4.8 맵이 준비되면 제거]
    //
    // 스폰 프레임(PE_SpawnFrame)이 하나도 없는 맵(바닐라 등)에서는 플레이어가 스폰될 좌표가
    // 없어 접속 후 영원히 스폰되지 못한다. 서버에서 기본 스폰 프레임이 0개이면 런타임에
    // 하나 주입해 스폰이 되도록 한다. 맵에 스폰 프레임이 이미 있으면 아무것도 하지 않는다.
    public class JoseonSpawnCompatBehavior : MissionNetwork
    {
        private const string Tag = "Joseon.SpawnCompat";
        private bool _injectDone;

        public override void AfterStart()
        {
            base.AfterStart();
            if (GameNetwork.IsServer) TryInject("AfterStart");
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            if (GameNetwork.IsServer && !_injectDone) TryInject("Tick");
        }

        // ---- 서버: 기본 스폰 프레임 주입 (스폰 프레임 없는 맵 대응) ----
        private void TryInject(string phase)
        {
            if (_injectDone) return;
            try
            {
                Mission mission = Mission.Current;
                if (mission == null) return;

                SpawnFrameSelectionBehavior selector = mission.GetMissionBehavior<SpawnFrameSelectionBehavior>();
                if (selector == null || selector.DefaultSpawnFrames == null) return;

                if (selector.DefaultSpawnFrames.Count > 0)
                {
                    JoseonLog.Info(Tag, "[" + phase + "] 기본 스폰 프레임 " + selector.DefaultSpawnFrames.Count + "개 이미 존재 — 주입 안 함");
                    _injectDone = true;
                    return;
                }

                Scene scene = mission.Scene;
                if (scene == null) return;

                MatrixFrame frame = ResolveSpawnFrame(scene);

                GameEntity entity = GameEntity.CreateEmpty(scene, false);
                entity.SetGlobalFrame(frame);
                entity.CreateAndAddScriptComponent(typeof(PE_SpawnFrame).Name, true);
                PE_SpawnFrame spawnFrame = entity.GetFirstScriptOfType<PE_SpawnFrame>();
                if (spawnFrame == null)
                {
                    JoseonLog.Info(Tag, "[" + phase + "] !! PE_SpawnFrame 스크립트 생성 실패");
                    return;
                }

                spawnFrame.FactionIndex = 0;
                spawnFrame.SpawnFromCastle = false;
                spawnFrame.CastleIndex = -1;

                selector.DefaultSpawnFrames.Add(spawnFrame);
                _injectDone = true;
                JoseonLog.Info(Tag, "[" + phase + "] 기본 스폰 프레임 주입 완료 @ ("
                    + frame.origin.X.ToString("0.0") + ", " + frame.origin.Y.ToString("0.0") + ", " + frame.origin.Z.ToString("0.0") + ")");
            }
            catch (System.Exception ex)
            {
                JoseonLog.Info(Tag, "!! 주입 예외: " + ex.Message);
                _injectDone = true;
            }
        }

        private MatrixFrame ResolveSpawnFrame(Scene scene)
        {
            foreach (string tag in new[] { "spawnpoint", "spawn_point", "mp_spawnpoint" })
            {
                GameEntity sp = scene.FindEntitiesWithTag(tag).FirstOrDefault();
                if (sp != null) return sp.GetGlobalFrame();
            }
            float terrainZ;
            Vec3 normal;
            scene.GetTerrainHeightAndNormal(new Vec2(0f, 0f), out terrainZ, out normal);
            return new MatrixFrame(Mat3.Identity, new Vec3(0f, 0f, terrainZ));
        }
    }
}
