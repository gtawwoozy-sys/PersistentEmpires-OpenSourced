namespace Joseon.Ranks
{
    // 조선 신분제. 값이 클수록 높은 신분 (양반 > 중인 > 상민 > 천민).
    public enum JoseonSinbun
    {
        Cheonmin = 0, // 천민 (최하)
        Sangmin  = 1, // 상민 (평민, 기본 신분)
        Jungin   = 2, // 중인
        Yangban  = 3, // 양반 (최상)
    }

    // 신분 메타데이터 (한글명, 기본값, 순서). 로직은 C#, 수치/콘텐츠는 추후 XML로 분리 예정.
    public static class SinbunInfo
    {
        public const JoseonSinbun Default = JoseonSinbun.Sangmin;

        public static string KoreanName(JoseonSinbun s)
        {
            switch (s)
            {
                case JoseonSinbun.Yangban:  return "양반";
                case JoseonSinbun.Jungin:   return "중인";
                case JoseonSinbun.Sangmin:  return "상민";
                case JoseonSinbun.Cheonmin: return "천민";
                default: return "상민";
            }
        }

        // 낮은 신분 → 높은 신분 순
        public static readonly JoseonSinbun[] All =
        {
            JoseonSinbun.Cheonmin,
            JoseonSinbun.Sangmin,
            JoseonSinbun.Jungin,
            JoseonSinbun.Yangban,
        };
    }
}
