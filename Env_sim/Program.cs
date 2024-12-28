using Env_sim;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Env_sim
{



    public class env
    {
        public List<grid_element> grid_elements_list = new List<grid_element>();
        public int grid_size = 10;
        public Timer timer;
        public void init_env(PaintEventArgs e, int grid_size, Panel panel)
        {
            Graphics g = e.Graphics;
            int rows = (panel.Width / grid_size) - 10;
            int cols = panel.Width / grid_size;

            for (int i = 0; i < rows; i++)
            {
                g.DrawLine(Pens.Gray, 0, i * grid_size, panel.Width, i * grid_size);
            }

            for (int j = 0; j < cols; j++)
            {
                g.DrawLine(Pens.Gray, j * grid_size, 0, j * grid_size, panel.Height);
            }

        }


        public void Generate_Grid_Elements()
        {
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                int roll = rnd.Next(1, 100);

                if (roll <= 50)
                {
                    grid_elements_list.Add(new Plant_1());
                }
                else if (roll >= 50)
                {
                    grid_elements_list.Add(new Plant_2());
                }
            }
            Console.WriteLine($"{grid_elements_list.Count()}");
        }

        public void Place_element(int grid_size, Graphics g, Panel panel)
        {
            Random rnd = new Random();
            foreach (var element in grid_elements_list)
            {

                //Console.WriteLine($"haspos = {element.has_pos}, name: {element.ToString()}");
                if (element.has_pos == false)
                {
                    Console.WriteLine("Generowanie pozycji");
                    element.y = rnd.Next(0, 55) * grid_size;
                    element.x = rnd.Next(0, 150) * grid_size;
                    element.has_pos = true;

                    g.FillRectangle(element.color, element.x, element.y, grid_size, grid_size);

                }
                else if (element.has_pos == true && element.is_new == true)
                {
                    Console.WriteLine("malowanie na ustalonej pozycji");
                    g.FillRectangle(element.color, element.x, element.y, grid_size, grid_size);
                    element.is_new = false;
                }
            }
        }






        public void Simulate()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += on_tick;
            timer.Start();
            Console.WriteLine("Started");
        }


        public void is_place_taken(int newX,int newY,Random rnd,Plant_1 newplant )
        {
            int who_wins;

            Console.WriteLine(grid_elements_list.Count());

            for (int i2 = grid_elements_list.Count - 1; i2 >= 0; i2--)
            {
                if (grid_elements_list[i2].y == newY && grid_elements_list[i2].x == newX)
                {

                    who_wins = rnd.Next(0, 2);
                    Console.WriteLine($"who wins {who_wins}");



                    if (who_wins == 0)
                    {
                        grid_elements_list.Add(newplant);
                        break;
                    }
                    else
                    {
                        break;
                    }

                }
                else
                {
                    grid_elements_list.Add(newplant);
                    break;
                }
            }
        }

        public void on_tick(object sender, EventArgs a)
        {
            Random rnd = new Random();
            Console.WriteLine("Tick");
            int who_wins;

            for (int i = grid_elements_list.Count - 1; i >= 0; i--)
            {
                grid_elements_list[i].Act(grid_elements_list);

                grid_elements_list[i].xp += 10;
                grid_elements_list[i].needed_xp = 100;


                if (grid_elements_list[i].xp >= grid_elements_list[i].needed_xp)
                {
                    Plant_1 newplant = new Plant_1();

                    int newX = grid_elements_list[i].x + rnd.Next(-1, 2) * grid_size;
                    int newY = grid_elements_list[i].y + rnd.Next(-1, 2) * grid_size;

                    newplant.x = newX;
                    newplant.y = newY;
                    newplant.color = grid_elements_list[i].color;
                    newplant.has_pos = true;


                    if (newX >= 0 && newX + grid_size <= 1479 && newY >= 0 && newY + grid_size <= 552)
                    {
                        is_place_taken(newX, newY, rnd, newplant);
                        Console.WriteLine("w granicach");
                    }
                    else
                    {
                        Console.WriteLine("Poza Granicami");
                    }

                    grid_elements_list[i].xp = 0; 
                }
            }

        }
    }






    public class grid_element
    {
        public int y { get; set; }
        public int x { get; set; }
        public Brush color { get; set; }
        public int width;
        public int height;
        public int xp { get; set; }
        public int needed_xp { get; set; }

        public bool has_pos = false;
        public bool is_new = true;


        public virtual void Act(List<grid_element> lista)
        {

        }
    }

    public class Plant_1 : grid_element
    {

        public Plant_1()
        {
            this.color = Brushes.Green;
            this.xp = 0;
            this.needed_xp = 100;
            this.is_new = true;
        }


        public override void Act(List<grid_element> lista)
        {
        }

    }

    public class Plant_2 : grid_element
    {
        public Plant_2()
        {
            this.xp = 0;
            this.needed_xp = 100;
            this.color = Brushes.Indigo;
            this.is_new = true;

        }


        public override void Act(List<grid_element> lista)
        {
        }
    }
}










    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
