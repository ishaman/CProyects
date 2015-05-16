namespace IntegracionNumerica
{
     partial class IntegracionNumerica
     {
          /// <summary>
          /// Variable del diseñador requerida.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary>
          /// Limpiar los recursos que se estén utilizando.
          /// </summary>
          /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
          protected override void Dispose( bool disposing )
          {
               if (disposing && (components != null))
               {
                    components.Dispose();
               }
               base.Dispose(disposing);
          }

          #region Código generado por el Diseñador de Windows Forms

          /// <summary>
          /// Método necesario para admitir el Diseñador. No se puede modificar
          /// el contenido del método con el editor de código.
          /// </summary>
          private void InitializeComponent()
          {
               this.btnConfiguracion = new System.Windows.Forms.Button();
               this.gpbFuncion = new System.Windows.Forms.GroupBox();
               this.groupBox22 = new System.Windows.Forms.GroupBox();
               this.lblB = new System.Windows.Forms.Label();
               this.lblA = new System.Windows.Forms.Label();
               this.txtB = new System.Windows.Forms.TextBox();
               this.txtA = new System.Windows.Forms.TextBox();
               this.groupBox21 = new System.Windows.Forms.GroupBox();
               this.txtNumNodos = new System.Windows.Forms.TextBox();
               this.btnIniciar = new System.Windows.Forms.Button();
               this.btnFFC = new System.Windows.Forms.Button();
               this.groupBox20 = new System.Windows.Forms.GroupBox();
               this.txtFuncion = new System.Windows.Forms.TextBox();
               this.btnGraficar = new System.Windows.Forms.Button();
               this.cmbFuncionesconSngularidad = new System.Windows.Forms.ComboBox();
               this.cmbFunOscilantes = new System.Windows.Forms.ComboBox();
               this.cmbFuncionesPrueba = new System.Windows.Forms.ComboBox();
               this.gpbALgoritmos = new System.Windows.Forms.GroupBox();
               this.cmbAlgoritmosRNAS = new System.Windows.Forms.ComboBox();
               this.groupBox1 = new System.Windows.Forms.GroupBox();
               this.groupBox6 = new System.Windows.Forms.GroupBox();
               this.txtSipsonAdaptivo = new System.Windows.Forms.TextBox();
               this.rbtSimpsonAdaptivo = new System.Windows.Forms.RadioButton();
               this.rbtIntegralReal = new System.Windows.Forms.RadioButton();
               this.groupBox5 = new System.Windows.Forms.GroupBox();
               this.txtIntegralRealFuncion = new System.Windows.Forms.TextBox();
               this.groupBox2 = new System.Windows.Forms.GroupBox();
               this.rbtNumEntrenamientos = new System.Windows.Forms.RadioButton();
               this.rbtTolFuncion = new System.Windows.Forms.RadioButton();
               this.rbtTolIntegral = new System.Windows.Forms.RadioButton();
               this.groupBox8 = new System.Windows.Forms.GroupBox();
               this.txtToleranciaFuncion = new System.Windows.Forms.TextBox();
               this.groupBox9 = new System.Windows.Forms.GroupBox();
               this.txtNumEntrenamientos = new System.Windows.Forms.TextBox();
               this.groupBox7 = new System.Windows.Forms.GroupBox();
               this.txtToleranciaIntegral = new System.Windows.Forms.TextBox();
               this.groupBox3 = new System.Windows.Forms.GroupBox();
               this.groupBox14 = new System.Windows.Forms.GroupBox();
               this.txtTiempoEjecucion = new System.Windows.Forms.TextBox();
               this.groupBox13 = new System.Windows.Forms.GroupBox();
               this.txtNumEntrenAlgoritmo = new System.Windows.Forms.TextBox();
               this.groupBox12 = new System.Windows.Forms.GroupBox();
               this.txtErroFuncion = new System.Windows.Forms.TextBox();
               this.groupBox11 = new System.Windows.Forms.GroupBox();
               this.txtErroIntegral = new System.Windows.Forms.TextBox();
               this.groupBox10 = new System.Windows.Forms.GroupBox();
               this.txtValorIntegral = new System.Windows.Forms.TextBox();
               this.btnSalir = new System.Windows.Forms.Button();
               this.groupBox4 = new System.Windows.Forms.GroupBox();
               this.cmbOtrosMetodos = new System.Windows.Forms.ComboBox();
               this.groupBox15 = new System.Windows.Forms.GroupBox();
               this.groupBox16 = new System.Windows.Forms.GroupBox();
               this.groupBox19 = new System.Windows.Forms.GroupBox();
               this.groupBox18 = new System.Windows.Forms.GroupBox();
               this.groupBox17 = new System.Windows.Forms.GroupBox();
               this.prBProgreso = new System.Windows.Forms.ProgressBar();
               this.gpbFuncion.SuspendLayout();
               this.groupBox22.SuspendLayout();
               this.groupBox21.SuspendLayout();
               this.groupBox20.SuspendLayout();
               this.gpbALgoritmos.SuspendLayout();
               this.groupBox1.SuspendLayout();
               this.groupBox6.SuspendLayout();
               this.groupBox5.SuspendLayout();
               this.groupBox2.SuspendLayout();
               this.groupBox8.SuspendLayout();
               this.groupBox9.SuspendLayout();
               this.groupBox7.SuspendLayout();
               this.groupBox3.SuspendLayout();
               this.groupBox14.SuspendLayout();
               this.groupBox13.SuspendLayout();
               this.groupBox12.SuspendLayout();
               this.groupBox11.SuspendLayout();
               this.groupBox10.SuspendLayout();
               this.groupBox4.SuspendLayout();
               this.groupBox15.SuspendLayout();
               this.groupBox16.SuspendLayout();
               this.groupBox19.SuspendLayout();
               this.groupBox18.SuspendLayout();
               this.groupBox17.SuspendLayout();
               this.SuspendLayout();
               // 
               // btnConfiguracion
               // 
               this.btnConfiguracion.Location = new System.Drawing.Point(181, 133);
               this.btnConfiguracion.Name = "btnConfiguracion";
               this.btnConfiguracion.Size = new System.Drawing.Size(75, 23);
               this.btnConfiguracion.TabIndex = 0;
               this.btnConfiguracion.Text = "Configurar";
               this.btnConfiguracion.UseVisualStyleBackColor = true;
               this.btnConfiguracion.Click += new System.EventHandler(this.btnConfiguracion_Click);
               // 
               // gpbFuncion
               // 
               this.gpbFuncion.Controls.Add(this.groupBox22);
               this.gpbFuncion.Controls.Add(this.groupBox21);
               this.gpbFuncion.Controls.Add(this.btnIniciar);
               this.gpbFuncion.Controls.Add(this.btnFFC);
               this.gpbFuncion.Controls.Add(this.groupBox20);
               this.gpbFuncion.Controls.Add(this.btnGraficar);
               this.gpbFuncion.Controls.Add(this.btnConfiguracion);
               this.gpbFuncion.Location = new System.Drawing.Point(12, 0);
               this.gpbFuncion.Name = "gpbFuncion";
               this.gpbFuncion.Size = new System.Drawing.Size(262, 212);
               this.gpbFuncion.TabIndex = 2;
               this.gpbFuncion.TabStop = false;
               this.gpbFuncion.Text = "Datos de la Función";
               // 
               // groupBox22
               // 
               this.groupBox22.Controls.Add(this.lblB);
               this.groupBox22.Controls.Add(this.lblA);
               this.groupBox22.Controls.Add(this.txtB);
               this.groupBox22.Controls.Add(this.txtA);
               this.groupBox22.Location = new System.Drawing.Point(7, 127);
               this.groupBox22.Name = "groupBox22";
               this.groupBox22.Size = new System.Drawing.Size(162, 75);
               this.groupBox22.TabIndex = 13;
               this.groupBox22.TabStop = false;
               this.groupBox22.Text = "Intervalos de integración";
               // 
               // lblB
               // 
               this.lblB.AutoSize = true;
               this.lblB.Location = new System.Drawing.Point(6, 46);
               this.lblB.Name = "lblB";
               this.lblB.Size = new System.Drawing.Size(14, 13);
               this.lblB.TabIndex = 3;
               this.lblB.Text = "B";
               // 
               // lblA
               // 
               this.lblA.AutoSize = true;
               this.lblA.Location = new System.Drawing.Point(6, 23);
               this.lblA.Name = "lblA";
               this.lblA.Size = new System.Drawing.Size(14, 13);
               this.lblA.TabIndex = 2;
               this.lblA.Text = "A";
               // 
               // txtB
               // 
               this.txtB.Location = new System.Drawing.Point(26, 46);
               this.txtB.Name = "txtB";
               this.txtB.Size = new System.Drawing.Size(130, 20);
               this.txtB.TabIndex = 1;
               // 
               // txtA
               // 
               this.txtA.Location = new System.Drawing.Point(26, 20);
               this.txtA.Name = "txtA";
               this.txtA.Size = new System.Drawing.Size(130, 20);
               this.txtA.TabIndex = 0;
               // 
               // groupBox21
               // 
               this.groupBox21.Controls.Add(this.txtNumNodos);
               this.groupBox21.Location = new System.Drawing.Point(6, 68);
               this.groupBox21.Name = "groupBox21";
               this.groupBox21.Size = new System.Drawing.Size(163, 43);
               this.groupBox21.TabIndex = 12;
               this.groupBox21.TabStop = false;
               this.groupBox21.Text = "Número de Nodos";
               // 
               // txtNumNodos
               // 
               this.txtNumNodos.Location = new System.Drawing.Point(6, 19);
               this.txtNumNodos.Name = "txtNumNodos";
               this.txtNumNodos.Size = new System.Drawing.Size(151, 20);
               this.txtNumNodos.TabIndex = 9;
               // 
               // btnIniciar
               // 
               this.btnIniciar.Location = new System.Drawing.Point(181, 162);
               this.btnIniciar.Name = "btnIniciar";
               this.btnIniciar.Size = new System.Drawing.Size(75, 23);
               this.btnIniciar.TabIndex = 8;
               this.btnIniciar.Text = "Iniciar";
               this.btnIniciar.UseVisualStyleBackColor = true;
               this.btnIniciar.Click += new System.EventHandler(this.cmdIniciar_Click);
               // 
               // btnFFC
               // 
               this.btnFFC.Location = new System.Drawing.Point(181, 104);
               this.btnFFC.Name = "btnFFC";
               this.btnFFC.Size = new System.Drawing.Size(75, 23);
               this.btnFFC.TabIndex = 10;
               this.btnFFC.Text = "FFC";
               this.btnFFC.UseVisualStyleBackColor = true;
               this.btnFFC.Click += new System.EventHandler(this.btnFFC_Click);
               // 
               // groupBox20
               // 
               this.groupBox20.Controls.Add(this.txtFuncion);
               this.groupBox20.Location = new System.Drawing.Point(8, 19);
               this.groupBox20.Name = "groupBox20";
               this.groupBox20.Size = new System.Drawing.Size(246, 43);
               this.groupBox20.TabIndex = 11;
               this.groupBox20.TabStop = false;
               this.groupBox20.Text = "Función";
               // 
               // txtFuncion
               // 
               this.txtFuncion.Location = new System.Drawing.Point(6, 19);
               this.txtFuncion.Name = "txtFuncion";
               this.txtFuncion.Size = new System.Drawing.Size(234, 20);
               this.txtFuncion.TabIndex = 2;
               // 
               // btnGraficar
               // 
               this.btnGraficar.Location = new System.Drawing.Point(181, 75);
               this.btnGraficar.Name = "btnGraficar";
               this.btnGraficar.Size = new System.Drawing.Size(75, 23);
               this.btnGraficar.TabIndex = 10;
               this.btnGraficar.Text = "Graficar";
               this.btnGraficar.UseVisualStyleBackColor = true;
               this.btnGraficar.Click += new System.EventHandler(this.btnGraficar_Click);
               // 
               // cmbFuncionesconSngularidad
               // 
               this.cmbFuncionesconSngularidad.FormattingEnabled = true;
               this.cmbFuncionesconSngularidad.Location = new System.Drawing.Point(10, 16);
               this.cmbFuncionesconSngularidad.Name = "cmbFuncionesconSngularidad";
               this.cmbFuncionesconSngularidad.Size = new System.Drawing.Size(114, 21);
               this.cmbFuncionesconSngularidad.TabIndex = 7;
               this.cmbFuncionesconSngularidad.SelectedIndexChanged += new System.EventHandler(this.cmbFuncionesconSngularidad_SelectedIndexChanged);
               this.cmbFuncionesconSngularidad.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbFuncionesconSngularidad_MouseClick);
               // 
               // cmbFunOscilantes
               // 
               this.cmbFunOscilantes.FormattingEnabled = true;
               this.cmbFunOscilantes.Location = new System.Drawing.Point(9, 13);
               this.cmbFunOscilantes.Name = "cmbFunOscilantes";
               this.cmbFunOscilantes.Size = new System.Drawing.Size(115, 21);
               this.cmbFunOscilantes.TabIndex = 6;
               this.cmbFunOscilantes.SelectedIndexChanged += new System.EventHandler(this.cmbFunOscilantes_SelectedIndexChanged);
               this.cmbFunOscilantes.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbFunOscilantes_MouseClick);
               // 
               // cmbFuncionesPrueba
               // 
               this.cmbFuncionesPrueba.FormattingEnabled = true;
               this.cmbFuncionesPrueba.Location = new System.Drawing.Point(9, 16);
               this.cmbFuncionesPrueba.Name = "cmbFuncionesPrueba";
               this.cmbFuncionesPrueba.Size = new System.Drawing.Size(115, 21);
               this.cmbFuncionesPrueba.TabIndex = 0;
               this.cmbFuncionesPrueba.SelectedIndexChanged += new System.EventHandler(this.cmbFuncionesPrueba_SelectedIndexChanged);
               this.cmbFuncionesPrueba.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbFuncionesPrueba_MouseClick);
               // 
               // gpbALgoritmos
               // 
               this.gpbALgoritmos.Controls.Add(this.cmbAlgoritmosRNAS);
               this.gpbALgoritmos.Location = new System.Drawing.Point(15, 19);
               this.gpbALgoritmos.Name = "gpbALgoritmos";
               this.gpbALgoritmos.Size = new System.Drawing.Size(176, 47);
               this.gpbALgoritmos.TabIndex = 3;
               this.gpbALgoritmos.TabStop = false;
               this.gpbALgoritmos.Text = "Algoritmos de RNAS";
               // 
               // cmbAlgoritmosRNAS
               // 
               this.cmbAlgoritmosRNAS.FormattingEnabled = true;
               this.cmbAlgoritmosRNAS.Location = new System.Drawing.Point(6, 18);
               this.cmbAlgoritmosRNAS.Name = "cmbAlgoritmosRNAS";
               this.cmbAlgoritmosRNAS.Size = new System.Drawing.Size(160, 21);
               this.cmbAlgoritmosRNAS.TabIndex = 0;
               this.cmbAlgoritmosRNAS.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbAlgoritmosRNAS_MouseClick);
               // 
               // groupBox1
               // 
               this.groupBox1.Controls.Add(this.groupBox6);
               this.groupBox1.Controls.Add(this.rbtSimpsonAdaptivo);
               this.groupBox1.Controls.Add(this.rbtIntegralReal);
               this.groupBox1.Controls.Add(this.groupBox5);
               this.groupBox1.Location = new System.Drawing.Point(313, 373);
               this.groupBox1.Name = "groupBox1";
               this.groupBox1.Size = new System.Drawing.Size(321, 122);
               this.groupBox1.TabIndex = 4;
               this.groupBox1.TabStop = false;
               this.groupBox1.Text = "Datos para el Calculo del Error a la Integral";
               // 
               // groupBox6
               // 
               this.groupBox6.Controls.Add(this.txtSipsonAdaptivo);
               this.groupBox6.Location = new System.Drawing.Point(31, 70);
               this.groupBox6.Name = "groupBox6";
               this.groupBox6.Size = new System.Drawing.Size(284, 46);
               this.groupBox6.TabIndex = 1;
               this.groupBox6.TabStop = false;
               this.groupBox6.Text = "Calcular con Sipson Adaptivo";
               // 
               // txtSipsonAdaptivo
               // 
               this.txtSipsonAdaptivo.Location = new System.Drawing.Point(6, 20);
               this.txtSipsonAdaptivo.Name = "txtSipsonAdaptivo";
               this.txtSipsonAdaptivo.Size = new System.Drawing.Size(272, 20);
               this.txtSipsonAdaptivo.TabIndex = 1;
               // 
               // rbtSimpsonAdaptivo
               // 
               this.rbtSimpsonAdaptivo.AutoSize = true;
               this.rbtSimpsonAdaptivo.Location = new System.Drawing.Point(11, 90);
               this.rbtSimpsonAdaptivo.Name = "rbtSimpsonAdaptivo";
               this.rbtSimpsonAdaptivo.Size = new System.Drawing.Size(14, 13);
               this.rbtSimpsonAdaptivo.TabIndex = 0;
               this.rbtSimpsonAdaptivo.UseVisualStyleBackColor = true;
               // 
               // rbtIntegralReal
               // 
               this.rbtIntegralReal.AutoSize = true;
               this.rbtIntegralReal.Checked = true;
               this.rbtIntegralReal.Location = new System.Drawing.Point(11, 38);
               this.rbtIntegralReal.Name = "rbtIntegralReal";
               this.rbtIntegralReal.Size = new System.Drawing.Size(14, 13);
               this.rbtIntegralReal.TabIndex = 0;
               this.rbtIntegralReal.TabStop = true;
               this.rbtIntegralReal.UseVisualStyleBackColor = true;
               // 
               // groupBox5
               // 
               this.groupBox5.Controls.Add(this.txtIntegralRealFuncion);
               this.groupBox5.Location = new System.Drawing.Point(31, 20);
               this.groupBox5.Name = "groupBox5";
               this.groupBox5.Size = new System.Drawing.Size(284, 46);
               this.groupBox5.TabIndex = 0;
               this.groupBox5.TabStop = false;
               this.groupBox5.Text = "Integral Real de la Función";
               // 
               // txtIntegralRealFuncion
               // 
               this.txtIntegralRealFuncion.Location = new System.Drawing.Point(6, 18);
               this.txtIntegralRealFuncion.Name = "txtIntegralRealFuncion";
               this.txtIntegralRealFuncion.Size = new System.Drawing.Size(272, 20);
               this.txtIntegralRealFuncion.TabIndex = 1;
               // 
               // groupBox2
               // 
               this.groupBox2.Controls.Add(this.rbtNumEntrenamientos);
               this.groupBox2.Controls.Add(this.rbtTolFuncion);
               this.groupBox2.Controls.Add(this.rbtTolIntegral);
               this.groupBox2.Controls.Add(this.groupBox8);
               this.groupBox2.Controls.Add(this.groupBox9);
               this.groupBox2.Controls.Add(this.groupBox7);
               this.groupBox2.Location = new System.Drawing.Point(313, 191);
               this.groupBox2.Name = "groupBox2";
               this.groupBox2.Size = new System.Drawing.Size(321, 177);
               this.groupBox2.TabIndex = 5;
               this.groupBox2.TabStop = false;
               this.groupBox2.Text = "Datos para el calculo de la integral";
               // 
               // rbtNumEntrenamientos
               // 
               this.rbtNumEntrenamientos.AutoSize = true;
               this.rbtNumEntrenamientos.Location = new System.Drawing.Point(7, 142);
               this.rbtNumEntrenamientos.Name = "rbtNumEntrenamientos";
               this.rbtNumEntrenamientos.Size = new System.Drawing.Size(14, 13);
               this.rbtNumEntrenamientos.TabIndex = 14;
               this.rbtNumEntrenamientos.UseVisualStyleBackColor = true;
               // 
               // rbtTolFuncion
               // 
               this.rbtTolFuncion.AutoSize = true;
               this.rbtTolFuncion.Checked = true;
               this.rbtTolFuncion.Location = new System.Drawing.Point(8, 92);
               this.rbtTolFuncion.Name = "rbtTolFuncion";
               this.rbtTolFuncion.Size = new System.Drawing.Size(14, 13);
               this.rbtTolFuncion.TabIndex = 13;
               this.rbtTolFuncion.TabStop = true;
               this.rbtTolFuncion.UseVisualStyleBackColor = true;
               // 
               // rbtTolIntegral
               // 
               this.rbtTolIntegral.AutoSize = true;
               this.rbtTolIntegral.Location = new System.Drawing.Point(11, 38);
               this.rbtTolIntegral.Name = "rbtTolIntegral";
               this.rbtTolIntegral.Size = new System.Drawing.Size(14, 13);
               this.rbtTolIntegral.TabIndex = 12;
               this.rbtTolIntegral.UseVisualStyleBackColor = true;
               // 
               // groupBox8
               // 
               this.groupBox8.Controls.Add(this.txtToleranciaFuncion);
               this.groupBox8.Location = new System.Drawing.Point(31, 70);
               this.groupBox8.Name = "groupBox8";
               this.groupBox8.Size = new System.Drawing.Size(284, 45);
               this.groupBox8.TabIndex = 11;
               this.groupBox8.TabStop = false;
               this.groupBox8.Text = "Tolerancia en la aproximación a la función";
               // 
               // txtToleranciaFuncion
               // 
               this.txtToleranciaFuncion.Location = new System.Drawing.Point(7, 20);
               this.txtToleranciaFuncion.Name = "txtToleranciaFuncion";
               this.txtToleranciaFuncion.Size = new System.Drawing.Size(271, 20);
               this.txtToleranciaFuncion.TabIndex = 0;
               // 
               // groupBox9
               // 
               this.groupBox9.Controls.Add(this.txtNumEntrenamientos);
               this.groupBox9.Location = new System.Drawing.Point(31, 121);
               this.groupBox9.Name = "groupBox9";
               this.groupBox9.Size = new System.Drawing.Size(284, 45);
               this.groupBox9.TabIndex = 11;
               this.groupBox9.TabStop = false;
               this.groupBox9.Text = "Número de entrenamientos para algorirmos RNAS";
               // 
               // txtNumEntrenamientos
               // 
               this.txtNumEntrenamientos.Location = new System.Drawing.Point(7, 18);
               this.txtNumEntrenamientos.Name = "txtNumEntrenamientos";
               this.txtNumEntrenamientos.Size = new System.Drawing.Size(271, 20);
               this.txtNumEntrenamientos.TabIndex = 0;
               // 
               // groupBox7
               // 
               this.groupBox7.Controls.Add(this.txtToleranciaIntegral);
               this.groupBox7.Location = new System.Drawing.Point(31, 19);
               this.groupBox7.Name = "groupBox7";
               this.groupBox7.Size = new System.Drawing.Size(284, 45);
               this.groupBox7.TabIndex = 10;
               this.groupBox7.TabStop = false;
               this.groupBox7.Text = "Tolerancia en la aproximacion de la integral";
               // 
               // txtToleranciaIntegral
               // 
               this.txtToleranciaIntegral.Location = new System.Drawing.Point(7, 20);
               this.txtToleranciaIntegral.Name = "txtToleranciaIntegral";
               this.txtToleranciaIntegral.Size = new System.Drawing.Size(271, 20);
               this.txtToleranciaIntegral.TabIndex = 0;
               // 
               // groupBox3
               // 
               this.groupBox3.Controls.Add(this.groupBox14);
               this.groupBox3.Controls.Add(this.groupBox13);
               this.groupBox3.Controls.Add(this.groupBox12);
               this.groupBox3.Controls.Add(this.groupBox11);
               this.groupBox3.Controls.Add(this.groupBox10);
               this.groupBox3.Location = new System.Drawing.Point(12, 212);
               this.groupBox3.Name = "groupBox3";
               this.groupBox3.Size = new System.Drawing.Size(289, 283);
               this.groupBox3.TabIndex = 6;
               this.groupBox3.TabStop = false;
               this.groupBox3.Text = "Resultados";
               // 
               // groupBox14
               // 
               this.groupBox14.Controls.Add(this.txtTiempoEjecucion);
               this.groupBox14.Location = new System.Drawing.Point(6, 230);
               this.groupBox14.Name = "groupBox14";
               this.groupBox14.Size = new System.Drawing.Size(274, 45);
               this.groupBox14.TabIndex = 15;
               this.groupBox14.TabStop = false;
               this.groupBox14.Text = "Tiempo de ejecución del algoritmo";
               // 
               // txtTiempoEjecucion
               // 
               this.txtTiempoEjecucion.Location = new System.Drawing.Point(6, 19);
               this.txtTiempoEjecucion.Name = "txtTiempoEjecucion";
               this.txtTiempoEjecucion.Size = new System.Drawing.Size(258, 20);
               this.txtTiempoEjecucion.TabIndex = 0;
               // 
               // groupBox13
               // 
               this.groupBox13.Controls.Add(this.txtNumEntrenAlgoritmo);
               this.groupBox13.Location = new System.Drawing.Point(6, 179);
               this.groupBox13.Name = "groupBox13";
               this.groupBox13.Size = new System.Drawing.Size(274, 45);
               this.groupBox13.TabIndex = 15;
               this.groupBox13.TabStop = false;
               this.groupBox13.Text = "Número de entrenamientos";
               // 
               // txtNumEntrenAlgoritmo
               // 
               this.txtNumEntrenAlgoritmo.Location = new System.Drawing.Point(6, 19);
               this.txtNumEntrenAlgoritmo.Name = "txtNumEntrenAlgoritmo";
               this.txtNumEntrenAlgoritmo.Size = new System.Drawing.Size(258, 20);
               this.txtNumEntrenAlgoritmo.TabIndex = 0;
               // 
               // groupBox12
               // 
               this.groupBox12.Controls.Add(this.txtErroFuncion);
               this.groupBox12.Location = new System.Drawing.Point(6, 128);
               this.groupBox12.Name = "groupBox12";
               this.groupBox12.Size = new System.Drawing.Size(274, 45);
               this.groupBox12.TabIndex = 15;
               this.groupBox12.TabStop = false;
               this.groupBox12.Text = "Error en la aproximación a la funcón";
               // 
               // txtErroFuncion
               // 
               this.txtErroFuncion.Location = new System.Drawing.Point(6, 19);
               this.txtErroFuncion.Name = "txtErroFuncion";
               this.txtErroFuncion.Size = new System.Drawing.Size(258, 20);
               this.txtErroFuncion.TabIndex = 0;
               // 
               // groupBox11
               // 
               this.groupBox11.Controls.Add(this.txtErroIntegral);
               this.groupBox11.Location = new System.Drawing.Point(6, 72);
               this.groupBox11.Name = "groupBox11";
               this.groupBox11.Size = new System.Drawing.Size(274, 45);
               this.groupBox11.TabIndex = 15;
               this.groupBox11.TabStop = false;
               this.groupBox11.Text = "Error en la aproximación a la integral";
               // 
               // txtErroIntegral
               // 
               this.txtErroIntegral.Location = new System.Drawing.Point(6, 19);
               this.txtErroIntegral.Name = "txtErroIntegral";
               this.txtErroIntegral.Size = new System.Drawing.Size(258, 20);
               this.txtErroIntegral.TabIndex = 0;
               // 
               // groupBox10
               // 
               this.groupBox10.Controls.Add(this.txtValorIntegral);
               this.groupBox10.Location = new System.Drawing.Point(6, 21);
               this.groupBox10.Name = "groupBox10";
               this.groupBox10.Size = new System.Drawing.Size(274, 45);
               this.groupBox10.TabIndex = 15;
               this.groupBox10.TabStop = false;
               this.groupBox10.Text = "Integral de la función";
               // 
               // txtValorIntegral
               // 
               this.txtValorIntegral.Location = new System.Drawing.Point(6, 19);
               this.txtValorIntegral.Name = "txtValorIntegral";
               this.txtValorIntegral.Size = new System.Drawing.Size(258, 20);
               this.txtValorIntegral.TabIndex = 0;
               // 
               // btnSalir
               // 
               this.btnSalir.Location = new System.Drawing.Point(99, 145);
               this.btnSalir.Name = "btnSalir";
               this.btnSalir.Size = new System.Drawing.Size(92, 23);
               this.btnSalir.TabIndex = 7;
               this.btnSalir.Text = "Salir";
               this.btnSalir.UseVisualStyleBackColor = true;
               this.btnSalir.Click += new System.EventHandler(this.cmdSalir_Click);
               // 
               // groupBox4
               // 
               this.groupBox4.Controls.Add(this.cmbOtrosMetodos);
               this.groupBox4.Location = new System.Drawing.Point(15, 72);
               this.groupBox4.Name = "groupBox4";
               this.groupBox4.Size = new System.Drawing.Size(176, 46);
               this.groupBox4.TabIndex = 9;
               this.groupBox4.TabStop = false;
               this.groupBox4.Text = "Otros Métodos";
               // 
               // cmbOtrosMetodos
               // 
               this.cmbOtrosMetodos.FormattingEnabled = true;
               this.cmbOtrosMetodos.Location = new System.Drawing.Point(6, 18);
               this.cmbOtrosMetodos.Name = "cmbOtrosMetodos";
               this.cmbOtrosMetodos.Size = new System.Drawing.Size(159, 21);
               this.cmbOtrosMetodos.TabIndex = 0;
               this.cmbOtrosMetodos.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbOtrosMetodos_MouseClick);
               // 
               // groupBox15
               // 
               this.groupBox15.Controls.Add(this.groupBox4);
               this.groupBox15.Controls.Add(this.gpbALgoritmos);
               this.groupBox15.Controls.Add(this.btnSalir);
               this.groupBox15.Location = new System.Drawing.Point(438, 8);
               this.groupBox15.Name = "groupBox15";
               this.groupBox15.Size = new System.Drawing.Size(200, 177);
               this.groupBox15.TabIndex = 11;
               this.groupBox15.TabStop = false;
               this.groupBox15.Text = "Métodos de Integración Numérica";
               // 
               // groupBox16
               // 
               this.groupBox16.Controls.Add(this.groupBox19);
               this.groupBox16.Controls.Add(this.groupBox18);
               this.groupBox16.Controls.Add(this.groupBox17);
               this.groupBox16.Location = new System.Drawing.Point(284, 8);
               this.groupBox16.Name = "groupBox16";
               this.groupBox16.Size = new System.Drawing.Size(148, 177);
               this.groupBox16.TabIndex = 12;
               this.groupBox16.TabStop = false;
               this.groupBox16.Text = "Funciones de Ejemplo";
               // 
               // groupBox19
               // 
               this.groupBox19.Controls.Add(this.cmbFuncionesconSngularidad);
               this.groupBox19.Location = new System.Drawing.Point(7, 129);
               this.groupBox19.Name = "groupBox19";
               this.groupBox19.Size = new System.Drawing.Size(134, 42);
               this.groupBox19.TabIndex = 2;
               this.groupBox19.TabStop = false;
               this.groupBox19.Text = "Singularidad";
               // 
               // groupBox18
               // 
               this.groupBox18.Controls.Add(this.cmbFunOscilantes);
               this.groupBox18.Location = new System.Drawing.Point(7, 79);
               this.groupBox18.Name = "groupBox18";
               this.groupBox18.Size = new System.Drawing.Size(134, 39);
               this.groupBox18.TabIndex = 1;
               this.groupBox18.TabStop = false;
               this.groupBox18.Text = "Oscilantes";
               // 
               // groupBox17
               // 
               this.groupBox17.Controls.Add(this.cmbFuncionesPrueba);
               this.groupBox17.Location = new System.Drawing.Point(7, 20);
               this.groupBox17.Name = "groupBox17";
               this.groupBox17.Size = new System.Drawing.Size(134, 43);
               this.groupBox17.TabIndex = 0;
               this.groupBox17.TabStop = false;
               this.groupBox17.Text = "Simples";
               // 
               // prBProgreso
               // 
               this.prBProgreso.Location = new System.Drawing.Point(12, 501);
               this.prBProgreso.Name = "prBProgreso";
               this.prBProgreso.Size = new System.Drawing.Size(622, 23);
               this.prBProgreso.TabIndex = 13;
               // 
               // IntegracionNumerica
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(646, 537);
               this.Controls.Add(this.prBProgreso);
               this.Controls.Add(this.groupBox15);
               this.Controls.Add(this.groupBox16);
               this.Controls.Add(this.groupBox3);
               this.Controls.Add(this.groupBox2);
               this.Controls.Add(this.groupBox1);
               this.Controls.Add(this.gpbFuncion);
               this.Name = "IntegracionNumerica";
               this.Text = "Integracion Numerica";
               this.Load += new System.EventHandler(this.IntegracionNumerica_Load);
               this.gpbFuncion.ResumeLayout(false);
               this.groupBox22.ResumeLayout(false);
               this.groupBox22.PerformLayout();
               this.groupBox21.ResumeLayout(false);
               this.groupBox21.PerformLayout();
               this.groupBox20.ResumeLayout(false);
               this.groupBox20.PerformLayout();
               this.gpbALgoritmos.ResumeLayout(false);
               this.groupBox1.ResumeLayout(false);
               this.groupBox1.PerformLayout();
               this.groupBox6.ResumeLayout(false);
               this.groupBox6.PerformLayout();
               this.groupBox5.ResumeLayout(false);
               this.groupBox5.PerformLayout();
               this.groupBox2.ResumeLayout(false);
               this.groupBox2.PerformLayout();
               this.groupBox8.ResumeLayout(false);
               this.groupBox8.PerformLayout();
               this.groupBox9.ResumeLayout(false);
               this.groupBox9.PerformLayout();
               this.groupBox7.ResumeLayout(false);
               this.groupBox7.PerformLayout();
               this.groupBox3.ResumeLayout(false);
               this.groupBox14.ResumeLayout(false);
               this.groupBox14.PerformLayout();
               this.groupBox13.ResumeLayout(false);
               this.groupBox13.PerformLayout();
               this.groupBox12.ResumeLayout(false);
               this.groupBox12.PerformLayout();
               this.groupBox11.ResumeLayout(false);
               this.groupBox11.PerformLayout();
               this.groupBox10.ResumeLayout(false);
               this.groupBox10.PerformLayout();
               this.groupBox4.ResumeLayout(false);
               this.groupBox15.ResumeLayout(false);
               this.groupBox16.ResumeLayout(false);
               this.groupBox19.ResumeLayout(false);
               this.groupBox18.ResumeLayout(false);
               this.groupBox17.ResumeLayout(false);
               this.ResumeLayout(false);

          }

          #endregion

          private System.Windows.Forms.Button btnConfiguracion;
          private System.Windows.Forms.GroupBox gpbFuncion;
          private System.Windows.Forms.ComboBox cmbFuncionesPrueba;
          private System.Windows.Forms.GroupBox gpbALgoritmos;
          private System.Windows.Forms.ComboBox cmbAlgoritmosRNAS;
          private System.Windows.Forms.GroupBox groupBox1;
          private System.Windows.Forms.GroupBox groupBox2;
          private System.Windows.Forms.GroupBox groupBox3;
          private System.Windows.Forms.TextBox txtFuncion;
          private System.Windows.Forms.ComboBox cmbFuncionesconSngularidad;
          private System.Windows.Forms.ComboBox cmbFunOscilantes;
          private System.Windows.Forms.Button btnSalir;
          private System.Windows.Forms.Button btnIniciar;
          private System.Windows.Forms.Button btnGraficar;
          private System.Windows.Forms.TextBox txtNumNodos;
          private System.Windows.Forms.GroupBox groupBox4;
          private System.Windows.Forms.ComboBox cmbOtrosMetodos;
          private System.Windows.Forms.GroupBox groupBox5;
          private System.Windows.Forms.TextBox txtIntegralRealFuncion;
          private System.Windows.Forms.RadioButton rbtIntegralReal;
          private System.Windows.Forms.GroupBox groupBox6;
          private System.Windows.Forms.TextBox txtSipsonAdaptivo;
          private System.Windows.Forms.RadioButton rbtSimpsonAdaptivo;
          private System.Windows.Forms.RadioButton rbtNumEntrenamientos;
          private System.Windows.Forms.RadioButton rbtTolFuncion;
          private System.Windows.Forms.RadioButton rbtTolIntegral;
          private System.Windows.Forms.GroupBox groupBox8;
          private System.Windows.Forms.TextBox txtToleranciaFuncion;
          private System.Windows.Forms.GroupBox groupBox9;
          private System.Windows.Forms.TextBox txtNumEntrenamientos;
          private System.Windows.Forms.GroupBox groupBox7;
          private System.Windows.Forms.TextBox txtToleranciaIntegral;
          private System.Windows.Forms.GroupBox groupBox14;
          private System.Windows.Forms.TextBox txtTiempoEjecucion;
          private System.Windows.Forms.GroupBox groupBox13;
          private System.Windows.Forms.TextBox txtNumEntrenAlgoritmo;
          private System.Windows.Forms.GroupBox groupBox12;
          private System.Windows.Forms.TextBox txtErroFuncion;
          private System.Windows.Forms.GroupBox groupBox11;
          private System.Windows.Forms.TextBox txtErroIntegral;
          private System.Windows.Forms.GroupBox groupBox10;
          private System.Windows.Forms.TextBox txtValorIntegral;
          private System.Windows.Forms.Button btnFFC;
          private System.Windows.Forms.GroupBox groupBox16;
          private System.Windows.Forms.GroupBox groupBox19;
          private System.Windows.Forms.GroupBox groupBox18;
          private System.Windows.Forms.GroupBox groupBox17;
          private System.Windows.Forms.GroupBox groupBox21;
          private System.Windows.Forms.GroupBox groupBox20;
          private System.Windows.Forms.GroupBox groupBox15;
          private System.Windows.Forms.GroupBox groupBox22;
          private System.Windows.Forms.Label lblB;
          private System.Windows.Forms.Label lblA;
          private System.Windows.Forms.TextBox txtB;
          private System.Windows.Forms.TextBox txtA;
          private System.Windows.Forms.ProgressBar prBProgreso;
     }
}

