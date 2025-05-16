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
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Try_To_ESCAPE__GAME
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        double time; // sert de chrono, chaque 0.1 seconde on lui ajoute 0.1 ( il est arrondi avec la variable "arrondie" permettant de ne pas avoir ces chiffres : 9.999999999999 )
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\games"; // reprends le chemin d'accès de ce fichier
        string langue = "fr"; // langue choisi par l'utilisateur ( en appuyant sur le bouton "langue" une fenêtre s'ouvre pour laisser le choix à l'utilisateur entre l'anglais, le français, l'espagnol et l'arabe )
        bool tr = false; // pour que le texte de fin se fasse qu'une seule foix (si tr == false, le texte apprait puis tr devients true)
        private void label10_Click(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string filname = path + @"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            string FileFini = path + @"\fini.txt";
            if (File.Exists(filname))
            {
                langue = "fr";
            }
            if (File.Exists(filname2))
            {
                langue = "es";
            }
            if (File.Exists(filname3))
            {
                langue = "en";
            }
            if (File.Exists(filname4))
            {
                langue = "ar";
            }
            string filename = path + @"\timer.txt";
            if (File.Exists(filename))
            {
                this.Show();
            }
            if (File.Exists(filename) == false)
            {
                this.Hide();
            }
            string MenuFile = path + @"\menu.txt";
            string TimeFile = path + @"\timer.txt";
           double arrondie = Math.Round(time, 2);
                if (File.Exists(MenuFile) == false && File.Exists(TimeFile) == true) // si le menu est ouvert et que le timer est activé
                {
                     if (File.Exists(FileFini) == false){ // si le fichier de fin de jeu n'exist pas
                        time = time + 0.01;
                        
                        if (langue == "fr")
                        {
                            label1.Text = "Timer : " + arrondie;
                        }
                        if (langue == "es")
                        {
                            label1.Text = "Temporizador : " + arrondie;
                        }
                        if (langue == "en")
                        {
                            label1.Text = "Timer : " + arrondie;
                        }
                        if (langue == "ar")
                        {
                            label1.Text = "المؤقِّت : " + arrondie;
                        }
                     }
                     else if (tr == false) // si le fichier de fin exist et que le texte de fin n'est pas encore apparu
                     {
                        tr = true;
                        if (langue == "fr")
                        {
                            label1.Text = "Vous avez réussi avec\n un temps de : "+ arrondie;
                        }
                        if (langue == "es")
                        {
                            label1.Text = "Has tenido éxito con\n un tiempo de : " + arrondie;
                        }
                        if (langue == "en")
                        {
                            label1.Text = "You have succeeded with\n a time of : " + arrondie;
                        }
                        if (langue == "ar")
                        {
                            label1.Text = arrondie + " : لقد نجحت في وقت";
                        }
                     }
                }
                else  // si le timer n'est pas activé ou que le menu est ouvertm ou que le chargement est encore présent
                {
                    if (File.Exists(FileFini) == false)
                    {
                        if (langue == "fr")
                        {
                            label1.Text = "Timer : Pause";
                        }
                        if (langue == "es")
                        {
                            label1.Text = "Temporizador : Quebrar";
                        }
                        if (langue == "en")
                        {
                            label1.Text = "Timer : Break";
                        }
                        if (langue == "ar")
                        {
                            label1.Text = "المؤقِّت : كسر";
                        }
                    }
                    else if (tr == false) // si le fichier de fin exist et que le texte de fin n'est pas encore apparu
                {
                        tr = true;
                        if (langue == "fr")
                        {
                            label1.Text = "Vous avez réussi avec\n un temps de : " + arrondie;
                        }
                        if (langue == "es")
                        {
                            label1.Text = "Has tenido éxito con\n un tiempo de : " + arrondie;
                        }
                        if (langue == "en")
                        {
                            label1.Text = "You have succeeded with\n a time of : " + arrondie;
                        }
                        if (langue == "ar")
                        {
                            label1.Text = arrondie + " : لقد نجحت في وقت";
                        }
                        
                    }
                }
            
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }
    }
}
