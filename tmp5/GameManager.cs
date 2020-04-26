using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiplayerThread;
using StatisticsManager;
namespace tmp5
{
    class GameManager
    {
        int real_size_x;
        int real_size_y;

        MenuManager menuManager;

        MainThread multiplayerGame;
        SinglePlayerGameManager singlelayerGame;

        public DateTime timer { get; private set; }
        public Thread timerThread;

       
        public GameManager(MenuManager menuManager)
        {
          
            this.menuManager = menuManager;
            this.menuManager.DisplayMenu();
            
            if (GameTypeConfig.isMuliplayer)
            {
                real_size_x = 2*MultiplayerConfig.Width-1;
                real_size_y = 2*MultiplayerConfig.Height-1;
                multiplayerGame = new MainThread(UpdateSidePlayBar,PlayGameMusic,VolumeUp,VolumeDown,MultiplayerConfig.Name,MultiplayerConfig.isHost, MultiplayerConfig.IP,real_size_x, real_size_y);
                if (!multiplayerGame.InitSuccess)
                    return;
                StartTimer();
                multiplayerGame._Thread();

                ResultStatConfig.Type = "MultiPlayer";
                ResultStatConfig.Time = timer.ToString("mm:ss");
                ResultStatConfig.Name = multiplayerGame.YourName;
                ResultStatConfig.Opponet_name = multiplayerGame.Opponentname;
                ResultStatConfig.Steps = multiplayerGame.steps.ToString();
                ResultStatConfig.Result = multiplayerGame.realLoose ? "Loose" : "win";
                ResultStatConfig.Date = DateTime.Today.ToString("d");

            }
            else
            {
                real_size_x = FieldConfig.Width;
                real_size_y = FieldConfig.Height;
                singlelayerGame = new SinglePlayerGameManager(menuManager);

                ResultStatConfig.Type = "SinglePlayer";
                ResultStatConfig.Time = singlelayerGame.timer.ToString("mm:ss");
                ResultStatConfig.Name = !string.IsNullOrEmpty(FieldConfig.Name)? FieldConfig.Name: "???";
                ResultStatConfig.Steps = singlelayerGame.steps.ToString();
                ResultStatConfig.Result = ResultConfig.Status == 1 ? "win" : "loose";
                ResultStatConfig.Date = DateTime.Today.ToString("d");
            }

            StatisticHelper.WriteResult();
           
        }

      
        void UpdateTimer()
        {
            Thread.Sleep(100);
            while (true)
            {
                timer = timer.AddSeconds(1);
                Thread.Sleep(1000);
            }
        }

        void StartTimer()
        {
            timerThread = new Thread(UpdateTimer);
            timerThread.Start();
        }

        private void UpdateSidePlayBar(string steps)
        {
            if (GameTypeConfig.isMuliplayer && !MultiplayerConfig.isHost)
            {
                real_size_x = multiplayerGame.real_size_x;
                real_size_y = multiplayerGame.real_size_y;
            }
            menuManager.DisplayGameSideBar(real_size_x + 10, 4, timer, steps, real_size_y);
        }


        void PlayGameMusic()
        {
            MusicManager.PlayGameMusic();
        }
        void VolumeUp()
        {
            MusicManager.ChangeVolume(true);
        }
        void VolumeDown()
        {
            MusicManager.ChangeVolume(false);
        }
    }
}
