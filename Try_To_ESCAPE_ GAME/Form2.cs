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
    public partial class Form2 : Form
    {
        int speed = 12; // la vitesse de marche du joueur
        bool question1 = false; // false : pas répondu juste à la première question / True : Répondu juste à la première question
        bool question2 = false; // false : pas répondu juste à la deuxième question / True : Répondu juste à la deuxième question
        bool timer = false; // false : timer désactivé / True : timer activé
        bool question3 = false; // false : pas répondu juste à la troisième question / True : Répondu juste à la troisième question
        bool monde3 = false; // false : Monde trois pas débloquer / True : monde trois débloquer (répndu à toutes les questions juste)
        bool question4 = false; // false : pas répondu juste à la quatrième question / True : Répondu juste à la quatrième question
        string textUse; // variable contenant le texte besoin pour le "CustomMessageBox"
        string langue = "fr"; // langue choisie par l'utilisateur dans le menu
        bool question5 = false; // false : pas répondu juste à la cinquième question / True : Répondu juste à la cinquième question
        bool question6 = false; // false : pas répondu juste à la sixième question / True : Répondu juste à la sixième question 
        bool level2 = false; // false : pas répondu à toutes les questions / True :  répondu à toutes les questions juste 
        bool Change = false; // false : Le personnage est toujours dans le monde 2 / true : Le personnage à changer de monde
        bool menu = false; // false menu fermé: / True : menu ouvert
        string path = AppDomain.CurrentDomain.BaseDirectory; // chemin d'accès de ce fichier
        bool question7 = false; // false : pas répondu juste à la septième question / True : Répondu juste à la septième question
        bool move = true; // false : ne peut pas bouger / True : peut bouger
        bool interact =  false; // false : E n'est pas enfoncé / True : E est enfoncé
        bool moveRight, moveLeft, moveUp, moveDown; // initialisation des mouvements du personnage.
        public Form2()
        {
            InitializeComponent();
            panel1.GetType().InvokeMember("DoubleBuffered", // permet de rendre le jeu plus fluide
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.SetProperty,
            null, panel1, new object[] { true });
            string filname5 = path + @"\timer.txt";
            if (File.Exists(filname5)) // vérifie si le fichier "timer" exist
            {
                timer = true; // active le timer
            }
            else // dans le cas ou le fichier n'exist pas
            {
                timer = false; // désactive le timer
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public partial class CustomDialogForm : Form
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

                // Pour éviter les problèmes de focus
                btnOK.TabIndex = 0;

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
                    cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
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
        public partial class CustomDialogForm3 : Form
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

            if (e.KeyCode == Keys.A) // Fonction permettant d'utiliser la touche A.
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
            if (e.KeyCode == Keys.E) // si E est appuyer, interacte deviens true (permettant d'intéragir avec les objets)
            {
                interact = true;
                action();
            }
            if (e.KeyCode == Keys.Escape && menu == false)// ouvre le menu en appuyant sur ESC
            {
                menu = true;
                men();
                return;
            }
            if (e.KeyCode == Keys.Escape && menu == true) // ferme le menu en appuyant sur ESC
            {
                menu = false;
                men();
                return;
            }
        }
        private void men() // permet de fermer ou ouvrir le menu
        {
            if (menu == false)
            {
                panel96.Visible = false;
            }
            if (menu == true)
            {
                panel96.Visible = true;
            }
        }
        private void Key_Up(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
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
            if (e.KeyCode == Keys.E) // fini la possibilité d'intéragir (la touche E n'est plus appuyée
            {
                interact = false;
            }
        }
        private void action() // permet d'accéder aux actions
        {
            foreach (Control x in panel1.Controls)
            {
                if (x is Panel && (string)x.Tag == "pnj1Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && question7 == false) // si on parle au PNJ avant d'avoir fini toutes les questions
                    {
                        stopRun();
                        if (langue == "fr"){
                            textUse = "LE SORCIER FOU !\n\nBravo Vous avez réussi toutes les salles du premier monde !\n Vous voilà dans un monde nouveau où vous aurez plusieur quiz ! et si vous perdez vous devrez TOUT RECOMMENCER !!!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡EL HECHICERO LOCO!\n\n¡Bravo Has triunfado en todas las habitaciones del primer mundo!\n ¡Ahora estás en un nuevo mundo donde tendrás varios cuestionarios! y si pierdes tendrás que EMPEZAR DE NUEVO!!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "THE MAD SORCERER!\n\nBravo You have succeeded in all the rooms of the first world!\n You are now in a new world where you will have several quizzes! and if you lose you will have to START ALL OVER !!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "!الساحر المجنون\n\nأحسنت، لقد أكملت جميع الغرف في العالم الأول.\n ها أنت ذا في عالم جديد حيث سيكون عليك اجتياز مجموعة كاملة من الاختبارات! وإذا خسرت، سيكون عليك البدء من جديد";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();                    
                    }
                    if (player.Bounds.IntersectsWith(x.Bounds) && interact == true && question7 == true && level2 == false) // si on parle au PNJ après avoir réussi son quiz
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "LE SORCIER FOU !\n\nBravo Vous avez réussi mon quiz de géni !\n souhaitez vous passer au niveau supérieure ?!";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "¡EL MAGO LOCO!\n\nBien hecho ¡Has superado mi brillante cuestionario!\n ¿Le gustaría pasar al siguiente nivel?";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "THE MAD WIZARD!\n\nWell done, you've passed my test with flying colours!\n Would you like to take your business to the next level?";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "الساحر المجنون\n\nأحسنت لقد نجحت في اختباري الرائع\n هل ترغب في الانتقال إلى المستوى التالي؟";
                            var result = CustomDialogForm.Show(textUse, langue);
                       }

                        if (langue == "fr"){

                            textUse = "souhaitez vous passer au niveau supérieure ?! -- LE SORCIER FOU ! --";
                            var result = CustomDialogForm3.Show(textUse, langue);
                            if (result == DialogResult.Yes)
                            {
                                textUse = "La prochaine épreuve est une course à pieds ! \n vous devrez appuyer sur LMB (Left Mouse Button) pour augmenter votre vitesse ! \n Bonne Chance ^^";
                                var r = CustomDialogForm.Show(textUse, langue);
                                level2 = true;
                                label1.Text = "NIVEAU 2";
                            }
                            if (result == DialogResult.No)
                            {
                                textUse = "Revenez me voir si vous le souhaitez...";
                                var r = CustomDialogForm.Show(textUse, langue);
                            }
                        }
                        if (langue == "es")
                        {
                            textUse = "¿Le gustaría pasar al siguiente nivel? -- ¡EL HECHICERO LOCO! --";
                            var result = CustomDialogForm3.Show(textUse, langue);
                            if (result == DialogResult.Yes)
                            {
                                textUse = "El próximo evento es una carrera a pie. \n ¡tendrás que pulsar LMB (botón izquierdo del ratón) para aumentar tu velocidad! \n Buena suerte ^^";
                                var r = CustomDialogForm.Show(textUse, langue);
                                level2 = true;
                                label1.Text = "NIVEL 2";
                            }
                            if (result == DialogResult.No)
                            {
                                textUse = "Ven a verme otra vez si quieres...";
                                var r = CustomDialogForm.Show(textUse, langue);
                            }
                        }
                        if (langue == "en")
                        {
                            textUse = "Would you like to go to the next level? -- THE MAD WIZARD! --";
                            var result = CustomDialogForm3.Show(textUse, langue);
                            if (result == DialogResult.Yes)
                            {
                                textUse = "The next event is a foot race! \n you'll have to press LMB (Left Mouse Button) to increase your speed! \n Good Luck ^^";
                                var r = CustomDialogForm.Show(textUse, langue);
                               level2 = true;
                                label1.Text = "LEVEL 2";
                            }
                            if (result == DialogResult.No)
                            {
                                textUse = "Come and see me again if you like...";
                                var r = CustomDialogForm.Show(textUse, langue);
                            }
                        }
                        if (langue == "ar")
                        {
                            textUse = "-- هل ترغب في الانتقال إلى المستوى التالي؟ -- الساحر المجنون ";
                            var result = CustomDialogForm3.Show(textUse, langue);
                            if (result == DialogResult.Yes)
                            {
                                textUse = "الفعالية التالية هي سباق على الأقدام  \n اضغط على LMB (زر الماوس الأيسر) لزيادة سرعتك  \n ^^حظاً موفقاً! ";
                                var r = CustomDialogForm.Show(textUse, langue);
                                level2 = true;
                                label1.Text = "المستوى 2";
                            }
                            if (result == DialogResult.No)
                            {
                                textUse = "عُد لرؤيتي إذا كنت ترغب في ذلكs";
                                var r = CustomDialogForm.Show(textUse, langue);
                            }
                        }
                        canRun();
                    }
                    else if (level2 == true) // si on parle au PNJ après lui avoir parler
                    {
                        stopRun();
                        if (langue == "fr")
                        {
                            textUse = "Je n'ai rien à vous dire...";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "es")
                        {
                            textUse = "No tengo nada que decirte...";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "ar")
                        {
                            textUse = "...ليس لدي ما أقوله لك";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        if (langue == "en")
                        {
                            textUse = "I've got nothing to say to you...";
                            var result = CustomDialogForm.Show(textUse, langue);
                        }
                        canRun();
                    }
                }
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

        private void Form2_FormClosing(object sender, FormClosingEventArgs e) // si le Form2 se ferm, il ferm tout le jeu (autrement le processus tourne en arrière plan)
        {
            Application.Exit();

        }

        private void timer2_Tick(object sender, EventArgs e) // se fait chaque seconde (1000 milisecondes)
        {
            Random rdm = new Random();
            int color = rdm.Next(0, 10);
            switch (color) // change de couleur chaque seconde
            {
                case 0: 
                    label1.ForeColor = Color.Blue;
                    break;
                case 2:
                    label1.ForeColor = Color.Pink;
                    break;
                case 3:
                    label1.ForeColor = Color.Green;
                    break;
                case 4:
                    label1.ForeColor = Color.Yellow;
                    break;
                case 5:
                    label1.ForeColor = Color.LightBlue;
                    break;
                case 6:
                    label1.ForeColor = Color.Coral;
                    break;
                case 7:
                    label1.ForeColor = Color.Beige;
                    break;
                case 8:
                    label1.ForeColor = Color.Orange;
                    break;
                case 9:
                    label1.ForeColor = Color.Red;
                    break;
                case 10:
                    label1.ForeColor = Color.White;
                    break;
            }
        }

        private void label1_Click(object sender, EventArgs e) // si on clique sur le bouton pour commencer le quiz, il vérifie si on a les trois papier sinon il ferme le jeu
        {
            string fileName = path + @"\Papers.txt";
            if (level2 == false && File.Exists(fileName)) // si on a pas encore réussi le quiz et que les trois papiers on été pris
            {

                if (question1 == false) // si la question 1 n'a pas encore été faite
                {

                    stopRun();
                    if (langue == "fr")
                    {
                        textUse = "Qu'est ce que L'hippopotomonstrosesquippedaliophobie \n\n Y = La peure des monstres \n N = La peure des animaux géants \n C = La peure des mots longs  \n  QUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            textUse = "FAUX !!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            textUse = "FAUX !!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            Application.Exit();

                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            textUse = "Bravo !! Vous avez trouver la bonne réponse ! Vous pouvez passer au quiz 2 dès aprésent !";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            question1 = true;
                            label1.Text = "Commencer le Quiz 2";
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        textUse = "¿Qué es la hipopotomonstrosesquippedaliofobia? \n\n Y = El miedo a los monstruos \n N = El miedo a los animales gigantes \n C = El miedo a las palabras largas \n QUIZ  ---  ¡EL MAGO LOCO! ";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            textUse = "¡FALSO!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            textUse = "¡FALSO!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            Application.Exit();

                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            textUse = "¡Bien hecho! Has encontrado la respuesta correcta. Ahora puedes pasar a la prueba 2.";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            question1 = true;
                            label1.Text = "Iniciar Cuestionario 2";
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        textUse = "What is hippopotomonstrosesquippedaliophobia? \n\n Y = The fear of monsters \n N = The fear of giant animals \n C = The fear of long words \n QUIZ  --- THE MAD WIZZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            textUse = "False!!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            textUse = "False!!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            Application.Exit();

                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            textUse = "Congratulations! You've found the right answer! You can now go on to quiz 2!";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            question1 = true;
                            label1.Text = "Start Quiz 2";
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        textUse = "ما هو رهاب فرس النهر؟ \n\n Y = الخوف من الوحوش \n N = الخوف من الحيوانات العملاقة \n C = الخوف من الكلمات الطويلة \n مسابقة --- الساحر المجنون";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            textUse = "خطأ";
                            var r = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            textUse = "خطأ";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            Application.Exit();

                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            textUse = "!أحسنت! لقد وجدت الإجابة الصحيحة! يمكنك الآن الانتقال إلى الاختبار 2";
                            var r = CustomDialogForm.Show(textUse, langue);
                            canRun();
                            question1 = true;
                            label1.Text = "بدء الاختبار 2";
                            return;
                        }
                    }
                }
                if (question1 == true && question2 == false) // si la question 1 à été répondu et que la question deux n'a pas encore été répondue
                {
                    stopRun();
                    if (langue == "fr")
                    { 
                        textUse = "Quiz 2 :";
                        var r = CustomDialogForm.Show(textUse, langue);
                        textUse = "Hawaï appartient à quel pays ? \n\n Y = États-Unis \n N = Amérique du sud \n C = Asie \n QUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            textUse = "Bravo !! Vous avez trouvé la bonne réponse ! Vous pouvez passer au quiz 3 dès aprésent !";
                            var res = CustomDialogForm.Show(textUse, langue);
                            question2 = true;
                            label1.Text = "Commencer Quiz 3";
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            textUse = "FAUX !!";
                            var res = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            textUse = "FAUX !!";
                            var res = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        textUse = "Cuestionario 2:";
                        var res = CustomDialogForm.Show(textUse, langue);
                        textUse = "¿A qué país pertenece Hawai? \n\n Y = Estados Unidos \n N = América del Sur \n C = Asia \n QUIZ  --  ¡EL HECHICERO LOCO!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            textUse = "¡Bien hecho! Has encontrado la respuesta correcta. Ahora puedes pasar a la prueba 3.";
                            var a = CustomDialogForm.Show(textUse, langue);
                            question2 = true;
                            label1.Text = "Comenzar Cuestionario 3";
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            textUse = "¡FALSO!";
                            var a = CustomDialogForm.Show(textUse, langue);
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("Quiz 2:");
                        textUse = "Which country does Hawaii belong to? \n\n Y = United States \n N = Amérique du sud \n C = Asia \n QUIZ  --  THE MAD WIZZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Well done! You've found the right answer! You can now go on to quiz 3!");
                            question2 = true;
                            label1.Text = "Start Quiz 3";
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("False!!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("False!!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show(": السؤال 2");
                        textUse = "إلى أي دولة تنتمي هاواي؟ \n\n Y = الولايات المتحدة الأمريكية \n N = أمريكا الجنوبية \n C = آسيا \n مسابقة --- الساحر المجنون ";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("أحسنت! لقد وجدت الإجابة الصحيحة! يمكنك الآن الانتقال إلى الاختبار 3 ");
                            question2 = true;
                            label1.Text = "بدء الاختبار 3";
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                }
                if (question2 == true && question3 == false) // si la question deux à été répondu et que la question trois n'a pas encore été répondue
                {
                    stopRun();
                    if (langue == "fr"){
                        MessageBox.Show("Quiz 3 :");
                        textUse = "Quelle est la planète la plus proche du soleil ? \n\n Y = Mars \n N = Terre \n C = Mercure \nQUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("FAUX !!");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Bravo !! Vous avez trouver la bonne réponse ! Vous pouvez passer au quiz 4 dès aprésent !");
                            label1.Text = "Start Quiz 4";
                            question3 = true;
                            canRun();
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("Quiz 3:");
                        textUse = "Which planet is closest to the sun? \n\n Y = March \n N = Earth \n C = Mercure \nQUIZ  --  THE MAD WIZZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("False!!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("False!!");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Congratulations! You've found the right answer! You can now move on to quiz 4!");
                            label1.Text = "Start Quiz 4";
                            question3 = true;
                            canRun();
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        MessageBox.Show("Quiz 3 :");
                        textUse = "¿Qué planeta está más cerca del Sol? \n\n Y = Marzo \n N = Tierra \n C = Mercure \nQUIZ -- ¡EL MAGO LOCO!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("¡FALSO!");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("¡Bien hecho! Has encontrado la respuesta correcta. Ahora puedes pasar a la prueba 4.");
                            label1.Text = "Iniciar Cuestionario 4";
                            question3 = true;
                            canRun();
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show(":الاختبار 3");
                        textUse = "ما الكوكب الأقرب إلى الشمس؟ \n\n Y = مارس \n N = الأرض \n C = ميركيور \nمسابقة - الساحر المجنون";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("خطأ");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("أحسنت! لقد وجدت الإجابة الصحيحة! يمكنك الآن الانتقال إلى الاختبار 4");
                            label1.Text = "بدء الاختبار 4";
                            question3 = true;
                            canRun();
                            return;
                        }
                    }
                }
                if (question3 == true && question4 == false) // si la question 3 à été répondu et que la question 4 n'a pas encore été répondue
                {
                    stopRun();
                    if (langue == "fr")
                    {
                        textUse = "Où se trouve l’hypothalamus dans le corps humain? \n\n Y = Dans les yeux \n N = Dans le cerveau \n C = Dabs l'estomac \nQUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        MessageBox.Show("Quiz 4 :");
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Bravo !! Vous avez trouver la bonne réponse ! Vous pouvez passer au quiz 5 dès aprésent !");
                            label1.Text = "Start Quiz 5";
                            question4 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("Quiz 4:");
                        textUse = "Where is the hypothalamus located in the human body? \n\n Y = In the eyes \n N = In the brain \n A = In the stomach \nQUIZ  --  THE MAD WIZZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("False!!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Congratulations! You've found the right answer! You can now move on to quiz 5!");
                            label1.Text = "Start Quiz 5";
                            question4 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("False!!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        MessageBox.Show("Cuestionario 4:");
                        textUse = "¿Dónde se encuentra el hipotálamo en el cuerpo humano? \n\n Y = En los ojos \n N = En el cerebro \n C = En el estómago \nQUIZ -- ¡EL MAGO LOCO!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("¡Bien hecho! Has encontrado la respuesta correcta. Ahora puedes pasar a la prueba 5.");
                            label1.Text = "Iniciar Cuestionario 5";
                            question4 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show(":الاختبار 4");
                        textUse = "أين يقع الوطاء في جسم الإنسان؟ \n\n Y = في العيون \n N = في الدماغ \n C = في المعدة \nمسابقة - الساحر المجنون";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("أحسنت! لقد وجدت الإجابة الصحيحة! يمكنك الآن الانتقال إلى الاختبار 5");
                            label1.Text = "ابدأ الاختبار 5";
                            question4 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                }
                if (question4 == true && question5 == false) // si la question 4 à été répondu et que la question 5 n'a pas encore été répondue
                {
                    stopRun();
                    if (langue == "fr"){
                        MessageBox.Show("Quiz 5 :");
                        textUse = "Quel est le plus grand lac entièrement en Suisse ? \n\n Y = Lac de Zurich \n N =  Lac de Neuchâtel\n C = Lac Léman \nQUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Bravo !! Vous avez trouver la bonne réponse ! Vous pouvez passer au quiz 6 dès aprésent !");
                            label1.Text = "Start Quiz 6";
                            question5 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        MessageBox.Show("Cuestionario 5 :");
                        textUse = "¿Cuál es el lago más grande de Suiza? \n\n Y = Lago de Zúrich \n N =  Lago de Neuchâtel\n C = Lago Lemán \nQUIZ -- ¡EL MAGO LOCO!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("¡Bien hecho! Has encontrado la respuesta correcta. Ahora puedes pasar a la prueba 6.");
                            label1.Text = "Iniciar Cuestionario 6";
                            question5 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("Quiz 5:");
                        textUse = "Which is the largest lake entirely in Switzerland? \n\n Y = Lake Zurich \n N =  Lake Neuchâtel\n C = Lake Leman \nQUIZ  --  THE MAD WIZZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("False!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Congratulations! You've found the right answer! You can now move on to quiz 6!");
                            label1.Text = "Start Quiz 6";
                            question5 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("False!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show(":الاختبار 5");
                        textUse = "ما هي أكبر بحيرة في سويسرا؟ \n\n Y = بحيرة زيورخ \n N =  بحيرة نوشاتيل\n C = بحيرة جنيف \nمسابقة - الساحر المجنون";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("أحسنت! لقد وجدت الإجابة الصحيحة! يمكنك الآن الانتقال إلى الاختبار 6");
                            label1.Text = "بدء الاختبار 6";
                            question5 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                }
                if (question5 == true && question6 == false) // si la question 5 à été répondu et que la question 6 n'a pas encore été répondue
                {
                    stopRun();
                    if (langue == "fr"){
                        MessageBox.Show("Quiz 6 :");
                        textUse = "Dans quelle ville se déroule l’action principale de Les Trois Mousquetaires ? \n\n Y = Paris \n N =  Londres\n C = Bordeaux \nQUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Bravo !! Vous avez trouver la bonne réponse ! Vous pouvez passer au quiz 7 dès aprésent !");
                            label1.Text = "Start Quiz 7";
                            question6 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        MessageBox.Show("Cuestionario 6 :");
                        textUse = "¿En qué ciudad transcurre la acción principal de Los tres mosqueteros? \n\n Y = París \n N =  Londres\n C = Burdeos \nQUIZ -- ¡EL MAGO LOCO!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("¡Bien hecho! Has encontrado la respuesta correcta. Ahora puedes pasar a la prueba 7.");
                            label1.Text = "Iniciar Cuestionario 7";
                            question6 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("Quiz 6 :");
                        textUse = "In which city does the main action of The Three Musketeers take place? \n\n Y = Paris \n N =  London\n C = Bordeaux \nQUIZ -- THE MAD WIZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("Congratulations! You've found the right answer! You can now move on to quiz 7!");
                            label1.Text = "Start Quiz 7";
                            question6 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("FALSE!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("FALSE!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show("الاختبار 6 :");
                        textUse = "في أي مدينة تدور الأحداث الرئيسية لفيلم الفرسان الثلاثة؟ \n\n Y = باريس \n N =  لندن\n C = بوردو \nمسابقة - الساحر المجنون";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            MessageBox.Show("أحسنت! لقد وجدت الإجابة الصحيحة! يمكنك الآن الانتقال إلى الاختبار 7");
                            label1.Text = "بدء الاختبار 7";
                            question6 = true;
                            canRun();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                    }
                }
                if (question6 == true && question7 == false) // si la question 6 à été répondu et que la question 7 n'a pas encore été répondue
                {
                    stopRun();
                    if (langue == "fr"){
                        MessageBox.Show("Quiz 7 :");
                        textUse = "Quel film a remporté la Palme d’Or au Festival de Cannes 2024 ? \n\n Y = The Substance de Coralie Fargeat \n N = La Plus Précieuse des marchandises de Michel Hazanavicius\n C = Anora de Sean Baker \nQUIZ  --  LE SORCIER FOU !";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("FAUX !!");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("FAUX !!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            question7 = true;
                            MessageBox.Show("Bravo !! Vous avez trouver la bonne réponse !C'était le dernier quiz ! Vous pouvez changer de salle");
                            label1.Text = "Plus de QUIZ";
                            canRun();
                            return;
                        }
                    }
                    if (langue == "es")
                    {
                        MessageBox.Show("Cuestionario 7 :");
                        textUse = "¿Qué película ganó la Palma de Oro en el Festival de Cannes 2024? \n\n Y = La sustancia de Coralie Fargeat \n N = La mercancía más preciada de Michel Hazanavicius\n C = Anora de Sean Baker \nQUIZ -- ¡EL MAGO LOCO!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("¡FALSO!");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("¡FALSO!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            question7 = true;
                            MessageBox.Show("¡Bien hecho! ¡Has encontrado la respuesta correcta! ¡Esa era la última prueba! Puedes cambiar de habitación");
                            label1.Text = "Más QUIZ";
                            canRun();
                            return;
                        }
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("Quiz 7:");
                        textUse = "Which film won the Palme d'Or at Cannes 2024? \n\n Y = The Substance by Coralie Fargeat \n N = The Most Precious Commodity by Michel Hazanavicius\n C = Anora by Sean Baker \nQUIZ  --  THE MAD WIZZARD!";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("False!!");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("False!!");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            question7 = true;
                            MessageBox.Show("Congratulations! You've found the right answer! That was the last quiz! You can change room");
                            label1.Text = "No more quizzes";
                            canRun();
                            return;
                        }
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show(":الاختبار 7 ");
                        textUse = "أي فيلم فاز بالسعفة الذهبية في مهرجان كان 2024؟ \n\n Y = المادة بقلم كورالي فارجيت \n N = أثمن سلعة ثمينة لميشيل هازانافيتشيوس\n C = أنورا من شون بيكر \nمسابقة - الساحر المجنون";
                        var result = CustomDialogForm2.Show(textUse, langue);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("خطأ");
                            canRun();
                            Application.Exit();
                            return;
                        }
                        if (result == DialogResult.No)
                        {
                            MessageBox.Show("خطأ");
                            Application.Exit();
                            canRun();
                            return;
                        }
                        if (result == DialogResult.Cancel)
                        {
                            SoundPlayer acclamer = new SoundPlayer(Properties.Resources.Acclmation);
                            acclamer.Play();
                            question7 = true;
                            MessageBox.Show("أحسنت! لقد وجدت الإجابة الصحيحة! كان هذا آخر اختبار! يمكنك تغيير الغرف");
                            label1.Text = "المزيد من الاختبارات";
                            canRun();
                            return;
                        }
                    }
                }
                if (question7 == true ) // si la question 7 à été répondu
                {
                    stopRun(); 
                    if (langue == "fr"){
                        MessageBox.Show("Il n'y a plus de quiz. Vous avez tout déchirer ! \n Mais si vous souhaîter encore avoir des questions, vous pouvez très bien relancer le jeu, même si ce sera les mêmes...");
                    }
                    if (langue == "es")
                    {
                        MessageBox.Show("No hay más pruebas. ¡Lo has clavado! \n Pero si sigues teniendo dudas, puedes reiniciar el juego, aunque sean las mismas...");
                    }
                    if (langue == "en")
                    {
                        MessageBox.Show("There are no more quizzes. You've killed it! \n But if you still have questions, you can always restart the game, even if it's the same ones...");
                    }
                    if (langue == "ar")
                    {
                        MessageBox.Show("لم يعد هناك المزيد من الاختبارات. لقد نجحت \n ولكن إذا كانت لا تزال لديك أسئلة، يمكنك إعادة تشغيل اللعبة، حتى لو كانت الأسئلة نفسها");
                    }
                    canRun();
                        return;
                }
            }
            if (File.Exists(fileName))
            {

            }
            else // si on a pas récupérer les trois papiers ( donc que le fichier "Papers" n'exist pas )
            {
                stopRun();
                if (langue == "fr")
                {
                    MessageBox.Show("VOUS N'AVEZ PAS PRIS LES PAPIERS (en tout cas pas tous) DONC VOUS DEVEZ RECOMMENCER !!!!! À PARTIR DE ZÉRO !!!!");
                    MessageBox.Show("^^");
                    MessageBox.Show(":D");
                }
                if (langue == "es")
                {
                    MessageBox.Show("NO HA TOMADO LOS PAPELES (al menos no todos) POR LO QUE DEBE RECOMENDAR !!!!! desde cero !!!!");
                    MessageBox.Show("^^");
                    MessageBox.Show(":D");
                }
                if (langue == "ar")
                {
                    MessageBox.Show("لم تقم بأخذ الأوراق (على الأقل ليس كلها) لذا يجب عليك التوصية !!!!! من الصفر !!!!");
                    MessageBox.Show("^^");
                    MessageBox.Show(":D");
                }
                if (langue == "en")
                {
                    MessageBox.Show("YOU HAVE NOT TAKEN THE PAPERS (at least not all of them) SO YOU MUST RESTART !!!!! FROM ZERO !!!!");
                    MessageBox.Show("^^");
                    MessageBox.Show(":D");
                }
                Application.Exit();
                canRun();
            }
            if (level2 == true) // si on vas au monde 3 (niveau 2)
            {
                Change = true;
                this.Hide();
                Form3 frm = new Form3(); // lance le form3
                frm.Show();
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel96_Paint(object sender, PaintEventArgs e)
        {

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

        private void button12_Click(object sender, EventArgs e) // si on clique sur le bouton pour activé/désacivé le timer
        {
            if (timer == false && Change == false) // si le timer est désactivé, alors on l'active et que si on est toujours dans le monde actuelle
            {
                timer = true;

                this.ActiveControl = null; // Désélectionne le bouton
                return;
            }
            if (timer == true && Change == false) // si le timer est actife, alors on le désactive et que si on est toujours dans le monde actuelle
            {
                timer = false;

                this.ActiveControl = null; // Désélectionne le bouton
                return;
            }
            this.ActiveControl = null; // Désélectionne le bouton
        }
        private void fr () // change la langue en fr et supprime les fichiers des autres langue (les autres forms vérifies c'est fichier pour changer leur langue)
        {
            string filname = path +@"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
            if (File.Exists(filname) == false)
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
            langue = "fr";
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
            langue = "es";
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
            langue = "en";
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
            langue = "ar";
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

        private void panel96_Click(object sender, EventArgs e) // si on clique sur le menu
        {
            this.ActiveControl = null; // Désélectionne le bouton
        }

        private void label12_Click(object sender, EventArgs e) // si on clique sur le bouton retour ( enlève les règles )
        {
            panel96.Visible = true;
            panel98.Visible = false;
        }

        private void button15_Click(object sender, EventArgs e) // si on clique sur le bouton des règles ( fait appraitre les règles )
        {
            panel96.Visible = false;
            panel98.Visible = true;
        }

        private void panel98_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
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

        private void canRun() // Redonne la possibilité de marcher au joueur
        {
            move = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (langue == "fr" && Change == false) 
            {
                label13.Text = "Crédits";
                label10.Text = "Règles";
                label11.Text = "Il faut s'échapper de ce monde en résolvant toutes les énigmes qui viendrons à se présenter\r\n\r\nSe déplacer : \r\nUtilisez WASD\r\n\r\nLes intéractions se feront toujours avec E";
                label12.Text = "Retour";
                button15.Text = "Règles";
                button14.Text = "Réinitialiser";
                button13.Text = "Langue : Français";
                string filename = path + @"\fr.txt";
                string filename2 = path + @"\es.txt";
                string filename3 = path + @"\en.txt";
                string filename4 = path + @"\ar.txt";
                if (File.Exists(filename) == false)
                {
                    File.Create(filename).Close();
                }
                if (File.Exists(filename2))
                {
                    File.Delete(filename2);
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
            if (langue == "es" && Change == false)
            {
                label13.Text = "Créditos";
                label10.Text = "Menstruación";
                label11.Text = "Tienes que escapar de este mundo resolviendo todos los acertijos que se te presenten\r\n\r\nMover: \r\nUsar WASD\r\n\r\nLas interacciones siempre serán con E";
                label12.Text = "Devolución";
                button15.Text = "Menstruación";
                button14.Text = "Restablecimiento";
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
            if (langue == "en" && Change == false)
            {
                label13.Text = "Credits";
                label10.Text = "Rules";
                label11.Text = "You have to escape from this world by solving all the riddles that come your way\r\n\r\nMove: \r\nUse WASD\r\n\r\nInteractions will always be with E";
                label12.Text = "Return";
                button15.Text = "Rules";
                button14.Text = "Restart";
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
            if (langue == "ar" && Change == false)
            {
                label13.Text = "الائتمانات";
                label10.Text = "حيض";
                label11.Text = "عليك الهروب من هذا العالم من خلال حل كل الألغاز التي تأتي في طريقك\r\n\r\n:حرك\r\nWASD استخدم\r\n\r\nE ستكون التفاعلات دائما مع ";
                label12.Text = "أعاد";
                button15.Text = "حيض";
                button14.Text = "اعاده تعيين";
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
            string filname5 = path + @"\timer.txt";
            if (monde3 == false && Change == false){ 
                if (menu == true || panel98.Visible == true && Change == false) // si le menu est ouvert ou si l'ecran de chargement est encore présent
                {
                    string MenuFile = path + @"\menu.txt";
                    if (File.Exists(MenuFile) == false)
                    {
                        File.Create(MenuFile).Close(); // crée le fichier entre paranthèse
                    }
                }
                else if (Change == false) 
                {

                    string MenuFile = path + @"\menu.txt";
                    if (File.Exists(MenuFile) == true && Change == false)
                    {
                        File.Delete(MenuFile); // supprime le fichier entre paranthèse
                    }
                }
                if (timer == false && Change == false)
                {
                    if (File.Exists(filname5))
                    {
                        File.Delete(filname5); // supprime le fichier entre paranthèse
                    }
                }
                if (timer == true && Change == false)
                {
                    if (File.Exists(filname5) == false)
                    {
                        File.Create(filname5).Close(); // crée le fichier entre paranthèse
                    }
                }
            }
            
            if (File.Exists(filname5) == false) // si le timer est désactivé
            {
                if (langue == "en")
                {
                    button12.Text = "Timer : Disabled";
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
            if (File.Exists(filname5) == true) // si le timer est activés
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
            string filname = path + @"\fr.txt";
            string filname2 = path + @"\es.txt";
            string filname3 = path + @"\en.txt";
            string filname4 = path + @"\ar.txt";
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
            foreach (Control x in panel1.Controls)
            {
                if (x is Panel && (string)x.Tag == "pnj1Int")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        Pnjlbl.Location = new Point(245, 215);
                        if (langue == "fr"){
                            Pnjlbl.Text = "E : Parler";
                        }
                        if (langue == "en")
                        {
                            Pnjlbl.Text = "E : Speak";
                        }
                        if (langue == "es")
                        {
                            Pnjlbl.Text = "E : Hablar";
                        }
                        if (langue == "ar")
                        {
                            Pnjlbl.Text = "E : تحدث";
                        }
                    }
                    else
                    {
                        Pnjlbl.Text = "";
                    }

                }
                if (langue == "fr")
                {
                    label1.Text = "Commencer \n Quiz";
                }
                if (langue == "en")
                {
                    label1.Text = "Start Quiz";
                }
                if (langue == "es")
                {
                    label1.Text = "Iniciar cuestionario";
                }
                if (langue == "ar")
                {
                    label1.Text = "بدء الاختبار";
                }
                if (x is Panel && (string)x.Tag == "Left")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        player.Left -= 13;

                    }
                    else
                    {
                    }
                }
                if (x is Panel && (string)x.Tag == "Right")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // si le personnage se trouve dans la deuxième salle
                    {
                        player.Left += 13;
                    }
                    else
                    {
                    }
                }

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
            if (moveLeft && move == true)
            {
                player.Left -= speed;
                player.Image = Properties.Resources.profil_A;
            }
            if (moveRight && move == true)
            {

                player.Image = Properties.Resources.profil_D;
                player.Left += speed;
            }


            if (moveUp && move == true)
            {
                player.Image = Properties.Resources.profil_w;
                player.Top -= speed;

            }

            if (moveDown && move == true)
            {
                player.Image = Properties.Resources.profil__2_;
                player.Top += speed;


            }
        }
        protected override CreateParams CreateParams // rend le jeu plus fluide
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
