using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Env_sim
{

    public partial class Form1 : Form
    {


        public Timer timer;


        public Form1()
        {
            InitializeComponent();
        }

        public void panel1_Paint(object sender, PaintEventArgs e1)
        {
            env enva = new env();
            Random rnd = new Random();

            enva.init_env(e1, 10, panel1);
            ///enva.Place_element(plant.brush, 1, 1, 10, e1, panel1);

        }

        private void Init_Click(object sender, EventArgs e)
        {
            env enva = new env();
            Random rnd = new Random();

            enva.Generate_Grid_Elements();

            using (Graphics g = panel1.CreateGraphics()) {
                
                enva.Place_element(enva.grid_size, g, panel1);
            }

            enva.Simulate();


            timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += (sender1, args) =>
            {
                using (Graphics g = panel1.CreateGraphics())
                {

                    Update_elements(enva, g);
                }

            };
            timer.Start();
            Console.WriteLine("Timer Started");
        }



        public void Update_elements(env enva, Graphics g)
        {
            enva.Place_element(enva.grid_size, g, panel1);
            Console.WriteLine("Updated");

        }



        /// add Plant1
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
