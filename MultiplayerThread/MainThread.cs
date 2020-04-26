using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Maze;

namespace MultiplayerThread
{
    public class MainThread
    {
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;

        BinaryWriter writer;
        BinaryReader reader;

        int playerPosX;
        int playerPosY;

        int enemyPosX;
        int enemyPosY;

        int enemyPosX_;//it's buffers for displaying eneny character
        int enemyPosY_;

        bool isHost;
        bool isWin = false;
        public bool isLose = false;
        public bool realLoose = false;
        ConsoleKeyInfo k;

        public bool opponentIsOff = false;

        Labr maze;

        List<char> keys;
        List<char> doors;

        List<Point> enemy_keys;
        List<Point> enemy_doors;

        int count_doors_open = 0;
        int Initial_door_count = 3;

        char PickedKey;
        char OpenedDoor;
        bool delete = true;

        public string YourName;
        public string Opponentname;
        Thread thread = null;

        public Action<string> UpdateSideBar;
        public int steps = 0;

        string ip;
        public int real_size_x;
        public int real_size_y;

        ControlManager controlManager;

        public bool InitSuccess = true;

        public MainThread(Action<string> _UpdateSideBar, Action PlayGameMusic, Action VolumeUp, Action VolumeDown,
            string name, bool isHost, string ip, int size_x, int size_y)
        {
            this.UpdateSideBar = _UpdateSideBar;
            YourName = name;
            this.isHost = isHost;
            this.ip = ip;
            real_size_x = size_x;
            real_size_y = size_y;

            InitSuccess = GameInit();

            if (InitSuccess)
            {
                PlayGameMusic.Invoke();
                controlManager = new ControlManager()
                {
                    writer = this.writer,
                    reader = this.reader,
                    moveUp = this.moveUp,
                    moveDown = this.moveDown,
                    moveLeft = this.moveLeft,
                    moveRight = this.moveRight,
                    CheckAndPick_Key = this.CheckAndPick_Key,
                    DepictUnpickedDoor = this.DepictUnpickedDoor,
                    DepictUnpickedKey = this.DepictUnpickedKey,
                    SendMessage = this.SendMessage,
                    UpdateSideBar = _UpdateSideBar,
                    GetEnemyX = this.GetEnemyX,
                    GetEnemyY = this.GetEnemyY,
                    getStatus = this.GetStatus,
                    changeVolumeUp = VolumeUp,
                    changeVolumeDown = VolumeDown
                };


            }
            else
            {

                if (isHost)
                {
                    client = new TcpClient(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString(), 8888);
                    if (stream != null)
                        stream = null;
                    if (client != null)
                        client.Close();
                    listener.Stop();
                }
            }
        }

        bool GameInit()
        {
            Console.Clear();
            PickedKey = 'q';
            OpenedDoor = 'Q';


            keys = new List<char>();
            doors = new List<char>();

            enemy_keys = new List<Point>();
            enemy_doors = new List<Point>();

            maze = new Labr();

            playerPosX = 0;
            playerPosY = 0;

            if (isHost)
            {
                listener = new TcpListener(IPAddress.Any, 8888);

                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                Console.WriteLine("Your IP Address is : " + myIP + " Tell it to your opponent");
                Console.WriteLine("To terminate the server Press Q");
                listener.Start();
                Thread acceptor = new Thread
                    (
                    () => { client = listener.AcceptTcpClient(); }
                    );
                acceptor.Start();
                while (client == null)
                {
                    ConsoleKeyInfo k;
                    bool b = false;
                    while (!Console.KeyAvailable)
                    {
                        if (client != null)
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b)
                        break;
                    k = Console.ReadKey(true);
                    if (k.Key == ConsoleKey.Q)
                    {
                        return false;
                    }
                }

                Console.Title = "Host";

                ServerLabirint s_maze = new ServerLabirint(real_size_x, real_size_y);
                maze.maze = s_maze.maze;
                maze.start_x = s_maze.start_x;
                maze.start_y = s_maze.start_y;

            }
            else
            {
                bool accept = true;
                Thread acceptor = new Thread
                (
                () =>
                {
                    try
                    {
                        Console.WriteLine("Connecting to the server by ip: " + ip + " and port: 8888");
                        Console.WriteLine("To terminate the connection Press Q");
                        client = new TcpClient(ip, 8888);
                        accept = true;
                    }
                    catch
                    {
                        accept = false;
                    }
                }
                );
                acceptor.Start();


                while (client == null)
                {
                    ConsoleKeyInfo k;
                    bool b = false;
                    while (!Console.KeyAvailable)
                    {

                        if (!accept)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Can't connect to the server. Check the ip and if the server exists at all");
                            Console.WriteLine("Press any key to continue...");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.ReadKey();
                            return false;
                        }

                        if (client != null)
                        {
                            b = true;
                            break;
                        }
                    }
                    if (b)
                        break;
                    k = Console.ReadKey(true);
                    if (k.Key == ConsoleKey.Q)
                    {
                        return false;
                    }
                }

                Console.Title = "Client";

            }

            stream = client.GetStream();

            if (isHost)
            {
                SendLabr(maze);
            }
            else
            {
                maze = GetLabr();
                real_size_x = maze.maze[0].Count;
                real_size_y = maze.maze.Count;
            }
            Console.Clear();

            ClientLabirinth currentLabr = new ClientLabirinth();
            currentLabr.DisplayMaze(maze.maze, maze.maze.Count, maze.maze[0].Count);

            playerPosX = maze.start_x + 1;
            playerPosY = maze.start_y + 1;

            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);

            if (YourName != null)
                writer.Write(YourName);
            Opponentname = reader.ReadString();

            Console.Title += " " + YourName;

            Console.SetCursorPosition(real_size_x + 20, 1);

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Blue;
            string dispalayedName = string.IsNullOrEmpty(Opponentname) ? "???" : Opponentname;
            Console.Write("You are playing against: " + dispalayedName);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;

            thread = new Thread(new ThreadStart(receiveMessage));
            thread.Start();

            return true;
        }

        public void _Thread()
        {



            steps = controlManager.PLayerAndEnemyMove(playerPosX, playerPosY);


            isWin = controlManager.isWin;

            if (stream != null)
                stream = null;
            if (client != null)
                client.Close();

            if (isHost)
            {
                listener.Stop();
            }

            Console.Clear();
            realLoose = controlManager.realLoose;
            if (opponentIsOff)
                Console.WriteLine(Opponentname + " is off");
            if (isWin)
            {
                Console.WriteLine("You win");
                Console.WriteLine(Opponentname + " losed");
            }
            else if (isLose)
            {
                Console.WriteLine("You loose");
                Console.WriteLine(Opponentname + " won");
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }


        void DepictUnpickedKey()
        {
            if (PickedKey != 'q')
            {
                Point key = (from List<Point> itmes in maze.maze
                             from Point k in itmes
                             where k.door_key == PickedKey
                             select k).ToArray()[0];
                if (key != null)
                    if (!enemy_keys.Contains(key))
                        enemy_keys.Add(key);
            }

            if (enemy_keys.Count > 0)
            {
                foreach (var en_key in enemy_keys)
                {
                    if (!keys.Contains(en_key.door_key))
                    {

                        if (en_key.x + 1 == enemyPosX && en_key.y + 1 == enemyPosY)
                            return;
                        ConsoleColor color = ConsoleColor.Gray;
                        switch (en_key.door_key)
                        {
                            case 'a':
                                color = ConsoleColor.DarkRed;
                                break;
                            case 'b':
                                color = ConsoleColor.DarkGreen;
                                break;
                            case 'c':
                                color = ConsoleColor.DarkYellow;
                                break;
                        }
                        ConsoleColor cl = Console.ForegroundColor;

                        Console.SetCursorPosition(en_key.x + 1, en_key.y + 1);
                        Console.ForegroundColor = color;
                        Console.Write(en_key.door_key);
                        Console.ForegroundColor = cl;



                    }
                }

            }
        }

        void DepictUnpickedDoor()
        {

            if (OpenedDoor != 'Q')
            {
                var key = (from List<Point> itmes in maze.maze
                           from Point k in itmes
                           where k.door_key == OpenedDoor
                           select k).ToArray();
                Point dor;
                if (key.Length > 0)
                    dor = key[0];
                else
                    return;

                if (key != null)
                    if (!enemy_doors.Contains(dor))
                        enemy_doors.Add(dor);
            }

            if (enemy_doors.Count > 0)
            {
                foreach (var en_key in enemy_doors)
                {
                    if (!doors.Contains(en_key.door_key))
                    {
                        if (en_key.x + 1 == enemyPosX && en_key.y + 1 == enemyPosY)
                            return;
                        ConsoleColor color = ConsoleColor.Gray;
                        switch (en_key.door_key)
                        {
                            case 'A':
                                color = ConsoleColor.DarkRed;
                                break;
                            case 'B':
                                color = ConsoleColor.DarkGreen;
                                break;
                            case 'C':
                                color = ConsoleColor.DarkYellow;
                                break;
                        }
                        ConsoleColor cl = Console.ForegroundColor;

                        Console.SetCursorPosition(en_key.x + 1, en_key.y + 1);
                        Console.ForegroundColor = color;
                        Console.Write(en_key.door_key);
                        Console.ForegroundColor = cl;

                    }
                }

            }
        }


        bool moveUp()
        {
            if (maze.start_y - 1 >= 0 && !(maze.maze[maze.start_y - 1][maze.start_x].symbol == '#' || maze.maze[maze.start_y - 1][maze.start_x].symbol == '|'))
            {
                if (maze.maze[maze.start_y - 1][maze.start_x].door_key != ' ' && char.IsUpper(maze.maze[maze.start_y - 1][maze.start_x].door_key))
                {
                    if (keys.Contains(char.ToLower(maze.maze[maze.start_y - 1][maze.start_x].door_key)))
                    {
                        OpenedDoor = maze.maze[maze.start_y - 1][maze.start_x].door_key;
                        doors.Add(OpenedDoor);

                        maze.start_y--;
                        count_doors_open++;

                        Point temp = maze.maze[maze.start_y][maze.start_x];
                        temp.door_key = ' ';
                        maze.maze[maze.start_y][maze.start_x] = temp;
                        return true;
                    }
                }
                else if (maze.maze[maze.start_y - 1][maze.start_x].door_key == ' ' || char.IsLower(maze.maze[maze.start_y - 1][maze.start_x].door_key))
                {

                    maze.start_y--;
                    return true;
                }

            }
            return false;
        }

        bool moveDown()
        {
            if (maze.start_y + 1 < maze.maze.Count && !(maze.maze[maze.start_y + 1][maze.start_x].symbol == '#' || maze.maze[maze.start_y + 1][maze.start_x].symbol == '|'))
            {
                if (maze.maze[maze.start_y + 1][maze.start_x].door_key != ' ' && char.IsUpper(maze.maze[maze.start_y + 1][maze.start_x].door_key))
                {
                    if (keys.Contains(char.ToLower(maze.maze[maze.start_y + 1][maze.start_x].door_key)))
                    {
                        OpenedDoor = maze.maze[maze.start_y + 1][maze.start_x].door_key;
                        doors.Add(OpenedDoor);

                        count_doors_open++;
                        maze.start_y++;

                        Point temp = maze.maze[maze.start_y][maze.start_x];
                        temp.door_key = ' ';
                        maze.maze[maze.start_y][maze.start_x] = temp;
                        return true;
                    }
                }
                else if (maze.maze[maze.start_y + 1][maze.start_x].door_key == ' ' || char.IsLower(maze.maze[maze.start_y + 1][maze.start_x].door_key))
                {

                    maze.start_y++;
                    return true;
                }


            }
            return false;
        }

        bool moveLeft()
        {
            if (maze.start_x - 1 >= 0 && !(maze.maze[maze.start_y][maze.start_x - 1].symbol == '#' || maze.maze[maze.start_y][maze.start_x - 1].symbol == '|'))
            {
                if (maze.maze[maze.start_y][maze.start_x - 1].door_key != ' ' && char.IsUpper(maze.maze[maze.start_y][maze.start_x - 1].door_key))
                {
                    if (keys.Contains(char.ToLower(maze.maze[maze.start_y][maze.start_x - 1].door_key)))
                    {
                        OpenedDoor = maze.maze[maze.start_y][maze.start_x - 1].door_key;
                        doors.Add(OpenedDoor);


                        count_doors_open++;
                        maze.start_x--;


                        Point temp = maze.maze[maze.start_y][maze.start_x];
                        temp.door_key = ' ';
                        maze.maze[maze.start_y][maze.start_x] = temp;

                        return true;
                    }
                }
                else if (maze.maze[maze.start_y][maze.start_x - 1].door_key == ' ' || char.IsLower(maze.maze[maze.start_y][maze.start_x - 1].door_key))
                {
                    maze.start_x--;
                    return true;
                }


            }
            return false;
        }

        bool moveRight()
        {
            if (maze.start_x + 1 < maze.maze[0].Count && !(maze.maze[maze.start_y][maze.start_x + 1].symbol == '#' || maze.maze[maze.start_y][maze.start_x + 1].symbol == '|'))
            {
                if (maze.maze[maze.start_y][maze.start_x + 1].door_key != ' ' && char.IsUpper(maze.maze[maze.start_y][maze.start_x + 1].door_key))
                {
                    if (keys.Contains(char.ToLower(maze.maze[maze.start_y][maze.start_x + 1].door_key)))
                    {
                        OpenedDoor = maze.maze[maze.start_y][maze.start_x + 1].door_key;
                        doors.Add(OpenedDoor);


                        count_doors_open++;
                        maze.start_x++;


                        Point temp = maze.maze[maze.start_y][maze.start_x];
                        temp.door_key = ' ';
                        maze.maze[maze.start_y][maze.start_x] = temp;
                        return true;
                    }
                }
                else if (maze.maze[maze.start_y][maze.start_x + 1].door_key == ' ' || char.IsLower(maze.maze[maze.start_y][maze.start_x + 1].door_key))
                {
                    maze.start_x++;
                    return true;
                }

            }
            return false;
        }

        bool CheckAndPick_Key()
        {
            if (maze.maze[maze.start_y][maze.start_x].door_key != ' ')
            {
                if (char.IsLower(maze.maze[maze.start_y][maze.start_x].door_key))
                {
                    keys.Add(maze.maze[maze.start_y][maze.start_x].door_key);
                    PickedKey = maze.maze[maze.start_y][maze.start_x].door_key;
                }
            }

            if (count_doors_open < Initial_door_count)
            {
                return true;
            }
            else
                return false;
        }

        void SendMessage(int x, int y, bool isWin)
        {
            if (stream == null)
                return;
            writer.Write(x);
            writer.Write(y);
            writer.Write(PickedKey);
            writer.Write(OpenedDoor);
            writer.Write(isWin);
        }

        void receiveMessage()
        {

            try
            {
                while (true)
                {
                    if (isWin || isLose)
                        return;

                    DateTime time = DateTime.Now;

                    while (time < DateTime.Now)
                    {
                        if (stream == null)
                            return;
                        if (isWin || isLose)
                            return;
                        enemyPosX = reader.ReadInt32();
                        enemyPosY = reader.ReadInt32();
                        PickedKey = reader.ReadChar();
                        OpenedDoor = reader.ReadChar();
                        isLose = reader.ReadBoolean();

                        time = time.AddMilliseconds(1000 / 30);

                        if (time > DateTime.Now)
                        {
                            Thread.Sleep(time - DateTime.Now);
                        }
                    }
                }
            }
            catch (Exception)
            {

                if (isHost)
                {
                    listener.Stop();

                }
                if (stream != null)
                    stream = null;
                if (client != null)
                    client.Close();

                if (!controlManager.isWin && !isLose)
                {
                    controlManager.isWin = true;
                    realLoose = controlManager.realLoose;
                    //Console.Clear();
                    //Console.WriteLine("{0} is off", Opponentname);
                    opponentIsOff = true;

                }

            }
        }

        void SendLabr(Labr labr)
        {
            byte[] data = Serializator.ObjectToByteArray(labr);
            stream.Write(data, 0, data.Length);
        }

        Labr GetLabr()
        {
            byte[] data = new byte[30000];
            int bytes = 0;
            do
            {
                bytes = stream.Read(data, 0, data.Length);

            } while (stream.DataAvailable);

            Labr result = Serializator.ByteArrayToObject(data) as Labr;
            return result;

        }

        int GetEnemyX()
        {
            return enemyPosX;
        }

        int GetEnemyY()
        {
            return enemyPosY;
        }

        bool GetStatus()
        {
            return isLose;
        }
    }
}
