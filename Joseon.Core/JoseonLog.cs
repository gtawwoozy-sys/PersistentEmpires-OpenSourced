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

            string stamped = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + line + Environment.NewLine;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding(false);

            // 1) 작업폴더 기준 joseon.log (서버는 bin 폴더 = 기존 동작 유지)
            try
            {
                File.AppendAllText(Path.Combine(Directory.GetCurrentDirectory(), "joseon.log"), stamped, enc);
            }
            catch { /* CWD가 쓰기 불가일 수 있음(특히 클라) → 아래 절대경로로 보장 */ }

            // 2) 확실히 쓰기 가능한 절대경로, 프로세스별 파일 (클라 로그도 여기 남음)
            try
            {
                string dir = @"C:\dev\joseon-logs";
                Directory.CreateDirectory(dir);
                string proc = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
                File.AppendAllText(Path.Combine(dir, "joseon-" + proc + ".log"), stamped, enc);
            }
            catch { /* 로그 실패는 무시 (구동에 영향 주지 않음) */ }
        }
    }
}
