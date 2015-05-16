namespace IntegracionNumerica
{
     partial class Configuracion
     {
          /// <summary>
          /// Required designer variable.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary>
          /// Clean up any resources being used.
          /// </summary>
          /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
          protected override void Dispose( bool disposing )
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
               this.lblArchivoSalida = new System.Windows.Forms.Label();
               this.txtArchivoDeSalida = new System.Windows.Forms.TextBox();
               this.btnArchivoDeSalida = new System.Windows.Forms.Button();
               this.lblComponentes = new System.Windows.Forms.Label();
               this.lblLayouts = new System.Windows.Forms.Label();
               this.btnComponentes = new System.Windows.Forms.Button();
               this.btnLayout = new System.Windows.Forms.Button();
               this.txtComponentes = new System.Windows.Forms.TextBox();
               this.txtLayouts = new System.Windows.Forms.TextBox();
               this.btnCancelar = new System.Windows.Forms.Button();
               this.btnGuardar = new System.Windows.Forms.Button();
               this.SuspendLayout();
               // 
               // lblArchivoSalida
               // 
               this.lblArchivoSalida.AutoSize = true;
               this.lblArchivoSalida.Location = new System.Drawing.Point(13, 13);
               this.lblArchivoSalida.Name = "lblArchivoSalida";
               this.lblArchivoSalida.Size = new System.Drawing.Size(95, 13);
               this.lblArchivoSalida.TabIndex = 0;
               this.lblArchivoSalida.Text = "Archivos de Salida";
               // 
               // txtArchivoDeSalida
               // 
               this.txtArchivoDeSalida.Location = new System.Drawing.Point(115, 13);
               this.txtArchivoDeSalida.Name = "txtArchivoDeSalida";
               this.txtArchivoDeSalida.Size = new System.Drawing.Size(362, 20);
               this.txtArchivoDeSalida.TabIndex = 1;
               // 
               // btnArchivoDeSalida
               // 
               this.btnArchivoDeSalida.Location = new System.Drawing.Point(483, 12);
               this.btnArchivoDeSalida.Name = "btnArchivoDeSalida";
               this.btnArchivoDeSalida.Size = new System.Drawing.Size(33, 20);
               this.btnArchivoDeSalida.TabIndex = 2;
               this.btnArchivoDeSalida.Text = "...";
               this.btnArchivoDeSalida.UseVisualStyleBackColor = true;
               this.btnArchivoDeSalida.Click += new System.EventHandler(this.btnArchivoDeSalida_Click);
               // 
               // lblComponentes
               // 
               this.lblComponentes.AutoSize = true;
               this.lblComponentes.Location = new System.Drawing.Point(13, 40);
               this.lblComponentes.Name = "lblComponentes";
               this.lblComponentes.Size = new System.Drawing.Size(72, 13);
               this.lblComponentes.TabIndex = 3;
               this.lblComponentes.Text = "Componentes";
               // 
               // lblLayouts
               // 
               this.lblLayouts.AutoSize = true;
               this.lblLayouts.Location = new System.Drawing.Point(13, 66);
               this.lblLayouts.Name = "lblLayouts";
               this.lblLayouts.Size = new System.Drawing.Size(44, 13);
               this.lblLayouts.TabIndex = 4;
               this.lblLayouts.Text = "Layouts";
               // 
               // btnComponentes
               // 
               this.btnComponentes.Location = new System.Drawing.Point(483, 40);
               this.btnComponentes.Name = "btnComponentes";
               this.btnComponentes.Size = new System.Drawing.Size(33, 20);
               this.btnComponentes.TabIndex = 5;
               this.btnComponentes.Text = "...";
               this.btnComponentes.UseVisualStyleBackColor = true;
               this.btnComponentes.Click += new System.EventHandler(this.btnComponentes_Click);
               // 
               // btnLayout
               // 
               this.btnLayout.Location = new System.Drawing.Point(483, 66);
               this.btnLayout.Name = "btnLayout";
               this.btnLayout.Size = new System.Drawing.Size(33, 20);
               this.btnLayout.TabIndex = 6;
               this.btnLayout.Text = "...";
               this.btnLayout.UseVisualStyleBackColor = true;
               this.btnLayout.Click += new System.EventHandler(this.btnLayout_Click);
               // 
               // txtComponentes
               // 
               this.txtComponentes.Location = new System.Drawing.Point(115, 40);
               this.txtComponentes.Name = "txtComponentes";
               this.txtComponentes.Size = new System.Drawing.Size(362, 20);
               this.txtComponentes.TabIndex = 7;
               // 
               // txtLayouts
               // 
               this.txtLayouts.Location = new System.Drawing.Point(115, 66);
               this.txtLayouts.Name = "txtLayouts";
               this.txtLayouts.Size = new System.Drawing.Size(362, 20);
               this.txtLayouts.TabIndex = 8;
               // 
               // btnCancelar
               // 
               this.btnCancelar.Location = new System.Drawing.Point(441, 92);
               this.btnCancelar.Name = "btnCancelar";
               this.btnCancelar.Size = new System.Drawing.Size(75, 23);
               this.btnCancelar.TabIndex = 9;
               this.btnCancelar.Text = "Cancelar";
               this.btnCancelar.UseVisualStyleBackColor = true;
               this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
               // 
               // btnGuardar
               // 
               this.btnGuardar.Location = new System.Drawing.Point(360, 92);
               this.btnGuardar.Name = "btnGuardar";
               this.btnGuardar.Size = new System.Drawing.Size(75, 23);
               this.btnGuardar.TabIndex = 10;
               this.btnGuardar.Text = "Guardar";
               this.btnGuardar.UseVisualStyleBackColor = true;
               this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
               // 
               // Configuracion
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(528, 120);
               this.Controls.Add(this.btnGuardar);
               this.Controls.Add(this.btnCancelar);
               this.Controls.Add(this.txtLayouts);
               this.Controls.Add(this.txtComponentes);
               this.Controls.Add(this.btnLayout);
               this.Controls.Add(this.btnComponentes);
               this.Controls.Add(this.lblLayouts);
               this.Controls.Add(this.lblComponentes);
               this.Controls.Add(this.btnArchivoDeSalida);
               this.Controls.Add(this.txtArchivoDeSalida);
               this.Controls.Add(this.lblArchivoSalida);
               this.Name = "Configuracion";
               this.Text = "Configuracion";
               this.Load += new System.EventHandler(this.Configuracion_Load);
               this.ResumeLayout(false);
               this.PerformLayout();

          }

          #endregion

          private System.Windows.Forms.Label lblArchivoSalida;
          private System.Windows.Forms.TextBox txtArchivoDeSalida;
          private System.Windows.Forms.Button btnArchivoDeSalida;
          private System.Windows.Forms.Label lblComponentes;
          private System.Windows.Forms.Label lblLayouts;
          private System.Windows.Forms.Button btnComponentes;
          private System.Windows.Forms.Button btnLayout;
          private System.Windows.Forms.TextBox txtComponentes;
          private System.Windows.Forms.TextBox txtLayouts;
          private System.Windows.Forms.Button btnCancelar;
          private System.Windows.Forms.Button btnGuardar;
     }
}