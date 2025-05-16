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
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Try_To_ESCAPE__GAME
{
    public partial class Form3 : Form
    {
        int fire = 0; //quand on clique, fire +5 | le temps enlève 1 chaque 100 milieseconde (0.1secondes) son max est 250
        double time = 0; // quand fire arrvie à 250, time part de zero et fait un chrono ne 10 seconde. Si fire tombe en dessous de 240, time se remet à zero
        bool speak = false;// false : le texte de fin n'est pas encore apparu / true : le texte de fin est déjà apparu
        bool timer = false; // false : timer désactivé | true : timer activé
        string textUse; // variable contenant le texte besoin pour le "CustomMessageBox"
        string langue = "fr"; // langue choisie par le joueur dans le menu en appuyant sur le bouton "langue"
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\games"; // chemin de ce fichier
        bool menu = false; // false : menu fermé / true : menu ouvert
        public Form3()
        {
            InitializeComponent();
            panel1.GetType().InvokeMember("DoubleBuffered", // améliore la fluidité du jeu
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.SetProperty,
            null, panel1, new object[] { true });
            string filname5 = path + @"\timer.txt"; // chemin d'accès au fichier "timer.txt"
            if (File.Exists(filname5)) // vérifie si le fichier "timer" exist
            {
                timer = true; // active le timer
            }
            else // dans le cas ou le fichier n'exist pas
            {
                timer = false; // désactive le timer
            }

        }
        private void Form3_Load(object sender, EventArgs e)
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        private void fr()  // change la langue en fr et supprime les fichiers des autres langue (les autres forms vérifies c'est fichier pour changer leur langue)
        {
            string filname = path +@"\fr.txt"; // chemin d'accès au fichier "fr.txt"
            string filname2 = path + @"\es.txt"; // chemin d'accès au fichier "es.txt"
            string filname3 = path + @"\en.txt"; // chemin d'accès au fichier "en.txt"
            string filname4 = path + @"\ar.txt"; // chemin d'accès au fichier "ar.txt"
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
                File.Create(filname2);
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

        private void timer1_Tick(object sender, EventArgs e) // se fait chaque 10 miliesecondes
        {
            if (langue == "fr")
            {
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
            if (langue == "es")
            {
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
            if (langue == "en")
            {
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
            if (langue == "ar")
            {
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
            if (langue == "fr")
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
            if (langue == "es")
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
            if (langue == "en")
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
            if (langue == "ar")
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
            if (menu == true || panel98.Visible == true) // si le menu est ouvert ou si l'ecran de règles est ouvert
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
                    File.Delete(MenuFile); // supprime le fichier entre paranthèse
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
                    File.Create(filname5).Close();// crée le fichier entre paranthèse
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
            double arrondi = Math.Round(time, 2); // arrondi la varaible time ( pour ne pas avoir de chiffres : 9.99999999999999)
            if (fire -1 >= 0)
            {

                fire = fire - 1;
            }
            if (fire <= 250 && fire >= 240)
            {
                Red.Visible = true;
            }
            else
            {

                Red.Visible = false;
            }
            if (fire >= 200)
            {
                Orange.Visible = true;
            }
            else
            {
                Orange.Visible = false;
            }
            if (fire >= 150)
            {
                Yellow.Visible = true;
            }
            else
            {
                Yellow.Visible = false;
            }
            if (fire >= 100)
            {
                Green2.Visible = true;
            }
            else
            {
                Green2.Visible = false;
            }
            if (fire >= 50)
            {
                Green.Visible = true;
            }
            else
            {
                Green.Visible = false;
            }
            if (arrondi == 10 && speak == false)
            {
                speak = true;
                if (langue == "fr")
                {
                    textUse = "Magnifique entrainement vous avez tenu les 10 secondes au max de votre Vitesse ! Votre prise de masse est alucinante ! ";
                    DialogResult result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "es")
                {
                    textUse = "Magnífico entrenamiento, ¡has durado 10 segundos a máxima velocidad! ¡Tu aumento de peso es asombroso! ";
                    DialogResult result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "en")
                {
                    textUse = "Magnificent training - you lasted 10 seconds at your maximum speed! Your weight gain is amazing! ";
                    DialogResult result = CustomDialogForm.Show(textUse, langue);
                }
                if (langue == "ar")
                {
                    textUse = "تدريب رائع، لقد استغرقت 10 ثوانٍ بأقصى سرعة! زيادة وزنك مذهلة ";
                    DialogResult result = CustomDialogForm.Show(textUse, langue);
                }
                this.Hide();
                string filename2 = path + @"\fini.txt";
                if (File.Exists(filename2) == false)
                {
                    File.Create(filename2).Close();
                }
                Form4 frm = new Form4();
                frm.Show();
            }
        }
        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e) // si je clique sur le panel
        {
            if (fire + 5 >= 250) // si on ajoute 5 à fire  et qu'il est plus grand ou égale à 250 ( permet de ne pas avoir des chiffres plus grands que prévu)
            {
                fire = 250;
                if (langue == "fr"){
                    label1.Text = "Vitesse : " + fire;
                }
                if (langue == "es")
                {
                    label1.Text = "Velocidad : " + fire;
                }
                if (langue == "en")
                {
                    label1.Text = "Speed : " + fire;
                }
                if (langue == "ar")
                {
                    label1.Text = fire + ":السرعة";
                }
            }
            else 
            {

                fire = fire + 5;
                if (langue == "fr")
                {
                    label1.Text = "Vitesse : " + fire;
                }
                if (langue == "es")
                {
                    label1.Text = "Velocidad : " + fire;
                }
                if (langue == "en")
                {
                    label1.Text = "Speed : " + fire;
                }
                if (langue == "ar")
                {
                    label1.Text = fire + ":السرعة";
                }
            }
            
            
        }
        private void timer2_Tick(object sender, EventArgs e) // se fait chaque 100 miliesecondes (0.1 secondes)
        {
            if (fire + 5 >= 250)
            {

                if (langue == "fr")
                {
                    label1.Text = "Vitesse : " + fire;
                }
                if (langue == "es")
                {
                    label1.Text = "Velocidad : " + fire;
                }
                if (langue == "en")
                {
                    label1.Text = "Speed : " + fire;
                }
                if (langue == "ar")
                {
                    label1.Text = fire + ":السرعة";
                }
            }
            else
            {

                if (langue == "fr")
                {
                    label1.Text = "Vitesse : " + fire;
                }
                if (langue == "es")
                {
                    label1.Text = "Velocidad : " + fire;
                }
                if (langue == "en")
                {
                    label1.Text = "Speed : " + fire;
                }
                if (langue == "ar")
                {
                    label1.Text = fire + ":السرعة";
                }
            }
            double arrondi = Math.Round(time, 2); // arrondi timer pour ne pas avoir des chiffre : 9.999999999999
            if (langue == "fr"){
                label2.Text = "Timer : " + arrondi;
            }
            if (langue == "es")
            {
                label2.Text = "Temporizador : " + arrondi;
            }
            if (langue == "en")
            {
                label2.Text = "Timer : " + arrondi;
            }
            if (langue == "ar")
            {
                label2.Text = arrondi + ":المؤقِّت";
            }
            if (fire >= 240)
            {
                panel1.BackgroundImage = Properties.Resources.FireBack; // le fond se change en feu quand fire est plsu gran que 240
                if (arrondi + 0.01 <= 10 )
                {

                 time = time + 0.01;
                }
            }
            if (fire < 240)
            {
                panel1.BackgroundImage = Properties.Resources.piste_de_course; // mais si fire retombe en dessous de 240, le feu dissparait
                time = 0;
            }
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

        private void Form3_FormClosing(object sender, FormClosingEventArgs e) // permet de fermer l'application entière si on ferm Fomr3 ( pour ne pas avoir le processus qui tourne en fond sans le savoir)
        {
            Application.Exit();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
           
        }

        private void button12_Click(object sender, EventArgs e) // si on clique sur le bouton timer, sois ça le désactive, sois ça l'active
        {
            if (timer == false) // active le timer
            {
                timer = true;

                this.ActiveControl = null; // Désélectionne le bouton
                return;
            }
            if (timer == true) // désactive le timer
            {
                timer = false;

                this.ActiveControl = null; // Désélectionne le bouton
                return;
            }
            this.ActiveControl = null; // Désélectionne le bouton


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
        private void men() // permet d'ouvrir ou fermer le menu
        {
            if (menu == false) // ferm le menu
            {
                panel96.Visible = false;
            }
            if (menu == true) // ouvre le menu
            {
                panel96.Visible = true;
            }
        }

        private void Form3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && menu == false) // permet de ouvrir le menu
            {
                menu = true;
                men();
                return;
            }
            if (e.KeyCode == Keys.Escape && menu == true) // permet de fermer le menu
            {
                menu = false;
                men();
                return;
            }
        }

        private void panel96_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel96_Click(object sender, EventArgs e) // si on clique sur le fond noir du menu
        {
            this.ActiveControl = null; // Désélectionne le bouton 
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

        private void button15_Click(object sender, EventArgs e) // ouvre les règles
        {
            panel96.Visible = false;
            panel98.Visible = true;
        }

        private void label3_Click(object sender, EventArgs e) // ferm les règles
        {
            panel96.Visible = true;
            panel98.Visible = false;
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
    }
}
