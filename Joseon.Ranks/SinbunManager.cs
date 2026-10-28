using System.Collections.Generic;

namespace Joseon.Ranks
{
    // 플레이어별 신분 레지스트리.
    // 현재는 인메모리(서버 재시작 시 초기화). 추후 PE SaveSystem(MySQL)에 영속화 예정.
    // key = 플레이어 식별자 문자열(PE PlayerId 등) — 영속화 키로 그대로 쓸 수 있게 string.
    public static class SinbunManager
    {
        private static readonly Dictionary<string, JoseonSinbun> _map = new Dictionary<string, JoseonSinbun>();

        public static JoseonSinbun Get(string playerKey)
        {
            if (string.IsNullOrEmpty(playerKey)) return SinbunInfo.Default;
            JoseonSinbun s;
            return _map.TryGetValue(playerKey, out s) ? s : SinbunInfo.Default;
        }

        public static void Set(string playerKey, JoseonSinbun sinbun)
        {
            if (string.IsNullOrEmpty(playerKey)) return;
            _map[playerKey] = sinbun;
        }

        // 아직 신분이 지정되지 않았으면 기본 신분(상민) 부여. 이미 있으면 그대로.
        public static JoseonSinbun EnsureDefault(string playerKey)
        {
            if (string.IsNullOrEmpty(playerKey)) return SinbunInfo.Default;
            if (!_map.ContainsKey(playerKey)) _map[playerKey] = SinbunInfo.Default;
            return _map[playerKey];
        }

        public static int Count { get { return _map.Count; } }
    }
}
