using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using System.Threading;
static class Program
{
    public static int Main(string[] args)
    {
        // we don't want our console game ran into an terminal.
        if (!Interop.AllocConsole()) return 1; // return ERROR code if failed to allocate the console window.
        Dictionary<string, Stream> resource = new Dictionary<string, Stream>();
        using (BinaryReader res = new BinaryReader(File.OpenRead("assets.bin")))
            while (res.BaseStream.Position < res.BaseStream.Length) resource.Add(res.ReadString(), new MemoryStream(res.ReadBytes(res.ReadInt32())));
        IntPtr handle = Interop.GetConsoleWindow();
        Rectangle winRect;
        const int Width = 1200;
        const int Height = 720;
        Interop.GetWindowRect(handle, out winRect);
        Interop.MoveWindow(handle, winRect.X, winRect.Y, Width, Height, true);
        Console.Title = "Snake Game!";
        Graphics window = null;
        Rectangle Player = new Rectangle(0, 0, 50, 50);
        Rectangle Food = new Rectangle(0, 0, 20, 20);
        Rectangle Enemy = new Rectangle(100, 100, 50, 50);
        const int fps = 60;
        int speed = 23, enemySpeed = 25;
        int score = -1;
        int diedTimes = 0;
        int abilityMax = 30;
        int ability = abilityMax;
        const int maxDeath = 100;
        Image img2 = Image.FromStream(resource["img2.jpg"]);
        Image img = Image.FromStream(resource["img.png"]);
        bool reached = false;
        Direction dir = Direction.Right;
        Action reloadWindow = () =>
        {
            Thread.Sleep(1000 / fps);
            window = Graphics.FromHwnd(Interop.GetConsoleWindow());

            window.Clear(Color.Black);
            Console.CursorVisible = false;
        };
        Font baseFont = new Font("Segoe UI Light", 20);
        Direction dirEnemy = Direction.Left;
        new Thread(() =>
        {
            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Spacebar:
                        ability--;
                        break;
                    case ConsoleKey.LeftArrow:
                        dir = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        dir = Direction.Right;
                        break;
                    case ConsoleKey.UpArrow:
                        dir = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        dir = Direction.Down;
                        break;
                    case ConsoleKey.W:
                        dirEnemy = Direction.Up;
                        break;
                    case ConsoleKey.S:
                        dirEnemy = Direction.Down;
                        break;
                    case ConsoleKey.A:
                        dirEnemy = Direction.Left;
                        break;
                    case ConsoleKey.D:
                        dirEnemy = Direction.Right;
                        break;
                }
            }
        }).Start();
        // game loop

        while (true)
        {
            reloadWindow();
            if (Player.IntersectsWith(Food))
            {
                score++;
                Food.Location = new Point(Interop.Random.Next(0, Width), Interop.Random.Next(0, Height));
            }
            if (Enemy.IntersectsWith(Player))
            {
                score = 0;
                diedTimes++;
                Player.Location = new Point(Player.Width, Player.Height);
            }
            // directory Haha
            switch (dir)
            {
                case Direction.Up:
                    Player.Y = Math.Max(Player.Y - speed, 0);
                    break;
                case Direction.Down:
                    Player.Y = Math.Min(Player.Y + speed, Height - Player.Height);
                    break;
                case Direction.Left:
                    Player.X = Math.Max(Player.X - speed, 0);
                    break;
                case Direction.Right:
                    Player.X = Math.Min(Player.X + speed, Width - Player.Width);
                    break;
            }
            // the directory of ENEMY .\enemy
            switch (dirEnemy)
            {
                case Direction.Up:
                    Enemy.Y = Math.Max(Enemy.Y - enemySpeed, 0);
                    break;
                case Direction.Down:
                    Enemy.Y = Math.Min(Enemy.Y + enemySpeed, Height - Enemy.Height);
                    break;
                case Direction.Left:
                    Enemy.X = Math.Max(Enemy.X - enemySpeed, 0);
                    break;
                case Direction.Right:
                    Enemy.X = Math.Min(Enemy.X + enemySpeed, Width - Enemy.Width);
                    break;
            }
            if (diedTimes == maxDeath / 2)
            {
                lock (new object())
                {
                    Image img3 = Image.FromStream(resource["img3.jpg"]);
                    new SoundPlayer(resource["audio3.wav"]).Play();

                    for (int i = 0; i < 15; i++)
                    {
                        reloadWindow();
                        window.DrawImage(img3, 0, 0, Math.Min(img3.Width, Width), Math.Min(img3.Height, Height));

                    }
                }
            }
            if (diedTimes == maxDeath / 3)
            {
                object obj = new object();
                lock (obj)
                {
                    new SoundPlayer(resource["audio2.wav"]).Play();
                    for (int i = 0; i < 15; i++)
                    {
                        reloadWindow();
                        window.DrawImage(img2, 0, 0, Math.Min(img2.Width, Width), Math.Min(img2.Height, Height));

                    }
                }
            }
            if (diedTimes == maxDeath && !reached)
            {
                reached = true;
                var sound = new SoundPlayer(resource["audio.wav"]);
                lock (new object())
                {
                    Console.Title = "Menak lelah ya youssef";
                    sound.Play();

                    for (int i = 0; i < 40; i++)
                    {
                        reloadWindow();
                        // make the image fill in the window.
                        window.DrawImage(img, 0, 0, Math.Min(img.Width, Width), Math.Min(img.Height, Height));
                    }

                }
                Interop.ShowWindow(handle, 0);
                Interop.EnableWindow(handle, false);
                Thread.Sleep(3000);
                Interop.ShowWindow(handle, 5);
                //Interop.SetWindowPos(handle, (IntPtr)(-1), winRect.X, winRect.Y, 0, 0, 0);
                sound.PlayLooping();
                for (int i = 0; i < 10; i++)
                {
                    Interop.MoveWindow(handle, Interop.Random.Next(0, Screen.PrimaryScreen.Bounds.Width - Width), Interop.Random.Next(0, Screen.PrimaryScreen.Bounds.Height - Height), Width, Height, true);
                    for (int j = 0; j < 10; j++)
                    {
                        reloadWindow();

                        window.DrawImage(img, 0, 0, Math.Min(img.Width, Width), Math.Min(img.Height, Height));
                    }
                }

                sound.Stop();
                Interop.EnableWindow(handle, true);
            }
            if (reached)
            {
                window.DrawImage(img, Player);
                window.DrawImage(img2, Enemy);
            }
            else
            {
                window.FillRectangle(SolidColors.White, Player);
                window.FillRectangle(SolidColors.Red, Enemy);
            }
            window.FillRectangle(SolidColors.Green, Food);

            window.FillRectangle(new SolidBrush(Color.FromArgb(0, byte.MaxValue - Math.Min(byte.MaxValue, (speed * ability) / 2), Math.Min(byte.MaxValue, (speed * ability) / 2))), new Rectangle((Width - (ability * speed)) / 2, 30, (ability * speed), 30));
            window.DrawString(string.Format("Score: {0:0000}, Died Times: {1:0000}", score, diedTimes), baseFont, SolidColors.White, 0, 0);
        }
    }
}