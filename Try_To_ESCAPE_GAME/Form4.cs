/*
Nom du projet : TryToEscape!
Auteur : Hugo Schweizer
Date de création : Lundi 10 Mars 2025
Dernière mise à jour : Mardi 29 avril 2025
Langage : C# Windows Form.NET Framework 4.7.2
Description : On apprait dans un monde encore inconnu, nous serons affronter à pusieur énigmes, toutes différentes les unes que les autes ! Arriverez-vous à vous en sortir ?
*/
//stoprun == Fonction qui nous empêche de marcher 
//CanRun == Fonction Permettant marcher
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Try_To_ESCAPE__GAME
{
    
    public partial class Form4 : Form
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\games"; // reprends le chemin d'accès de ce fichier
        public Form4()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e) // se fait chaque 1800 miliesecondes ( 1.8 secondes )
        {
            SoundPlayer Win = new SoundPlayer(Properties.Resources.youWIn);
            Win.Play(); // joue le son "You Win" en boucle
        }

        private void timer2_Tick(object sender, EventArgs e) // se fait chaque miliesecondes (0.01 secondes) (change la couleur à chaque fois)
        {
            string filname = path +@"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            if (File.Exists(filname))
            {
                label1.Text = "VOUS AVEZ GAGNÉ !!!!";
            }
            if (File.Exists(filname2))
            {
                label1.Text = "¡¡¡¡HA GANADO !!!!";
            }
            if (File.Exists(filname3))
            {
                label1.Text = "YOU WON !!!!";
            }
            if (File.Exists(filname4))
            {
                label1.Text = "!!!!لقد فزت ";
            }

            Random rdm = new Random();
            int ro = rdm.Next(1,10);
            switch (ro) // selon le chiffre aléatoir, une couleur est choisie pour change le fond
            {
                case 0:
                    panel1.BackColor = Color.White; break;
                case 1:
                    panel1.BackColor = Color.Black; break;
                case 2: 
                    panel1.BackColor = Color.Blue;break;
                case 3:
                    panel1.BackColor = Color.Green; break;
                case 4: 
                    panel1.BackColor = Color.Red;break;
                case 5:
                    panel1.BackColor = Color.Yellow;break;
                case 6: 
                    panel1.BackColor = Color.Brown;break;
                case 7:
                    panel1.BackColor = Color.Pink;break;
                case 8:
                    panel1.BackColor = Color.Purple;break;
                case 9: 
                    panel1.BackColor = Color.Orange;break;
                case 10 : 
                    panel1.BackColor = Color.LightCyan;break;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
