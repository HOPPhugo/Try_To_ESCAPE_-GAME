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
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Configuration;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Try_To_ESCAPE__GAME
{
    public partial class Form1 : Form
    {
        int speed = 12; // vitesse de marche
        string marcher = "gauche"; // quel endroit on est pour l'animation de la porte ( quand il y a l'animation de prends cette données pour savoir de quel sense je fait bouger le panel12).
        int rage = 0; // si on parle trop au premier PNJ sans avoir lu la feuille au début le jeu se ferm (à chque fois qu'on lui parle sans avoir lu la fauille "rage" s'incrémente puis s'il atteint trois, le jeu se ferm).
        bool menu = false; // false quand le "menu" est fermé / true si le "menu" est ouvert ( utilisé pour quand le "menu" est ouvert "true" le le timer s'arrête "Timer : Pause").
        int papier = 0; // le nombre de papier ramassé ( s'incrémente à chaque fois qu'on ramasse un papier).
        bool monde2 = false; // false == le personnage est dans le monde 1 / true == le personnage est dans le monde 2. ( quand on a fini se monde avant d'être t'éléporter, monde2 devient true).
        int cibles = 0; // Cibles s'incrémente quand on clique sur une cilbes qui s'appelle "Target1" ( si "cibles" atteint 12 et +, la cible dissparait et le portail pour l'autre monde apparait).
        bool timer = false; // False == timer est pas activé / true == timer est activé.
        string langue = "fr"; // fr == français, es == espagnole, en == english, ar == arabe. ("langue" change selon le language choisi dans le menu).
        bool bouton1 = false; // si on intéragi sur le "button1" dans la deuxième salle, button1 devient true
        bool bouton2 = false;// si on intéragi sur le "bouton2" dans la deuxième salle, bouton2 devient true
        bool portal = true; // si on prends le portail, il devient false.
        double time = 0; 
        bool correct = false; // si on à juste à la première question du PNJ, il devient true
        bool start = false; // si start est égale à true et que le joueur intéragi avec le bateau, le jeu bataille navale se lance.
        bool piano = false; // si on réussi l'épreuve du piano, il devient true
        bool bouton3 = false; // si on intéragi sur le "bouton3" dans la deuxième salle, bouton3 devient true
        bool bouton4 = false;// si on intéragi sur le "bouton4" dans la deuxième salle, bouton4 devient true
        bool chargement = true; // au début du jeu, il y à un écran de démarrage qui explique comment ouvrir le menu et demande de cliquer pour lancer le jeu. Quand on clique, il disparrait et chargement devient false (le timer ne peut pas se lancer tant que chargement est true
        string Sound = ""; // quand on appuie sur une des touches du piano, sound devient le nom de la touche. (sa sert à vérifier si on appuie sur les touches dansle bonne ordre).
        bool texto = false; // selon si texto est false/true, un texte différent s'affiche quand on parle au pnj (c'est pour changer entre le texte de la première épreuve et la deixième)
        bool épreuve2 = false; // false si on à pas réussi la deuxième épreuve / true si on a réussi la deuxième épreuve.
        bool soulé = false; // soulé devien true si il doit fermer le jeu après lui avoir parler trop de fois.
        bool pays = false; // false si on a pas encore réussi l'épreuve des pays / true si on l'a réussie.
        bool juste = true; // true si on n'a pas eu faux plus d'une fois / false si on à eu faux plus d'une foix
        bool salle2 = false; // false si on a pas accès à la salle 2 / true si on y a accès
        bool cool = false; // false si on peut encore vérifier si le fichier "Victoire.txt" exist / true si on ne peut plus.
        bool connaissances = false; // false si on a pas lu la feuille / true si on l'a lue
        bool move = true; // false si on ne peut bouger / true si on peut bouger
        bool Marteau = false; // false si on ne possède pas le marteau / true si on le possède
        bool parler = false; // false si on peut pas parler au pnj / true si on peut
        int rage2 = 0; // il y a deux moments ou il y a la rage, il fonctionne comme "rage"
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\games"; // récupère le chemin d'accès de mon jeu ( je l'utilise pour crée et trouver les fichier pour les events)
        bool interact = false; // false si E n'est pas pressé / true si E est pressé
        int sleep = 0; // force a appuyer deux fois sur le bocal après l'avoir cassé
        bool bocalcassé = false; // false si le bocal est intacte / true si il est cassé
        bool question1 = false; // false si on n'a pas encor juste à la question 1 / true si on a juste
        bool labelmr = true; // false le label du marteau n'est pas visible / si true il est visible
        bool labelcl = true;// false le label du bocal n'est pas visible / si true il est visible
        bool moveLeft, moveRight, moveUp, moveDown; // initialisation des mouvements du personnage.
        string textUse; // variable contenant le texte pour le "CustomMessageBox"
        bool pianocasse = false;
        
        // le custom messagebox est une alternative au messagebox de VisualStudio, car ceux de vs ne peuvent être modifier outre du texte, donc j'ai fait ces "CustomMessageBox" pour povoir modifier l0'image de fond et les boutons.
        public Form1()
        {
            InitializeComponent();
            epreuve(); // appelle la fonction pour les épreuves
            spawn(); // permet de nous faire apparraître au point de départ
            Form5 frm = new Form5(); // ouvre le form5 (le timer)
            frm.Show();
            panel12.GetType().InvokeMember("DoubleBuffered", // permet de rendre plus fluide le jeu.
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.SetProperty,
            null, panel12, new object[] { true });
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public partial class CustomDialogForm : Form //cération d'un messagebox customisable
        {
            // Cache statique pour l'image de fond
            private static Image backgroundImageCache;

            // Cache pour les tailles de texte précalculées (optimisation du rendu de texte)
            private static Dictionary<string, Size> textSizeCache = new Dictionary<string, Size>();

            // Cache pour les polices (évite de créer plusieurs fois les mêmes polices)
            private static Font labelFont;

            public DialogResult Result { get; private set; }
            private string _Text; // variable pour garder les données de "textUse" et les réutiliser dans CustomDialogForm
            private string _langue;// variable pour garder les données de "langue" et les réutiliser dans CustomDialogForm

            static CustomDialogForm()
            {
                // Initialisation des ressources statiques
                labelFont = new Font("Segoe UI", 10);
            }

            // Méthode statique pour précharger l'image et initialiser les ressources
            public static void PreloadResources()
            {
                if (backgroundImageCache == null)
                {
                    // Charge l'image en mémoire une seule fois
                    backgroundImageCache = Properties.Resources.pixil_frame_0__6_;
                }
            }

            // Méthode optimisée pour calculer la taille du texte (avec mise en cache)
            private static Size GetTextSize(string text, int maxWidth)
            {
                string cacheKey = text + "_" + maxWidth.ToString();

                // Utilise la taille mise en cache si disponible
                if (textSizeCache.ContainsKey(cacheKey))
                    return textSizeCache[cacheKey];

                // Calcule et met en cache la taille pour les prochaines utilisations
                Size textSize;
                using (Bitmap dummyBitmap = new Bitmap(1, 1))
                using (Graphics g = Graphics.FromImage(dummyBitmap))
                {
                    textSize = TextRenderer.MeasureText(g, text, labelFont, new Size(maxWidth, 0), TextFormatFlags.WordBreak);
                }

                // Stocke dans le cache (limite la taille du cache à 100 entrées)
                if (textSizeCache.Count > 100)
                {
                    // Simple stratégie : vide le cache s'il devient trop grand
                    textSizeCache.Clear();
                }
                textSizeCache[cacheKey] = textSize;

                return textSize;
            }

            public CustomDialogForm(string text, string langue)
            {
                // Configuration initiale avec double buffering pour éviter les scintillements
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint,
                              true);

                this.SuspendLayout();
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                // Utilise l'image du cache
                if (backgroundImageCache == null)
                    backgroundImageCache = Properties.Resources.pixil_frame_0__6_;

                this.BackgroundImage = backgroundImageCache;

                _Text = text;
                _langue = langue;

                // Constantes pour le layout
                const int maxWidth = 400;
                const int padding = 20;

                // Utilise la fonction optimisée pour calculer la taille du texte
                Size textSizes = GetTextSize(_Text, maxWidth);

                // Calcul optimisé des dimensions
                int formWidth = textSizes.Width + 2 * padding;
                int formHeight = textSizes.Height + 30 + 3 * padding; // 30 = hauteur bouton

                // Définit la taille du formulaire immédiatement
                this.ClientSize = new Size(formWidth, formHeight);

                // Création du label avec les dimensions précalculées
                Label lblMessage = new Label
                {
                    Text = _Text,
                    AutoSize = false,
                    Size = textSizes,
                    Location = new Point(padding, padding),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = labelFont,
                    ForeColor = Color.Black,
                    UseMnemonic = false // Optimisation: désactive le traitement des mnémoniques (&)
                };

                // Création du bouton (optimisée)
                Button btnOK = new Button
                {
                    DialogResult = DialogResult.OK,
                    Width = 100,
                    Height = 30,
                    Location = new Point((formWidth - 100) / 2, lblMessage.Bottom + padding),
                    BackColor = Color.FromArgb(0x4CAF50),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Text = GetButtonText(langue)
                };
                int newSize = 12;
                // Délégués pré-alloués pour éviter les créations multiples
                lblMessage.MouseMove += (s, e) => lblMessage.ForeColor = Color.Blue;
                lblMessage.MouseLeave += (s, e) => lblMessage.ForeColor = Color.Black;
                btnOK.MouseMove += (s, e) => btnOK.Font = new Font(btnOK.Font.FontFamily, newSize);
                btnOK.MouseLeave += (s, e) => btnOK.Font = new Font(btnOK.Font.FontFamily, 8);

                // Utilisation de Controls.AddRange pour ajouter tous les contrôles en une seule fois
                this.Controls.AddRange(new Control[] { lblMessage, btnOK });
                this.AcceptButton = btnOK;


                this.ResumeLayout(false);
            }

            // Méthode helper pour déterminer le texte du bouton selon la langue
            private string GetButtonText(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    case "es": return "De acuerdo";
                    case "ar": return "موافق";
                    default: return "OK"; // fr, en, etc.
                }
            }

            // Optimisation: évite les redessins inutiles
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x02000000;
                    return cp;
                }
            }

            public static DialogResult Show(string text, string langue)
            {
                // Précharge les ressources si nécessaire
                EnsureResourcesLoaded();

                using (CustomDialogForm form = new CustomDialogForm(text, langue))
                {
                    return form.ShowDialog();
                }
            }

            // Version asynchrone préférée pour ne pas bloquer l'interface
            public static async Task<DialogResult> ShowAsync(string text, string langue)
            {
                // Précharge les ressources de manière asynchrone
                await Task.Run(() => EnsureResourcesLoaded());

                // Optimisation: précalcule la taille du texte en arrière-plan
                await Task.Run(() => GetTextSize(text, 400));

                // Utilise TaskCompletionSource pour exécuter ShowDialog de manière asynchrone
                TaskCompletionSource<DialogResult> tcs = new TaskCompletionSource<DialogResult>();

                Form mainForm = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;

                if (mainForm != null && !mainForm.IsDisposed)
                {
                    mainForm.BeginInvoke(new Action(() =>
                    {
                        using (CustomDialogForm form = new CustomDialogForm(text, langue))
                        {
                            DialogResult result = form.ShowDialog(mainForm);
                            tcs.SetResult(result);
                        }
                    }));
                }
                else
                {
                    // Fallback si aucun formulaire principal n'est disponible
                    await Task.Run(() =>
                    {
                        using (CustomDialogForm form = new CustomDialogForm(text, langue))
                        {
                            DialogResult result = form.ShowDialog();
                            tcs.SetResult(result);
                        }
                    });
                }

                return await tcs.Task;
            }

            // S'assure que toutes les ressources sont chargées
            private static void EnsureResourcesLoaded()
            {
                if (backgroundImageCache == null)
                    PreloadResources();

                if (labelFont == null)
                    labelFont = new Font("Segoe UI", 10);
            }
        }
        public partial class CustomDialogForm3: Form
        {
            // Cache statique pour l'image de fond
            private static Image backgroundImageCache;

            // Cache pour les tailles de texte précalculées (optimisation du rendu de texte)
            private static Dictionary<string, Size> textSizeCache = new Dictionary<string, Size>();

            // Cache pour les polices (évite de créer plusieurs fois les mêmes polices)
            private static Font labelFont;

            public DialogResult Result { get; private set; }
            private string _Text;
            private string _langue;

            static CustomDialogForm3()
            {
                // Initialisation des ressources statiques
                labelFont = new Font("Segoe UI", 10);
            }

            // Méthode statique pour précharger l'image et initialiser les ressources
            public static void PreloadResources()
            {
                if (backgroundImageCache == null)
                {
                    // Charge l'image en mémoire une seule fois
                    backgroundImageCache = Properties.Resources.pixil_frame_0__6_;
                }
            }

            // Méthode optimisée pour calculer la taille du texte (avec mise en cache)
            private static Size GetTextSize(string text, int maxWidth)
            {
                string cacheKey = text + "_" + maxWidth.ToString();

                // Utilise la taille mise en cache si disponible
                if (textSizeCache.ContainsKey(cacheKey))
                    return textSizeCache[cacheKey];

                // Calcule et met en cache la taille pour les prochaines utilisations
                Size textSize;
                using (Bitmap dummyBitmap = new Bitmap(1, 1))
                using (Graphics g = Graphics.FromImage(dummyBitmap))
                {
                    textSize = TextRenderer.MeasureText(g, text, labelFont, new Size(maxWidth, 0), TextFormatFlags.WordBreak);
                }

                // Stocke dans le cache (limite la taille du cache à 100 entrées)
                if (textSizeCache.Count > 100)
                {
                    // Simple stratégie : vide le cache s'il devient trop grand
                    textSizeCache.Clear();
                }
                textSizeCache[cacheKey] = textSize;

                return textSize;
            }

            public CustomDialogForm3(string text, string langue)
            {
                // Configuration initiale avec double buffering pour éviter les scintillements
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint,
                              true);

                this.SuspendLayout();
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                // Utilise l'image du cache
                if (backgroundImageCache == null)
                    backgroundImageCache = Properties.Resources.pixil_frame_0__6_;

                this.BackgroundImage = backgroundImageCache;

                _Text = text;
                _langue = langue;

                // Constantes pour le layout
                const int maxWidth = 400;
                const int padding = 20;

                // Utilise la fonction optimisée pour calculer la taille du texte
                Size textSizes = GetTextSize(_Text, maxWidth);
                this.MinimumSize = new System.Drawing.Size(170, 20);
                // Calcul optimisé des dimensions
                int formWidth = textSizes.Width + 2 * padding;
                int formHeight = textSizes.Height + 30 + 3 * padding; // 30 = hauteur bouton

                // Définit la taille du formulaire immédiatement
                this.ClientSize = new Size(formWidth, formHeight);

                // Création du label avec les dimensions précalculées
                Label lblMessage = new Label
                {
                    Text = _Text,
                    AutoSize = false,
                    Size = textSizes,
                    Location = new Point(+25, padding),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = labelFont,
                    ForeColor = Color.Black,
                    UseMnemonic = false // Optimisation: désactive le traitement des mnémoniques (&)
                };
                Button btnNo = new Button
                {
                    DialogResult = DialogResult.No,
                    Width = 70,
                    Height = 30,
                    Location = new Point((formWidth +0) / 2, lblMessage.Bottom + padding),
                    BackColor = Color.FromArgb(0x4CAF50),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Text = GetButtonText2(langue)
                };

                // Création du bouton (optimisée)
                Button btnOK = new Button
                {
                    DialogResult = DialogResult.Yes,
                    Width = 70,
                    Height = 30,
                    Location = new Point((formWidth - 150) / 2, lblMessage.Bottom + padding),
                    BackColor = Color.FromArgb(0x4CAF50),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Text = GetButtonText(langue)
                };
                int newSize = 12;
                // Délégués pré-alloués pour éviter les créations multiples
                lblMessage.MouseMove += (s, e) => lblMessage.ForeColor = Color.Blue;
                lblMessage.MouseLeave += (s, e) => lblMessage.ForeColor = Color.Black;
                btnNo.MouseMove += (s, e) => btnNo.Font = new Font(btnNo.Font.FontFamily, newSize);
                btnNo.MouseLeave += (s, e) => btnNo.Font = new Font(btnNo.Font.FontFamily, 8);
                btnOK.MouseMove += (s, e) => btnOK.Font = new Font(btnOK.Font.FontFamily, newSize);
                btnOK.MouseLeave += (s, e) => btnOK.Font = new Font(btnOK.Font.FontFamily, 8);

                // Utilisation de Controls.AddRange pour ajouter tous les contrôles en une seule fois
                this.Controls.AddRange(new Control[] { lblMessage, btnOK, btnNo });


                this.ResumeLayout(false);
            }

            // Méthode helper pour déterminer le texte du bouton selon la langue
            private string GetButtonText(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    default: return "Yes"; // fr, en, etc.
                }
            }
            private string GetButtonText1(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    default: return "Cancel"; // fr, en, etc.
                }
            }
            private string GetButtonText2(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    default: return "No"; // fr, en, etc.
                }
            }

            // Optimisation: évite les redessins inutiles
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x02000000; // fluidifie
                    return cp;
                }
            }

            public static DialogResult Show(string text, string langue)
            {
                // Précharge les ressources si nécessaire
                EnsureResourcesLoaded();

                using (CustomDialogForm3 form = new CustomDialogForm3(text, langue))
                {
                    return form.ShowDialog();
                }
            }

            // Version asynchrone préférée pour ne pas bloquer l'interface
            public static async Task<DialogResult> ShowAsync(string text, string langue)
            {
                // Précharge les ressources de manière asynchrone
                await Task.Run(() => EnsureResourcesLoaded());

                // Optimisation: précalcule la taille du texte en arrière-plan
                await Task.Run(() => GetTextSize(text, 400));

                // Utilise TaskCompletionSource pour exécuter ShowDialog de manière asynchrone
                TaskCompletionSource<DialogResult> tcs = new TaskCompletionSource<DialogResult>();

                Form mainForm = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;

                if (mainForm != null && !mainForm.IsDisposed)
                {
                    mainForm.BeginInvoke(new Action(() =>
                    {
                        using (CustomDialogForm3 form = new CustomDialogForm3(text, langue))
                        {
                            DialogResult result = form.ShowDialog(mainForm);
                            tcs.SetResult(result);
                        }
                    }));
                }
                else
                {
                    // Fallback si aucun formulaire principal n'est disponible
                    await Task.Run(() =>
                    {
                        using (CustomDialogForm3 form = new CustomDialogForm3(text, langue))
                        {
                            DialogResult result = form.ShowDialog();
                            tcs.SetResult(result);
                        }
                    });
                }

                return await tcs.Task;
            }

            // S'assure que toutes les ressources sont chargées
            private static void EnsureResourcesLoaded()
            {
                if (backgroundImageCache == null)
                    PreloadResources();

                if (labelFont == null)
                    labelFont = new Font("Segoe UI", 10);
            }
        }
        public partial class CustomDialogForm2 : Form
        {
            // Cache statique pour l'image de fond
            private static Image backgroundImageCache;

            // Cache pour les tailles de texte précalculées (optimisation du rendu de texte)
            private static Dictionary<string, Size> textSizeCache = new Dictionary<string, Size>();

            // Cache pour les polices (évite de créer plusieurs fois les mêmes polices)
            private static Font labelFont;

            public DialogResult Result { get; private set; }
            private string _Text;
            private string _langue;

            static CustomDialogForm2()
            {
                // Initialisation des ressources statiques
                labelFont = new Font("Segoe UI", 10);
            }

            // Méthode statique pour précharger l'image et initialiser les ressources
            public static void PreloadResources()
            {
                if (backgroundImageCache == null)
                {
                    // Charge l'image en mémoire une seule fois
                    backgroundImageCache = Properties.Resources.pixil_frame_0__6_;
                }
            }

            // Méthode optimisée pour calculer la taille du texte (avec mise en cache)
            private static Size GetTextSize(string text, int maxWidth)
            {
                string cacheKey = text + "_" + maxWidth.ToString();

                // Utilise la taille mise en cache si disponible
                if (textSizeCache.ContainsKey(cacheKey))
                    return textSizeCache[cacheKey];

                // Calcule et met en cache la taille pour les prochaines utilisations
                Size textSize;
                using (Bitmap dummyBitmap = new Bitmap(1, 1))
                using (Graphics g = Graphics.FromImage(dummyBitmap))
                {
                    textSize = TextRenderer.MeasureText(g, text, labelFont, new Size(maxWidth, 0), TextFormatFlags.WordBreak);
                }

                // Stocke dans le cache (limite la taille du cache à 100 entrées)
                if (textSizeCache.Count > 100)
                {
                    // Simple stratégie : vide le cache s'il devient trop grand
                    textSizeCache.Clear();
                }
                textSizeCache[cacheKey] = textSize;

                return textSize;
            }

            public CustomDialogForm2(string text, string langue)
            {
                // Configuration initiale avec double buffering pour éviter les scintillements
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                              ControlStyles.AllPaintingInWmPaint |
                              ControlStyles.UserPaint,
                              true);

                this.SuspendLayout();
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.CenterParent;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                // Utilise l'image du cache
                if (backgroundImageCache == null)
                    backgroundImageCache = Properties.Resources.pixil_frame_0__6_;

                this.BackgroundImage = backgroundImageCache;

                _Text = text;
                _langue = langue;

                // Constantes pour le layout
                const int maxWidth = 400;
                const int padding = 20;

                // Utilise la fonction optimisée pour calculer la taille du texte
                Size textSizes = GetTextSize(_Text, maxWidth);
                this.MinimumSize = new System.Drawing.Size(270, 20);
                // Calcul optimisé des dimensions
                int formWidth = textSizes.Width + 2 * padding;
                int formHeight = textSizes.Height + 30 + 3 * padding; // 30 = hauteur bouton

                // Définit la taille du formulaire immédiatement
                this.ClientSize = new Size(formWidth, formHeight);

                // Création du label avec les dimensions précalculées
                Label lblMessage = new Label
                {
                    Text = _Text,
                    AutoSize = false,
                    Size = textSizes,
                    Location = new Point(+50, padding),
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = labelFont,
                    ForeColor = Color.Black,
                    UseMnemonic = false // Optimisation: désactive le traitement des mnémoniques (&)
                };
                Button btnCancel = new Button
                {
                    DialogResult = DialogResult.Cancel,
                    Width = 70,
                    Height = 30,
                    Location = new Point((formWidth + 165) / 2, lblMessage.Bottom + padding),
                    BackColor = Color.FromArgb(0x4CAF50),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Text = GetButtonText1(langue)
                };
                Button btnNo = new Button
                {
                    DialogResult = DialogResult.No,
                    Width = 70,
                    Height = 30,
                    Location = new Point((formWidth + 0) / 2, lblMessage.Bottom + padding),
                    BackColor = Color.FromArgb(0x4CAF50),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Text = GetButtonText2(langue)
                };

                // Création du bouton (optimisée)
                Button btnOK = new Button
                {
                    DialogResult = DialogResult.OK,
                    Width = 70,
                    Height = 30,
                    Location = new Point((formWidth - 165) / 2, lblMessage.Bottom + padding),
                    BackColor = Color.FromArgb(0x4CAF50),
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    FlatAppearance = { BorderSize = 0 },
                    Text = GetButtonText(langue)
                };
                int newSize = 12;
                // Délégués pré-alloués pour éviter les créations multiples
                lblMessage.MouseMove += (s, e) => lblMessage.ForeColor = Color.Blue;
                lblMessage.MouseLeave += (s, e) => lblMessage.ForeColor = Color.Black;
                btnCancel.MouseMove += (s, e) => btnCancel.Font = new Font(btnCancel.Font.FontFamily, newSize);
                btnCancel.MouseLeave += (s, e) => btnCancel.Font = new Font(btnCancel.Font.FontFamily, 8);
                btnNo.MouseMove += (s, e) => btnNo.Font = new Font(btnNo.Font.FontFamily, newSize);
                btnNo.MouseLeave += (s, e) => btnNo.Font = new Font(btnNo.Font.FontFamily, 8);
                btnOK.MouseMove += (s, e) => btnOK.Font = new Font(btnOK.Font.FontFamily, newSize);
                btnOK.MouseLeave += (s, e) => btnOK.Font = new Font(btnOK.Font.FontFamily, 8);

                // Utilisation de Controls.AddRange pour ajouter tous les contrôles en une seule fois
                this.Controls.AddRange(new Control[] { lblMessage, btnOK, btnCancel, btnNo });


                this.ResumeLayout(false);
            }

            // Méthode helper pour déterminer le texte du bouton selon la langue
            private string GetButtonText(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    default: return "Yes"; // fr, en, etc.
                }
            }
            private string GetButtonText1(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    default: return "Cancel"; // fr, en, etc.
                }
            }
            private string GetButtonText2(string langue)
            {
                switch (langue.ToLowerInvariant())
                {
                    default: return "No"; // fr, en, etc.
                }
            }

            // Optimisation: évite les redessins inutiles
            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x02000000; // fluidifie
                    return cp;
                }
            }

            public static DialogResult Show(string text, string langue)
            {
                // Précharge les ressources si nécessaire
                EnsureResourcesLoaded();

                using (CustomDialogForm2 form = new CustomDialogForm2(text, langue))
                {
                    return form.ShowDialog();
                }
            }

            // Version asynchrone préférée pour ne pas bloquer l'interface
            public static async Task<DialogResult> ShowAsync(string text, string langue)
            {
                // Précharge les ressources de manière asynchrone
                await Task.Run(() => EnsureResourcesLoaded());

                // Optimisation: précalcule la taille du texte en arrière-plan
                await Task.Run(() => GetTextSize(text, 400));

                // Utilise TaskCompletionSource pour exécuter ShowDialog de manière asynchrone
                TaskCompletionSource<DialogResult> tcs = new TaskCompletionSource<DialogResult>();

                Form mainForm = Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;

                if (mainForm != null && !mainForm.IsDisposed)
                {
                    mainForm.BeginInvoke(new Action(() =>
                    {
                        using (CustomDialogForm2 form = new CustomDialogForm2(text, langue))
                        {
                            DialogResult result = form.ShowDialog(mainForm);
                            tcs.SetResult(result);
                        }
                    }));
                }
                else
                {
                    // Fallback si aucun formulaire principal n'est disponible
                    await Task.Run(() =>
                    {
                        using (CustomDialogForm2 form = new CustomDialogForm2(text, langue))
                        {
                            DialogResult result = form.ShowDialog();
                            tcs.SetResult(result);
                        }
                    });
                }

                return await tcs.Task;
            }

            // S'assure que toutes les ressources sont chargées
            private static void EnsureResourcesLoaded()
            {
                if (backgroundImageCache == null)
                    PreloadResources();

                if (labelFont == null)
                    labelFont = new Font("Segoe UI", 10);
            }
        }
        private void Key_Down(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.A)// Fonction permettant d'utiliser la touche écrite.
            {
                moveLeft = true;
            }
            if (e.KeyCode == Keys.D)
            {
             moveRight = true;            
            }
            if (e.KeyCode == Keys.W)
            {
                moveUp = true;
            }
            if (e.KeyCode == Keys.S)
            {
                moveDown = true;
            }
            if (e.KeyCode == Keys.E)
            {
                interact = true; // si interact est égale à true ça veux dire que la touche E est appuyée
                action();
            }
            if (e.KeyCode == Keys.Escape && menu == false) // ouvre le menu

            {
                menu = true; 
                men();
                return;
            }
            if (e.KeyCode == Keys.Escape && menu == true) // ferme le menu
            {
                menu = false;
                men();
                return; 
            }

        }

        private void marteau_Click(object sender, EventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Key_Up(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) // Fonction permettant de mettre fin à la fonction de Key_Down (selon la touche)
            {
                moveLeft = false;
                
            }
            if (e.KeyCode == Keys.D)
            {
                moveRight = false;
            }
            if (e.KeyCode == Keys.W)
            {
                moveUp = false;
            }
            if (e.KeyCode == Keys.S)
            {
                moveDown = false;
            }
            if (e.KeyCode == Keys.E)
            {
                interact = false; // si interact est égale à false ça veut dire que la touche E n'est pas appuyée
            }

            
        }
        


        private void button1_Click(object sender, EventArgs e) // fonction du clique du "button1"
        {
            if (soulé == false){ // si on a cliqué sur le "button1" et que soulé est = à false. la fonction se fait
            button1.Visible = false;
            button2.Visible = false;
                stopRun(); // appelle la fonction StopRun (pour enlever un bug qui nous fait marcher à l'infini)
                if (langue == "fr"){ // si langue == fr (pour le Français)
                    textUse = "???\n\nDonc commençons, mais avant ça je tiens à me présenter !";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "???\n\nJe m'appelles LE SORCIER FOU !";
                    var r = CustomDialogForm.Show(textUse, langue);
                    textUse = "LE SORCIER FOU !\n\nPremière épreuve !\nQuel est le pays le plus grand du monde ?";
                    var a = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")// si langue == es (pour l'espagnole)
                {
                    textUse = "???\n\nEmpecemos, pero antes me gustaría presentarme.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "???\n\n¡Me llamo LA BRUJA LOCA!";
                    var r = CustomDialogForm.Show(textUse, langue);
                    textUse = "¡EL MAGO LOCO!\n\n¡Primera prueba!\n¿Cuál es el país más grande del mundo?";
                    var a = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")// si langue == en (pour l'anglais)
                {
                    textUse = "???\n\nSo let's get started, but before that I want to introduce myself!";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "???\n\nMy name is THE MAD SORCERER!";
                    var r = CustomDialogForm.Show(textUse, langue);
                    textUse = "THE MAD SORCERER!\n\nFirst test!\nWhat is the largest country in the world?";
                    var a = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")// si langue == ar (pour l'arabe)
                {
                    textUse = "???\n\nلذلك دعونا نبدأ ، لكن قبل ذلك أريد أن أقدم";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "???\n\nاسمي الساحر المجنون";
                    var r = CustomDialogForm.Show(textUse, langue);
                    textUse = " الساحر المجنون\n\nالاختبار الأول\nما هي أكبر دولة في العالم؟";
                    var a = CustomDialogForm.Show(textUse, langue);
                }
                canRun();
                if (langue == "fr"){ 
                    Rep1.Text = "Suisse";
                    Rep2.Text = "Russie";
                    Rep3.Text = "Groenland";
                    Rep4.Text = "Canada";
                }
                if (langue == "en")
                {
                    Rep1.Text = "Switzerland";
                    Rep2.Text = "Russia";
                    Rep3.Text = "Greenland";
                    Rep4.Text = "Canada";
                }
                if (langue == "es")
                {
                    Rep1.Text = "Suiza";
                    Rep2.Text = "Rusia ";
                    Rep3.Text = "Groenlandia ";
                    Rep4.Text = "Canadá";
                }
                if (langue == "ar")
                {
                    Rep1.Text = "سويسرا";
                    Rep2.Text = "روسيا";
                    Rep3.Text = "جرينلاند";
                    Rep4.Text = "كندا";
                }
                Rep1.Visible = true;
            Rep2.Visible = true;
            Rep3.Visible = true;
            Rep4.Visible = true;
            Question.Visible = true;
            pays = true;
                réponses();
            }
            else // si on refait le quiz parce qu'on a eu faux
            {

                button1.Visible = false;
                button2.Visible = false;
                stopRun();
                if (langue == "fr"){
                        textUse = "LE SORCIER FOU !\n\nDonc commençons !";
                        var result = CustomDialogForm.Show(textUse, langue);
                        textUse = "LE SORCIER FOU !\n\nPremière épreuve...\nQuel est le pays le plus grand du monde ?";
                        var r = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "الساحر المجنون\n\nلذا دعونا نبدأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "الساحر المجنون\n\n...الاختبار الأول\nهي أكبر دولة في العالم؟";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "THE MAD WIZARD!\n\nSo let's get started!";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "THE MAD WIZARD!\n\nFirst test...\nWhat is the largest country in the world?";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "¡EL HECHICERO LOCO!\n\nAsí que empecemos.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    textUse = "¡EL HECHICERO LOCO!\n\nPrimera prueba...\n¿Cuál es el país más grande del mundo?";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                canRun();
                if (langue == "fr")
                {
                    Rep1.Text = "Suisse";
                    Rep2.Text = "Russie";
                    Rep3.Text = "Groenland";
                    Rep4.Text = "Canada";
                }
                if (langue == "en")
                {
                    Rep1.Text = "Switzerland";
                    Rep2.Text = "Russia";
                    Rep3.Text = "Greenland";
                    Rep4.Text = "Canada";
                }
                if (langue == "es")
                {
                    Rep1.Text = "Suiza";
                    Rep2.Text = "Rusia ";
                    Rep3.Text = "Groenlandia ";
                    Rep4.Text = "Canadá";
                }
                if (langue == "ar")
                {
                    Rep1.Text = "سويسرا";
                    Rep2.Text = "روسيا";
                    Rep3.Text = "جرينلاند";
                    Rep4.Text = "كندا";
                }
                Rep1.Visible = true; //devient visible
                Rep2.Visible = true;
                Rep3.Visible = true;
                Rep4.Visible = true;
                Rep1.Enabled = true; // refonctionne
                Rep2.Enabled = true;
                Rep3.Enabled = true;
                Rep4.Enabled = true;
                Question.Visible = true;
                pays = true;
                réponses(); // appelle la fonction "réponses"
            }
        }
        private void réponses () //Fonction qui permet de gérer les réponses de l'utilisateur aux questions posées."
        {
            question1 = true;
            if (pays == true && soulé == false) // si l'épreuve des pays est débloquée et que le pnj n'est pas soulé
            {

                Rep1.Click += async (s, e) => { // si on clique sur Rep1 (la mauvaise réponse)
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Faux !";
                        Rep2.Text = "Faux !";
                        Rep3.Text = "Faux !";
                        Rep4.Text = "Faux !";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "خطأ!";
                        Rep2.Text = "خطأ!";
                        Rep3.Text = "خطأ!";
                        Rep4.Text = "خطأ!";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "False !";
                        Rep2.Text = "False !";
                        Rep3.Text = "False !";
                        Rep4.Text = "False !";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "¡Incorrecto!";
                        Rep2.Text = "¡Incorrecto!";
                        Rep3.Text = "¡Incorrecto!";
                        Rep4.Text = "¡Incorrecto!";
                    }
                    await Task.Delay(2000); // crée une tâche invisible de 2seconde
                    Rep1.Enabled = true;
                    Rep2.Enabled = true;
                    Rep3.Enabled = true;
                    Rep4.Enabled = true;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Suisse";
                        Rep2.Text = "Russie";
                        Rep3.Text = "Groenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "Switzerland";
                        Rep2.Text = "Russia";
                        Rep3.Text = "Greenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "Suiza";
                        Rep2.Text = "Rusia ";
                        Rep3.Text = "Groenlandia ";
                        Rep4.Text = "Canadá";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "سويسرا";
                        Rep2.Text = "روسيا";
                        Rep3.Text = "جرينلاند";
                        Rep4.Text = "كندا";
                    }
                };
                Rep2.Click +=  async (s, e) => // si on clique sur Rep2 (la bonne réponse)
                {
                    if (soulé == false){ // si soulé est égale à false
                    panel10.Visible = true;
                    label4.Visible = true;
                    Rep1.Visible = false;
                    Rep2.Visible = false;
                    Rep3.Visible = false;
                    Rep4.Visible = false;
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    await Task.Delay(1000); // tâche invisible d'une seconde
                    panel10.Visible = false;
                    label4.Visible = false;
                    Question.Visible = false;
                        
                        stopRun();
                        if (langue == "fr"){
                            textUse = "LE SORCIER FOU !\n\nMainenent que vous avez réussi la deuxième épreuve...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            var rx= CustomDialogForm.Show(textUse, langue);
                            textUse = "LE SORCIER FOU !\n\nON PEUT PASSER À LA TROISIÈME ÉPREUVE !!!";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "LE SORCIER FOU !\n\nQui est le créateur de cet univer ?!";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡EL HECHICERO LOCO!\n\nAhora que has pasado la segunda prueba...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            textUse = "¡EL HECHICERO LOCO!\n\n¡¡¡PODEMOS PASAR A LA SEGUNDA RONDA!!!";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "¡EL HECHICERO LOCO!\n\nQuién es el creador de este universo?";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "الساحر المجنون\n\n...والآن بعد أن اجتزت الاختبار الثاني";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            textUse = "الساحر المجنون\n\nيمكننا الانتقال إلى الجولة الثانية";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "الساحر المجنون\n\nمن هو خالق هذا الكون؟";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "THE MAD WIZARD!\n\nNow that you've passed the second test...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            textUse = "THE MAD WIZARD!\n\nWE CAN MOVE ON TO THE SECOND TEST!!!";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "THE MAD WIZARD!\n\nWho is the creator of this universe?!";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun(); // appelle la fonction qui permet de recommencer à marcher
                        Question2.Visible = true;
                        textBox1.Visible = true;
                        button3.Visible = true;
                    }
                };
                Rep3.Click += async (s, e) => //(la mauvaise réponse)
                {
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Faux !";
                        Rep2.Text = "Faux !";
                        Rep3.Text = "Faux !";
                        Rep4.Text = "Faux !";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "خطأ!";
                        Rep2.Text = "خطأ!";
                        Rep3.Text = "خطأ!";
                        Rep4.Text = "خطأ!";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "False !";
                        Rep2.Text = "False !";
                        Rep3.Text = "False !";
                        Rep4.Text = "False !";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "¡Incorrecto!";
                        Rep2.Text = "¡Incorrecto!";
                        Rep3.Text = "¡Incorrecto!";
                        Rep4.Text = "¡Incorrecto!";
                    }
                    await Task.Delay(2000);
                    Rep1.Enabled = true;
                    Rep2.Enabled = true;
                    Rep3.Enabled = true;
                    Rep4.Enabled = true;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Suisse";
                        Rep2.Text = "Russie";
                        Rep3.Text = "Groenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "Switzerland";
                        Rep2.Text = "Russia";
                        Rep3.Text = "Greenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "Suiza";
                        Rep2.Text = "Rusia ";
                        Rep3.Text = "Groenlandia ";
                        Rep4.Text = "Canadá";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "سويسرا";
                        Rep2.Text = "روسيا";
                        Rep3.Text = "جرينلاند";
                        Rep4.Text = "كندا";
                    }
                };
                Rep4.Click += async (s, e) => //(la mauvaise réponse)
                {
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Faux !";
                        Rep2.Text = "Faux !";
                        Rep3.Text = "Faux !";
                        Rep4.Text = "Faux !";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "خطأ!";
                        Rep2.Text = "خطأ!";
                        Rep3.Text = "خطأ!";
                        Rep4.Text = "خطأ!";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "False !";
                        Rep2.Text = "False !";
                        Rep3.Text = "False !";
                        Rep4.Text = "False !";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "¡Incorrecto!";
                        Rep2.Text = "¡Incorrecto!";
                        Rep3.Text = "¡Incorrecto!";
                        Rep4.Text = "¡Incorrecto!";
                    }
                    await Task.Delay(2000);
                    Rep1.Enabled = true;
                    Rep2.Enabled = true;
                    Rep3.Enabled = true;
                    Rep4.Enabled = true;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Suisse";
                        Rep2.Text = "Russie";
                        Rep3.Text = "Groenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "Switzerland";
                        Rep2.Text = "Russia";
                        Rep3.Text = "Greenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "Suiza";
                        Rep2.Text = "Rusia ";
                        Rep3.Text = "Groenlandia ";
                        Rep4.Text = "Canadá";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "سويسرا";
                        Rep2.Text = "روسيا";
                        Rep3.Text = "جرينلاند";
                        Rep4.Text = "كندا";
                    }
                };
            }
            if (pays == true && soulé == true)
            {

                Rep1.Click += async (s, e) => { // (la mauvaise réponse)
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Faux !";
                        Rep2.Text = "Faux !";
                        Rep3.Text = "Faux !";
                        Rep4.Text = "Faux !";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "خطأ!";
                        Rep2.Text = "خطأ!";
                        Rep3.Text = "خطأ!";
                        Rep4.Text = "خطأ!";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "False !";
                        Rep2.Text = "False !";
                        Rep3.Text = "False !";
                        Rep4.Text = "False !";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "¡Incorrecto!";
                        Rep2.Text = "¡Incorrecto!";
                        Rep3.Text = "¡Incorrecto!";
                        Rep4.Text = "¡Incorrecto!";
                    }
                    await Task.Delay(2000);
                    Rep1.Enabled = true;
                    Rep2.Enabled = true;
                    Rep3.Enabled = true;
                    Rep4.Enabled = true;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Suisse";
                        Rep2.Text = "Russie";
                        Rep3.Text = "Groenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "Switzerland";
                        Rep2.Text = "Russia";
                        Rep3.Text = "Greenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "Suiza";
                        Rep2.Text = "Rusia ";
                        Rep3.Text = "Groenlandia ";
                        Rep4.Text = "Canadá";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "سويسرا";
                        Rep2.Text = "روسيا";
                        Rep3.Text = "جرينلاند";
                        Rep4.Text = "كندا";
                    }
                };
                Rep2.Click += async (s, e) =>
                {
                    if (soulé == true){ // (la bonne réponse)
                    panel10.Visible = true;
                    label4.Visible = true;
                    Rep1.Visible = false;
                    Rep2.Visible = false;
                    Rep3.Visible = false;
                    Rep4.Visible = false;
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    await Task.Delay(1000);
                    panel10.Visible = false;
                    label4.Visible = false;
                    Question.Visible = false;
                        stopRun();
                        if (langue == "fr"){

                            textUse = "LE SORCIER FOU !\n\nMainenent que vous avez réussi la deuxième épreuve...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            var rx = CustomDialogForm.Show(textUse, langue);
                            textUse = "LE SORCIER FOU !\n\nON PEUT PASSER À LA TROISIÈME ÉPREUVE !!!";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "LE SORCIER FOU !\n\nQui est le créateur de cet univer ?!";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "THE MAD WIZARD!\n\nNow that you've passed the second test...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            textUse = "THE MAD WIZARD!\n\nWE CAN MOVE ON TO THE SECOND TEST!!!";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "THE MAD WIZARD!\n\nWho is the creator of this universe?!";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "الساحر المجنون\n\n...والآن بعد أن اجتزت الاختبار الثاني";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            textUse = "الساحر المجنون\n\nيمكننا الانتقال إلى الجولة الثانية";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "الساحر المجنون\n\nمن هو خالق هذا الكون؟";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡EL HECHICERO LOCO!\n\nAhora que has pasado la segunda prueba...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "...";
                            var r = CustomDialogForm.Show(textUse, langue);
                            var a = CustomDialogForm.Show(textUse, langue);
                            var b = CustomDialogForm.Show(textUse, langue);
                            textUse = "¡EL HECHICERO LOCO!\n\n¡¡¡PODEMOS PASAR A LA SEGUNDA RONDA!!!";
                            var raw = CustomDialogForm.Show(textUse, langue);
                            textUse = "¡EL HECHICERO LOCO!\n\nQuién es el creador de este universo?";
                            var rad = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        textBox1.Visible = true;
                        button3.Visible = true;
                        textBox1.Enabled = true;
                        button3.Enabled = true;
                    }
                };
                Rep3.Click += async (s, e) => //(la mauvaise réponse)
                {
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Faux !";
                        Rep2.Text = "Faux !";
                        Rep3.Text = "Faux !";
                        Rep4.Text = "Faux !";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "خطأ!";
                        Rep2.Text = "خطأ!";
                        Rep3.Text = "خطأ!";
                        Rep4.Text = "خطأ!";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "False !";
                        Rep2.Text = "False !";
                        Rep3.Text = "False !";
                        Rep4.Text = "False !";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "¡Incorrecto!";
                        Rep2.Text = "¡Incorrecto!";
                        Rep3.Text = "¡Incorrecto!";
                        Rep4.Text = "¡Incorrecto!";
                    }
                    await Task.Delay(2000);
                    Rep1.Enabled = true;
                    Rep2.Enabled = true;
                    Rep3.Enabled = true;
                    Rep4.Enabled = true;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Suisse";
                        Rep2.Text = "Russie";
                        Rep3.Text = "Groenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "Switzerland";
                        Rep2.Text = "Russia";
                        Rep3.Text = "Greenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "Suiza";
                        Rep2.Text = "Rusia ";
                        Rep3.Text = "Groenlandia ";
                        Rep4.Text = "Canadá";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "سويسرا";
                        Rep2.Text = "روسيا";
                        Rep3.Text = "جرينلاند";
                        Rep4.Text = "كندا";
                    }
                };
                Rep4.Click += async (s, e) => //(la mauvaise réponse)
                {
                    Rep1.Enabled = false;
                    Rep2.Enabled = false;
                    Rep3.Enabled = false;
                    Rep4.Enabled = false;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Faux !";
                        Rep2.Text = "Faux !";
                        Rep3.Text = "Faux !";
                        Rep4.Text = "Faux !";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "خطأ!";
                        Rep2.Text = "خطأ!";
                        Rep3.Text = "خطأ!";
                        Rep4.Text = "خطأ!";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "False !";
                        Rep2.Text = "False !";
                        Rep3.Text = "False !";
                        Rep4.Text = "False !";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "¡Incorrecto!";
                        Rep2.Text = "¡Incorrecto!";
                        Rep3.Text = "¡Incorrecto!";
                        Rep4.Text = "¡Incorrecto!";
                    }
                    await Task.Delay(2000);
                    Rep1.Enabled = true;
                    Rep2.Enabled = true;
                    Rep3.Enabled = true;
                    Rep4.Enabled = true;
                    if (langue == "fr")
                    {
                        Rep1.Text = "Suisse";
                        Rep2.Text = "Russie";
                        Rep3.Text = "Groenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "en")
                    {
                        Rep1.Text = "Switzerland";
                        Rep2.Text = "Russia";
                        Rep3.Text = "Greenland";
                        Rep4.Text = "Canada";
                    }
                    if (langue == "es")
                    {
                        Rep1.Text = "Suiza";
                        Rep2.Text = "Rusia ";
                        Rep3.Text = "Groenlandia ";
                        Rep4.Text = "Canadá";
                    }
                    if (langue == "ar")
                    {
                        Rep1.Text = "سويسرا";
                        Rep2.Text = "روسيا";
                        Rep3.Text = "جرينلاند";
                        Rep4.Text = "كندا";
                    }
                };
            }
        }
        private void button2_Click(object sender, EventArgs e) // quand le PNJ demande si on veut commencer le quiz et qu'on appuie sur le bouton "non"
        {
            stopRun();
            if (langue == "fr"){
                textUse = "???\n\nReviens vites alors, si tu veux finir ce jeu !";
                var result = CustomDialogForm.Show(textUse, langue);
            }
            if (langue == "en")
            {
                textUse = "???\n\nCome back soon then, if you want to finish this game!";
                var result = CustomDialogForm.Show(textUse, langue);
            }
            if (langue == "es")
            {
                textUse = "???\n\nSi quieres terminar este juego, ¡date prisa!";
                var result = CustomDialogForm.Show(textUse, langue);
            }
            if (langue == "ar")
            {
                textUse = "???\n\nأسرع بالعودة إذن، إذا كنت تريد إنهاء اللقاء";
                var result = CustomDialogForm.Show(textUse, langue);
            }
            button1.Visible = false;
            button2.Visible = false;
            button1.Enabled = false;
            button2.Enabled = false;
            question1 = false;
        }

        private void Rep2_Click(object sender, EventArgs e)
        {

        }

        private void timer_moves_Tick(object sender, EventArgs e) // fonction se faisant chaque 20 milisecondes.
        {
            
            string filname = path +@"\fr.txt"; 
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            

            if (File.Exists(filname)) // vérifie si le fichier entre paranthèse exist.
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
            if (langue == "fr")
            {
                label13.Text = "Crédits";
                label10.Text = "Règles";
                label11.Text = "Il faut s'échapper de ce monde en résolvant toutes les énigmes qui viendrons à se présenter\r\n\r\nSe déplacer : \r\nUtilisez WASD\r\n\r\nLes intéractions se feront toujours avec E";
                label12.Text = "Retour";
                button15.Text = "Règles";
                button14.Text = "Réinitialiser";
                button13.Text = "Langue : Français";
                Question.Text = "Quel est le plus grand pays du monde ?";
                Question2.Text = "Qui est le créateur de cet univer ?";
                button1.Text = "Oui";
                button2.Text = "Non";
                button3.Text = "Répondre";
                string filename = path + @"\fr.txt";
                string filename2 = path + @"\es.txt";
                string filename3 = path + @"\en.txt";
                string filename4 = path + @"\ar.txt";
                if (File.Exists(filename) == false)
                {
                    File.Create(filename).Close(); // Crée le fichier mis entre parentèse "filename" == "fr.txt"
                }
                if (File.Exists(filename2))
                {
                    File.Delete(filename2); // supprime le fichier mis entre parentèse "filename2" == "es.txt"
                }
                if (File.Exists(filename3))
                {
                    File.Delete(filename3);
                }
                if (File.Exists(filename4))
                {
                    File.Delete(filename4);
                }
            }
            if (langue == "es") // Se fait seulement si "langue" est égale à "es" ( pour la langue espagnole )
            {
                label13.Text = "Créditos";
                label10.Text = "Menstruación";
                label11.Text = "Tienes que escapar de este mundo resolviendo todos los acertijos que se te presenten\r\n\r\nMover: \r\nUsar WASD\r\n\r\nLas interacciones siempre serán con E";
                label12.Text = "Devolución";
                button15.Text = "Menstruación";
                button14.Text = "Restablecimiento";
                Question.Text = "¿Cuál es el país más grande del mundo?";
                Question2.Text = "¿Quién es el creador de este universo?";
                button1.Text = "Sí";
                button2.Text = "No";
                button3.Text = "Respuesta";
                string filename = path + @"\fr.txt";
                string filename2 = path + @"\es.txt";
                string filename3 = path + @"\en.txt";
                string filename4 = path + @"\ar.txt";
                if (File.Exists(filename2) == false)
                {
                    File.Create(filename2).Close();
                }
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                if (File.Exists(filename3))
                {
                    File.Delete(filename3);
                }
                if (File.Exists(filename4))
                {
                    File.Delete(filename4);
                }
            }
            if (langue == "en")
            {
                label13.Text = "Credits";
                label10.Text = "Rules";
                label11.Text = "You have to escape from this world by solving all the riddles that come your way\r\n\r\nMove: \r\nUse WASD\r\n\r\nInteractions will always be with E";
                label12.Text = "Return";
                button15.Text = "Rules";
                button14.Text = "Restart";
                Question.Text = "What is the biggest country in the world?";
                Question2.Text = "Who is the creator of this universe?";
                button1.Text = "Yes";
                button2.Text = "No";
                button3.Text = "Answer";
                string filename = path + @"\fr.txt";
                string filename2 = path + @"\es.txt";
                string filename3 = path + @"\en.txt";
                string filename4 = path + @"\ar.txt";
                if (File.Exists(filename3) == false)
                {
                    File.Create(filename3).Close();
                }
                if (File.Exists(filename2))
                {
                    File.Delete(filename2);
                }
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                if (File.Exists(filename4))
                {
                    File.Delete(filename4);
                }
            }
            if (langue == "ar")
            {
                label13.Text = "الائتمانات";
                label10.Text = "حيض";
                label11.Text = "عليك الهروب من هذا العالم من خلال حل كل الألغاز التي تأتي في طريقك\r\n\r\n:حرك\r\nWASD استخدم\r\n\r\nE ستكون التفاعلات دائما مع ";
                label12.Text = "أعاد";
                button15.Text = "حيض";
                button14.Text = "اعاده تعيين";
                Question.Text = "ما هي أكبر دولة في العالم؟";
                Question2.Text = "من هو خالق هذا الكون؟";
                button1.Text = "نعم";
                button2.Text = "لا يوجد";
                button3.Text = "الرد";
                string filename = path + @"\fr.txt";
                string filename2 = path + @"\es.txt";
                string filename3 = path + @"\en.txt";
                string filename4 = path + @"\ar.txt";
                if (File.Exists(filename4) == false)
                {
                    File.Create(filename4).Close();
                }
                if (File.Exists(filename2))
                {
                    File.Delete(filename2);
                }
                if (File.Exists(filename3))
                {
                    File.Delete(filename3);
                }
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }

            foreach (Control x in panel12.Controls) //Se fait pour tout les objet dans le panel12
            {
                if (x is Panel && (string)x.Tag == "Left") // Se fait pour tout les objet dans le panel12 portant le tag "Left"
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))// Se fait si le joueur rentre en contacte avec les objets possible 
                    {
                        player.Left -= 13; // Fait bouger le personnage à gauche. 13 est le chiffre de combien est bouger le joueur

                    }
                    else 
                    {
                    }
                }
               

            }
            foreach (Control x in panel12.Controls)
            {

                if (x is Panel && (string)x.Tag == "Right")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Left += 13;
                    }
                    else
                    {
                    }
                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "Top")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Top -= 13;
                    }
                    else
                    {
                    }
                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "Bottom")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Top += 13;
                    }
                    else
                    {
                    }
                }
            }
            int playerX = (player.Location.X); // reprends la position X du joueur
            int playerY = (player.Location.Y);// reprends la position Y du joueur
            foreach (Control x in panel12.Controls)
            {
               
                if (x is Panel && (string)x.Tag == "NetherInt" && Nether.Visible == true) 
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // si le joueur rentre en contacte avec la zone d'intéraction du portail
                    {
                        Netherlbl.Location = new Point(318, 707); // position du texte expliquand l'action a faire et avec quel touche à côter du portail  
                        if ( langue == "en"){
                            Netherlbl.Text = "E : Teleport"; 
                        }
                        if (langue == "es")
                        {
                            Netherlbl.Text = "E: Teletransportador";
                        }
                        if (langue == "fr")
                        {
                            Netherlbl.Text = "E: Teleporter";
                        }
                        if (langue == "ar")
                        {
                            Netherlbl.Text = "  الناقل الآني : E";
                        }
                    }
                    else
                    {
                        Netherlbl.Text = "";

                    }

                }
                if (x is Panel && (string)x.Name == "panel17")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // si je suis dans la zone de l'arbre
                    {
                        Treelbl.Location = new Point(902, 16); // position du texte a côter de l'arbre
                        if (langue == "en"){
                            Treelbl.Text = "E : Interact";
                        }
                        if (langue == "fr")
                        {
                            Treelbl.Text = "E : Interagir";
                        }
                        if (langue == "ar")
                        {
                            Treelbl.Text = "هـ: التفاعل";
                        }
                        if (langue == "es")
                        {
                            Treelbl.Text = "E: Interactuar";
                        }
                    }
                    else 
                    {
                        Treelbl.Text = "";

                    }

                }
                if (x is Panel && (string)x.Name == "panel39")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // si je suis dans la zone du carton
                    {
                        cartonlbl.Location = new Point(1122, 358); // position du texte du carton
                        if (langue == "en")
                        {
                            cartonlbl.Text = "E : Interact";
                        }
                        if (langue == "fr")
                        {
                            cartonlbl.Text = "E : Interagir";
                        }
                        if (langue == "ar")
                        {
                            cartonlbl.Text = "هـ: التفاعل";
                        }
                        if (langue == "es")
                        {
                            cartonlbl.Text = "E : Interactuar";
                        }
                    }
                    else
                    {
                        cartonlbl.Text = "";

                    }

                }
                if (x is Panel && (string)x.Name == "panel40")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // si je suis à côter de la poubelle
                    {
                        Rubbishlbl.Location = new Point(1458, 279); // position du texte de la poubelle
                        if (langue == "en")
                        {
                            Rubbishlbl.Text = "E : Interact";
                        }
                        if (langue == "fr")
                        {
                            Rubbishlbl.Text = "E : Interagir";
                        }
                        if (langue == "es")
                        {
                            Rubbishlbl.Text = "E : Interactuar";
                        }
                        if (langue == "ar")
                        {
                            Rubbishlbl.Text = "هـ: التفاعل";
                        }
                    }
                    else
                    {
                        Rubbishlbl.Text = "";

                    }

                }
                if (x is Panel && (string)x.Name == "panel44")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        Blacklbl.Location = new Point(playerX, playerY); // si je suis dans la zone du bouton noir 
                        if (langue == "en")
                        {
                            Blacklbl.Text = "E : Interact";
                        }
                        if (langue == "fr")
                        {
                            Blacklbl.Text = "E : Interagir";
                        }
                        if (langue == "es")
                        {
                            Blacklbl.Text = "E : Interactuar";
                        }
                        if (langue == "ar")
                        {
                            Blacklbl.Text = "هـ: التفاعل";
                        }
                    }
                    else
                    {
                        Blacklbl.Text = "";

                    }

                }
               
                if (x is Panel && (string)x.Tag == "Paper1Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && Paper1.Visible == true) // si je suis dans la zone du premier papier
                    {
                        paper1lbl.Location = new Point(1550, 10); // position du texte du papier
                        if (langue == "en")
                        {
                            paper1lbl.Text = "E : Take";    
                        }
                        if (langue == "fr")
                        {
                            paper1lbl.Text = "E : Prendre";
                        }
                        if (langue == "es")
                        {
                            paper1lbl.Text = "E : Tomar";
                        }
                        if (langue == "ar")
                        {
                            paper1lbl.Text = "ه: أن تأخذ";
                        }
                    }
                    else
                    {

                        paper1lbl.Text = "";
                    }

                }
                if (x is Panel && (string)x.Tag == "Paper2Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && Papier2.Visible == true) // si je suis dans la zone du second papier
                    {
                        papier2lbl.Location = new Point(1556, 964); // position du texte du second papier

                        if (langue == "en")
                        {
                            papier2lbl.Text = "E : Take";
                        }
                        if (langue == "fr")
                        {
                            papier2lbl.Text = "E : Prendre";
                        }
                        if (langue == "es")
                        {
                            papier2lbl.Text = "E : Tomar";
                        }
                        if (langue == "ar")
                        {
                            papier2lbl.Text = "ه: أن تأخذ";
                        }
                    }
                    else
                    {

                        papier2lbl.Text = "";
                    }

                }
                if (x is Panel && (string)x.Tag == "Papier3Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && Papier3.Visible == true) // si je suis dans la zone du troisième papier
                    {
                        papier3lbl.Location = new Point(19, 988);//position du texte du troisième papier
                        if (langue == "en"){
                            papier3lbl.Text = "E : Take";
                        }
                        if (langue == "fr")
                        {
                            papier3lbl.Text = "E : Prendre";
                        }
                        if (langue == "es")
                        {
                            papier3lbl.Text = "E : Tomar";
                        }
                        if (langue == "ar")
                        {
                            papier3lbl.Text = "ه: أن تأخذ";
                        }
                    }
                    else
                    {

                        papier3lbl.Text = "";
                    }

                }
                if (x is Panel && (string)x.Tag == "MarteauInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && labelmr == true) // si je suis dans la zone d'intéraction du marteau
                    {
                        label1.Location = new Point(667, 431); // position du texte du marteau
                        if (langue == "en")
                        {
                            label1.Text = "E : Take";
                        }
                        if (langue == "fr")
                        {
                            label1.Text = "E : Prendre";
                        }
                        if (langue == "es")
                        {
                            label1.Text = "E : Tomar";
                        }
                        if (langue == "ar")
                        {
                            label1.Text = "ه: أن تأخذ";
                        }

                    }
                    else
                    {
                        label1.Text = "";
                    }
                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "pnj1Int") //zone d'intéraction du pnj
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && question1 == false)
                    {
                        label3.Location = new Point(2, -1); //position du texte du PNJ
                        if ( langue == "en")
                        {
                            label3.Text = "E : Speak";
                        }
                        if (langue == "fr")
                        {
                            label3.Text = "E : Parler";
                        }
                        if (langue == "es")
                        {
                            label3.Text = "E: Hablar";
                        }
                        if (langue == "ar")
                        {
                            label3.Text = "هـ: التحدث";
                        }
                    }
                    else
                    {
                        label3.Text = "";
                    }
                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "ClocheInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && labelcl == true) // si je suis dans la zone de la cloche
                    {
                        label2.Location = new Point(-1, 327); // position du texte de la cloche
                        if ( langue == "en")
                        {
                            label2.Text = "E : Break";
                        }
                        if (langue == "fr")
                        {
                            label2.Text = "E : Casser";
                        }
                        if (langue == "es")
                        {
                            label2.Text = "E: Pausa";
                        }
                        if (langue == "ar")
                        {
                            label2.Text = "هـ: استراحة";
                        }

                    }
                    else
                    {
                        label2.Text = "";
                    }
                    
                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "ClocheInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && bocalcassé == true)
                    {
                        label5.Location = new Point(-1, 327);
                        if ( langue == "en"){
                            label5.Text = "E : Read";
                        }
                        if (langue == "fr")
                        {
                            label5.Text = "E : Lire";
                        }
                        if (langue == "es")
                        {
                            label5.Text = "E: Leer";
                        }
                        if (langue == "ar")
                        {
                            label5.Text = "هـ: اقرأ";
                        }
                    }
                    else
                    {
                        label5.Text = "";
                    }

                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "ShipInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        Shiplbl.Location = new Point(1072, 714);
                        if ( langue == "en"){
                            Shiplbl.Text = "E : Use";
                        }
                        if (langue == "fr")
                        {
                            Shiplbl.Text = "E : Utiliser";
                        }
                        if (langue == "es")
                        {
                            Shiplbl.Text = "E: Utilizar";
                        }
                        if (langue == "ar")
                        {
                            Shiplbl.Text = "هـ: الاستخدام";
                        }
                    }
                    else
                    {
                        Shiplbl.Text = "";
                    }

                }
            }
            if (moveLeft && move == true)
            {
                player.Left -= speed; // fait bouger le joueur à gauche
                player.Image = Properties.Resources.profil_A; // charge l'image du joueur quand il marche à Gauche
            }
            if (moveRight && move == true)
            {
                
                player.Image = Properties.Resources.profil_D;// charge l'image du joueur quand il marche à droite
                player.Left += speed;// fait bouger le joueur à droite
            }


            if (moveUp && move == true)
            {
                player.Image = Properties.Resources.profil_w;// charge l'image du joueur quand il marche en haut
                player.Top -= speed;// fait bouger le joueur en haut

            }

            if (moveDown && move == true)
            {
                player.Image = Properties.Resources.profil__2_;// charge l'image du joueur quand il marche en bas
                player.Top += speed;// fait bouger le joueur en bas


            }
        }

        private void player_Click(object sender, EventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }
        private void spawn() // Permet d'apparaitre au Point de départ "le spawn".
        {
            player.Location = new Point(362, 218); // position d'apparission du joueur
        }
        private void Animation() // Je doit l'enlever, je l'utilise plus (c'était pour contrer un bug mais je l'ai réglé plus facilement).
        {
            int gameX = panel12.Location.X;
            if (gameX == -799)
            {
            }
            if (gameX == 1)
            {
            }
        }
        private void timer1_Tick(object sender, EventArgs e) // se fait chaque 50 milisecondes.
        {
            string filname5 = path + @"\timer.txt";
            if (monde2 == false){ 
                if (panel97.Visible == false) // si l'écrand de chargement à disparu
                {
                    chargement = false;
                }
                if (menu == true || chargement == true ) // si le menu est ouvert ou si l'ecran de chargement est encore présent
                {
                    string MenuFile = path + @"\menu.txt";
                    if (File.Exists(MenuFile) == false) // vérifie si le fichier "MenuFile" exist
                    {
                        File.Create(MenuFile).Close(); // crée le fichier entre paranthèse
                    }
                }
                else
                {
                
                    string MenuFile = path + @"\menu.txt";
                    if (File.Exists(MenuFile) == true)
                    {
                        File.Delete(MenuFile);// supprime le fichier entre paranthèse
                    }
                }
                if (timer == false)
                {
                    if (File.Exists(filname5))
                    {
                       File.Delete(filname5);// supprime le fichier entre paranthèse
                    }
                }
                if (timer == true)
                {
                    if (File.Exists(filname5) == false)
                    {
                        File.Create(filname5).Close(); // crée le fichier entre paranthèse
                    }
                }
            }
            if (File.Exists(filname5) == false)// si le timer est désactivé
            {

                if (langue == "en")
                {
                    button12.Text = "Timer : Disabled"; // change le texte de "button12".
                }
                if (langue == "fr")
                {
                    button12.Text = "Timer : Désactivé";
                }
                if (langue == "ar")
                {
                    button12.Text = "المؤقِّت: إيقاف التشغيل";
                }
                if (langue == "es")
                {
                    button12.Text = "Temporizador : Desactivado";
                }
            }
            if (File.Exists(filname5) == true) // si le timer est activé
            {
                
                if (langue == "en")
                {
                    button12.Text = "Timer : Enabled";
                }
                if (langue == "fr")
                {
                    button12.Text = "Timer : Activé";
                }
                if (langue == "es")
                {
                    button12.Text = "Temporizador : Activado";
                }
                if (langue == "ar")
                {
                    button12.Text = "المؤقِّت: تشغيل";
                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "droite")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // si le personnage se trouve dans la deuxième salle
                    {
                        marcher = "droite";
                    }
                }
                if (x is Panel && (string)x.Tag == "CibleInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && juste == true) // si je suis dans la salle avec les cibles
                    {
                        if (cibles < 12) //si on a cliqué sur les cibles moins de douze fois
                        {

                            Target1.Visible = true; // rends visible "Target1".
                        }
                    }
                    else if (cibles >= 12) // si on a cliqué sur les cibles plus de douze fois
                    {
                        Target1.Visible = false; // rends invisible "Target1"
                    }
                }
                if (x is Panel && (string)x.Tag == "gauche")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        marcher =  "gauche";
                    }
                }
                if (x is Panel && (string)x.Tag == "bas")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        marcher = "bas";
                    }
                }
                if (x is Panel && (string)x.Tag == "haut")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        marcher = "haut";
                    }
                }
                if (x is Panel && (string)x.Tag == "salle1")
                {
                    
                    if (player.Bounds.IntersectsWith(x.Bounds) && bouton4 == true && marcher == "haut") // si le joueur est en contacte avec la porte, l'animation de changement de salle s'éffectue.
                    {
                        int game = panel12.Location.X; // reprends la position X du jeu
                        int gameY = panel12.Location.Y;// reprends la position Y du jeu
                        int Pause = panel96.Location.X;// reprends la position X du menu
                        int PauseY = panel96.Location.Y;// reprends la position Y du menu
                        if (gameY != -500 ) // si la position Y du jeu est différente à -500
                        {
                            stopRun();
                            moveDown = false;
                            moveLeft = false;
                            moveRight = false;
                            moveUp = false;
                        }
                        else
                        {
                            canRun();
                        }
                        while (gameY != -500) // tant que la position Y du jeu est différente à -500
                        {
                            moveDown = false;
                            moveLeft = false;
                            moveRight = false;
                            moveUp = false;
                            panel12.Location = new Point(game, gameY - 25); // reprends la nouvelle position
                            panel96.Location = new Point(Pause, PauseY + 25); // reprends la nouvelle position
                            break;
                        }
                        Animation(); // appelle la fonction "Animation"
                    }
                    if (player.Bounds.IntersectsWith(x.Bounds) && marcher == "bas")
                    {
                        int game = panel12.Location.X;
                        int gameY = panel12.Location.Y;
                        int Pause = panel96.Location.X;
                        int PauseY = panel96.Location.Y;
                        if (gameY != 0) 
                        {
                            stopRun();
                            moveDown = false;
                            moveLeft = false;
                            moveRight = false;
                            moveUp = false;
                        }
                        else
                        {
                            canRun();
                        }
                        while (gameY != 0)
                        {
                            panel12.Location = new Point(game, gameY + 25); // reprends la nouvelle position
                            panel96.Location = new Point(Pause, PauseY - 25); // reprends la nouvelle position
                            moveDown = false;
                            moveLeft = false;
                            moveRight = false;
                            moveUp = false;
                            break;
                        }
                        Animation();
                    }
                }
                if (x is Panel && (string)x.Tag == "salle2")
                {

                    if (player.Bounds.IntersectsWith(x.Bounds) && panel65.Tag == "salle2" && marcher == "gauche")
                    {
                        int game = panel12.Location.X;
                        int gameY = panel12.Location.Y;
                        int Pause = panel96.Location.X;
                        int PauseY = panel96.Location.Y;
                        if (game != -800)
                        {
                            stopRun();
                        } else
                        {
                            canRun();
                        }
                        while (game != -800)
                        {
                            panel12.Location = new Point(game - 25, gameY);
                            panel96.Location = new Point(Pause + 25, PauseY);
                            moveDown = false;
                            moveLeft = false;
                            moveRight = false;
                            moveUp = false;

                            break;
                        }
                    }
                    if (player.Bounds.IntersectsWith(x.Bounds) && panel65.Tag == "salle2"  && marcher == "droite")
                    {
                        int game = panel12.Location.X;
                        int gameY = panel12.Location.Y; 
                        int Pause = panel96.Location.X;
                        int PauseY = panel96.Location.Y;
                        if (game != 0)
                        {
                            stopRun();
                        }
                        else
                        {
                            canRun();
                        }
                        while (game != 0)
                        {
                            panel12.Location = new Point(game + 25, gameY);
                            panel96.Location = new Point(Pause - 25, PauseY);
                            moveDown = false;
                            moveLeft = false;
                            moveRight = false;
                            moveUp = false;
                            break;
                        }
                        Animation();
                    }
                }
                if (x is Panel && (string)x.Tag == "salle")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && salle2 == true && marcher == "gauche")
                    {
                        int game = panel12.Location.X;
                        int gameY = panel12.Location.Y;
                        int Pause = panel96.Location.X;
                        if (game != -800)
                        {
                            stopRun();
                        }
                        else
                        {
                            canRun();
                        }
                        while (game != -800)
                        {
                            panel12.Location = new Point(game - 25, gameY);
                            panel96.Location = new Point(Pause + 25, gameY);

                            break;
                        }
                    }
                    if (player.Bounds.IntersectsWith(x.Bounds) && salle2 == true && marcher == "droite")
                    {
                        int game = panel12.Location.X;
                        int gameY = panel12.Location.Y;
                        int Pause = panel96.Location.X;
                        int PauseY = panel96.Location.Y;
                        if (game != 0)
                        {
                            stopRun();
                        }
                        else { 
                        canRun(); 
                        }
                        while (game != 0)
                        {
                            panel12.Location = new Point(game + 25, gameY);
                            panel96.Location = new Point(Pause - 25, PauseY);
                            break;
                        }
                        Animation();
                    }
                }
            }
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void victoire_Tick(object sender, EventArgs e)
        {

            foreach (Control x in panel12.Controls)
            {

                    Process jeuProcess = new Process();
                    jeuProcess.StartInfo.FileName = path + @"\bataille navale escape.exe";
                if (x is Panel && (string)x.Tag == "ShipInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true ) // si j'intéragie avec le bateau
                    {
                        if (start == false) // si le jeu n'a pas encore été lancer
                        {
                                stopRun();
                                start = true;
                            jeuProcess.Start();// Lance le jeu "bataille navale escape.exe"

                        }

                    }
                    string fileName = path + @"\Victoire.txt";
                    if (File.Exists(fileName) && cool == false) // vérifie si le fichier "Victoire.txt" exist (il se crée quand on gagne à la bataille navale)
                    {
                        
                        cool = true;
                        if (langue == "fr"){
                            textUse = "Bravo vous avez gagné au jeu --bataille navale-- !\nVous pouvez passer à la prochaine épreuve !";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "Congratulations on winning the naval battle!\nYou can move on to the next test!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡Enhorabuena por haber ganado la batalla naval!\nPuedes pasar a la siguiente ronda.";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "تهانينا على الفوز بالمعركة البحرية\nيمكنك الانتقال إلى الجولة التالية";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        File.Delete(fileName); // supprime le Fichier entre Paranthèse

                        pictureBox6.Image = Properties.Resources.piano; // change l'image de bateau en piano
                        piano = true;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) // si on clique sur le "button4
        {
            if (Sound == "Si" || Sound == "") // si la dernière note était "si" ou rien
            {
                SoundPlayer Do = new SoundPlayer(Properties.Resources.Do);
                Do.Play(); // fait le son "do"
                if (Sound == "Si")
                {
                    stopRun();
                    if (langue == "fr"){
                        textUse = "Bravo ! \n Vous avez le rythme dans la peau ! \n Vous pouvez désormai changer de salle !";
                        
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    if (langue == "es")
                    {
                        textUse = "¡Bien hecho! \n ¡Llevas el ritmo en la sangre! \n ¡Ya puedes cambiar de local!";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    if (langue == "ar")
                    {
                        textUse = "برافو \n الإيقاع يسري في دمك \n يمكنك الآن تغيير الغرف";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    if (langue == "en")
                    {
                        textUse = "Bravo ! \n You've got rhythm in your blood! \n You can change rooms now!";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    canRun();
                    Papier2.Visible = true;
                    juste = true;
                    panel65.Visible = false;
                    panel75.Visible = false;
                    panel65.Tag = "salle2";
                    panel75.Tag = "salle2";
                    button4.Visible = false;
                    button5.Visible = false;
                    button6.Visible = false;
                    button7.Visible = false;
                    button8.Visible = false;
                    button10.Visible = false;
                    button11.Visible = false;
                    button9.Visible = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Enabled = false;
                    button7.Enabled = false;
                    button8.Enabled = false;
                    button10.Enabled = false;
                    button11.Enabled = false;
                    button9.Enabled = false;
                    Sound = "";
                };
                if (Sound =="")
                {
                    Sound = "Do";
                }
            }
            else // si la dernière note n'était pas "si" ou rien
            {
                if (langue == "fr"){

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                Sound = "";
            }
            
            
        }

        private void button11_Click(object sender, EventArgs e) // si on clique sur le "button11"
        {
            if ( Sound =="Si" || Sound == "")
            {
                SoundPlayer Do = new SoundPlayer(Properties.Resources.Do);
                Do.Play();//  lance le son "do"
                if (Sound == "Si")
                {
                    stopRun();
                    if (langue == "fr"){

                        textUse = "Bravo !\nVous avez le rythme dans la peau !\nVous pouvez désormais changer de salle !";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    if (langue == "es")
                    {
                        textUse = "Bravo !\nLlevas el ritmo en la sangre.\nYa puedes cambiar de habitación.";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    if (langue == "en")
                    {
                        textUse = "Well done!\nRhythm is in your blood!\nNow you can change rooms!";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    if (langue == "ar")
                    {
                        textUse = "أحسنت صنعاً\nالإيقاع يسري في دمك\nيمكنك الآن تغيير الغرف";
                        var result = CustomDialogForm.Show(textUse, langue);
                    }
                    canRun();
                    Papier2.Visible = true;
                    juste = true;
                    pictureBox6.Image = Properties.Resources.piano_break;
                    piano = false;
                    pianocasse = true;
                    panel65.Visible = false;
                    panel75.Visible = false;
                    panel65.Tag = "salle2";
                    panel75.Tag = "salle2";
                    button4.Visible = false;
                    button5.Visible = false;
                    button6.Visible = false;
                    button7.Visible = false;
                    button8.Visible = false;
                    button10.Visible = false;
                    button11.Visible = false;
                    button9.Visible = false;
                    button4.Enabled = false;
                    button5.Enabled = false;
                    button6.Enabled = false;
                    button7.Enabled = false;
                    button8.Enabled = false;
                    button10.Enabled = false;
                    button11.Enabled = false;
                    button9.Enabled = false;
                }
                Sound = "Do";
            }
            else
            {
                if (langue == "fr")
                {

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                Sound = "";
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Sound == "Re")
            {
                Sound = "Mi";
                SoundPlayer Mi = new SoundPlayer(Properties.Resources.Mi);
                Mi.Play();// lance le son "mi"
            }
            else
            {
                if (langue == "fr")
                {

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                Sound = "";
            }
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Sound == "Mi")
            {
                Sound = "Fa";
                SoundPlayer Fa = new SoundPlayer(Properties.Resources.Fa);
                Fa.Play(); // lance le son "fa"
            }
            else
            {
                Sound = "";
                if (langue == "fr")
                {

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Sound == "Fa")
            {
                Sound = "Sol";
                SoundPlayer Sol = new SoundPlayer(Properties.Resources.Sol);
                Sol.Play(); // lance le son "sol"
            }
            else
            {
                if (langue == "fr")
                {

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                Sound = "";
            }
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Sound == "Sol")
            {
                Sound = "La";
                SoundPlayer La = new SoundPlayer(Properties.Resources.La);
                La.Play(); // lance le son "la"
            }
            else
            {
                Sound = "";
                if (langue == "fr")
                {

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Sound == "La")
            {
                Sound = "Si";
                SoundPlayer Si = new SoundPlayer(Properties.Resources.Si);
                Si.Play(); // lance le son "si"
            }
            else
            {
                if (langue == "fr")
                {

                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                Sound = "";
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (Sound == "Do")
            {
                Sound = "Re";
                SoundPlayer Re = new SoundPlayer(Properties.Resources.Re);
                Re.Play(); // lance le son "re"
            }
            else
            {
                Sound = "";
                if (langue == "fr")
                {
                    textUse = "Faux !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡Incorrecto!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "False!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "خطأ";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
            }
            
        }

        private void epreuve() // fonction appellée utlérieurement, permet de fair les épreuves avec les questions
        {
            if (épreuve2 == true && texto == false) // si on a réussi la troisième épreuve
            {
                texto = true;
                textBox1.Text = "";
                Question2.Visible = false;
                textBox1.Visible = false;
                textBox1.Enabled = false;
                button3.Visible = false;
                button3.Enabled = false;
                stopRun();
                if (langue == "fr"){
                    textUse = "LE SORCIER FOU !\n\nBravo !  Vous avez réussi cette troisième épreuve avec brio !\nMaintenant place à la quatrième énigme...";
                    var result = CustomDialogForm.Show(textUse, langue);
                    SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                    Zelda.Play(); // lance le son de "zelda"
                    textUse = "LE SORCIER FOU !\n\nVous devrez changer de salle...\nET TROUVER LE BOUTON QUI SI CACHE !!!\n... et j'espere qu'on se reverra bientôt... \nEt pour info, les prochaines salle vous devrez vous passer de mon aide et devrez trouver l'énigme avant de la résoudre.";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "¡EL MAGO LOCO!\n\nBravo !  Muy bien.  Has superado esta tercera prueba con nota.\nAhora es el momento del cuarto acertijo...";
                    var result = CustomDialogForm.Show(textUse, langue);
                    SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                    Zelda.Play(); // lance le son de "zelda"
                    textUse = "¡EL MAGO LOCO!\n\nTendrás que cambiar de habitación...\n¡¡¡Y ENCUENTRA EL BOTÓN OCULTO !!!\n... y espero que nos volvamos a ver pronto... \nY para que lo sepas, en las próximas salas no necesitarás mi ayuda y tendrás que encontrar el acertijo antes de resolverlo.";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "THE MAD WIZARD!\n\nCongratulations!  You've passed this third test with flying colors!\nNow it's time for the fourth riddle...";
                    var result = CustomDialogForm.Show(textUse, langue);
                    SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                    Zelda.Play(); // lance le son de "zelda"
                    textUse = "THE MAD WIZARD!\n\nYou'll have to change rooms...\nAND FIND THE HIDDEN BUTTON!!!\n... and I hope to see you soon...\nAnd just so you know, for the next few rooms you'll have to do without my help and will have to find the riddle before you can solve it.";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "الساحر المجنون\n\nأحسنت صنعا  لقد نجحت في هذا الاختبار الثالث بامتياز\n...والآن حان وقت اللغز الرابع";
                    var result = CustomDialogForm.Show(textUse, langue);
                    SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                    Zelda.Play(); // lance le son de "zelda"
                    textUse = "الساحر المجنون\n\n...سيكون عليك تغيير الغرف\nوالعثور على الزر المخفي\n... وآمل أن نرى بعضنا البعض مرة أخرى قريبًا...\nولعلمك فقط، في الغرف القليلة القادمة لن تحتاج إلى مساعدتي وسيكون عليك إيجاد اللغز قبل حله";
                    var r = CustomDialogForm.Show(textUse, langue);
                }
                canRun();
                salle2 = true;
                panel19.Visible = false;
                panel20.Visible = false;
                panel19.Tag = "salle";
                panel20.Tag = "salle";
            }
        }
        private void stopRun() // empêche le personnage de faire quoi que se soi
        {
            interact = false;
            moveLeft = false;
            moveRight = false;
            moveDown = false;
            moveUp = false;
            move = false;
            
        }

        private void cible_Tick(object sender, EventArgs e) // se fait chaque 750 miliseconde(0.75 seconde)
        {
            if (juste == true) // si on est dans la salle des cibles
            {
                Random random = new Random(); // crée une instance random
                int cible = random.Next(1,6); // choisi aléatoirement un chiffre entre 1 et 6
                while (cibles < 12)
                {

                       
                    switch (cible) // change la position de la cible selon le chiffre choisi
                    {
                        case 1 :

                            Target1.Location = new Point(171, 569); // la position ajoutée à la cible
                            break;
                        case 2:
                            Target1.Location = new Point(429, 569);
                            break;
                        case 3:
                            Target1.Location = new Point(311, 693);
                            break;
                        case 4:
                            Target1.Location = new Point(60, 673);
                            break;
                        case 5:
                            Target1.Location = new Point(199, 822);
                            break;
                        case 6:
                            Target1.Location = new Point(405, 867);
                            break;
                    }
                    break;
                }
                if (cibles >= 12 ) // si on a cliqué sur les cibles plus que douzes fois
                {
                    Target1.Visible = false;
                    Nether.Visible = true;
                    panel90.Tag = "Left";
                    panel91.Tag = "Bottom";
                    panel92.Tag = "Right";
                    panel94.Tag = "Top";

                    action(); // appelles la foncion "action"
                    correct = true;
                }
            }
        }

        private void Target1_Click(object sender, EventArgs e) // quand on clique sur une cible
        {
            Target1.Tag = "Touche";
            cibles = cibles + 1; // cible s'incrémente à chaque fois

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void canRun() // redonne la possibilité de bouger
        {
            move = true;
        }

        private void timer2_Tick(object sender, EventArgs e) // se fait chaque 50 milisecondes(0.05secondes)
        {
            if (papier == 3) // si on a récupérer tout les papiers ( crée un fichier se nomment "Papers.txt", le Form2 vérifie s'il exist sinon il ferme le jeu après d'avoir dit qu'il fallait récupérer tout les papiers )
            {
                string fileName = path + @"\Papers.txt";

                try
                {
                    
                    if (File.Exists(fileName)) // si le fichier "papers" exist
                    {
                        
                    }

                    using (FileStream fileStr = File.Create(fileName)) // ajoutes des informations au fichier
                    {
                        Byte[] text = new UTF8Encoding(true).GetBytes("Lorem Ipsum"); 
                        fileStr.Write(text, 0, text.Length);
                    }
                    using (StreamReader sr = File.OpenText(fileName)) // si je veux ajouter du textes
                    {
                        string sa = ""; // texte qui sera ajouter
                        while ((sa = sr.ReadLine()) != null)
                        {
                        }
                    }
                }
                catch (Exception) // si il y a une erreur
                {
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // quand le Fomr1 se ferme ( je supprime tout les fichiers qui ne sont pas existant au début du jeu )
        {
            string MenuFile = path + @"\menu.txt";
            string TimerFile = path + @"\timer.txt";
            string FiniFile = path + @"\fini.txt";
            string fileName1 = path + @"\Papers.txt";
            string fileName7 = path + @"\Victoire.txt";
            string fileName4 = path + @"\ar.txt";
            string fileName5 = path + @"\es.txt";
            string fileName6 = path + @"\en.txt";
            string fileName2 = path + @"\fr.txt";
            if (File.Exists(fileName1))
            {
                File.Delete(fileName1); // supprime le fichier
            }
            if (File.Exists(FiniFile))
            {
                File.Delete(FiniFile);
            }
            if (File.Exists(MenuFile))
            {
                File.Delete(MenuFile);
            }
            if (File.Exists(TimerFile))
            {
                File.Delete(TimerFile);
            }
            if (File.Exists(fileName7))
            {
                File.Delete(fileName7);
            }
            if (File.Exists(fileName2)== false)
            {
                File.Create(fileName2).Close(); // crée le fichier puis le ferm
            }
            if (File.Exists(fileName4))
            {
                File.Delete(fileName4);
            }
            if (File.Exists(fileName5))
            {
                File.Delete(fileName5);
            }
            if (File.Exists(fileName6))
            {
                File.Delete(fileName6);
            }
        }

        private void Menue_Tick(object sender, EventArgs e)
        {
            
        }

        private void timer3_Tick(object sender, EventArgs e) // se fait chaque 10 milisecondes(0.01 seconde)
        {
            time = time + 0.01; // ajoute 0.01 à time
            double arrondie = Math.Round(time, 2); // arrondi time pour ne pas avoir des chiffre comme "9.9999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999"
            if (langue == "fr"){
                label7.Text = "Timer : " + arrondie; // écrit Timer : plus le chiffre arrondi
            }
            if (langue == "es")
            {
                label7.Text = "Temporizador : " + arrondie;
            }
            if (langue == "en")
            {
                label7.Text = "Timer : " + arrondie;
            }
            if (langue == "ar")
            {
                label7.Text = "المؤقِّت : " + arrondie;
            }

        }
        private void men () // quand on appuie sur la touche "Esc" ça appelle la fonction "men" qui permet de sois fermer sois ouvrir le menu
        {
            if (menu == false)
            {
                panel96.Visible = false; // ferm le menu ( il devient juste invisible )
            }
            if (menu == true)
            {
                panel96.Visible = true; // ouvre le menu ( il devient juste visible )
            }
        }
        private void fr () // change la langue en fr et supprime les fichiers des autres langue (les autres forms vérifies c'est fichier pour changer leur langue)
        {
            string filname = path + @"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            if (File.Exists(filname)== false)
            {
                File.Create(filname).Close();
            }
            if (File.Exists(filname2))
            {
                File.Delete(filname2);
            }
            if (File.Exists(filname3))
            {
                File.Delete(filname3);
            }
            if (File.Exists(filname4))
            {
                File.Delete(filname4);
            }
        }
        private void es()
        {
            string filname = path + @"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            if (File.Exists(filname))
            {
                File.Delete(filname);
            }
            if (File.Exists(filname2) == false)
            {
                File.Create(filname2).Close();
            }
            if (File.Exists(filname3))
            {
                File.Delete(filname3);
            }
            if (File.Exists(filname4))
            {
                File.Delete(filname4);
            }
        }
        private void en()
        {
            string filname = path + @"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            if (File.Exists(filname))
            {
                File.Delete(filname);
            }
            if (File.Exists(filname2))
            {
                File.Delete(filname2);
            }
            if (File.Exists(filname3) == false)
            {
                File.Create(filname3).Close();
            }
            if (File.Exists(filname4))
            {
                File.Delete(filname4);
            }
        }
        private void ar()
        {
            string filname = path + @"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            if (File.Exists(filname))
            {
                File.Delete(filname);
            }
            if (File.Exists(filname2))
            {
                File.Delete(filname2);
            }
            if (File.Exists(filname3))
            {
                File.Delete(filname3);
            }
            if (File.Exists(filname4) == false)
            {
                File.Create(filname4).Close();
            }
        }

        private void button13_Click(object sender, EventArgs e) // si on clique sur le bouton de changement de langue
        {
            this.ActiveControl = null; // Désélectionne le bouton
            
            if (langue == "fr")
            {
                textUse = "Choisissez votre langue. \n Y = Français \n N = English \n C = + de langues";
                var r = CustomDialogForm2.Show(textUse, langue);
                if (r == DialogResult.Yes)
                {
                    textUse = "Votre jeu est déjà en Français.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    langue = "fr";
                    button13.Text = "Langue : Français";
                    fr();
                    return;
                }
                if (r == DialogResult.No)
                {
                    textUse = "Your game changed to English language.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    langue = "en";
                    button13.Text = "Language : English";
                    en();
                    return;
                }
                if (r == DialogResult.Cancel)
                {
                    textUse = "Choisissez votre langue. \n Y = العربية \n N = Español \n C = Annuler";
                    var res = CustomDialogForm2.Show(textUse, langue);
                    if (res == DialogResult.Yes)
                    {
                        langue = "ar";
                        textUse = "لقد تغيرت لعبتك باللغة العربية";
                        var result = CustomDialogForm.Show(textUse, langue);
                        button13.Text = "لُغَةٌ : العربية";
                        ar();
                        return;
                    }
                    if (res == DialogResult.No)
                    {
                        langue = "es";
                        textUse = "Tu juego ha cambiado en español.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        button13.Text = "lengua : Español";
                        es();
                        return;
                    }
                    if (res == DialogResult.Cancel)
                    {
                        langue = "fr";
                        textUse = "Votre jeu est resté en Français.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        fr();
                        return;
                    }
                }
            }
            if (langue == "en")
            {
                textUse = "Choose your language. \n Y = Français \n N = English \n C = more languages";
                var res = CustomDialogForm2.Show(textUse, langue);
                if (res == DialogResult.Yes)
                {
                    button13.Text = "Langue : Français";
                    langue = "fr";
                    textUse = "Votre jeu à changer en Français.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    fr();
                    return;
                }
                if (res == DialogResult.No)
                {
                    button13.Text = "Language : English";
                    langue = "en";
                    textUse = "Your game is already in English.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    en();
                    return;
                }
                if (res == DialogResult.Cancel)
                {
                    textUse = "Choose your language. \n Y = العربية \n N = Español \n C = Cancel";
                    var esr = CustomDialogForm2.Show(textUse, langue);
                    if (esr == DialogResult.Yes)
                    {
                        langue = "ar";
                        textUse = "لقد تغيرت لعبتك باللغة العربية";
                        var result = CustomDialogForm.Show(textUse, langue);
                        button13.Text = "لُغَةٌ : العربية";
                        ar();
                        return;
                    }
                    if (esr == DialogResult.No)
                    {
                        langue = "es";
                        textUse = "Tu juego ha cambiado en español.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        button13.Text = "lengua : Español";
                        es();
                        return;
                    }
                    if (esr == DialogResult.Cancel)
                    {
                        langue = "en";
                        textUse = "Your game has remained in English.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        en();
                        return;
                    }
                }
            }
            if (langue == "ar")
            {
                textUse = "اختر لغتك. \n Y = Français \n N = English \n C = المزيد من اللغات";
                var esr = CustomDialogForm2.Show(textUse, langue);
                if (esr == DialogResult.Yes)
                {
                    button13.Text = "Langue : Français";
                    langue = "fr";
                    textUse = "Votre jeu à changer en Français.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    fr();
                    return;
                }
                if (esr == DialogResult.No)
                {
                    button13.Text = "Language : English";
                    langue = "en";
                    textUse = "Your game changed to English language.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    en();
                    return;
                }
                if (esr == DialogResult.Cancel)
                {
                    textUse = "اختر لغتك. \n Y = العربية \n N = Español \n C = إلغاء الأمر ";
                    var res = CustomDialogForm2.Show(textUse, langue);
                    if (res == DialogResult.Yes)
                    {
                        langue = "ar";
                        textUse = "لعبتك باللغة العربية بالفعل";
                        var result = CustomDialogForm.Show(textUse, langue);
                        ar();
                        return;
                    }
                    if (res == DialogResult.No)
                    {
                        langue = "es";
                        textUse = "Tu juego ha cambiado en español.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        button13.Text = "lengua : Español";
                        es();
                        return;
                    }
                    if (res == DialogResult.Cancel)
                    {
                        ar();
                        langue = "ar";
                        textUse = "بقيت لعبتك باللغة العربية";
                        var result = CustomDialogForm.Show(textUse, langue);
                        return;
                    }
                }
            }
            if (langue == "es")
            {
                textUse = "Elige tu idioma. \n Y = Français \n N = English \n C = Más Idioma";
                var res = CustomDialogForm2.Show(textUse, langue);
                if (res == DialogResult.Yes)
                {
                    langue = "fr";
                    textUse = "Votre jeu à changer en Français.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    button13.Text = "Langue : Français";
                    fr();
                    return;
                }
                if (res == DialogResult.No)
                {
                    button13.Text = "Language : English";
                    langue = "en";
                    textUse = "Your game changed to English language.";
                    var result = CustomDialogForm.Show(textUse, langue);
                    en();
                        return;
                }
                if (res == DialogResult.Cancel)
                {
                    textUse = "Elige tu idioma. \n Y = العربية \n N = Español \n C = Cancelar";
                    var ress = CustomDialogForm2.Show(textUse, langue);
                    if (ress == DialogResult.Yes)
                    {
                        langue = "ar";
                        textUse = "لقد تغيرت لعبتك باللغة العربية";
                        var result = CustomDialogForm.Show(textUse, langue);
                        button13.Text = "لُغَةٌ : العربية";
                        ar();
                        return;
                    }
                    if (ress == DialogResult.No)
                    {
                        langue = "es";
                        textUse = "Tu juego ya está en español.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        es();
                        return;
                    }
                    if (ress == DialogResult.Cancel)
                    {
                        langue = "es";
                        textUse = "Tu juego ha permanecido en español.";
                        var result = CustomDialogForm.Show(textUse, langue);
                        es();
                        return;
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e) // si on clique sur le bouton pour activé/desactivé le chronomètre 
        {
            if (timer == false) // ouvre le cronomètre
            {
                timer = true;
                
                this.ActiveControl = null; // Désélectionne le bouton
                return;
            }
            if (timer == true) // ferme le cronomètre
            {
                timer = false;
                
                this.ActiveControl = null; // Désélectionne le bouton
                return;
            }
            this.ActiveControl = null; // Désélectionne le bouton
        }

        private void panel96_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void button12_TextChanged(object sender, EventArgs e)
        {
        }

        private void panel96_Click(object sender, EventArgs e) // si on clique sur le panel noir (le menu)
        {
            this.ActiveControl = null; // Désélectionne le bouton
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel97_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel97_Click(object sender, EventArgs e) // si on clique sur l'écran de démarrage
        {
            chargement = false;
            panel97.Visible = false; // enlève l'écrand de démarrage
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            panel97.Visible = false;// enlève l'écrand de démarrage
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            panel97.Visible = false;// enlève l'écrand de démarrage
        }

        private void label8_Click_1(object sender, EventArgs e)
        {
            panel97.Visible = false;// enlève l'écrand de démarrage
        }

        private void label9_Click(object sender, EventArgs e)
        {
            panel97.Visible = false;// enlève l'écrand de démarrage
        }

        private void button14_Click(object sender, EventArgs e)// si on clique sur le de réinitialisation
        {
            if (langue == "fr")
            {
                textUse = "Voulez vous vraiment TOUT recommencer ?";
                var ress = CustomDialogForm3.Show(textUse, langue);
                if (ress == DialogResult.Yes)
                {
                    Application.Restart(); // relance le jeu
                }
                if (ress == DialogResult.No)
                {

                }
            }
            if (langue == "en")
            {
                textUse = "Do you really want to start ALL over again?";
                var ress = CustomDialogForm3.Show(textUse, langue);
                if (ress == DialogResult.Yes)
                {
                    Application.Restart();// relance le jeu
                }
                if (ress == DialogResult.No)
                {

                }
            }
            if (langue == "es")
            {
                textUse = "¿De verdad quieres empezar TODO de nuevo?";
                var ress = CustomDialogForm3.Show(textUse, langue);
                if (ress == DialogResult.Yes)
                {
                    Application.Restart();// relance le jeu
                }
                if (ress == DialogResult.No)
                {

                }
            }
            if (langue == "ar")
            {
                textUse = "هل تريد حقا أن تبدأ من جديد؟";
                var ress = CustomDialogForm3.Show(textUse, langue);
                if (ress == DialogResult.Yes)
                {
                    Application.Restart();// relance le jeu
                }
                if (ress == DialogResult.No)
                {

                }
            }
        }

        private void button15_Click(object sender, EventArgs e) // si on clique sur le bouton des règles
        {
            panel98.Visible = true; //affiche les règles
        }

        private void label12_Click(object sender, EventArgs e)// si on clique sur le bouton retour (ferme les règles)
        {
            panel98.Visible = false; // ferme les règles
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel98_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label13_Click(object sender, EventArgs e)
        {
            if (langue == "fr")
            {
                textUse = "Réalisateur du jeu : Hugo Schweizer \nGame Designer : Hugo Schweizer \nLevel Designer : Hugo Schweizer \nGameplay Programmer : Hugo Schweizer \n \n \n \n \n ---------------------------------------------- \n \n \n \n \nVoix : Chadi Toundi \nTête : Chadi Toundi \nTraduction en Arabe : Chadi Toundi\nContributions créatives : Chadi Toundi ";
                DialogResult result = CustomDialogForm.Show(textUse, langue);
            }
            if (langue == "en")
            {
                textUse = "Game Director : Hugo Schweizer \nLead Designer : Hugo Schweizer \nLevel Designer : Hugo Schweizer \nGameplay Programmer : Hugo Schweizer \n \n \n \n \n ---------------------------------------------- \n \n \n \n \nVoice : Chadi Toundi \nHead : Chadi Toundi \nArabic Traduction : Chadi Toundi ";
                DialogResult result = CustomDialogForm.Show(textUse, langue);
            }
            if (langue == "es")
            {
                textUse = "Director de Juego : Hugo Schweizer \nDiseñador de juegos : Hugo Schweizer \nDiseñador de niveles : Hugo Schweizer \nProgramación de juegos : Hugo Schweizer \n \n \n \n \n ---------------------------------------------- \n \n \n \n \nVoz : Chadi Toundi \nCabeza : Chadi Toundi \nTraducción al árabe : Chadi Toundi ";
                DialogResult result = CustomDialogForm.Show(textUse, langue);
            }
            if (langue == "ar")
            {
                textUse = "Hugo Schweizer : مدير اللعبة  \nHugo Schweizer : مصمم الألعاب \nHugo Schweizer : مصمم المستوى \nHugo Schweizer : برمجة اللعب  \n \n \n \n \n ---------------------------------------------- \n \n \n \n \nChadi Toundi : صوت \nChadi Toundi : رأس \nChadi Toundi : الترجمة العربيةChadi Toundi ";
                DialogResult result = CustomDialogForm.Show(textUse, langue);
            }

        }

        private void panel98_Click(object sender, EventArgs e)
        {

        }

        private void action() // permet d'accéder aux actions
        {
            if (correct == true && parler == false) // si on a cliqué douze  fois sur les cibles
            {
                parler = true;
                stopRun();
                SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                Zelda.Play(); // lance le son zelda
                Target1.Visible = false;
                if (langue == "fr")
                {
                    textUse = "Bravo vous avez touché toutes les cibles !";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "Enhorabuena por alcanzar todos los objetivos.";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "أحسنت في تحقيق جميع الأهداف";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "Bravo, you've hit all the targets!";
                    var result = CustomDialogForm.Show(textUse, langue);
                }
                canRun();
                Papier3.Visible = true;
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "MarteauInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && Marteau == false) // si on récupère le marteau
                    {
                        Marteau = true;
                        pictureBox4.Visible = false;
                        label1.Visible = false;
                        panel1.Tag = "";
                        panel2.Tag = "";
                        labelmr = false;
                        stopRun();
                        if (langue == "fr"){
                            textUse = "Vous avez Trouvé...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                            Zelda.Play();
                            textUse = "Un MARTEAU !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "Te has recuperado...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                            Zelda.Play();
                            textUse = "¡UN MARTILLO!";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "لقد تعافيت...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                            Zelda.Play();
                            textUse = "مطرقة";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "You've recovered...";
                            var result = CustomDialogForm.Show(textUse, langue);
                            SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                            Zelda.Play();
                            textUse = "A HAMMER!";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }
                }
                if (x is Panel && (string)x.Tag == "NetherInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && Nether.Visible == true && interact == true && portal == true) // si on intéragie avec le portail
                    {
                        portal = false;
                        interact = false;
                        if (langue == "fr")
                        {
                            textUse = "Voulez vous changer de monde ?? \n Aucun retour ne sera possible !";
                            var ress = CustomDialogForm3.Show(textUse, langue);
                            if (ress == DialogResult.Yes)
                            {
                                stopRun();
                                textUse = "Vous allez êtres téléporter !\n Attention accrocher vous !";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                                Form2 frm = new Form2();
                                frm.Show();
                                this.Hide();
                            }
                            if (ress == DialogResult.No)
                            {
                                stopRun();
                                textUse = "Le portail restera ouvert en cas de besoin...";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                            }
                        }
                        if (langue == "es")
                        {
                            textUse = "¿Quieres cambiar el mundo?  \n ¡No hay devoluciones!";
                            var ress = CustomDialogForm3.Show(textUse, langue);
                            if (ress == DialogResult.Yes)
                            {
                                stopRun();
                                textUse = "¡Vas a ser teletransportado!\n ¡Agárrate fuerte!";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                                Form2 frm = new Form2();
                                frm.Show();
                                this.Hide();
                            }
                            if (ress == DialogResult.No)
                            {
                                stopRun();
                                textUse = "La puerta permanecerá abierta si es necesario...";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                            }
                        }
                        if (langue == "ar")
                        {
                            textUse = "هل تريد تغيير العالم؟  \n لا مجال للتراجع!";
                            var ress = CustomDialogForm3.Show(textUse, langue);
                            if (ress == DialogResult.Yes)
                            {
                                stopRun();
                                textUse = "سيتم نقلك فورياً\n تمسك جيداً";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                                Form2 frm = new Form2();
                                frm.Show();
                                this.Hide();
                            }
                            if (ress == DialogResult.No)
                            {
                                stopRun();
                                textUse = "ستبقى البوابة مفتوحة إذا لزم الأمر";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                            }
                        }
                        if (langue == "en")
                        {
                            textUse = "Do you want to change the world? \n No return possible!";
                            var ress = CustomDialogForm3.Show(textUse, langue);
                            if (ress == DialogResult.Yes)
                            {
                                stopRun();
                                textUse = "You're going to be teleported!\n Hold on tight!";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                                Form2 frm = new Form2();
                                frm.Show();
                                this.Hide();
                            }
                            if (ress == DialogResult.No)
                            {
                                stopRun();
                                textUse = "The gate will remain open in case of need...";
                                var result = CustomDialogForm.Show(textUse, langue);
                                canRun();
                            
                            }
                        }
                        portal = true;
                        monde2 = true;
                    }
                }
                if (x is Panel && (string)x.Tag == "ShipInt") 
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && piano == true && pianocasse == false) // si on interagie avec le piano
                    {
                       button4.Visible = true;
                       button5.Visible = true;
                       button6.Visible = true;
                       button7.Visible = true;
                       button8.Visible = true;
                       button10.Visible = true;
                        button11.Visible = true;
                       button9.Visible = true;
                        button4.Enabled = true;
                        button5.Enabled = true;
                        button6.Enabled = true;
                        button7.Enabled = true;
                        button8.Enabled = true;
                        button10.Enabled = true;
                        button11.Enabled = true;
                        button9.Enabled = true;
                    }
                }
            }
            
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Name == "panel17")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && bouton1 == false) // si on intéragie avec l'arbre
                    {
                        stopRun();
                        if (langue == "fr"){

                            textUse = "Bravo vous avez trouvé le Premier bouton !\nMaintenant trouvez le deuxième ainsi de suite ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡Enhorabuena por encontrar el Primer Botón!\nAhora encuentra el segundo y así sucesivamente";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "تهانينا على العثور على الزر الأول\nوالآن أوجد الثاني وهكذا دواليك";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "Congratulations on finding the First Button!\nNow find the second and so on";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        bouton1 = true;
                    }
                    else if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && bouton1 == true)
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "/--Rien ne ce passe--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "/--No pasa nada--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "/--لا شيء يحدث";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "/--Nothing happens--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }
                }
                if (x is Panel && (string)x.Name == "panel39")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && bouton1 == true && bouton2 == false) // si on intéragie avec les cartons
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "Bravo vous avez trouvé le deuxième bouton !";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "Enhorabuena por encontrar el segundo botón. ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "Congratulations on finding the second button! ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "تهانينا على العثور على الزر الثاني";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        bouton2 = true;
                        canRun();
                    }
                    else if(player.Bounds.IntersectsWith(x.Bounds) && interact == true && (bouton1 == false || bouton2 == true))
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "/--Rien ne ce passe--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "/--No pasa nada--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "/--لا شيء يحدث";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "/--Nothing happens--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }
                }
                if (x is Panel && (string)x.Tag == "Paper1Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && Paper1.Visible == true) // si on récupère le premier papier 
                    {
                        stopRun();
                        SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                        Zelda.Play();
                        papier = papier + 1;
                        if (langue == "fr")
                        {
                            textUse = "--Papier + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "Vous êtes à : " + papier + "/??? de papiers !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "--Papel + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "Se encuentra en: " + papier + "/??? ¡Papeles!";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "--Paper + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "You have: " + papier + "/??? paper !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "--ورق + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "أنت في : " + papier + "/??? أوراق";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        paper1lbl.Visible = false;
                        panel47.Tag = "";
                        panel46.Tag = "";
                        Paper1.Visible = false;
                    }
                }
                if (x is Panel && (string)x.Tag == "Paper2Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && Papier2.Visible == true) // si on récupère le deuxième papier
                    {
                        stopRun();
                        SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                        Zelda.Play();
                        papier = papier + 1;
                        if (langue == "fr")
                        {
                            textUse = "--Papier + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "Vous êtes à : " + papier + "/??? de papiers !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "--Papel + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "Se encuentra en: " + papier + "/??? ¡Papeles!";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "--Paper + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "You have: " + papier + "/??? paper !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "--ورق + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "أنت في : " + papier + "/??? أوراق";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }

                        canRun();
                        papier2lbl.Visible = false;
                        Papier2.Visible = false;
                    }
                }
                if (x is Panel && (string)x.Tag == "Papier3Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && Papier3.Visible == true) // si on récupère le troisième papier
                    {
                        stopRun();
                        SoundPlayer Zelda = new SoundPlayer(Properties.Resources.Item_get_zelda);
                        Zelda.Play();
                        papier = papier + 1;
                        if (langue == "fr")
                        {
                            textUse = "--Papier + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "Vous êtes à : " + papier + "/??? de papiers !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "--Papel + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "Se encuentra en: " + papier + "/??? ¡Papeles!";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "--Paper + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "You have: " + papier + "/??? paper !";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "--ورق + 1--";
                            var result = CustomDialogForm.Show(textUse, langue);
                            textUse = "أنت في : " + papier + "/??? أوراق";
                            var r = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        papier3lbl.Visible = false;

                        Papier3.Visible = false;
                    }
                }
                if (x is Panel && (string)x.Name == "panel40")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && bouton2 == true && bouton3 == false) // si on intéragie avec la poubelle

                    {
                        stopRun();
                        if (langue == "fr"){
                            textUse = "Bravo vous avez trouvé le troisième bouton ! ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "Enhorabuena por encontrar el tercer botón. ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "Congratulations on finding the third button! ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "تهانينا على العثور على الزر الثالث";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        bouton3 = true;
                        canRun();
                    }
                    else if(player.Bounds.IntersectsWith(x.Bounds) && interact == true && (bouton2 == false || bouton3 == true)) // si je suis dans la zone de la poubelle et que j'appuie sur E et que ce n'est pas le bouton qui doit être actionner
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "/--Rien ne ce passe--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "/--No pasa nada--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "/--لا شيء يحدث";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "/--Nothing happens--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }
                }
                if (x is Panel && (string)x.Name == "panel44")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && bouton3 == true && bouton4 == false) // si on intéragie avec le bouton noir
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "Bravo vous avez trouvé le DERNIER bouton !\nVous pouvez changer de salle";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡Enhorabuena por encontrar el ÚLTIMO botón!\nPuede cambiar de habitación";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "تهانينا على العثور على الزر الأخير\nيمكنك تغيير الغرف";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "Congratulations on finding the LAST button!\nYou can change rooms";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        bouton4 = true;
                        panel61.Visible = false;
                        panel62.Visible = false;
                        panel61.Tag = "salle1";
                        panel62.Tag = "salle1";
                        Paper1.Visible = true;
                    }
                    if (bouton4 == true)
                    {
                        panel47.Tag = "Bottom";
                        panel46.Tag = "Left";
                    }
                    else if(player.Bounds.IntersectsWith(x.Bounds) && interact == true &&( bouton3 == false || bouton4 == true))
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "/--Rien ne ce passe--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "/--No pasa nada--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "/--لا شيء يحدث";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "/--Nothing happens--/";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }
                }
                if (x is Panel && (string)x.Tag == "pnj1Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && connaissances == true && question1 == false && soulé == false) // si on parle au PNJ après avoir cassé la cloche et lu la feuille
                    {
                        stopRun();
                        if (langue == "fr"){
                            SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__2_);
                            chadi.Play();
                            textUse = "???\n\nBonjour je vois que vous venez de réussir la première épreuve ! Félicitation !\nMaintenant passons à la deuxième épreuve !\nVous devrez répondre à des questions !\nÊtes Vous Prêts  ?!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__8_);
                            chadi.Play();
                            textUse = "???\n\nHola, veo que acabas de pasar la primera prueba. Te felicito.\nAhora pasemos a la segunda prueba.\nTendrás que responder a algunas preguntas.\n¿Estás preparado?";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__4_);
                            chadi.Play();
                            textUse = "???\n\nHello, I see you've just passed the first test! Congratulations!\nNow let's move on to the second test!\nYou'll have to answer questions!\nAre you ready?!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__6_);
                            chadi.Play();
                            textUse = "???\n\nمرحباً، أرى أنك اجتزت الاختبار الأول! تهانينا\nوالآن لننتقل إلى الاختبار الثاني\nسيكون عليك الإجابة عن بعض الأسئلة\nهل أنت جاهز؟";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        button1.Enabled = true;
                        button2.Enabled = true;
                        button1.Visible = true;
                        button2.Visible = true;
                        question1 = true;
                        
                    }
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && connaissances == true && question1 == false && soulé == true) // si on a échoué au quiz et qu'on vient le refaire
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "LE SORCIER FOU !\n\nReBonjour je vois que vous venez pour essayer de réussir la deuxième épreuve... où vous avez échoué lamentablement..." +
                                "\nMais le passé c'est du passé, passons à la deuxième épreuve !" +
                                "\nVous devrez répondre à des questions !" +
                                "\nÊtes Vous Prêts  ?!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡EL MAGO LOCO!\n\nHola de nuevo, veo que has venido a intentar pasar la segunda prueba... en la que has fracasado estrepitosamente...." +
                                "\nPero el pasado es el pasado, ¡así que pasemos a la segunda prueba!" +
                                "\nTe lo repito porque aún eres capaz de olvidarlo..." +
                                "\n¿Estás preparado?";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "THE MAD WIZARD!\n\nHello see you've come to try and pass the second test... where you failed miserably..." +
                                "\nBut the past is the past, so let's move on to the second test!" +
                                "\nYou'll have to answer questions!" +
                                "\nAre you ready?!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "الساحر المجنون\n\nمرحباً مرة أخرى، أرى أنك جئت لمحاولة اجتياز الاختبار الثاني الذي فشلت فيه فشلاً ذريعاً" +
                                "\nلكن الماضي هو الماضي، لذا دعونا ننتقل إلى الاختبار الثاني" +
                                "\nعليك أن تجيب على الأسئلة! أقول لك مرة أخرى لأنك ما زلت قادرًا على النسيان" +
                                "\nهل أنت جاهز؟";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        button1.Enabled = true;
                        button2.Enabled = true;
                        button1.Visible = true;
                        
                        button2.Visible = true;
                        question1 = true;
                    }

                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && connaissances != true) // si je parle au PNJ sans avoir lu la feuille
                    {
                        rage = rage + 1;
                        if (rage <= 3){
                            stopRun();
                            if (langue == "fr"){
                                SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement);
                                chadi.Play();
                                textUse = "???\n\nJe ne croix pas que vous ayez les connaissances nécéssaires." +
                                    "\nRevennez plus tard...";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            if (langue == "en")
                            {
                                SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__3_);
                                chadi.Play();
                                textUse = "???\n\nI don't think you have the necessary knowledge." +
                                    "\nCome back later...";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            if (langue == "es")
                            {
                                SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__7_);
                                chadi.Play();
                                textUse = "???\n\nNo creo que tengas los conocimientos necesarios." +
                                    "\nVuelve más tarde...";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            if (langue == "ar")
                            {
                                SoundPlayer chadi = new SoundPlayer(Properties.Resources.Enregistrement__5_);
                                chadi.Play();
                                textUse = "???\n\nلا أعتقد أن لديك المعرفة اللازمة" +
                                    "\nعُد لاحقاً";
                            }
                            canRun();
                        }
                        if (rage >= 4)
                        {
                            rage2 = rage2 +1;
                            if (rage2 <= 2){ // si je parle au PNJ moins de trois fois sans avoir lu la feuille après l'avoir cassée
                                stopRun();
                                if (langue == "fr"){

                                    textUse = "???\n\nALLEZ LIRE CETTE PURÉE DE FEUILLE !!!!!" +
                                        "\nPuis revenez me voir, bonne suite.";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "es")
                                {
                                    textUse = "???\n\n¡¡¡¡¡VE Y LEE ESTA HOJA !!!!!" +
                                        "\nEntonces vuelve a verme.";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "en")
                                {
                                    textUse = "???\n\nGO READ THIS PAPER !!!!!" +
                                        "\nThen come back and see me.";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "ar")
                                {
                                    textUse = "???\n\nاذهب واقرأ هذه الورقة" +
                                        "\nثم عد لرؤيتي";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                canRun();
                            }

                            if (rage2 >= 3) // si je parle 3 fois au PNJ sans avoir lu la feuille après l'avoir cassée
                            {
                                stopRun();
                                if (langue == "fr")
                                {
                                    textUse = "LE SORCIER FOU !\n\nALLEZ VOUS EN !!";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "es")
                                {
                                    textUse = "¡EL MAGO LOCO!\n\n¡¡VETE!!";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "ar")
                                {
                                    textUse = "الساحر المجنون\n\nابتعد";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "en")
                                {
                                    textUse = "THE MAD WIZARD!\n\nGO AWAY!!";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                this.Close();
                            }
                        }
                    }
                    button3.Click += async (s, e) => // si je clique sur le bouton "réponse"
                    {

                        if (textBox1.Text == "Hugo" || textBox1.Text == "hugo" || textBox1.Text == "Hugo Schweizer" || textBox1.Text == "hugo Schweizer" || textBox1.Text == "hugo schweizer" || textBox1.Text == "Hugo schweizer") // si la réponse est différente de mon nom, on doit recommencer le quiz
                        {
                            
                            textBox1.BackColor = Color.Lime;
                            soulé = false;
                            await Task.Delay(1000);
                            épreuve2 = true;
                            epreuve();
                        }
                        else  // si on a eu faux
                        {
                            if (soulé == false){
                            question1 = false;
                                stopRun();
                                if (langue == "fr")
                                {
                                    textUse = "LE SORCIER FOU !\n\nPFFFF..\n" +
                                    "Recommencez... vous me saoulez";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "es")
                                {
                                    textUse = "¡EL MAGO LOCO!\n\nPFFFF..\n" +
                                    "Hazlo otra vez... me estás poniendo de los nervios.";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "en")
                                {
                                    textUse = "THE MAD WIZARD!\n\nPFFFF..\n" +
                                    "Do it again... you're boring me.";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }
                                if (langue == "ar")
                                {
                                    textUse = "الساحر المجنون\n\nPFFFF..\n" +
                                    "افعلها مرة أخرى ... أنت تثير أعصابي";
                                    var result = CustomDialogForm.Show(textUse, langue);
                                }

                                canRun();
                                soulé = true;
                                textBox1.Visible = false;
                                Question2.Visible = false;
                                button3.Visible = false;
                                pays = false;
                                textBox1.Enabled = false;
                                button3.Enabled = false;
                                textBox1.Text = "";
                            }
                        }
                    };



                }
            }
            
            

            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "ClocheInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && Marteau == true && bocalcassé == false) // si on a récuperer le marteau et qu'on intéragie avec la cloche
                    {
                        bocal.Image = Properties.Resources.Minecraft_paper;
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "Vous avez cassé le verre, intéragissez avec la feuille pour lire";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "Has roto el cristal, interactúa con la hoja para leer";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "You've broken the glass, interact with the leaf to read";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "لقد كسرت الزجاج، تفاعل مع الورقة لتقرأ";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                        bocalcassé = true;

                    }
                    if (player.Bounds.IntersectsWith(x.Bounds) && Marteau == false && interact == true) // si on interagie avec la cloche sans avoir le marteau
                    {

                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "Il vous manque quelque chose...";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "Te estás perdiendo algo...";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "You're missing something...";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "لقد فاتك شيء ما";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }

                }
            }
            foreach (Control x in panel12.Controls)
            {
                if (x is Panel && (string)x.Tag == "ClocheInt")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && bocalcassé == true && interact == true ) // si on intéragie avec la feuille
                    {
                        sleep++;
                        if (sleep >= 2) { 
                        
                            bocal.Visible = false;
                            label5.Visible = false;
                            
                            stopRun();
                            if (langue == "fr")
                            {
                                textUse = "Si vous lisez se message, c'est que vous avez réussi la première épreuve --Casser la cloche--.\n" +
                                    "Allez parler à ce vieux monsieur, il saura vous aidez pour sortir de ce jeux.\n" +
                                    "Je n'ai qu'un indice pour vous << Les papiers sont la clés >>\n" +
                                    "Ne perdez pas votre temps, surtout pour l'épreuve suivante !";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            if (langue == "es")
                            {
                                textUse = "Si estás leyendo este mensaje, es que has superado la primera prueba: romper la campana.\n" +
                                    "Ve a hablar con este anciano, él podrá ayudarte a salir de este juego.\n" +
                                    "Sólo tengo una pista para ti <<Los papeles son la clave >>\n" +
                                    "No pierdas el tiempo, ¡sobre todo para el próximo examen!";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            if (langue == "en")
                            {
                                textUse = "If you're reading this message, you've passed the first test - Breaking the bell.\n" +
                                    "Go and talk to this old man, he'll be able to help you get out of this game.\n" +
                                    "I've only got one clue for you << Papers are the key >>\n" +
                                    "Don't waste your time, especially for the next test!";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            if (langue == "ar")
                            {
                                textUse = "إذا كنت تقرأ هذه الرسالة ، فهذا يعني أنك نجحت في الاختبار الأول - كسر الجرس\n" +
                                    "اذهب وتحدث إلى هذا الرجل العجوز، سيكون قادرًا على مساعدتك في الخروج من هذه اللعبة\n" +
                                    "لدي تلميح واحد فقط لك <<الأوراق هي المفتاح\n" +
                                    "لا تضيع وقتك، خاصة في الاختبار التالي";
                                var result = CustomDialogForm.Show(textUse, langue);
                            }
                            canRun();
                            connaissances = true;
                            bocal.Visible = true;
                            label5.Visible = true;
                        }
                    }
                   
                }
            }

            if (bocalcassé == true)
            {
                label2.Visible = false;
                labelcl = false;
            }

        }
        private void devMod() // permet de tester le jeu en lançant tout les forms ( papier = 3 permet de lancer le quiz dans le form2, sans ça le jeu se ferm)
        {
            papier = 3;
            Form2 frm2 = new Form2();
            Form3 form3 = new Form3();
            Form4 form4 = new Form4();
            frm2.Show();
            form3.Show();
            form4.Show();
        }
        protected override CreateParams CreateParams // améliore la fluidité du jeu
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Pour un rendu fluide
                return cp;

            }
        }
    }
}
