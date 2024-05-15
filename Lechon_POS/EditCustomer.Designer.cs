namespace Lechon_POS
{
    partial class EditCustomer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCustomer));
            edit = new Guna.UI2.WinForms.Guna2Button();
            label1 = new Label();
            name = new Guna.UI2.WinForms.Guna2TextBox();
            SuspendLayout();
            // 
            // edit
            // 
            edit.Animated = true;
            edit.BorderColor = Color.FromArgb(251, 204, 27);
            edit.BorderRadius = 10;
            edit.BorderThickness = 1;
            edit.CustomizableEdges = customizableEdges5;
            edit.DisabledState.BorderColor = Color.DarkGray;
            edit.DisabledState.CustomBorderColor = Color.DarkGray;
            edit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            edit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            edit.FillColor = Color.FromArgb(251, 204, 27);
            edit.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            edit.ForeColor = Color.FromArgb(121, 84, 88);
            edit.HoverState.BorderColor = Color.FromArgb(251, 204, 27);
            edit.HoverState.FillColor = Color.FromArgb(250, 241, 214);
            edit.Location = new Point(107, 88);
            edit.Name = "edit";
            edit.ShadowDecoration.CustomizableEdges = customizableEdges6;
            edit.Size = new Size(128, 39);
            edit.TabIndex = 2;
            edit.Text = "Edit";
            edit.Click += edit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.FromArgb(250, 241, 214);
            label1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(121, 84, 88);
            label1.Location = new Point(110, 20);
            label1.Name = "label1";
            label1.Size = new Size(121, 15);
            label1.TabIndex = 20;
            label1.Text = "Customer's Name";
            label1.Click += label1_Click;
            // 
            // name
            // 
            name.BackColor = Color.FromArgb(250, 241, 214);
            name.BorderColor = Color.FromArgb(121, 84, 88);
            name.BorderRadius = 6;
            name.CustomizableEdges = customizableEdges7;
            name.DefaultText = "";
            name.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            name.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            name.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            name.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            name.FillColor = Color.FromArgb(250, 241, 214);
            name.FocusedState.BorderColor = Color.FromArgb(121, 84, 88);
            name.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point);
            name.ForeColor = Color.FromArgb(121, 84, 88);
            name.HoverState.BorderColor = Color.FromArgb(121, 84, 88);
            name.Location = new Point(40, 29);
            name.Margin = new Padding(4, 4, 4, 4);
            name.Name = "name";
            name.PasswordChar = '\0';
            name.PlaceholderForeColor = Color.FromArgb(121, 84, 88);
            name.PlaceholderText = "";
            name.SelectedText = "";
            name.ShadowDecoration.CustomizableEdges = customizableEdges8;
            name.Size = new Size(271, 37);
            name.TabIndex = 1;
            name.TextAlign = HorizontalAlignment.Center;
            name.TextChanged += kilo_amount_TextChanged;
            // 
            // EditCustomer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(250, 241, 214);
            ClientSize = new Size(349, 149);
            Controls.Add(label1);
            Controls.Add(name);
            Controls.Add(edit);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "EditCustomer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Edit Customer";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button edit;
        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox name;
    }
}