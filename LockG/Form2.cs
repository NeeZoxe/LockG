using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Management.Automation.Language;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using DotRas;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin.Messaging;
using Guna.UI2.WinForms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static LockG.Form2;

namespace LockG
{
    public partial class Form2 : Form
    {
        private string userId;
        private FirebaseClient firebaseClient;
        private Image ImageUser; // Déclarez la variable au niveau de la classe
        private Image ImageAddUser; // Déclarez la variable au niveau de la classe
        private string selectedUserId = null;
        // Variable pour stocker l'ID de l'utilisateur partageant des informations
        private string selectedSharedUserId = null;

        public Form2(string userId)
        {
            InitializeComponent();
            this.userId = userId;
            firebaseClient = new FirebaseClient("https://lockg-92e09-default-rtdb.europe-west1.firebasedatabase.app/");
            ImageUser = Image.FromFile("ImageUser.png");
            ImageAddUser = Image.FromFile("AddFriends.png");
        }

        private async Task<string> GetUsernameFromId(string id)
        {
            try
            {
                var utilisateur = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.Id == id);

                if (utilisateur != null)
                {
                    return utilisateur.Object.nom_utilisateur;
                }
                else
                {
                    return "Utilisateur non trouvé"; // Ou une autre valeur par défaut
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return "Erreur"; // Gérer l'erreur selon vos besoins
            }
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            // Charger les sites par défaut
            foreach (SiteInfo site in siteList)
            {
                string listItemText = $"Site: {site.SiteName} User: {site.Username} Password: {site.Password}";
                listSites.Items.Add(listItemText);
            }

            // Limiter la largeur maximale du Panel6 à 230 pixels
            panel6.MaximumSize = new Size(230, int.MaxValue);

            // Ajuster automatiquement la hauteur du Panel6 en fonction de la taille de la fenêtre
            panel6.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            // Panel invible start appli
            EditListPanel.Visible = false;
            DashBoardPanel.Visible = false;
            SettingsPanel.Visible = false;
            VPNpanel.Visible = false;
            panelShare.Visible = false;
            panelSearchUser.Visible = false;
            PanelShareViewer.Visible = false;
            PanelPartageMdpFinal.Visible = false;
            PanelViewerInfosSelection.Visible = false;
            MyPagePanel.Visible = true;

            // vpn point default
            guna2CirclePictureBox2.Visible = true;

            LoadListFromFile(userId);

            // Initialiser les informations
            InitializeUserInfo();
            InitializeNetworkInfo();
            InitializeSystemInfo();

            // Affichez la valeur de userId pour le débogage
            string username = await GetUsernameFromId(userId);
            Usernameboxlog.Text = "User : " + username;

            // Chargez les informations de l'utilisateur actuel
            var currentUser = new Utilisateur { Id = userId };

            EnsureVpnConnection();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private int encryptionShift = 3; // Utilisez la même valeur que vous utilisez pour le chiffrement

        private string EncryptSimple(string input, int shift)
        {
            string encryptedText = "";

            foreach (char c in input)
            {
                char encryptedChar = c;

                if (IsPrintableCharacter(c))
                {
                    char baseChar = ' ';
                    int charRange = 95; // Number of printable ASCII characters
                    encryptedChar = (char)(baseChar + (c - baseChar + shift) % charRange);
                }

                encryptedText += encryptedChar;
            }

            return encryptedText;
        }

        private string DecryptSimple(string encryptedInput, int shift)
        {
            string decryptedText = "";

            foreach (char c in encryptedInput)
            {
                char decryptedChar = c;

                if (IsPrintableCharacter(c))
                {
                    char baseChar = ' ';
                    int charRange = 95; // Number of printable ASCII characters
                    decryptedChar = (char)(baseChar + (c - baseChar - shift + charRange) % charRange);
                }

                decryptedText += decryptedChar;
            }

            return decryptedText;
        }

        private bool IsPrintableCharacter(char c)
        {
            // Check if the character is within the printable ASCII range
            return c >= ' ' && c <= '~';
        }

        // Chiffrement et enregistrement
        private void SaveListToFile(string userId)
        {
            string json = JsonConvert.SerializeObject(siteList);

            string encryptedJson = EncryptSimple(json, encryptionShift);

            string fileName = $"{userId}_siteList.json"; // Ajoutez l'ID de l'utilisateur au nom du fichier
            File.WriteAllText(fileName, encryptedJson);
        }


        private void LoadListFromFile(string userId)
        {
            string fileName = $"{userId}_siteList.json"; // Ajoutez l'ID de l'utilisateur au nom du fichier

            if (File.Exists(fileName))
            {
                string encryptedJson = File.ReadAllText(fileName);

                string decryptedJson = DecryptSimple(encryptedJson, encryptionShift);

                siteList = JsonConvert.DeserializeObject<List<SiteInfo>>(decryptedJson);

                listSites.Items.Clear(); // Clear the ListBox first

                foreach (SiteInfo site in siteList)
                {
                    string listItemText = $"Site: {site.SiteName} User: {site.Username} Password: {site.Password}";
                    listSites.Items.Add(listItemText);
                }
            }
            else
            {
                siteList = new List<SiteInfo>();
            }
        }


        // Créez une classe pour stocker les informations du site
        public class SiteInfo
        {
            public string SiteName { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class SiteListBoxItem
        {
            public string SiteName { get; set; }
            public SiteInfo SiteInfo { get; set; }

            public override string ToString()
            {
                return SiteName;
            }
        }

        // Dans le gestionnaire d'événements du bouton "Enregistrer"
        private List<SiteInfo> siteList = new List<SiteInfo>(); // Liste pour stocker les sites temporairement

        private void btnSave_Click(object sender, EventArgs e)
        {
            string siteName = txtSiteName.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            SiteInfo newSite = new SiteInfo
            {
                SiteName = siteName,
                Username = username,
                Password = password
            };
            siteList.Add(newSite);

            txtSiteName.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";

            SiteListBoxItem listItem = new SiteListBoxItem
            {
                SiteName = siteName,
                SiteInfo = newSite
            };
            string listItemText = $"Site: {siteName} User: {username} Password: {password}";
            listSites.Items.Add(listItemText);

            // Appelez la nouvelle fonction pour enregistrer dans la base de données Firebase
            SaveToFirebaseAsync(siteName, username, password);

            SaveListToFile(userId); // Sauvegarde la liste dans le fichier
        }

        // enregistre les informations sauvegarder dans la bdd
        private async Task SaveToFirebaseAsync(string siteName, string username, string password)
        {
            try
            {
                // Créez une clé unique basée sur le nom du site et le nom d'utilisateur
                string uniqueKey = $"{siteName}_{username}";

                // Recherchez l'utilisateur actuel dans la base de données Firebase
                var utilisateur = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.Id == userId);

                if (utilisateur != null)
                {
                    // Recherchez le Credential correspondant par nom, s'il existe
                    Credential existingCredential = null;

                    foreach (var credential in utilisateur.Object.Credentials)
                    {
                        if (credential.CredentialName == uniqueKey)
                        {
                            existingCredential = credential;
                            break;
                        }
                    }

                    // Si le Credential n'existe pas, créez-le
                    if (existingCredential == null)
                    {
                        existingCredential = new Credential
                        {
                            CredentialName = uniqueKey
                        };
                        utilisateur.Object.Credentials.Add(existingCredential);
                    }

                    // Mettez à jour le mot de passe du site existant ou ajoutez un nouveau SiteInfo
                    bool siteInfoUpdated = false;

                    foreach (var siteInfo in existingCredential.SiteInfos)
                    {
                        if (siteInfo.SiteName == siteName)
                        {
                            siteInfo.Password = password;
                            siteInfoUpdated = true;
                            break;
                        }
                    }

                    if (!siteInfoUpdated)
                    {
                        existingCredential.SiteInfos.Add(new SiteInfo
                        {
                            SiteName = siteName,
                            Username = username,
                            Password = password
                        });
                    }

                    // Mettez à jour l'utilisateur dans la base de données Firebase
                    await firebaseClient
                        .Child("utilisateurs")
                        .Child(utilisateur.Key)
                        .PutAsync(utilisateur.Object);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de l'enregistrement dans Firebase : {ex.Message}");
                // Gérer l'erreur selon vos besoins
            }
        }

        private void listSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSites.SelectedIndex != -1)
            {
                // Ne pas remplir les zones de texte ici
            }
            else
            {
                // Si aucun élément n'est sélectionné, effacer les champs
                txtSiteName.Text = "";
                txtUsername.Text = "";
                txtPassword.Text = "";
            }
        }

        private void listSites_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listSites.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                string selectedItemText = listSites.Items[index].ToString();
                string[] parts = selectedItemText.Split(' ');

                if (parts.Length >= 6) // Vérifiez que l'élément a au moins 6 parties (Site:, Nom, User:, Username, Password:, Password)
                {
                    string password = parts[5]; // Index 5 correspond au mot de passe
                    txtSiteInfo.Text = $"{password}";
                }
                else
                {
                    txtSiteInfo.Text = "Invalid item format"; // Gestion si le format de l'élément est incorrect
                }
            }
            else
            {
                txtSiteInfo.Text = ""; // Effacer le contenu du TextBox si aucun élément n'est sélectionné
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            // Effacer la liste actuelle
            listSites.Items.Clear();

            // Ajouter les éléments filtrés à la liste
            foreach (SiteInfo site in siteList)
            {
                if (site.SiteName.ToLower().Contains(searchText))
                {
                    string listItemText = $"Site: {site.SiteName} User: {site.Username} Password: {site.Password}";
                    listSites.Items.Add(listItemText);
                }
            }
        }

        private void siticoneControlBox1_Click(object sender, EventArgs e)
        {
            // Vous pouvez fermer Form2 ici si nécessaire
            this.Close();
            Application.Exit();
        }

        private void siticoneButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (siticoneButton2.Checked == true)
            {
                EditListPanel.Visible = false;
                MyPagePanel.Visible = false;
                SettingsPanel.Visible = false;
                PanelShareViewer.Visible = false;
                VPNpanel.Visible = false;
                panelShare.Visible = false;
                panelSearchUser.Visible = false;
                PanelPartageMdpFinal.Visible = false;
                PanelViewerInfosSelection.Visible = false;
                DashBoardPanel.Visible = true;
            }
        }

        private void ShareButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ShareButton.Checked == true)
            {
                EditListPanel.Visible = false;
                DashBoardPanel.Visible = false;
                MyPagePanel.Visible = false;
                SettingsPanel.Visible = false;
                VPNpanel.Visible = false;
                panelSearchUser.Visible = false;
                PanelShareViewer.Visible = false;
                PanelPartageMdpFinal.Visible = false;
                PanelViewerInfosSelection.Visible = false;
                panelShare.Visible = true;
            }
        }

        private void siticoneButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (siticoneButton3.Checked == true)
            {
                DashBoardPanel.Visible = false;
                MyPagePanel.Visible = false;
                SettingsPanel.Visible = false;
                VPNpanel.Visible = false;
                panelShare.Visible = false;
                panelSearchUser.Visible = false;
                PanelShareViewer.Visible = false;
                PanelPartageMdpFinal.Visible = false;
                PanelViewerInfosSelection.Visible = false;
                EditListPanel.Visible = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (listSites.SelectedIndex != -1)
            {
                int selectedIndex = listSites.SelectedIndex;
                SiteInfo selectedSite = siteList[selectedIndex];

                // Mettre à jour les informations de l'élément sélectionné
                selectedSite.SiteName = txtSiteName.Text;
                selectedSite.Username = txtUsername.Text;
                selectedSite.Password = txtPassword.Text;

                // Mettre à jour la liste dans la ListBox
                listSites.Items[selectedIndex] = $"Site: {selectedSite.SiteName} User: {selectedSite.Username} Password: {selectedSite.Password}";

                SaveListToFile(userId); // Sauvegarde la liste dans le fichier
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listSites.SelectedIndex != -1)
            {
                int selectedIndex = listSites.SelectedIndex;
                siteList.RemoveAt(selectedIndex);

                listSites.Items.RemoveAt(selectedIndex); // Retirer l'élément de la ListBox

                SaveListToFile(userId); // Sauvegarde la liste dans le fichier
            }
        }

        private void txtSearch_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtSearch.Text == "Search")
            {
                txtSearch.Text = ""; // Efface le texte par défaut si c'est "username"
            }
        }

        private void txtSiteName_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtSiteName.Text == "exemple.com")
            {
                txtSiteName.Text = ""; // Efface le texte par défaut si c'est "username"
            }
        }

        private void txtUsername_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Text = ""; // Efface le texte par défaut si c'est "username"
            }
        }

        private void txtPassword_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = ""; // Efface le texte par défaut si c'est "username"
            }
        }

        // Lorsque l'utilisateur clique sur le bouton "Générer"
        private void btnGeneratePassword_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtPasswordLength.Text, out int desiredLength) && desiredLength > 0)
            {
                string generatedPassword = GenerateRandomPassword(desiredLength);
                txtGeneratedPassword.Text = generatedPassword;
                btnCopyPassword.Enabled = true;
            }
            else
            {
                MessageBox.Show("Veuillez entrer un nombre de caractères valide pour le mot de passe.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Lorsque l'utilisateur clique sur le bouton de copie
        private void btnCopyPassword_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGeneratedPassword.Text))
            {
                Clipboard.SetText(txtGeneratedPassword.Text); // Copie le mot de passe dans le presse-papiers
                MessageBox.Show("Mot de passe copié dans le presse-papiers !");
            }
            else
            {
                MessageBox.Show("Veuillez renseigné le nombre de charactère souhaité");
            }
        }

        // Fonction pour générer un mot de passe aléatoire robuste
        private string GenerateRandomPassword(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            Random random = new Random();
            string password = new string(Enumerable.Repeat(characters, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return password;
        }

        // Fonction pour évaluer la force d'un mot de passe
        private int EvaluatePasswordStrength(string password)
        {
            int score = 0;

            // Vérifier la longueur du mot de passe
            if (password.Length >= 8)
            {
                score++;
            }

            // Vérifier la présence de caractères minuscules et majuscules
            if (password.Any(char.IsLower) && password.Any(char.IsUpper))
            {
                score++;
            }

            // Vérifier la présence de chiffres
            if (password.Any(char.IsDigit))
            {
                score++;
            }

            // Vérifier la présence de caractères spéciaux
            if (password.Any(c => !char.IsLetterOrDigit(c)))
            {
                score++;
            }

            return score;
        }

        // Lorsque l'utilisateur clique sur le bouton "Analyser"
        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            int strongPasswords = 0;
            int mediumPasswords = 0;
            int weakPasswords = 0;
            StringBuilder reportBuilder = new StringBuilder();

            foreach (SiteInfo site in siteList)
            {
                int passwordStrength = EvaluatePasswordStrength(site.Password);

                if (passwordStrength >= 4)
                {
                    strongPasswords++;
                }
                else if (passwordStrength >= 3)
                {
                    mediumPasswords++;
                }
                else
                {
                    weakPasswords++;
                    reportBuilder.AppendLine($"Site: {site.SiteName}, Username: {site.Username}, Password: {site.Password}");
                    reportBuilder.AppendLine("Suggestions: Ajouter des caractères spéciaux, nombres, et lettres majuscules.");
                    reportBuilder.AppendLine();
                }
            }

            string report = $"Fort passwords: {strongPasswords}, Moyen passwords: {mediumPasswords}, Faible passwords: {weakPasswords}\r\n\r\n";
            report += "Faible Passwords:\r\n" + reportBuilder.ToString().TrimEnd(); // Utilisation de TrimEnd pour supprimer le dernier saut de ligne

            txtAuditReport.Text = report; // Afficher le rapport dans la TextBox
        }

        // Lorsque l'utilisateur clique sur le bouton "Télécharger Rapport"
        private void btnDownloadReport_Click(object sender, EventArgs e)
        {
            // Vérifier si le rapport est vide
            if (string.IsNullOrWhiteSpace(txtAuditReport.Text))
            {
                MessageBox.Show("Le rapport est vide. Analysez d'abord les mots de passe.", "Rapport Vide", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Ouvrir une boîte de dialogue pour enregistrer le fichier
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Fichiers texte (*.txt)|*.txt";
                saveFileDialog.FileName = "rapport_audit.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Écrire le contenu du rapport dans le fichier
                        File.WriteAllText(saveFileDialog.FileName, txtAuditReport.Text);
                        MessageBox.Show("Le rapport a été enregistré avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Une erreur s'est produite lors de l'enregistrement du rapport : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtLog.Text))
            {
                txtLog.Clear();
            }

            try
            {
                // Sauvegarder l'espace disque avant l'optimisation
                long initialFreeSpace = DriveInfo.GetDrives()[0].AvailableFreeSpace;

                // Mesurer le temps avant l'optimisation
                DateTime startTime = DateTime.Now;

                // Suppression des fichiers temporaires
                Process.Start("cleanmgr.exe", "/sagerun:1");

                // Suppression des fichiers temporaires de l'utilisateur
                string tempFolderPath = Path.GetTempPath();
                long tempFolderSizeBefore = GetDirectorySize(tempFolderPath);
                try
                {
                    Directory.Delete(tempFolderPath, true);
                }
                catch (IOException)
                {
                    // Ignorer les erreurs si le dossier est en cours d'utilisation
                }
                long tempFolderSizeAfter = GetDirectorySize(tempFolderPath);
                long tempSpaceFreed = tempFolderSizeBefore - tempFolderSizeAfter;

                // Calculer le temps écoulé pour l'optimisation
                TimeSpan optimizationTime = DateTime.Now - startTime;

                // Calculer l'espace disque libéré
                long finalFreeSpace = DriveInfo.GetDrives()[0].AvailableFreeSpace;
                long spaceFreed = initialFreeSpace - finalFreeSpace;

                // Calculer le pourcentage d'optimisation en fonction du temps écoulé
                double speedupPercentage = CalculateSpeedupPercentage(optimizationTime);

                // Afficher les statistiques dans la zone de texte
                string statsMessage = $"Espace libéré : {FormatBytes(spaceFreed)}\r\n";
                statsMessage += $"Espace libéré en supprimant les fichiers temporaires : {FormatBytes(tempSpaceFreed)}\r\n";
                statsMessage += $"Votre PC a gagné en rapidité de {speedupPercentage:0.#}% !\r\n";
                txtLog.AppendText(statsMessage);
            }
            catch (Exception ex)
            {
                txtLog.AppendText("Une erreur est survenue : " + ex.Message + "\r\n");
            }
        }

        private long GetDirectorySize(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
        }

        private double CalculateSpeedupPercentage(TimeSpan optimizationTime)
        {
            double baseTimeSeconds = 180; // Temps de référence en secondes
            double speedupPercentage = (baseTimeSeconds - optimizationTime.TotalSeconds) / baseTimeSeconds * 100;
            return Math.Max(speedupPercentage, 0); // Assurez-vous que le pourcentage ne soit pas négatif
        }


        private string FormatBytes(long bytes)
        {
            string[] sizes = { "octets", "Ko", "Mo", "Go", "To" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                order++;
                bytes /= 1024;
            }

            return $"{bytes:0.##} {sizes[order]}";
        }


        private void siticoneButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (siticoneButton1.Checked == true)
            {
                EditListPanel.Visible = false;
                DashBoardPanel.Visible = false;
                SettingsPanel.Visible = false;
                VPNpanel.Visible = false;
                panelShare.Visible = false;
                panelSearchUser.Visible = false;
                PanelPartageMdpFinal.Visible = false;
                PanelShareViewer.Visible = false;
                PanelViewerInfosSelection.Visible = false;
                MyPagePanel.Visible = true;
            }
        }

        private void btnCopyPasswordtxt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSiteInfo.Text))
            {
                Clipboard.SetText(txtSiteInfo.Text);
                MessageBox.Show("Le mot de passe a été copié dans le presse-papiers.", "Copie réussie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("La zone de texte est vide. Aucun mot de passe à copier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSpeedTest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtLog.Text))
            {
                txtLog.Clear();
            }
            txtLog.Text = "Test en cours...\r\n";
            gifLoadSpeedTest.Visible = true;

            try
            {
                string downloadUrl = "http://link.testfile.org/150MB";
                string downloadPath = Path.Combine(Path.GetTempPath(), "speedtest.bin");

                Stopwatch downloadStopwatch = new Stopwatch();
                downloadStopwatch.Start();

                string downloadedContent = await DownloadFileAsync(downloadUrl, downloadPath);

                downloadStopwatch.Stop();

                if (!string.IsNullOrEmpty(downloadedContent))
                {
                    double downloadSpeed = MeasureDownloadSpeed(downloadedContent.Length, downloadStopwatch);

                    gifLoadSpeedTest.Visible = false;

                    txtLog.AppendText($"Débit descendant : {downloadSpeed:F2} Mbps\r\n");
                    txtLog.AppendText($"Temps de téléchargement : {downloadStopwatch.Elapsed.TotalSeconds:F2} secondes\r\n");

                    double uploadSpeed = MeasureUploadSpeed();

                    txtLog.AppendText($"Débit ascendant : {uploadSpeed:F2} Mbps\r\n");

                    string connectionQuality = GetConnectionQuality(downloadSpeed, uploadSpeed);
                    txtLog.AppendText(connectionQuality);
                }
                else
                {
                    gifLoadSpeedTest.Visible = false;

                    txtLog.AppendText("Erreur lors du téléchargement du fichier de test.\r\n");
                }
            }
            catch (Exception ex)
            {
                gifLoadSpeedTest.Visible = false;

                txtLog.AppendText($"Une erreur est survenue : {ex.Message}\r\n");
            }
        }

        private double MeasureDownloadSpeed(long fileSize, Stopwatch downloadStopwatch)
        {
            double fileSizeMB = fileSize / (1024.0 * 1024.0);
            double downloadSpeed = fileSizeMB / downloadStopwatch.Elapsed.TotalSeconds;
            return downloadSpeed;
        }

        private async Task<string> DownloadFileAsync(string url, string filePath)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        byte[] contentBytes = await response.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes(filePath, contentBytes);
                        return File.ReadAllText(filePath);
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        private double MeasureUploadSpeed()
        {
            Ping pingSender = new Ping();
            int timeout = 5000;

            string testAddress = "www.google.com";
            byte[] buffer = new byte[32];
            PingReply reply = pingSender.Send(testAddress, timeout, buffer);

            if (reply.Status == IPStatus.Success)
            {
                double uploadSpeed = reply.Buffer.Length * 8.0 / reply.RoundtripTime;
                return uploadSpeed;
            }
            else
            {
                return 0.0;
            }
        }

        private string GetConnectionQuality(double downloadSpeed, double uploadSpeed)
        {
            if (downloadSpeed >= 50 && uploadSpeed >= 10)
            {
                return "Votre connexion est excellente, profitez d'une expérience en ligne fluide et rapide ! 😃";
            }
            else if (downloadSpeed >= 20 && uploadSpeed >= 5)
            {
                return "Votre connexion est bonne, vous devriez pouvoir utiliser Internet sans trop de problèmes. 🙂";
            }
            else if (downloadSpeed >= 10 && uploadSpeed >= 2)
            {
                return "Votre connexion est moyenne, certains services en ligne pourraient être un peu lents. 😐";
            }
            else
            {
                return "Votre connexion est faible, vous pourriez rencontrer des difficultés avec certains services en ligne. ☹️";
            }
        }


        private void InitializeUserInfo()
        {
            lblUserName.Text = "Name : " + Environment.UserName;
        }

        private void InitializeNetworkInfo()
        {
            string ipAddress = GetLocalIPAddress();
            lblIpAddress.Text += string.IsNullOrEmpty(ipAddress) ? "Non disponible" : ipAddress;
            IPClientTxtBox.Text += string.IsNullOrEmpty(ipAddress) ? "Non disponible" : ipAddress;

            int pingValue = GetPingValue("8.8.8.8"); // Exemple d'adresse IP à pinger (serveur Google DNS)
            lblPingStatus.Text += $"{pingValue} ms";
        }

        private void InitializeSystemInfo()
        {
            lblOsVersion.Text = "OS : " + Environment.OSVersion.ToString();
        }

        private string GetLocalIPAddress()
        {
            string ipAddress = "Non disponible";

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var ipAddressInfo in networkInterface.GetIPProperties().UnicastAddresses)
                {
                    if (ipAddressInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = ipAddressInfo.Address.ToString();
                        break;
                    }
                }

                if (ipAddress != "Non disponible")
                {
                    break;
                }
            }

            return ipAddress;
        }

        private int GetPingValue(string ipAddress)
        {
            int pingValue = -1; // Valeur par défaut en cas d'erreur

            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = ping.Send(ipAddress);
                    if (reply.Status == IPStatus.Success)
                    {
                        pingValue = (int)reply.RoundtripTime;
                    }
                }
                catch (PingException)
                {
                    // Gérer l'exception en cas d'erreur de ping
                }
            }

            return pingValue;
        }

        private void siticoneButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (siticoneButton4.Checked == true)
            {
                EditListPanel.Visible = false;
                DashBoardPanel.Visible = false;
                MyPagePanel.Visible = false;
                VPNpanel.Visible = false;
                panelShare.Visible = false;
                panelSearchUser.Visible = false;
                PanelPartageMdpFinal.Visible = false;
                PanelViewerInfosSelection.Visible = false;
                PanelShareViewer.Visible = false;
                SettingsPanel.Visible = true;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            string discordInviteURL = "https://discord.gg/GxEjCuGPXj";
            Process.Start("cmd.exe", "/c start " + discordInviteURL);
        }

        // Déclarez la variable importedSites au niveau de la classe Form2
        private List<ImportedSite> importedSites;
        private async Task AddSitesToFirebaseAsync()
        {
            // Récupérez l'utilisateur actuel de la base de données Firebase
            var utilisateur = (await firebaseClient
                .Child("utilisateurs")
                .OnceAsync<Utilisateur>())
                .FirstOrDefault(u => u.Object.Id == userId);

            if (utilisateur != null)
            {
                foreach (var importedSite in importedSites)
                {
                    string siteName = importedSite.Name;
                    string username = importedSite.Username;
                    string password = importedSite.Password;

                    // Créez un nom pour le Credential basé sur siteName
                    string credentialName = $"{siteName}";

                    // Recherchez le Credential correspondant par nom, s'il existe
                    Credential existingCredential = null;

                    foreach (var credential in utilisateur.Object.Credentials)
                    {
                        if (credential.CredentialName == credentialName)
                        {
                            existingCredential = credential;
                            break;
                        }
                    }

                    if (existingCredential == null)
                    {
                        // Si le Credential n'existe pas, créez-le
                        existingCredential = new Credential
                        {
                            CredentialName = credentialName
                        };
                        utilisateur.Object.Credentials.Add(existingCredential);
                    }
                    else
                    {
                        // Si le Credential existe déjà, vérifiez si les informations sont identiques
                        bool siteExists = existingCredential.SiteInfos.Any(site => site.SiteName == siteName && site.Username == username && site.Password == password);

                        if (siteExists)
                        {
                            // Si les informations sont identiques, passez au site suivant
                            continue;
                        }
                    }

                    // Ajoutez le nouveau site au Credential
                    SiteInfo newSite = new SiteInfo
                    {
                        SiteName = siteName,
                        Username = username,
                        Password = password
                    };

                    existingCredential.SiteInfos.Add(newSite);
                }

                // Mettez à jour l'utilisateur dans la base de données Firebase
                await firebaseClient
                    .Child("utilisateurs")
                    .Child(utilisateur.Key)
                    .PutAsync(utilisateur.Object);
            }
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers CSV|*.csv";
            openFileDialog.Title = "Sélectionnez le fichier CSV à importer";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var reader = new StreamReader(openFileDialog.FileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    importedSites = csv.GetRecords<ImportedSite>().ToList(); // Utilisez la variable de classe

                    foreach (var importedSite in importedSites)
                    {
                        SiteInfo newSite = new SiteInfo
                        {
                            SiteName = importedSite.Name,
                            Username = importedSite.Username,
                            Password = importedSite.Password
                        };
                        siteList.Add(newSite);
                    }

                    SaveListToFile(userId);
                    MessageBox.Show("Importation terminée !");
                    RefreshSiteList();
                    AddSitesToFirebaseAsync();
                }
            }
        }

        private void RefreshSiteList()
        {
            listSites.Items.Clear();

            foreach (SiteInfo siteInfo in siteList)
            {
                SiteListBoxItem listItem = new SiteListBoxItem
                {
                    SiteName = siteInfo.SiteName,
                    SiteInfo = siteInfo
                };
                string listItemText = $"Site: {siteInfo.SiteName} User: {siteInfo.Username} Password: {siteInfo.Password}";
                listSites.Items.Add(listItemText);
            }
        }
        public class ImportedSite
        {
            [Name("name")]
            public string Name { get; set; }

            [Name("url")]
            public string Url { get; set; }

            [Name("username")]
            public string Username { get; set; }

            [Name("password")]
            public string Password { get; set; }

            [Name("note")]
            public string Note { get; set; }
        }

        private bool IsVpnConnectionExists(string vpnName)
        {
            RasPhoneBook rasPhoneBook = new RasPhoneBook();
            string phoneBookPath = RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User);
            rasPhoneBook.Open(phoneBookPath);

            return rasPhoneBook.Entries.Any(e => e.Name == vpnName);
        }

        private async void EnsureVpnConnection()
        {
            string vpnName = "vpnbook";

            if (!IsVpnConnectionExists(vpnName))
            {
                // Créez la connexion VPN en arrière-plan
                await Task.Run(() => CreateVpnConnection(vpnName));
            }
        }

        private void CreateVpnConnection(string vpnName)
        {
            try
            {
                // Personnalisez les paramètres de la connexion VPN IKEv2
                string vpnServer = "FR200.vpnbook.com";

                // Créez la commande PowerShell pour ajouter la connexion VPN IKEv2
                string command = $"Add-VpnConnection -Name \"{vpnName}\" -ServerAddress \"{vpnServer}\" -TunnelType Automatic -AuthenticationMethod MSChapv2 -EncryptionLevel Optional -RememberCredential -PassThru";

                // Exécutez la commande PowerShell pour ajouter la connexion VPN IKEv2
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy unrestricted -Command \"{command}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process()
                {
                    StartInfo = psi
                };

                process.Start();
                process.WaitForExit();

                // Vérifiez le code de sortie pour détecter les erreurs lors de l'ajout de la connexion VPN
                int exitCode = process.ExitCode;
                process.Close();

                if (exitCode != 0)
                {
                    MessageBox.Show("Erreur lors de la création de la connexion VPN IKEv2.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la création de la connexion VPN IKEv2 : " + ex.Message);
            }
        }

        private void VPNButton_CheckedChanged(object sender, EventArgs e)
        {
            if (VPNButton.Checked == true)
            {
                EditListPanel.Visible = false;
                DashBoardPanel.Visible = false;
                MyPagePanel.Visible = false;
                SettingsPanel.Visible = false;
                panelShare.Visible = false;
                panelSearchUser.Visible = false;
                PanelPartageMdpFinal.Visible = false;
                PanelShareViewer.Visible = false;
                PanelViewerInfosSelection.Visible = false;
                VPNpanel.Visible = true;
            }
        }

        string vpnServer = "FR200.vpnbook.com"; // Serveur VPN de votre choix (ex: us2.vpnbook.com)
        private void ConnectionVpn()
        {
            // Informations de connexion VPNBook
            string vpnUsername = "vpnbook";
            string vpnPassword = "w8m9tb7";

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "rasdial.exe";
                process.StartInfo.Arguments = $"\"VPNBook\" {vpnUsername} {vpnPassword} /phone:{vpnServer}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion au VPN : " + ex.Message);
            }
        }

        private void DeconnectionVpn()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "rasdial.exe";
                process.StartInfo.Arguments = "\"VPNBook\" /disconnect";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la déconnexion du VPN : " + ex.Message);
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            gifloading.Visible = true;
            guna2CircleButton2.Visible = false;
            guna2CircleButton1.Visible = false;

            // Démarrer la tâche de connexion VPN dans un Thread séparé
            Thread vpnThread = new Thread(() =>
            {
                ConnectionVpn();

                // À la fin de la connexion VPN, masquez le GIF de chargement et effectuez d'autres actions si nécessaires
                this.Invoke((MethodInvoker)delegate
                {
                    guna2CirclePictureBox1.FillColor = Color.Lime;
                    guna2CirclePictureBox2.FillColor = Color.Lime;
                    guna2CirclePictureBox3.FillColor = Color.Lime;
                    guna2CirclePictureBox4.FillColor = Color.Lime;
                    guna2CirclePictureBox5.FillColor = Color.Lime;

                    if (vpnServer == "US2.vpnbook.com")
                    {
                        guna2CirclePictureBox1.Visible = true;
                        pictureBox8.Enabled = false;
                        pictureBox10.Enabled = false;
                        pictureBox11.Enabled = false;
                        pictureBox13.Enabled = false;
                    }
                    else if (vpnServer == "FR200.vpnbook.com")
                    {
                        guna2CirclePictureBox2.Visible = true;
                        pictureBox9.Enabled = false;
                        pictureBox10.Enabled = false;
                        pictureBox11.Enabled = false;
                        pictureBox13.Enabled = false;
                    }
                    else if (vpnServer == "CA196.vpnbook.com")
                    {
                        guna2CirclePictureBox3.Visible = true;
                        pictureBox9.Enabled = false;
                        pictureBox10.Enabled = false;
                        pictureBox8.Enabled = false;
                        pictureBox13.Enabled = false;
                    }
                    else if (vpnServer == "DE20.vpnbook.com")
                    {
                        guna2CirclePictureBox4.Visible = true;
                        pictureBox9.Enabled = false;
                        pictureBox11.Enabled = false;
                        pictureBox8.Enabled = false;
                        pictureBox13.Enabled = false;
                    }

                    else if (vpnServer == "UK205.vpnbook.com")
                    {
                        guna2CirclePictureBox5.Visible = true;
                        pictureBox9.Enabled = false;
                        pictureBox11.Enabled = false;
                        pictureBox10.Enabled = false;
                        pictureBox8.Enabled = false;
                    }

                    // affiche l'adresse du vpn
                    GetVpnIpAddress();

                    // enleve le gif
                    gifloading.Visible = false;

                    // switch les couleurss des boutons
                    guna2CircleButton1.Visible = false;
                    guna2CircleButton2.Visible = true;
                });
            });
            vpnThread.Start(); // Démarrez le Thread
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            gifloading.Visible = true;
            guna2CircleButton2.Visible = false;
            guna2CircleButton1.Visible = false;


            // Démarrer la tâche de déconnexion VPN dans un Thread séparé
            Thread vpnThread = new Thread(() =>
            {
                // deconnection vpn
                DeconnectionVpn();

                // À la fin de la déconnexion VPN, masquez le GIF de chargement et effectuez d'autres actions si nécessaires
                this.Invoke((MethodInvoker)delegate
                {
                    // enleve les points sur les cartes
                    if (vpnServer == "US2.vpnbook.com")
                    {
                        guna2CirclePictureBox1.Visible = true;
                        guna2CirclePictureBox1.FillColor = Color.Red;
                        pictureBox8.Enabled = true;
                        pictureBox10.Enabled = true;
                        pictureBox11.Enabled = true;
                        pictureBox13.Enabled = true;
                    }
                    else if (vpnServer == "FR200.vpnbook.com")
                    {
                        guna2CirclePictureBox2.Visible = true;
                        guna2CirclePictureBox2.FillColor = Color.Red;
                        pictureBox9.Enabled = true;
                        pictureBox10.Enabled = true;
                        pictureBox11.Enabled = true;
                        pictureBox13.Enabled = true;
                    }
                    else if (vpnServer == "CA196.vpnbook.com")
                    {
                        guna2CirclePictureBox3.Visible = true;
                        guna2CirclePictureBox3.FillColor = Color.Red;
                        pictureBox9.Enabled = true;
                        pictureBox10.Enabled = true;
                        pictureBox8.Enabled = true;
                        pictureBox13.Enabled = true;
                    }
                    else if (vpnServer == "DE20.vpnbook.com")
                    {
                        guna2CirclePictureBox4.Visible = true;
                        guna2CirclePictureBox4.FillColor = Color.Red;
                        pictureBox9.Enabled = true;
                        pictureBox11.Enabled = true;
                        pictureBox8.Enabled = true;
                        pictureBox13.Enabled = true;
                    }

                    else if (vpnServer == "UK205.vpnbook.com")
                    {
                        guna2CirclePictureBox5.Visible = true;
                        guna2CirclePictureBox5.FillColor = Color.Red;
                        pictureBox9.Enabled = true;
                        pictureBox11.Enabled = true;
                        pictureBox8.Enabled = true;
                    }

                    // enleve l'adresse du vpn
                    IPVpnTxtBox.Text = "";

                    // enleve le gif
                    gifloading.Visible = false;

                    // change de couleur le bouton (switch entre les deux)
                    guna2CircleButton2.Visible = false;
                    guna2CircleButton1.Visible = true;
                });
            });
            vpnThread.Start(); // Démarrez le Thread
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            vpnServer = "FR200.vpnbook.com";
            guna2Panel1.Visible = true;
            // enleve la surbrillance des autres cartes
            guna2Panel2.Visible = false;
            guna2Panel4.Visible = false;
            guna2Panel3.Visible = false;
            guna2Panel5.Visible = false;
            guna2CirclePictureBox3.Visible = false;
            guna2CirclePictureBox4.Visible = false;
            guna2CirclePictureBox1.Visible = false;
            guna2CirclePictureBox5.Visible = false;

            // met le point selectionner sur la carte rouge désactiver
            guna2CirclePictureBox2.FillColor = Color.Red;
            guna2CirclePictureBox2.Visible = true;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            vpnServer = "US2.vpnbook.com";
            guna2Panel2.Visible = true;

            // enleve la surbrillance des autres cartes
            guna2Panel1.Visible = false;
            guna2Panel4.Visible = false;
            guna2Panel3.Visible = false;
            guna2Panel5.Visible = false;
            guna2CirclePictureBox3.Visible = false;
            guna2CirclePictureBox2.Visible = false;
            guna2CirclePictureBox4.Visible = false;
            guna2CirclePictureBox5.Visible = false;

            // met le point selectionner sur la carte rouge désactiver
            guna2CirclePictureBox1.FillColor = Color.Red;
            guna2CirclePictureBox1.Visible = true;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            vpnServer = "DE20.vpnbook.com";
            guna2Panel3.Visible = true;
            // enleve la surbrillance des autres cartes
            guna2Panel1.Visible = false;
            guna2Panel2.Visible = false;
            guna2Panel4.Visible = false;
            guna2Panel5.Visible = false;
            guna2CirclePictureBox3.Visible = false;
            guna2CirclePictureBox2.Visible = false;
            guna2CirclePictureBox1.Visible = false;
            guna2CirclePictureBox5.Visible = false;

            // met le point selectionner sur la carte rouge désactiver
            guna2CirclePictureBox4.FillColor = Color.Red;
            guna2CirclePictureBox4.Visible = true;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            vpnServer = "CA196.vpnbook.com";
            guna2Panel4.Visible = true;

            // enleve la surbrillance des autres cartes
            guna2Panel1.Visible = false;
            guna2Panel2.Visible = false;
            guna2Panel3.Visible = false;
            guna2Panel5.Visible = false;
            guna2CirclePictureBox4.Visible = false;
            guna2CirclePictureBox2.Visible = false;
            guna2CirclePictureBox1.Visible = false;
            guna2CirclePictureBox5.Visible = false;

            // met le point selectionner sur la carte rouge désactiver
            guna2CirclePictureBox3.FillColor = Color.Red;
            guna2CirclePictureBox3.Visible = true;
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            vpnServer = "UK205.vpnbook.com";
            guna2Panel5.Visible = true;

            // enleve la surbrillance des autres cartes
            guna2Panel1.Visible = false;
            guna2Panel2.Visible = false;
            guna2Panel3.Visible = false;
            guna2Panel4.Visible = false;
            guna2CirclePictureBox4.Visible = false;
            guna2CirclePictureBox2.Visible = false;
            guna2CirclePictureBox1.Visible = false;
            guna2CirclePictureBox3.Visible = false;

            // met le point selectionner sur la carte rouge désactiver
            guna2CirclePictureBox5.FillColor = Color.Red;
            guna2CirclePictureBox5.Visible = true;
        }

        private void GetVpnIpAddress()
        {
            try
            {
                // Créez une requête HTTP GET vers httpbin.org
                WebRequest request = WebRequest.Create("https://httpbin.org/ip");

                // Obtenez la réponse à la requête
                WebResponse response = request.GetResponse();

                // Lisez les données de la réponse
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();

                    // Analysez la réponse JSON pour obtenir l'adresse IP
                    dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseFromServer);
                    string vpnIpAddress = jsonResponse.origin;

                    // Affichez l'adresse IP du VPN dans la TextBox
                    IPVpnTxtBox.Text = vpnIpAddress;
                }

                // Fermez la réponse
                response.Close();
            }
            catch (Exception ex)
            {
                // Gérez les erreurs ici
                MessageBox.Show("Erreur lors de la récupération de l'adresse IP du VPN : " + ex.Message);
            }
        }

        // Page Share
        private void panel12_MouseDown(object sender, MouseEventArgs e)
        {
            // Effet de pression lorsque l'utilisateur clique
            pictureBox14.Size = new Size(178, 174); // Ajustez selon vos préférences
            pictureBox14.Location = new Point(10, 10); // Ajustez selon vos préférences
        }

        private void panel12_MouseUp(object sender, MouseEventArgs e)
        {
            // Bouton remonte lorsque l'utilisateur relâche le clic
            pictureBox14.Size = new Size(188, 184); // Ajustez selon vos préférences
            pictureBox14.Location = new Point(0, 0); // Ajustez selon vos préférences

            EditListPanel.Visible = false;
            DashBoardPanel.Visible = false;
            MyPagePanel.Visible = false;
            SettingsPanel.Visible = false;
            VPNpanel.Visible = false;
            PanelPartageMdpFinal.Visible = false;
            panelShare.Visible = false;
            PanelShareViewer.Visible = false;
            PanelViewerInfosSelection.Visible = false;
            panelSearchUser.Visible = true;
        }

        private void panel13_MouseDown(object sender, MouseEventArgs e)
        {
            // Effet de pression lorsque l'utilisateur clique
            pictureBox15.Size = new Size(178, 174); // Ajustez selon vos préférences
            pictureBox15.Location = new Point(10, 10); // Ajustez selon vos préférences
        }

        private void panel13_MouseUp(object sender, MouseEventArgs e)
        {
            // Bouton remonte lorsque l'utilisateur relâche le clic
            pictureBox15.Size = new Size(188, 184); // Ajustez selon vos préférences
            pictureBox15.Location = new Point(0, 0); // Ajustez selon vos préférences

            EditListPanel.Visible = false;
            DashBoardPanel.Visible = false;
            MyPagePanel.Visible = false;
            SettingsPanel.Visible = false;
            VPNpanel.Visible = false;
            PanelPartageMdpFinal.Visible = false;
            panelShare.Visible = false;
            panelSearchUser.Visible = false;
            PanelViewerInfosSelection.Visible = false;
            PanelShareViewer.Visible = true;

            LoadSharedUsers();
        }

        // Page Share recherche Page user
        private void txtSearchUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearch();
            }
        }

        private async void PerformSearch()
        {
            // Effacez les résultats précédents
            flowLayoutPanelResults.Controls.Clear();

            // Récupérez la chaîne de recherche
            string searchTerm = txtSearchUser.Text;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("Veuillez entrer un terme de recherche.");
                return;
            }

            try
            {
                // Recherchez tous les utilisateurs dans la base de données
                var allUtilisateurs = await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>();

                // Filtrez les résultats en utilisant LINQ pour une comparaison insensible à la casse
                var filteredUtilisateurs = allUtilisateurs
                    .Select(u => u.Object)
                    .Where(u => u.nom_utilisateur.ToLower().Contains(searchTerm.ToLower()));

                foreach (var utilisateur in filteredUtilisateurs)
                {
                    // Créez un Panel pour chaque utilisateur
                    Panel panel = new Panel();
                    panel.Size = new Size(140, 204);
                    panel.Cursor = Cursors.Hand;

                    // Ajoutez une PictureBox pour l'image de l'utilisateur
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Size = new Size(100, 140);
                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox.Image = ImageUser; // Utilisez l'image chargée au démarrage
                    pictureBox.Dock = DockStyle.Top;
                    pictureBox.Enabled = false;

                    // Ajoutez une étiquette pour le nom de l'utilisateur
                    Label label = new Label();
                    label.Text = ShortenUsername(utilisateur.nom_utilisateur);
                    label.Dock = DockStyle.Bottom;
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.ForeColor = Color.White; // Définissez la couleur du texte en blanc

                    // Ajoutez un label "(amis)" si l'utilisateur est déjà un ami
                    Label amisLabel = new Label();
                    amisLabel.Text = "(amis)";
                    amisLabel.Dock = DockStyle.Bottom;
                    amisLabel.TextAlign = ContentAlignment.MiddleCenter;
                    amisLabel.ForeColor = Color.FromArgb(57, 115, 224); // Couleur du texte pour les amis

                    // Vérifie si l'utilisateur est déjà un ami
                    bool isFriend = await IsUserFriend(utilisateur.Id);

                    // Ajoutez un gestionnaire d'événements pour le clic sur le Panel
                    panel.Click += (sender, e) => Panel_Click(sender, e, utilisateur.Id);

                    // Créez et ajoutez le bouton d'ajout d'amis uniquement si l'utilisateur n'est pas déjà un ami
                    if (!isFriend)
                    {
                        Guna.UI2.WinForms.Guna2Button addButton = CreateAddFriendButton(utilisateur);
                        addButton.Size = new Size(panel.Width - 10, 30); // Ajustez la largeur du bouton
                        addButton.Dock = DockStyle.Bottom;
                        panel.Size = new Size(140, 210);

                        // Ajoutez les contrôles au Panel
                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(label);
                        panel.Controls.Add(addButton);
                    }
                    else
                    {
                        // Si l'utilisateur est déjà un ami, ajoutez la PictureBox et le label sans le bouton
                        panel.Controls.Add(pictureBox);
                        panel.Controls.Add(label);
                        panel.Controls.Add(amisLabel);
                    }

                    // Ajoutez le Panel au FlowLayoutPanel
                    flowLayoutPanelResults.Controls.Add(panel);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private void Panel_Click(object sender, EventArgs e, string targetUserId)
        {
            // Mettez à jour la variable globale avec l'ID de la personne sélectionnée
            selectedUserId = targetUserId;

            // Après avoir initialisé SiteListPartage
            LoadListIntoSiteListPartage(userId);

            // Passer au nouveau Panel
            EditListPanel.Visible = false;
            DashBoardPanel.Visible = false;
            MyPagePanel.Visible = false;
            SettingsPanel.Visible = false;
            VPNpanel.Visible = false;
            panelShare.Visible = false;
            panelSearchUser.Visible = false;
            PanelShareViewer.Visible = false;
            PanelViewerInfosSelection.Visible = false;
            PanelPartageMdpFinal.Visible = true;
        }

        private async Task<bool> IsUserFriend(string friendId)
        {
            try
            {
                // Récupérez l'ID de l'utilisateur actuel
                string currentUserId = userId;

                // Récupérez l'utilisateur actuel (celui qui veut ajouter un ami)
                var currentUser = await firebaseClient
                    .Child("utilisateurs")
                    .OrderBy("Id")
                    .EqualTo(currentUserId)
                    .OnceAsync<Utilisateur>();

                if (currentUser.Any())
                {
                    var currentUserObject = currentUser.First().Object;

                    // Vérifiez si l'ami est déjà dans la liste d'amis
                    return currentUserObject.Friends != null && currentUserObject.Friends.Contains(friendId);
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
                return false;
            }
        }

        private Guna.UI2.WinForms.Guna2Button CreateAddFriendButton(Utilisateur utilisateur)
        {
            Guna.UI2.WinForms.Guna2Button addButton = new Guna.UI2.WinForms.Guna2Button();
            addButton.Size = new Size(30, 30);
            addButton.Image = ImageAddUser; // Utilisez l'image d'ajout d'amis
            addButton.Cursor = Cursors.Hand;

            // Style pour des coins arrondis
            addButton.BorderRadius = 5;

            // Associez le gestionnaire d'événements au clic du bouton d'ajout d'amis
            addButton.Click += (sender, e) => AddFriendButton_Click(sender, e, utilisateur);

            return addButton;
        }

        private Image ResizeImage(Image originalImage, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(originalImage, 0, 0, width, height);
            }
            return resizedImage;
        }

        private async void AddFriendButton_Click(object sender, EventArgs e, Utilisateur utilisateur)
        {
            Guna.UI2.WinForms.Guna2Button addButton = (Guna.UI2.WinForms.Guna2Button)sender;

            try
            {
                // Récupérez l'ID de l'utilisateur actuel
                string currentUserId = userId;

                // Récupérez l'utilisateur actuel (celui qui veut ajouter un ami)
                var currentUser = await firebaseClient
                    .Child("utilisateurs")
                    .OrderBy("Id")
                    .EqualTo(currentUserId)
                    .OnceAsync<Utilisateur>();

                if (currentUser.Any())
                {
                    var currentUserObject = currentUser.First().Object;

                    // Vérifiez si l'ami n'est pas déjà dans la liste d'amis
                    if (currentUserObject.Friends == null || !currentUserObject.Friends.Contains(utilisateur.Id))
                    {
                        // Ajoutez l'ami à la liste d'amis
                        currentUserObject.Friends ??= new List<string>();
                        currentUserObject.Friends.Add(utilisateur.Id);

                        // Mettez à jour l'utilisateur actuel dans la base de données
                        await firebaseClient
                            .Child("utilisateurs")
                            .Child(currentUser.First().Key)
                            .Child("Friends")
                            .PutAsync(currentUserObject.Friends);

                        // Ajustez la taille du panneau après avoir cliqué sur le bouton
                        Panel panel = (Panel)addButton.Parent;
                        panel.Size = new Size(140, 204);

                        // Cachez le bouton d'ajout d'amis
                        addButton.Visible = false;

                        // Ajoutez un label "(amis)" si l'utilisateur est déjà un ami
                        Label amisLabel = new Label();
                        amisLabel.Text = "(amis)";
                        amisLabel.Dock = DockStyle.Bottom;
                        amisLabel.TextAlign = ContentAlignment.MiddleCenter;
                        amisLabel.ForeColor = Color.FromArgb(57, 115, 224); // Couleur du texte pour les amis

                        panel.Controls.Add(amisLabel);

                        MessageBox.Show($"Vous avez ajouté {utilisateur.nom_utilisateur} à votre liste d'amis !");
                    }
                    else
                    {
                        MessageBox.Show($"{utilisateur.nom_utilisateur} est déjà dans votre liste d'amis.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private async void LoadFriends()
        {
            // Si le ToggleSwitch est désactivé, ne fais rien
            if (!ToggleSwitchOnlyFriends.Checked)
            {
                // Effacez les résultats précédents uniquement si le ToggleSwitch est activé
                flowLayoutPanelResults.Controls.Clear();
                return;
            }

            // Effacez les résultats précédents uniquement si le ToggleSwitch est activé
            flowLayoutPanelResults.Controls.Clear();

            try
            {
                // Récupérez l'ID de l'utilisateur actuel
                string currentUserId = userId;

                // Récupérez l'utilisateur actuel
                var currentUser = await firebaseClient
                    .Child("utilisateurs")
                    .OrderBy("Id")
                    .EqualTo(currentUserId)
                    .OnceAsync<Utilisateur>();

                if (currentUser.Any())
                {
                    var currentUserObject = currentUser.First().Object;

                    // Si l'utilisateur a des amis, récupère les utilisateurs correspondants
                    if (currentUserObject.Friends != null && currentUserObject.Friends.Any())
                    {
                        var friendIds = currentUserObject.Friends;

                        // Récupère les utilisateurs dont l'ID est dans la liste d'amis
                        var friends = await firebaseClient
                            .Child("utilisateurs")
                            .OrderBy("nom_utilisateur")
                            .OnceAsync<Utilisateur>();

                        foreach (var friend in friends)
                        {
                            if (friendIds.Contains(friend.Object.Id))
                            {
                                // Créez un Panel pour chaque ami
                                Panel panel = new Panel();
                                panel.Size = new Size(140, 204);
                                panel.Cursor = Cursors.Hand;

                                // Ajoutez une PictureBox pour l'image de l'ami
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Size = new Size(100, 140);
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                // Utilisez l'image chargée au démarrage, ou remplacez par l'image de l'ami si tu en as une
                                pictureBox.Image = ImageUser;
                                pictureBox.Dock = DockStyle.Top;
                                pictureBox.Enabled = false;

                                // Ajoutez une étiquette pour le nom de l'ami
                                Label label = new Label();
                                label.Text = ShortenUsername(friend.Object.nom_utilisateur);
                                label.Dock = DockStyle.Bottom;
                                label.TextAlign = ContentAlignment.MiddleCenter;
                                label.ForeColor = Color.White;

                                // Ajoutez un label "(amis)" si l'utilisateur est déjà un ami
                                Label amisLabel = new Label();
                                amisLabel.Text = "(amis)";
                                amisLabel.Dock = DockStyle.Bottom;
                                amisLabel.TextAlign = ContentAlignment.MiddleCenter;
                                amisLabel.ForeColor = Color.FromArgb(57, 115, 224); // Couleur du texte pour les amis

                                // Ajoutez un gestionnaire d'événements pour le clic sur le Panel
                                panel.Click += (sender, e) => Panel_Click(sender, e, friend.Object.Id);

                                // Ajoutez les contrôles au Panel
                                panel.Controls.Add(pictureBox);
                                panel.Controls.Add(label);
                                panel.Controls.Add(amisLabel);

                                // Ajoutez le Panel au FlowLayoutPanel
                                flowLayoutPanelResults.Controls.Add(panel);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private string ShortenUsername(string username, int maxLength = 15)
        {
            if (username.Length > maxLength)
            {
                // Si le pseudo dépasse la longueur maximale, le raccourcir et ajouter trois petits points
                return username.Substring(0, maxLength - 3) + "...";
            }

            return username;
        }

        private void ToggleSwitchOnlyFriends_CheckedChanged(object sender, EventArgs e)
        {
            LoadFriends();
        }

        private void LoadListIntoSiteListPartage(string userId)
        {
            string fileName = $"{userId}_siteList.json"; // Ajoutez l'ID de l'utilisateur au nom du fichier

            if (File.Exists(fileName))
            {
                string encryptedJson = File.ReadAllText(fileName);

                string decryptedJson = DecryptSimple(encryptedJson, encryptionShift);

                List<SiteInfo> siteList = JsonConvert.DeserializeObject<List<SiteInfo>>(decryptedJson);

                SiteListPartage.Items.Clear(); // Clear the ListBox first

                foreach (SiteInfo site in siteList)
                {
                    string listItemText = $"Site: {site.SiteName} User: {site.Username} Password: {site.Password}";
                    SiteListPartage.Items.Add(listItemText);
                }
            }
            else
            {
                SiteListPartage.Items.Clear(); // Clear the ListBox if the file doesn't exist
            }
        }

        private void SiteListPartage_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtenez la référence à la liste d'origine
            ListBox sourceListBox = (ListBox)sender;

            // Obtenez la référence à la liste de destination
            ListBox destinationListBox = listBoxAddShareInfos;

            // Copiez les éléments sélectionnés de la liste source à la liste de destination
            foreach (var selectedItem in sourceListBox.SelectedItems)
            {
                destinationListBox.Items.Add(selectedItem);
            }

            // Rafraîchissez la liste de destination pour afficher les nouveaux éléments
            destinationListBox.Refresh();
        }

        private void listBoxAddShareInfos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Obtenez la référence à la liste de destination
            ListBox destinationListBox = listBoxAddShareInfos;

            // Supprimez l'élément sélectionné de la liste de destination
            if (destinationListBox.SelectedItem != null)
            {
                destinationListBox.Items.Remove(destinationListBox.SelectedItem);
            }

            // Rafraîchissez la liste de destination pour afficher les modifications
            destinationListBox.Refresh();
        }

        private async Task SaveListToFirebaseAsync(List<string> shareInfos, string senderId)
        {
            try
            {
                // Recherchez l'utilisateur cible dans la base de données Firebase
                var targetUser = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.Id == selectedUserId);

                if (targetUser != null)
                {
                    // Initialisez la catégorie ShareInfoUser s'il n'en existe pas déjà une
                    targetUser.Object.ShareInfoUser ??= new Dictionary<string, List<string>>();

                    // Créez une sous-catégorie avec l'ID de l'expéditeur
                    string senderCategory = $"SenderId_{senderId}";

                    // Ajoutez les informations partagées à la sous-catégorie
                    if (!targetUser.Object.ShareInfoUser.ContainsKey(senderCategory))
                    {
                        targetUser.Object.ShareInfoUser[senderCategory] = new List<string>();
                    }

                    targetUser.Object.ShareInfoUser[senderCategory].AddRange(shareInfos);

                    // Mettez à jour l'utilisateur cible dans la base de données Firebase
                    await firebaseClient
                        .Child("utilisateurs")
                        .Child(targetUser.Key)
                        .PutAsync(targetUser.Object);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de l'enregistrement dans Firebase : {ex.Message}");
                // Gérer l'erreur selon vos besoins
            }
        }


        private void BtnShareInfos_Click(object sender, EventArgs e)
        {
            // Obtenez les informations de partage à partir de la listBoxAddShareInfos
            List<string> shareInfos = new List<string>();

            foreach (var item in listBoxAddShareInfos.Items)
            {
                shareInfos.Add(item.ToString());
            }

            // Appelez la fonction pour enregistrer les informations dans la base de données Firebase
            SaveListToFirebaseAsync(listBoxAddShareInfos.Items.Cast<string>().ToList(), userId);

            // Passer au nouveau Panel
            EditListPanel.Visible = false;
            DashBoardPanel.Visible = false;
            MyPagePanel.Visible = false;
            SettingsPanel.Visible = false;
            VPNpanel.Visible = false;
            panelSearchUser.Visible = false;
            PanelShareViewer.Visible = false;
            PanelPartageMdpFinal.Visible = false;
            PanelViewerInfosSelection.Visible = false;
            panelShare.Visible = true;
        }

        // page affichage personne ayant partager des infos
        private async void LoadSharedUsers()
        {
            // Effacez les résultats précédents
            flowLayoutPanelAffichageViewer.Controls.Clear();

            try
            {
                // Récupérez l'ID de l'utilisateur actuel
                string currentUserId = userId;

                // Récupérez l'utilisateur actuel
                var currentUser = await firebaseClient
                    .Child("utilisateurs")
                    .OrderBy("Id")
                    .EqualTo(currentUserId)
                    .OnceAsync<Utilisateur>();

                if (currentUser.Any())
                {
                    var currentUserObject = currentUser.First().Object;

                    // Si l'utilisateur a partagé des informations, récupérez les utilisateurs correspondants
                    if (currentUserObject.ShareInfoUser != null && currentUserObject.ShareInfoUser.Any())
                    {
                        foreach (var shareInfoCategory in currentUserObject.ShareInfoUser)
                        {
                            // Extract l'ID de l'expéditeur de la catégorie
                            string senderId = shareInfoCategory.Key.Replace("SenderId_", "");

                            // Récupérez l'utilisateur correspondant à l'expéditeur
                            var senderUser = await firebaseClient
                                .Child("utilisateurs")
                                .OrderBy("Id")
                                .EqualTo(senderId)
                                .OnceAsync<Utilisateur>();

                            if (senderUser.Any())
                            {
                                var senderUserObject = senderUser.First().Object;

                                // Créez un Panel pour chaque utilisateur partageant des informations
                                Panel panel = new Panel();
                                panel.Size = new Size(140, 204);
                                panel.Cursor = Cursors.Hand;

                                // Ajoutez une PictureBox pour l'image de l'utilisateur (utilisez l'image chargée au démarrage, ou remplacez par l'image de l'ami si tu en as une)
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Size = new Size(100, 140);
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox.Image = ImageUser;
                                pictureBox.Dock = DockStyle.Top;
                                pictureBox.Enabled = false;

                                // Ajoutez une étiquette pour le nom de l'ami
                                Label label = new Label();
                                label.Text = senderUserObject.nom_utilisateur;
                                label.Dock = DockStyle.Bottom;
                                label.TextAlign = ContentAlignment.MiddleCenter;
                                label.ForeColor = Color.White;

                                // Ajoutez un label "(partagé)" pour indiquer que ces informations ont été partagées par cet utilisateur
                                Label partageLabel = new Label();
                                partageLabel.Text = "(partagé)";
                                partageLabel.Dock = DockStyle.Bottom;
                                partageLabel.TextAlign = ContentAlignment.MiddleCenter;
                                partageLabel.ForeColor = Color.FromArgb(255, 69, 0); // Couleur du texte pour les informations partagées

                                // Ajoutez un nouvel écouteur d'événements pour le clic sur le Panel
                                panel.Click += (sender, e) => SharedUserPanel_Click(sender, e, senderUserObject.Id);

                                // Ajoutez les contrôles au Panel
                                panel.Controls.Add(pictureBox);
                                panel.Controls.Add(label);
                                panel.Controls.Add(partageLabel);

                                // Ajoutez le Panel au FlowLayoutPanel
                                flowLayoutPanelAffichageViewer.Controls.Add(panel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        // Nouvelle fonction d'écouteur d'événements pour les utilisateurs partageant des informations
        private async void SharedUserPanel_Click(object sender, EventArgs e, string sharedUserId)
        {
            try
            {
                // Mettre à jour la variable selectedSharedUserId
                selectedSharedUserId = sharedUserId;

                listBoxViewerShareInfos.Items.Clear();

                // Récupérer les informations partagées par l'utilisateur connecté
                var currentUserResponse = await firebaseClient
                    .Child("utilisateurs")
                    .OrderBy("Id")
                    .EqualTo(userId)
                    .OnceAsync<Utilisateur>();

                if (currentUserResponse.Any())
                {
                    var currentUserObject = currentUserResponse.First().Object;

                    if (currentUserObject != null && currentUserObject.ShareInfoUser != null && currentUserObject.ShareInfoUser.Any())
                    {
                        foreach (var shareInfoCategory in currentUserObject.ShareInfoUser)
                        {
                            // Extract l'ID de l'expéditeur de la catégorie
                            string senderId = shareInfoCategory.Key.Replace("SenderId_", "");

                            // Vérifie si cet ID correspond à l'utilisateur sélectionné
                            if (senderId == selectedSharedUserId)
                            {
                                var itemsToAdd = shareInfoCategory.Value.ToArray();
                                listBoxViewerShareInfos.Items.AddRange(itemsToAdd);
                            }
                        }

                        // Rafraîchir la listBoxViewerShareInfos
                        listBoxViewerShareInfos.Refresh();

                        // Afficher le PanelViewerInfosSelection
                        EditListPanel.Visible = false;
                        DashBoardPanel.Visible = false;
                        MyPagePanel.Visible = false;
                        SettingsPanel.Visible = false;
                        VPNpanel.Visible = false;
                        panelSearchUser.Visible = false;
                        PanelShareViewer.Visible = false;
                        PanelPartageMdpFinal.Visible = false;
                        panelShare.Visible = false;
                        PanelViewerInfosSelection.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("L'utilisateur connecté ou les informations partagées sont nuls");
                    }
                }
                else
                {
                    MessageBox.Show("L'utilisateur connecté n'a pas été trouvé");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

    }
    public class Credential
    {
        public string CredentialName { get; set; } // Nom du Credential (par exemple, "Compte de messagerie")
        public List<SiteInfo> SiteInfos { get; set; } = new List<SiteInfo>();
    }

}
