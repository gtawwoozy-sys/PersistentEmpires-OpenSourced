using System;
using System.IO;
using Debug = TaleWorlds.Library.Debug;

namespace Joseon.Core
{
    // 조선 모듈 공통 로그 유틸.
    // 서버 콘솔(Debug.Print) + 파일(서버 작업폴더\joseon.log) 양쪽에 남긴다.
    // 앞으로 모든 Joseon.* 모듈이 이걸 통해 로그를 남긴다.
    public static class JoseonLog
    {
        public static void Info(string tag, string message)
        {
            string line = "[" + tag + "] " + message;
            try { Debug.Print(line); } catch { }
            try
            {
                File.AppendAllText(
                    Path.Combine(Directory.GetCurrentDirectory(), "joseon.log"),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + line + Environment.NewLine,
                    new System.Text.UTF8Encoding(false));
            }
            catch { /* 로그 실패는 무시 (서버 구동에 영향 주지 않음) */ }
        }
    }
}
