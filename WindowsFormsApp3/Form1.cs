using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        int currentNum = 0;
        string folderPath = Application.StartupPath + "/images";
        string folderButtonPath = Application.StartupPath + "/button";
        IEnumerable<string> folderImages;
        bool isPause = false;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderImages = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".jpg") || s.EndsWith(".png"));
            timer1.Start();
            picMain.Image = Image.FromFile(folderImages.ToArray()[currentNum]);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            currentNum++;
            if (currentNum >= folderImages.ToArray().Length)
            {
                currentNum = 0;
            }
            picMain.Image = Image.FromFile(folderImages.ToArray()[currentNum]);
            
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            currentNum--;
            if (currentNum < 0)
            {
                currentNum = folderImages.ToArray().Length-1;
            }
            picMain.Image = Image.FromFile(folderImages.ToArray()[currentNum]);
            if (!isPause)
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            currentNum++;
            if (currentNum >= folderImages.ToArray().Length)
            {
                currentNum = 0;
            }
            picMain.Image = Image.FromFile(folderImages.ToArray()[currentNum]);
            if (!isPause)
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            Button a = (Button)sender;

            isPause = !isPause;
            if (isPause)
            {
                a.Image = Image.FromFile(folderButtonPath + "/play.png");
                timer1.Stop();
            }
            else
            {
                a.Image = Image.FromFile(folderButtonPath + "/pause.png");
                timer1.Start();
            }
        }
    }
}
