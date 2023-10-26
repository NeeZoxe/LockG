using Firebase.Database;
using Firebase.Database.Query;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace LockG
{
    public partial class Form1 : Form
    {
        private FirebaseClient firebaseClient;

        public Form1()
        {
            InitializeComponent();

            // Initialisez le client Firebase dans le constructeur
            firebaseClient = new FirebaseClient("https://lockg-92e09-default-rtdb.europe-west1.firebasedatabase.app/");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ContinueCountdown();
            PageLogin.Visible = true;
            PageRegister.Visible = false;
            panel10.Visible = false;
            PageConfirmation.Visible = false;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // R�cup�rez le nom d'utilisateur et le mot de passe saisis
            string nomUtilisateur = textBox1.Text;
            string motDePasse = textBox2.Text;

            try
            {
                // Utilisez Firebase pour rechercher un enregistrement correspondant
                var utilisateur = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.nom_utilisateur == nomUtilisateur && u.Object.mot_de_passe == motDePasse);

                if (utilisateur != null)
                {
                    // Authentification r�ussie, ouvrez le formulaire suivant en passant l'ID de l'utilisateur
                    Form2 form2 = new Form2(utilisateur.Object.Id);
                    form2.Show();
                    this.Hide();
                }
                else
                {
                    // Authentification �chou�e, affichez un message d'erreur
                    PinInvalidCompt += 1;
                    if (PinInvalidCompt >= 1 && PinInvalidCompt <= 3)
                    {
                        label3.Visible = true;
                    }
                    else if (PinInvalidCompt >= 4)
                    {
                        TimerPin.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }
        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox1.Text == "Username")
            {
                textBox1.Text = ""; // Efface le texte par d�faut si c'est "username"
            }
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.Text = ""; // Efface le texte par d�faut si c'est "username"
            }
        }

        private void siticoneControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int PinInvalidCompt = 0; // compteur d'erreur de pin entr�e
        // timer invalid password Part
        private void TimerPin_Tick(object sender, EventArgs e)
        {
            UpdateCountdownText();
        }

        private void SaveCountdownToFile()
        {
            string filePath = "countdown.txt";
            File.WriteAllText(filePath, countdown.ToString());
        }

        private int LoadCountdownFromFile()
        {
            string filePath = "countdown.txt";
            if (File.Exists(filePath))
            {
                string countdownStr = File.ReadAllText(filePath);
                int savedCountdown;
                if (int.TryParse(countdownStr, out savedCountdown))
                {
                    return savedCountdown;
                }
            }
            return 0; // Valeur par d�faut si le fichier n'existe pas ou s'il ne peut pas �tre analys�
        }

        private int countdown = 60; // Commencer le d�compte � 60 secondes
        private int timerUpgrade = 60; // chrono update
        private void UpdateCountdownText()
        {
            PageLogin.Visible = false;
            PageRegister.Visible = false;
            buttonRegister.Visible = false;
            PageConfirmation.Visible = false;
            panel10.Visible = true;
            countdown--; // R�duire le d�compte d'une seconde

            if (countdown >= 0)
            {
                txtCountdown.Text = countdown.ToString(); // Mettre � jour la zone de texte avec le d�compte
                if (countdown <= 9)
                {
                    txtCountdown.Location = new Point(305, 210);
                }
                else if (countdown <= 99)
                {
                    txtCountdown.Location = new Point(282, 210);
                }
                SaveCountdownToFile(); // Enregistrer le temps restant dans le fichier
            }
            else // si le timer est fini 
            {
                TimerPin.Stop(); // Arr�ter le timer lorsque le d�compte atteint 0
                txtCountdown.Text = "0"; // Afficher un message lorsque le d�compte est termin�
                PinInvalidCompt = 0; // remis a zero
                PageLogin.Visible = true;
                PageRegister.Visible = false;
                PageConfirmation.Visible = false;
                buttonRegister.Visible = true;
                label3.Visible = false;
                panel10.Visible = false;
                timerUpgrade = timerUpgrade * 2; // double le chrono si l'utilisateur ce trompe encore
                if (timerUpgrade > 600)
                {
                    timerUpgrade = 600; // condition pour que le timer ne d�passe pas les 10min
                }
                countdown = timerUpgrade; // countdown est �gale a une nouvelle valeur si l'utilateur ce trompe encore
                txtCountdown.Location = new Point(259, 210); // deplace le texte dans le cas ou le chrono est sup�rieure � 100
            }
        }

        private void ContinueCountdown()
        {
            int loadedCountdown = LoadCountdownFromFile(); // Charge le d�compte depuis le fichier

            if (loadedCountdown > 0)
            {
                countdown = loadedCountdown; // Met � jour le d�compte avec la valeur charg�e
                UpdateCountdownText(); // Met � jour le texte du d�compte sur l'interface
                TimerPin.Start(); // D�marre le chrono
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (buttonRegister.Text == "Register")
            {
                PageLogin.Visible = false;
                panel10.Visible = false;
                PageConfirmation.Visible = false;
                PageRegister.Visible = true;
                buttonRegister.Text = "Login";
            }
            else
            {
                PageLogin.Visible = true;
                panel10.Visible = false;
                PageConfirmation.Visible = false;
                PageRegister.Visible = false;
                buttonRegister.Text = "Register";
            }
        }

        // verification mail existant ou non
        private async Task<bool> IsEmailTaken(string email)
        {
            try
            {
                var existingUser = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.email == email);

                return existingUser != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la v�rification de l'e-mail : {ex.Message}");
                return true; // Traitez l'erreur selon vos besoins
            }
        }

        // verification du pseudo si il est deja utilis� ou non
        private async Task<bool> IsUsernameTaken(string username)
        {
            try
            {
                var existingUser = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.nom_utilisateur == username);

                return existingUser != null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de la v�rification du nom d'utilisateur : {ex.Message}");
                return true; // Traitez l'erreur selon vos besoins
            }
        }

        // btn register
        private async void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string nomUtilisateur = textUsernameRegister.Text;
                string motDePasse = textPasswordRegister.Text;
                string email = textBoxEmailRegister.Text;

                if (!string.IsNullOrEmpty(nomUtilisateur) && !string.IsNullOrEmpty(motDePasse) && !string.IsNullOrEmpty(email))
                {
                    // G�n�rez un code � 6 chiffres al�atoire
                    Random random = new Random();
                    int codeConfirmation = random.Next(100000, 999999);

                    // Cr�ez un nouvel utilisateur avec le code de confirmation
                    var nouvelUtilisateur = new Utilisateur
                    {
                        Id = Guid.NewGuid().ToString(), // G�n�rez un ID unique
                        nom_utilisateur = nomUtilisateur,
                        mot_de_passe = motDePasse,
                        email = email,
                        code_confirmation = codeConfirmation.ToString()
                    };

                    // Ajoutez l'utilisateur � la base de donn�es Firebase
                    var ajoutUtilisateur = await firebaseClient
                        .Child("utilisateurs")
                        .PostAsync(nouvelUtilisateur);

                    // Envoyez un e-mail de confirmation
                    EnvoyerEmailConfirmation(email, codeConfirmation);

                    // Affichez un message de confirmation
                    MessageBox.Show("Un e-mail de confirmation a �t� envoy� � votre adresse e-mail. Veuillez v�rifier votre bo�te de r�ception et saisir le code de confirmation.");

                    // R�initialisez les champs du formulaire d'inscription si n�cessaire
                    // textUsernameRegister.Text = "";
                    // textPasswordRegister.Text = "";
                    // textBoxEmailRegister.Text = "";

                    // Passez � la page de confirmation
                    PageLogin.Visible = false;
                    panel10.Visible = false;
                    PageRegister.Visible = false;
                    PageConfirmation.Visible = true;
                    buttonRegister.Visible = false;
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private void EnvoyerEmailConfirmation(string destinataire, int codeConfirmation)
        {
            // Configuration de l'envoi de l'e-mail
            string adresseEmail = "lockgcorp@gmail.com"; // Remplacez par votre adresse e-mail
            string motDePasseEmail = "laaz kwku gxyo uuli"; // Remplacez par votre mot de passe

            // Cr�er le corps de l'e-mail au format HTML centr�
            string body = "<html><body>";
            body += "<table align='center' width='600px' style='text-align: center;'>";
            body += "<tr><td><img src='cid:lockg_logo' alt='LockG Logo' width='600px' height='400px'></td></tr>";
            body += "<tr><td><h1>Bienvenue sur LOCKG</h1></td></tr>";
            body += "<tr><td><p>Merci de vous �tre inscrit sur LOCKG. Pour valider votre compte, veuillez utiliser le code de confirmation suivant :</p></td></tr>";
            body += $"<tr><td><strong>{codeConfirmation}</strong></td></tr>";
            body += "<tr><td><p>Si vous n'avez pas demand� � recevoir ce code, veuillez ignorer ce message.</p></td></tr>";
            body += "<tr><td><p>Cordialement,<br>L'�quipe LOCKG</p></td></tr>";
            body += "</table>";
            body += "</body></html>";

            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");

            message.From = new MailAddress(adresseEmail);
            message.To.Add(new MailAddress(destinataire));
            message.Subject = "Code de confirmation LockG";
            message.Body = body;
            message.IsBodyHtml = true; // D�finir le contenu comme HTML

            // Cr�er une ressource incorpor�e pour l'image
            LinkedResource imageResource = new LinkedResource("mailimg.png", MediaTypeNames.Image.Jpeg);
            imageResource.ContentId = "lockg_logo";

            // Ajouter l'image comme ressource incorpor�e au message
            AlternateView av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            av.LinkedResources.Add(imageResource);
            message.AlternateViews.Add(av);

            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(adresseEmail, motDePasseEmail);
            smtpClient.EnableSsl = true;

            // Envoi de l'e-mail
            try
            {
                smtpClient.Send(message);
                MessageBox.Show("E-mail de confirmation envoy� avec succ�s.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite lors de l'envoi de l'e-mail de confirmation : {ex.Message}");
            }
        }

        // Gestionnaire d'�v�nements btnConfirmerCode_Click
        private async void btnConfirmerCode_Click_1(object sender, EventArgs e)
        {
            try
            {
                string codeConfirmationUtilisateur = inputConfirmation.Text;

                // R�cup�rez l'utilisateur actuel
                var utilisateur = (await firebaseClient
                    .Child("utilisateurs")
                    .OnceAsync<Utilisateur>())
                    .FirstOrDefault(u => u.Object.code_confirmation == codeConfirmationUtilisateur);

                if (utilisateur != null)
                {
                    // Le code de confirmation est correct, supprimez-le de la base de donn�es
                    await firebaseClient
                        .Child("utilisateurs")
                        .Child(utilisateur.Key)
                        .Child("code_confirmation")
                        .DeleteAsync();

                    // Passez � l'�tape suivante (par exemple, affichez un message de bienvenue)
                    MessageBox.Show("Code de confirmation correct. Vous �tes maintenant enregistr�.");

                    // Passez � la page suivante ou effectuez toute autre action n�cessaire
                    PageLogin.Visible = true;
                    panel10.Visible = false;
                    PageRegister.Visible = false;
                    PageConfirmation.Visible = false;
                    buttonRegister.Visible = false;
                }
                else
                {
                    MessageBox.Show("Code de confirmation incorrect. Veuillez r�essayer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur s'est produite : {ex.Message}");
            }
        }

        private void textUsernameRegister_MouseDown(object sender, MouseEventArgs e)
        {
            if (textUsernameRegister.Text == "Username")
            {
                textUsernameRegister.Text = ""; // Efface le texte par d�faut si c'est "username"
            }
        }

        private void textBoxEmailRegister_MouseDown(object sender, MouseEventArgs e)
        {
            if (textBoxEmailRegister.Text == "Email")
            {
                textBoxEmailRegister.Text = ""; // Efface le texte par d�faut si c'est "username"
            }
        }

        private void textPasswordRegister_MouseDown(object sender, MouseEventArgs e)
        {
            if (textPasswordRegister.Text == "Password")
            {
                textPasswordRegister.Text = ""; // Efface le texte par d�faut si c'est "username"
            }
        }
    }
    public class Utilisateur
    {
        public string Id { get; set; } // Ajoutez un champ ID unique

        public string nom_utilisateur { get; set; }
        public string mot_de_passe { get; set; }
        public string email { get; set; }
        public string code_confirmation { get; set; }
        public List<Credential> Credentials { get; set; } = new List<Credential>();
        public List<string> Friends { get; set; } = new List<string>();
        public Dictionary<string, List<string>> ShareInfoUser { get; set; }
    }
}