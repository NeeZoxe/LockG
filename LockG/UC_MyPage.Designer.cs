namespace LockG
{
    partial class UC_MyPage
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.MyPageLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MyPageLabel
            // 
            this.MyPageLabel.AutoSize = true;
            this.MyPageLabel.Font = new System.Drawing.Font("Nirmala UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MyPageLabel.Location = new System.Drawing.Point(20, 20);
            this.MyPageLabel.Name = "MyPageLabel";
            this.MyPageLabel.Size = new System.Drawing.Size(102, 31);
            this.MyPageLabel.TabIndex = 0;
            this.MyPageLabel.Text = "My Page";
            // 
            // UC_MyPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MyPageLabel);
            this.Name = "UC_MyPage";
            this.Size = new System.Drawing.Size(917, 669);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label MyPageLabel;
    }
}
