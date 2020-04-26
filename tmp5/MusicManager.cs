using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace tmp5
{
    public class MusicManager
    {
        [DllImport("user32", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32")]
        static extern IntPtr GetConsoleWindow();

        const UInt32 WM_APPCOMMAND = 0x0319;
        const UInt32 APPCOMMAND_VOLUME_DOWN = 9;
        const UInt32 APPCOMMAND_VOLUME_UP = 10;

        static string menuMusicPath;
        static string gameMusicPath;
        static SoundPlayer player;

        static MusicManager()
        {
            player = new SoundPlayer();
            menuMusicPath = AppDomain.CurrentDomain.BaseDirectory + "/Music/MenuMusic8bit.wav";
            gameMusicPath = AppDomain.CurrentDomain.BaseDirectory + "/Music/PlayMusic8bit.wav";
        }

        public static void PlayMenuMusic()
        {
            player.Stop();
            player.SoundLocation = menuMusicPath;
            player.PlayLooping();

        }

        public static void PlayGameMusic()
        {
            player.Stop();
            player.SoundLocation = gameMusicPath;
            player.PlayLooping();
        }

        public static void ChangeVolume(bool b)
        {
            if (b)
            {
                var cw = GetConsoleWindow();
                SendMessage(cw, WM_APPCOMMAND, cw, new IntPtr(APPCOMMAND_VOLUME_UP << 16));
            }
            else if (!b)
            {
                var cw = GetConsoleWindow();
                SendMessage(cw, WM_APPCOMMAND, cw, new IntPtr(APPCOMMAND_VOLUME_DOWN << 16));
            }
        }

    }
}
