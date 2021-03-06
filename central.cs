﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Subsecsum.CHARTS;

namespace Subsecsum
{
    public partial class central : Form
    {
        private int [] tarray;
        private Random random;
        private int limita, limitb, xlength;
        private bool flag;
        private Stopwatch watch;
        private int optiontest;
        private shadow emergente;

        public chartbase grafica;
        
        public central()
        {
            InitializeComponent();
            this.ocultar();
            this.Text = "Maxima Suma Subsecuente";
            this.StartPosition = FormStartPosition.CenterScreen;            
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.rtb1.ReadOnly = true;

            this.nupdtotal.ValueChanged += (sender, args) => { int.TryParse(this.nupdtotal.Value.ToString(), out this.xlength); };
            this.nupd1.ValueChanged += (sender, args) => { int.TryParse(this.nupd1.Value.ToString(), out this.limita); };
            this.nupd2.ValueChanged += (sender, args) => { int.TryParse((this.nupd2.Value+1).ToString(), out this.limitb); };

            this.nupdtotal.KeyPress += (sender, args) => { if (args.KeyChar == (char)Keys.Enter) this.btngenerar(); };
            this.nupdtotal.Maximum = 9999999999999999999;
            this.nupdtotal.Text = "";
            this.nupd1.Minimum = -20;
            this.nupd2.Minimum = -20;
            this.nupd1.Value = -20;
            this.nupd2.Value = 20;
            this.cbmulti.CheckState = CheckState.Checked;
            
            this.random = new Random(System.DateTime.Today.Millisecond);
            this.watch = new Stopwatch();
            this.grafica = new chartbase();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.normalsize();
            this.ActiveControl = this.nupdtotal;
            this.grafica.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.btngenerar();
        }

        private void btngenerar()
        {
            this.normalsize();
            this.lbltime.Text = "Time of work = Unavailable";
            if (this.xlength == 0)
            {
                msg.error("Defina una longitud mayor a 0.");
                this.nupdtotal.Focus();
            }
            else
            {
                this.mostrar();
                this.rtb1.Text = "";
                this.rtb1.Enabled = false;
                this.threadserie();
            }
        }

        public async void threadserie() //FUNCTION TO CREATE THE SUBTHREAD OF RANDOMNESS
        {
            this.watch.Start();
            bool f = await Task.Run(() => this.createserie(this.xlength));
            this.watch.Stop();
            this.lbltime.Text = "Time of work = "+this.watch.Elapsed.ToString("hh\\:mm\\:ss\\.ff");
            this.watch.Reset();
            if (f)
            {
                this.ocultar();
                this.rtb1.Enabled = true;
                this.newsize();                
            }
            else
            {                
                this.clearvals();
                this.ocultar();
                msg.error("Error al generar numeros aleatorios");
                this.nupdtotal.Focus();
            }

        }

        public bool createserie(int x) //FUNCTION TO GENERATE THE RANDOM NUMBERS
        {
            if (this.flag)
            {
                this.flag = false;
            }
            try
            {                
                this.tarray = new int[(int)x];
                
                for (int a = 0; a < x; ++a)
                {
                    this.tarray[a] = this.random.Next(this.limita, this.limitb);

                    if (this.rtb1.InvokeRequired)
                    {
                        this.Invoke(new MethodInvoker(delegate () {
                            this.rtb1.Text += (a + 1).ToString() + ")-> " + this.tarray[a].ToString() + "\n";
                        }));
                        
                    }
                    else
                    {
                        this.rtb1.Text += (a + 1).ToString() + ")-> " + this.tarray[a].ToString() + "\n";
                    }
                    
                }

                this.flag = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return this.flag;
        }
        private void clearvals() //FUNCTION TO CLEAR VALUES OF INTEREST
        {
            this.rtb1.Text = "";
            this.nupdtotal.Value = 0;
            this.nupdtotal.Text = "";            
        }

        public void mostrar()
        {
            if (this.calculando.InvokeRequired || this.charging.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () {
                    this.paneltop.Visible = true;
                }));

                return;
            }
            
                this.paneltop.Visible = true;            
        }

        public void ocultar()
        {
            if(this.calculando.InvokeRequired || this.charging.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate () {
                    this.paneltop.Visible = false;
                }));

                return;
            }

            this.paneltop.Visible = false;
        }

        private void rendimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            msg.ok("-Proyecto: Maxima suma subsecuente \n\n-Alumno: PTBI Jesús José Navarrete Baca\n\n-Profesor: ISCH Carlos Eduardo Garcia Vargas\n\n-Materia:Estructura de datos II", "Información");
        }

        private void cb1_CheckedChanged(object sender, EventArgs e)
        {
            if (cb1.Checked)
            {
                this.optiontest = 1;
                cb2.CheckState = CheckState.Unchecked;
                cb3.CheckState = CheckState.Unchecked;
                cbmulti.CheckState = CheckState.Unchecked;
                cbnuevo.CheckState = CheckState.Unchecked;
            }
        }

        private void cb2_CheckedChanged(object sender, EventArgs e)
        {
            if (cb2.Checked)
            {
                this.optiontest = 2;
                cb1.CheckState = CheckState.Unchecked;
                cb3.CheckState = CheckState.Unchecked;
                cbmulti.CheckState = CheckState.Unchecked;
                cbnuevo.CheckState = CheckState.Unchecked;
            }
        }

        private void cb3_CheckedChanged(object sender, EventArgs e)
        {
            if (cb3.Checked)
            {
                this.optiontest = 3;
                cb1.CheckState = CheckState.Unchecked;
                cb2.CheckState = CheckState.Unchecked;
                cbmulti.CheckState = CheckState.Unchecked;
                cbnuevo.CheckState = CheckState.Unchecked;
            }
        }

        private void cbmulti_CheckedChanged(object sender, EventArgs e)
        {
            if (cbmulti.Checked)
            {
                this.optiontest = 4;
                cb2.CheckState = CheckState.Unchecked;
                cb3.CheckState = CheckState.Unchecked;
                cb1.CheckState = CheckState.Unchecked;
                cbnuevo.CheckState = CheckState.Unchecked;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.startCalc();
        }
        private void startCalc()
        {
            if (this.cb1.Checked || this.cb2.Checked || this.cb3.Checked || this.cbmulti.Checked ||this.cbnuevo.Checked)
            {
                this.emergente = new shadow(ref this.tarray, this.optiontest, this);
                this.emergente.Show();
            }
            else
            {
                msg.danger("Debe seleccionar una prueba de ejecucion");
            }
        }

        private void cbnuevo_CheckedChanged(object sender, EventArgs e)
        {
            if (cbnuevo.Checked)
            {
                this.optiontest = 5;
                cb2.CheckState = CheckState.Unchecked;
                cb3.CheckState = CheckState.Unchecked;
                cb1.CheckState = CheckState.Unchecked;
                cbmulti.CheckState = CheckState.Unchecked;   
            }
        }

        private void normalsize()
        {
            this.Height = 310;
        }

        private void newsize()
        {
            this.Height = 467;  
        }


    }
}
