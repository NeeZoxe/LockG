namespace LockG
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            panel1 = new Panel();
            textBox1 = new TextBox();
            panel2 = new Panel();
            buttonRegister = new Button();
            PageLogin = new Panel();
            label4 = new Label();
            ConnexionAuto = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            MdpOublie = new Label();
            label3 = new Label();
            button2 = new Button();
            panel4 = new Panel();
            textBox2 = new TextBox();
            label2 = new Label();
            siticonePanel1 = new Siticone.Desktop.UI.WinForms.SiticonePanel();
            panel11 = new Panel();
            button1 = new Button();
            panel12 = new Panel();
            textBox8 = new TextBox();
            label5 = new Label();
            panel13 = new Panel();
            textBox9 = new TextBox();
            label6 = new Label();
            siticoneControlBox2 = new Siticone.Desktop.UI.WinForms.SiticoneControlBox();
            siticoneControlBox1 = new Siticone.Desktop.UI.WinForms.SiticoneControlBox();
            siticoneDragControl1 = new Siticone.Desktop.UI.WinForms.SiticoneDragControl(components);
            panel10 = new Panel();
            txtCountdown = new Label();
            gifRemaining = new PictureBox();
            textBox7 = new TextBox();
            TimerPin = new System.Windows.Forms.Timer(components);
            PageRegister = new Panel();
            label7 = new Label();
            label8 = new Label();
            panel3 = new Panel();
            textBoxEmailRegister = new TextBox();
            buttonRegisterData = new Button();
            panel14 = new Panel();
            textPasswordRegister = new TextBox();
            panel15 = new Panel();
            textUsernameRegister = new TextBox();
            PageConfirmation = new Panel();
            textBox10 = new TextBox();
            btnConfirmerCode = new Siticone.Desktop.UI.WinForms.SiticoneGradientButton();
            label9 = new Label();
            panel20 = new Panel();
            inputConfirmation = new TextBox();
            label10 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            PageLogin.SuspendLayout();
            panel4.SuspendLayout();
            siticonePanel1.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel13.SuspendLayout();
            panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gifRemaining).BeginInit();
            PageRegister.SuspendLayout();
            panel3.SuspendLayout();
            panel14.SuspendLayout();
            panel15.SuspendLayout();
            PageConfirmation.SuspendLayout();
            panel20.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(357, 228);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Nirmala UI", 19.8F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(150, 92);
            label1.Name = "label1";
            label1.Size = new Size(106, 45);
            label1.TabIndex = 2;
            label1.Text = "Login";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(30, 30, 30);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(159, 195);
            panel1.Name = "panel1";
            panel1.Size = new Size(373, 49);
            panel1.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.FromArgb(30, 30, 30);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.ForeColor = Color.White;
            textBox1.Location = new Point(13, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(275, 23);
            textBox1.TabIndex = 0;
            textBox1.Text = "Username";
            textBox1.MouseDown += textBox1_MouseDown;
            // 
            // panel2
            // 
            panel2.Controls.Add(buttonRegister);
            panel2.Controls.Add(pictureBox1);
            panel2.Location = new Point(3, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(349, 578);
            panel2.TabIndex = 4;
            // 
            // buttonRegister
            // 
            buttonRegister.BackColor = Color.FromArgb(14, 165, 123);
            buttonRegister.Cursor = Cursors.Hand;
            buttonRegister.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 123);
            buttonRegister.FlatStyle = FlatStyle.Flat;
            buttonRegister.Font = new Font("Nirmala UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            buttonRegister.ForeColor = Color.White;
            buttonRegister.Location = new Point(105, 510);
            buttonRegister.Name = "buttonRegister";
            buttonRegister.Size = new Size(151, 39);
            buttonRegister.TabIndex = 2;
            buttonRegister.Text = "Register";
            buttonRegister.UseVisualStyleBackColor = false;
            buttonRegister.Click += buttonRegister_Click;
            // 
            // PageLogin
            // 
            PageLogin.BackColor = Color.FromArgb(23, 23, 23);
            PageLogin.Controls.Add(label4);
            PageLogin.Controls.Add(ConnexionAuto);
            PageLogin.Controls.Add(MdpOublie);
            PageLogin.Controls.Add(label3);
            PageLogin.Controls.Add(button2);
            PageLogin.Controls.Add(panel4);
            PageLogin.Controls.Add(label2);
            PageLogin.Controls.Add(panel1);
            PageLogin.Controls.Add(label1);
            PageLogin.Location = new Point(358, 39);
            PageLogin.Name = "PageLogin";
            PageLogin.Size = new Size(694, 536);
            PageLogin.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = SystemColors.WindowFrame;
            label4.Location = new Point(194, 308);
            label4.Name = "label4";
            label4.Size = new Size(113, 20);
            label4.TabIndex = 9;
            label4.Text = "Connexion auto";
            // 
            // ConnexionAuto
            // 
            ConnexionAuto.CheckedState.BorderColor = Color.FromArgb(60, 60, 60);
            ConnexionAuto.CheckedState.BorderRadius = 2;
            ConnexionAuto.CheckedState.BorderThickness = 1;
            ConnexionAuto.CheckedState.FillColor = Color.FromArgb(23, 23, 23);
            ConnexionAuto.CheckMarkColor = SystemColors.WindowFrame;
            ConnexionAuto.Cursor = Cursors.Hand;
            ConnexionAuto.CustomizableEdges = customizableEdges1;
            ConnexionAuto.ForeColor = Color.White;
            ConnexionAuto.Location = new Point(170, 309);
            ConnexionAuto.Name = "ConnexionAuto";
            ConnexionAuto.ShadowDecoration.CustomizableEdges = customizableEdges2;
            ConnexionAuto.Size = new Size(20, 20);
            ConnexionAuto.TabIndex = 8;
            ConnexionAuto.UncheckedState.BorderColor = Color.FromArgb(60, 60, 60);
            ConnexionAuto.UncheckedState.BorderRadius = 2;
            ConnexionAuto.UncheckedState.BorderThickness = 1;
            ConnexionAuto.UncheckedState.FillColor = Color.FromArgb(23, 23, 23);
            // 
            // MdpOublie
            // 
            MdpOublie.AutoSize = true;
            MdpOublie.Cursor = Cursors.Hand;
            MdpOublie.Font = new Font("Segoe UI", 9F, FontStyle.Underline, GraphicsUnit.Point);
            MdpOublie.ForeColor = SystemColors.WindowFrame;
            MdpOublie.Location = new Point(379, 309);
            MdpOublie.Name = "MdpOublie";
            MdpOublie.Size = new Size(144, 20);
            MdpOublie.TabIndex = 7;
            MdpOublie.Text = "Mot de passe oublié";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.White;
            label3.Location = new Point(253, 399);
            label3.Name = "label3";
            label3.Size = new Size(188, 23);
            label3.TabIndex = 5;
            label3.Text = "Mot de passe incorrect.";
            label3.Visible = false;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(14, 165, 123);
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 123);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Nirmala UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button2.ForeColor = Color.White;
            button2.Location = new Point(158, 342);
            button2.Name = "button2";
            button2.Size = new Size(374, 39);
            button2.TabIndex = 2;
            button2.Text = "Login";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(30, 30, 30);
            panel4.Controls.Add(textBox2);
            panel4.Location = new Point(159, 250);
            panel4.Name = "panel4";
            panel4.Size = new Size(373, 49);
            panel4.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.FromArgb(30, 30, 30);
            textBox2.BorderStyle = BorderStyle.None;
            textBox2.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.ForeColor = Color.White;
            textBox2.Location = new Point(13, 12);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(275, 23);
            textBox2.TabIndex = 0;
            textBox2.Text = "Password";
            textBox2.UseSystemPasswordChar = true;
            textBox2.MouseDown += textBox2_MouseDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Nirmala UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(154, 147);
            label2.Name = "label2";
            label2.Size = new Size(313, 28);
            label2.TabIndex = 4;
            label2.Text = "Log in to access your data securely";
            // 
            // siticonePanel1
            // 
            siticonePanel1.BackColor = Color.FromArgb(23, 23, 23);
            siticonePanel1.Controls.Add(panel11);
            siticonePanel1.Controls.Add(siticoneControlBox2);
            siticonePanel1.Controls.Add(siticoneControlBox1);
            siticonePanel1.Location = new Point(358, 0);
            siticonePanel1.Name = "siticonePanel1";
            siticonePanel1.Size = new Size(694, 40);
            siticonePanel1.TabIndex = 5;
            // 
            // panel11
            // 
            panel11.BackColor = Color.FromArgb(23, 23, 23);
            panel11.Controls.Add(button1);
            panel11.Controls.Add(panel12);
            panel11.Controls.Add(label5);
            panel11.Controls.Add(panel13);
            panel11.Controls.Add(label6);
            panel11.Location = new Point(0, 39);
            panel11.Name = "panel11";
            panel11.Size = new Size(704, 539);
            panel11.TabIndex = 6;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(14, 165, 123);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 123);
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Nirmala UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            button1.ForeColor = Color.White;
            button1.Location = new Point(121, 321);
            button1.Name = "button1";
            button1.Size = new Size(374, 39);
            button1.TabIndex = 2;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = false;
            // 
            // panel12
            // 
            panel12.BackColor = Color.FromArgb(30, 30, 30);
            panel12.Controls.Add(textBox8);
            panel12.Location = new Point(122, 250);
            panel12.Name = "panel12";
            panel12.Size = new Size(373, 49);
            panel12.TabIndex = 4;
            // 
            // textBox8
            // 
            textBox8.BackColor = Color.FromArgb(30, 30, 30);
            textBox8.BorderStyle = BorderStyle.None;
            textBox8.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textBox8.ForeColor = Color.White;
            textBox8.Location = new Point(13, 12);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(275, 23);
            textBox8.TabIndex = 0;
            textBox8.Text = "Password";
            textBox8.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Nirmala UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.White;
            label5.Location = new Point(117, 147);
            label5.Name = "label5";
            label5.Size = new Size(379, 28);
            label5.TabIndex = 4;
            label5.Text = "log in with your account to save your data";
            // 
            // panel13
            // 
            panel13.BackColor = Color.FromArgb(30, 30, 30);
            panel13.Controls.Add(textBox9);
            panel13.Location = new Point(122, 195);
            panel13.Name = "panel13";
            panel13.Size = new Size(373, 49);
            panel13.TabIndex = 3;
            // 
            // textBox9
            // 
            textBox9.BackColor = Color.FromArgb(30, 30, 30);
            textBox9.BorderStyle = BorderStyle.None;
            textBox9.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textBox9.ForeColor = Color.White;
            textBox9.Location = new Point(13, 12);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(275, 23);
            textBox9.TabIndex = 0;
            textBox9.Text = "Username";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Nirmala UI", 19.8F, FontStyle.Bold, GraphicsUnit.Point);
            label6.ForeColor = Color.White;
            label6.Location = new Point(113, 92);
            label6.Name = "label6";
            label6.Size = new Size(143, 45);
            label6.TabIndex = 2;
            label6.Text = "SIGN IN";
            // 
            // siticoneControlBox2
            // 
            siticoneControlBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            siticoneControlBox2.ControlBoxType = Siticone.Desktop.UI.WinForms.Enums.ControlBoxType.MinimizeBox;
            siticoneControlBox2.FillColor = Color.FromArgb(23, 23, 23);
            siticoneControlBox2.IconColor = Color.White;
            siticoneControlBox2.Location = new Point(582, 0);
            siticoneControlBox2.Name = "siticoneControlBox2";
            siticoneControlBox2.Size = new Size(56, 36);
            siticoneControlBox2.TabIndex = 1;
            // 
            // siticoneControlBox1
            // 
            siticoneControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            siticoneControlBox1.FillColor = Color.FromArgb(23, 23, 23);
            siticoneControlBox1.IconColor = Color.White;
            siticoneControlBox1.Location = new Point(638, 0);
            siticoneControlBox1.Name = "siticoneControlBox1";
            siticoneControlBox1.Size = new Size(56, 36);
            siticoneControlBox1.TabIndex = 0;
            siticoneControlBox1.Click += siticoneControlBox1_Click;
            // 
            // siticoneDragControl1
            // 
            siticoneDragControl1.DockIndicatorTransparencyValue = 1D;
            siticoneDragControl1.DragStartTransparencyValue = 1D;
            siticoneDragControl1.TargetControl = siticonePanel1;
            siticoneDragControl1.TransparentWhileDrag = false;
            // 
            // panel10
            // 
            panel10.BackColor = Color.FromArgb(23, 23, 23);
            panel10.Controls.Add(txtCountdown);
            panel10.Controls.Add(gifRemaining);
            panel10.Controls.Add(textBox7);
            panel10.Location = new Point(358, 39);
            panel10.Name = "panel10";
            panel10.Size = new Size(694, 533);
            panel10.TabIndex = 7;
            // 
            // txtCountdown
            // 
            txtCountdown.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtCountdown.AutoSize = true;
            txtCountdown.BackColor = Color.Transparent;
            txtCountdown.Font = new Font("Nirmala UI", 48F, FontStyle.Bold, GraphicsUnit.Point);
            txtCountdown.ForeColor = Color.White;
            txtCountdown.Location = new Point(282, 210);
            txtCountdown.Name = "txtCountdown";
            txtCountdown.Size = new Size(137, 106);
            txtCountdown.TabIndex = 6;
            txtCountdown.Text = "60";
            txtCountdown.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gifRemaining
            // 
            gifRemaining.BackColor = Color.FromArgb(23, 23, 23);
            gifRemaining.Image = (Image)resources.GetObject("gifRemaining.Image");
            gifRemaining.Location = new Point(603, 449);
            gifRemaining.Name = "gifRemaining";
            gifRemaining.Size = new Size(79, 71);
            gifRemaining.SizeMode = PictureBoxSizeMode.Zoom;
            gifRemaining.TabIndex = 4;
            gifRemaining.TabStop = false;
            // 
            // textBox7
            // 
            textBox7.BackColor = Color.FromArgb(23, 23, 23);
            textBox7.BorderStyle = BorderStyle.None;
            textBox7.Font = new Font("Nirmala UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point);
            textBox7.ForeColor = Color.White;
            textBox7.HideSelection = false;
            textBox7.Location = new Point(256, 168);
            textBox7.MaxLength = 1;
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(222, 36);
            textBox7.TabIndex = 2;
            textBox7.Text = "Temps restant.";
            // 
            // TimerPin
            // 
            TimerPin.Interval = 1000;
            TimerPin.Tick += TimerPin_Tick;
            // 
            // PageRegister
            // 
            PageRegister.BackColor = Color.FromArgb(23, 23, 23);
            PageRegister.Controls.Add(label7);
            PageRegister.Controls.Add(label8);
            PageRegister.Controls.Add(panel3);
            PageRegister.Controls.Add(buttonRegisterData);
            PageRegister.Controls.Add(panel14);
            PageRegister.Controls.Add(panel15);
            PageRegister.Location = new Point(358, 39);
            PageRegister.Name = "PageRegister";
            PageRegister.Size = new Size(701, 530);
            PageRegister.TabIndex = 6;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Nirmala UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label7.ForeColor = Color.White;
            label7.Location = new Point(155, 147);
            label7.Name = "label7";
            label7.Size = new Size(340, 28);
            label7.TabIndex = 4;
            label7.Text = "register to back up your data securely";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Nirmala UI", 19.8F, FontStyle.Bold, GraphicsUnit.Point);
            label8.ForeColor = Color.White;
            label8.Location = new Point(151, 92);
            label8.Name = "label8";
            label8.Size = new Size(147, 45);
            label8.TabIndex = 2;
            label8.Text = "Register";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(30, 30, 30);
            panel3.Controls.Add(textBoxEmailRegister);
            panel3.Location = new Point(160, 250);
            panel3.Name = "panel3";
            panel3.Size = new Size(373, 49);
            panel3.TabIndex = 4;
            // 
            // textBoxEmailRegister
            // 
            textBoxEmailRegister.BackColor = Color.FromArgb(30, 30, 30);
            textBoxEmailRegister.BorderStyle = BorderStyle.None;
            textBoxEmailRegister.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxEmailRegister.ForeColor = Color.White;
            textBoxEmailRegister.Location = new Point(13, 12);
            textBoxEmailRegister.Name = "textBoxEmailRegister";
            textBoxEmailRegister.Size = new Size(275, 23);
            textBoxEmailRegister.TabIndex = 0;
            textBoxEmailRegister.Text = "Email";
            textBoxEmailRegister.MouseDown += textBoxEmailRegister_MouseDown;
            // 
            // buttonRegisterData
            // 
            buttonRegisterData.BackColor = Color.FromArgb(14, 165, 123);
            buttonRegisterData.Cursor = Cursors.Hand;
            buttonRegisterData.FlatAppearance.BorderColor = Color.FromArgb(14, 165, 123);
            buttonRegisterData.FlatStyle = FlatStyle.Flat;
            buttonRegisterData.Font = new Font("Nirmala UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point);
            buttonRegisterData.ForeColor = Color.White;
            buttonRegisterData.Location = new Point(159, 376);
            buttonRegisterData.Name = "buttonRegisterData";
            buttonRegisterData.Size = new Size(374, 39);
            buttonRegisterData.TabIndex = 2;
            buttonRegisterData.Text = "Register";
            buttonRegisterData.UseVisualStyleBackColor = false;
            buttonRegisterData.Click += button3_Click;
            // 
            // panel14
            // 
            panel14.BackColor = Color.FromArgb(30, 30, 30);
            panel14.Controls.Add(textPasswordRegister);
            panel14.Location = new Point(160, 305);
            panel14.Name = "panel14";
            panel14.Size = new Size(373, 49);
            panel14.TabIndex = 4;
            // 
            // textPasswordRegister
            // 
            textPasswordRegister.BackColor = Color.FromArgb(30, 30, 30);
            textPasswordRegister.BorderStyle = BorderStyle.None;
            textPasswordRegister.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textPasswordRegister.ForeColor = Color.White;
            textPasswordRegister.Location = new Point(13, 12);
            textPasswordRegister.Name = "textPasswordRegister";
            textPasswordRegister.Size = new Size(275, 23);
            textPasswordRegister.TabIndex = 0;
            textPasswordRegister.Text = "Password";
            textPasswordRegister.UseSystemPasswordChar = true;
            textPasswordRegister.MouseDown += textPasswordRegister_MouseDown;
            // 
            // panel15
            // 
            panel15.BackColor = Color.FromArgb(30, 30, 30);
            panel15.Controls.Add(textUsernameRegister);
            panel15.Location = new Point(160, 195);
            panel15.Name = "panel15";
            panel15.Size = new Size(373, 49);
            panel15.TabIndex = 3;
            // 
            // textUsernameRegister
            // 
            textUsernameRegister.BackColor = Color.FromArgb(30, 30, 30);
            textUsernameRegister.BorderStyle = BorderStyle.None;
            textUsernameRegister.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textUsernameRegister.ForeColor = Color.White;
            textUsernameRegister.Location = new Point(13, 12);
            textUsernameRegister.Name = "textUsernameRegister";
            textUsernameRegister.Size = new Size(275, 23);
            textUsernameRegister.TabIndex = 0;
            textUsernameRegister.Text = "Username";
            textUsernameRegister.MouseDown += textUsernameRegister_MouseDown;
            // 
            // PageConfirmation
            // 
            PageConfirmation.BackColor = Color.FromArgb(23, 23, 23);
            PageConfirmation.Controls.Add(textBox10);
            PageConfirmation.Controls.Add(btnConfirmerCode);
            PageConfirmation.Controls.Add(label9);
            PageConfirmation.Controls.Add(panel20);
            PageConfirmation.Controls.Add(label10);
            PageConfirmation.Location = new Point(358, 39);
            PageConfirmation.Name = "PageConfirmation";
            PageConfirmation.Size = new Size(694, 539);
            PageConfirmation.TabIndex = 7;
            // 
            // textBox10
            // 
            textBox10.BackColor = Color.FromArgb(23, 23, 23);
            textBox10.BorderStyle = BorderStyle.None;
            textBox10.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            textBox10.ForeColor = Color.White;
            textBox10.HideSelection = false;
            textBox10.Location = new Point(186, 172);
            textBox10.MaxLength = 1;
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(330, 23);
            textBox10.TabIndex = 7;
            textBox10.Text = "Un mail de confirmation vous a été envoyé";
            // 
            // btnConfirmerCode
            // 
            btnConfirmerCode.BorderRadius = 15;
            btnConfirmerCode.Cursor = Cursors.Hand;
            btnConfirmerCode.DisabledState.BorderColor = Color.DarkGray;
            btnConfirmerCode.DisabledState.CustomBorderColor = Color.DarkGray;
            btnConfirmerCode.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnConfirmerCode.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            btnConfirmerCode.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnConfirmerCode.FillColor = Color.FromArgb(14, 165, 123);
            btnConfirmerCode.FillColor2 = Color.FromArgb(14, 165, 123);
            btnConfirmerCode.Font = new Font("Nirmala UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            btnConfirmerCode.ForeColor = Color.White;
            btnConfirmerCode.Image = (Image)resources.GetObject("btnConfirmerCode.Image");
            btnConfirmerCode.ImageSize = new Size(40, 70);
            btnConfirmerCode.Location = new Point(319, 288);
            btnConfirmerCode.Name = "btnConfirmerCode";
            btnConfirmerCode.Size = new Size(73, 71);
            btnConfirmerCode.TabIndex = 6;
            btnConfirmerCode.Click += btnConfirmerCode_Click_1;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Nirmala UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label9.ForeColor = Color.White;
            label9.Location = new Point(122, 150);
            label9.Name = "label9";
            label9.Size = new Size(0, 28);
            label9.TabIndex = 4;
            // 
            // panel20
            // 
            panel20.BackColor = Color.FromArgb(30, 30, 30);
            panel20.Controls.Add(inputConfirmation);
            panel20.Location = new Point(183, 207);
            panel20.Name = "panel20";
            panel20.Size = new Size(340, 66);
            panel20.TabIndex = 3;
            // 
            // inputConfirmation
            // 
            inputConfirmation.BackColor = Color.FromArgb(30, 30, 30);
            inputConfirmation.BorderStyle = BorderStyle.None;
            inputConfirmation.Font = new Font("Nirmala UI", 22.2F, FontStyle.Regular, GraphicsUnit.Point);
            inputConfirmation.ForeColor = Color.White;
            inputConfirmation.Location = new Point(12, 9);
            inputConfirmation.MaxLength = 6;
            inputConfirmation.Name = "inputConfirmation";
            inputConfirmation.Size = new Size(316, 50);
            inputConfirmation.TabIndex = 0;
            inputConfirmation.TextAlign = HorizontalAlignment.Center;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Nirmala UI", 19.8F, FontStyle.Bold, GraphicsUnit.Point);
            label10.ForeColor = Color.White;
            label10.Location = new Point(244, 120);
            label10.Name = "label10";
            label10.Size = new Size(226, 45);
            label10.TabIndex = 2;
            label10.Text = "Confirmation";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(17, 17, 17);
            ClientSize = new Size(1052, 571);
            Controls.Add(siticonePanel1);
            Controls.Add(panel2);
            Controls.Add(PageLogin);
            Controls.Add(panel10);
            Controls.Add(PageConfirmation);
            Controls.Add(PageRegister);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LOCKG";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            PageLogin.ResumeLayout(false);
            PageLogin.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            siticonePanel1.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panel12.ResumeLayout(false);
            panel12.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gifRemaining).EndInit();
            PageRegister.ResumeLayout(false);
            PageRegister.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel14.ResumeLayout(false);
            panel14.PerformLayout();
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            PageConfirmation.ResumeLayout(false);
            PageConfirmation.PerformLayout();
            panel20.ResumeLayout(false);
            panel20.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Panel panel1;
        private Panel panel2;
        private Panel PageLogin;
        private Label label2;
        private TextBox textBox1;
        private Panel panel4;
        private TextBox textBox2;
        private Button button2;
        private Siticone.Desktop.UI.WinForms.SiticonePanel siticonePanel1;
        private Siticone.Desktop.UI.WinForms.SiticoneDragControl siticoneDragControl1;
        private Siticone.Desktop.UI.WinForms.SiticoneControlBox siticoneControlBox2;
        private Siticone.Desktop.UI.WinForms.SiticoneControlBox siticoneControlBox1;
        private Panel panel10;
        private System.Windows.Forms.Timer TimerPin;
        private TextBox textBox7;
        private Label txtCountdown;
        private PictureBox gifRemaining;
        private Panel panel11;
        private Button button1;
        private Panel panel12;
        private TextBox textBox8;
        private Label label5;
        private Panel panel13;
        private TextBox textBox9;
        private Label label6;
        private Button buttonRegister;
        private Panel PageRegister;
        private Button buttonRegisterData;
        private Panel panel14;
        private TextBox textPasswordRegister;
        private Label label7;
        private Panel panel15;
        private TextBox textUsernameRegister;
        private Label label8;
        private Panel panel3;
        private TextBox textBoxEmailRegister;
        private Panel PageConfirmation;
        private Siticone.Desktop.UI.WinForms.SiticoneGradientButton btnConfirmerCode;
        private Label label9;
        private Panel panel20;
        private TextBox inputConfirmation;
        private Label label10;
        private TextBox textBox10;
        private Label label3;
        private Label MdpOublie;
        private Guna.UI2.WinForms.Guna2CustomCheckBox ConnexionAuto;
        private Label label4;
    }
}