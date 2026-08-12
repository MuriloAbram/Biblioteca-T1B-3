namespace T1B_3Library.Desktop.Forms
{
    partial class BookFormDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            lblDialogTitle = new Label();
            lblTitle = new Label();
            txtTitle = new Guna.UI2.WinForms.Guna2TextBox();
            lblAuthor = new Label();
            txtAuthor = new Guna.UI2.WinForms.Guna2TextBox();
            lblGender = new Label();
            cmbGender = new Guna.UI2.WinForms.Guna2ComboBox();
            lblYear = new Label();
            txtYear = new Guna.UI2.WinForms.Guna2TextBox();
            lblQuantity = new Label();
            txtQuantity = new Guna.UI2.WinForms.Guna2TextBox();
            btnSave = new Guna.UI2.WinForms.Guna2Button();
            btnCancel = new Guna.UI2.WinForms.Guna2Button();
            borderForm = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            dragControl = new Guna.UI2.WinForms.Guna2DragControl(components);
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblDialogTitle);
            pnlHeader.CustomizableEdges = customizableEdges3;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.FillColor = Color.FromArgb(24, 30, 54);
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlHeader.Size = new Size(480, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.CustomizableEdges = customizableEdges1;
            btnClose.FillColor = Color.Transparent;
            btnClose.IconColor = Color.White;
            btnClose.Location = new Point(435, 10);
            btnClose.Name = "btnClose";
            btnClose.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnClose.Size = new Size(35, 30);
            btnClose.TabIndex = 0;
            // 
            // lblDialogTitle
            // 
            lblDialogTitle.AutoSize = true;
            lblDialogTitle.BackColor = Color.Transparent;
            lblDialogTitle.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDialogTitle.ForeColor = Color.White;
            lblDialogTitle.Location = new Point(20, 9);
            lblDialogTitle.Name = "lblDialogTitle";
            lblDialogTitle.Size = new Size(160, 30);
            lblDialogTitle.TabIndex = 1;
            lblDialogTitle.Text = "Cadastrar Livro";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(20, 66);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(121, 21);
            lblTitle.TabIndex = 24;
            lblTitle.Text = "Título do Livro";
            // 
            // txtTitle
            // 
            txtTitle.BorderRadius = 8;
            txtTitle.CustomizableEdges = customizableEdges5;
            txtTitle.DefaultText = "";
            txtTitle.FillColor = Color.FromArgb(37, 42, 64);
            txtTitle.Font = new Font("Segoe UI", 9F);
            txtTitle.Location = new Point(20, 90);
            txtTitle.Name = "txtTitle";
            txtTitle.PlaceholderText = "";
            txtTitle.SelectedText = "";
            txtTitle.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtTitle.Size = new Size(200, 36);
            txtTitle.TabIndex = 23;
            // 
            // lblAuthor
            // 
            lblAuthor.AutoSize = true;
            lblAuthor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblAuthor.ForeColor = Color.White;
            lblAuthor.Location = new Point(268, 66);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(53, 21);
            lblAuthor.TabIndex = 22;
            lblAuthor.Text = "Autor";
            // 
            // txtAuthor
            // 
            txtAuthor.BorderRadius = 8;
            txtAuthor.CustomizableEdges = customizableEdges7;
            txtAuthor.DefaultText = "";
            txtAuthor.FillColor = Color.FromArgb(37, 42, 64);
            txtAuthor.Font = new Font("Segoe UI", 9F);
            txtAuthor.Location = new Point(268, 90);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.PlaceholderText = "";
            txtAuthor.SelectedText = "";
            txtAuthor.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtAuthor.Size = new Size(200, 36);
            txtAuthor.TabIndex = 21;
            // 
            // lblCategory
            // 
            lblGender.AutoSize = true;
            lblGender.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblGender.ForeColor = Color.White;
            lblGender.Location = new Point(20, 243);
            lblGender.Name = "lblGender";
            lblGender.Size = new Size(84, 21);
            lblGender.TabIndex = 18;
            lblGender.Text = "Gender";
            // 
            // cmbCategory
            // 
            cmbGender.BackColor = Color.Transparent;
            cmbGender.BorderRadius = 8;
            cmbGender.CustomizableEdges = customizableEdges9;
            cmbGender.DrawMode = DrawMode.OwnerDrawFixed;
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.FillColor = Color.FromArgb(37, 42, 64);
            cmbGender.FocusedColor = Color.Empty;
            cmbGender.Font = new Font("Segoe UI", 9.5F);
            cmbGender.ForeColor = Color.White;
            cmbGender.ItemHeight = 30;
            cmbGender.Location = new Point(20, 267);
            cmbGender.Name = "cmbGender";
            cmbGender.ShadowDecoration.CustomizableEdges = customizableEdges10;
            cmbGender.Size = new Size(200, 36);
            cmbGender.TabIndex = 17;
            // 
            // lblYear
            // 
            lblYear.AutoSize = true;
            lblYear.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblYear.ForeColor = Color.White;
            lblYear.Location = new Point(268, 161);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(152, 21);
            lblYear.TabIndex = 16;
            lblYear.Text = "Ano de Publicação";
            // 
            // txtYear
            // 
            txtYear.BorderRadius = 8;
            txtYear.CustomizableEdges = customizableEdges11;
            txtYear.DefaultText = "";
            txtYear.FillColor = Color.FromArgb(37, 42, 64);
            txtYear.Font = new Font("Segoe UI", 9F);
            txtYear.Location = new Point(268, 185);
            txtYear.Name = "txtYear";
            txtYear.PlaceholderText = "";
            txtYear.SelectedText = "";
            txtYear.ShadowDecoration.CustomizableEdges = customizableEdges12;
            txtYear.Size = new Size(200, 36);
            txtYear.TabIndex = 15;
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblQuantity.ForeColor = Color.White;
            lblQuantity.Location = new Point(20, 161);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(193, 21);
            lblQuantity.TabIndex = 14;
            lblQuantity.Text = "Quantidade em Estoque";
            // 
            // txtQuantity
            // 
            txtQuantity.BorderRadius = 8;
            txtQuantity.CustomizableEdges = customizableEdges13;
            txtQuantity.DefaultText = "";
            txtQuantity.FillColor = Color.FromArgb(37, 42, 64);
            txtQuantity.Font = new Font("Segoe UI", 9F);
            txtQuantity.Location = new Point(20, 185);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.PlaceholderText = "";
            txtQuantity.SelectedText = "";
            txtQuantity.ShadowDecoration.CustomizableEdges = customizableEdges14;
            txtQuantity.Size = new Size(200, 36);
            txtQuantity.TabIndex = 13;
            // 
            // btnSave
            // 
            btnSave.BorderRadius = 8;
            btnSave.CustomizableEdges = customizableEdges15;
            btnSave.FillColor = Color.FromArgb(0, 126, 249);
            btnSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(268, 390);
            btnSave.Name = "btnSave";
            btnSave.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnSave.Size = new Size(100, 40);
            btnSave.TabIndex = 12;
            btnSave.Text = "Salvar";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BorderRadius = 8;
            btnCancel.CustomizableEdges = customizableEdges17;
            btnCancel.FillColor = Color.FromArgb(108, 117, 125);
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(120, 390);
            btnCancel.Name = "btnCancel";
            btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges18;
            btnCancel.Size = new Size(100, 40);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Cancelar";
            btnCancel.Click += btnCancel_Click;
            // 
            // borderForm
            // 
            borderForm.BorderRadius = 15;
            borderForm.ContainerControl = this;
            borderForm.DockIndicatorTransparencyValue = 0.6D;
            borderForm.TransparentWhileDrag = true;
            // 
            // dragControl
            // 
            dragControl.DockIndicatorTransparencyValue = 0.6D;
            dragControl.TargetControl = pnlHeader;
            dragControl.UseTransparentDrag = true;
            // 
            // BookFormDialog
            // 
            BackColor = Color.FromArgb(46, 51, 73);
            ClientSize = new Size(480, 455);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(txtQuantity);
            Controls.Add(lblQuantity);
            Controls.Add(txtYear);
            Controls.Add(lblYear);
            Controls.Add(cmbGender);
            Controls.Add(lblGender);
            Controls.Add(txtAuthor);
            Controls.Add(lblAuthor);
            Controls.Add(txtTitle);
            Controls.Add(lblTitle);
            Controls.Add(pnlHeader);
            FormBorderStyle = FormBorderStyle.None;
            Name = "BookFormDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "BookFormDialog";
            Load += BookFormDialog_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private void ConfigureTextBox(Guna.UI2.WinForms.Guna2TextBox txt, string placeholder, int left, int top, int width)
        {
            txt.BorderRadius = 8;
            txt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(126)))), ((int)(((byte)(249)))));
            txt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(42)))), ((int)(((byte)(64)))));
            txt.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txt.ForeColor = System.Drawing.Color.White;
            txt.Location = new System.Drawing.Point(left, top);
            txt.Name = txt.Name;
            txt.PlaceholderForeColor = System.Drawing.Color.Gray;
            txt.PlaceholderText = placeholder;
            txt.Size = new System.Drawing.Size(width, 36);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private System.Windows.Forms.Label lblDialogTitle;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtTitle;
        private System.Windows.Forms.Label lblAuthor;
        private Guna.UI2.WinForms.Guna2TextBox txtAuthor;
        private System.Windows.Forms.Label lblGender;
        private Guna.UI2.WinForms.Guna2ComboBox cmbGender;
        private System.Windows.Forms.Label lblYear;
        private Guna.UI2.WinForms.Guna2TextBox txtYear;
        private System.Windows.Forms.Label lblQuantity;
        private Guna.UI2.WinForms.Guna2TextBox txtQuantity;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2BorderlessForm borderForm;
        private Guna.UI2.WinForms.Guna2DragControl dragControl;
    }
}