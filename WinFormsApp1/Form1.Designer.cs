namespace InvestigacionAI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtPrompt;
        private System.Windows.Forms.TextBox txtResultado;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblResultado;

        private void InitializeComponent()
        {
            txtPrompt = new TextBox();
            txtResultado = new TextBox();
            btnConsultar = new Button();
            btnGuardar = new Button();
            lblPrompt = new Label();
            lblResultado = new Label();
            SuspendLayout();
            // 
            // txtPrompt
            // 
            txtPrompt.Location = new Point(30, 60);
            txtPrompt.Margin = new Padding(3, 4, 3, 4);
            txtPrompt.Multiline = true;
            txtPrompt.Name = "txtPrompt";
            txtPrompt.Size = new Size(760, 60);
            txtPrompt.TabIndex = 0;
            // 
            // txtResultado
            // 
            txtResultado.Location = new Point(30, 230);
            txtResultado.Margin = new Padding(3, 4, 3, 4);
            txtResultado.Multiline = true;
            txtResultado.Name = "txtResultado";
            txtResultado.ReadOnly = true;
            txtResultado.ScrollBars = ScrollBars.Vertical;
            txtResultado.Size = new Size(760, 173);
            txtResultado.TabIndex = 3;
            txtResultado.TextChanged += txtResultado_TextChanged;
            // 
            // btnConsultar
            // 
            btnConsultar.Location = new Point(220, 140);
            btnConsultar.Margin = new Padding(3, 4, 3, 4);
            btnConsultar.Name = "btnConsultar";
            btnConsultar.Size = new Size(150, 40);
            btnConsultar.TabIndex = 1;
            btnConsultar.Text = "Consultar";
            btnConsultar.UseVisualStyleBackColor = true;
            btnConsultar.Click += btnConsultar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(430, 140);
            btnGuardar.Margin = new Padding(3, 4, 3, 4);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(150, 40);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // lblPrompt
            // 
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new Point(30, 30);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new Size(132, 20);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "Ingrese su prompt:";
            // 
            // lblResultado
            // 
            lblResultado.AutoSize = true;
            lblResultado.Location = new Point(30, 200);
            lblResultado.Name = "lblResultado";
            lblResultado.Size = new Size(78, 20);
            lblResultado.TabIndex = 3;
            lblResultado.Text = "Resultado:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 444);
            Controls.Add(lblPrompt);
            Controls.Add(txtPrompt);
            Controls.Add(btnConsultar);
            Controls.Add(btnGuardar);
            Controls.Add(lblResultado);
            Controls.Add(txtResultado);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Investigación AI";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
