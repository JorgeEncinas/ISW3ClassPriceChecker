using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PuntoVenta
{
    public partial class Form1 : Form
    {

        private string codigo = "";
        private int segundos = 0;
        //sealed protected public private

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pbLogo.Location = new Point(this.Width/2 - pbLogo.Width/2,
                        Convert.ToInt32(this.Height * 0.30) - pbLogo.Height/2);
            label1.Location = new Point(this.Width/2 - label1.Width / 2,
                       Convert.ToInt32(this.Height * 0.75) - label1.Height / 2);
            pbScangif.Location = new Point(this.Width / 2 - pbScangif.Width / 2,
                                           this.Height / 2 - pbScangif.Height / 2);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //     || &&       > < >= <= 
            {
                MessageBox.Show("Vamos a buscar el Producto " + codigo);
                try
                {
                    MySqlConnection connObj;
                    connObj = new MySqlConnection("server=127.0.0.1; user=root; database=verpres3;SSL Mode=None;");
                    connObj.Open();
                    String query = " SELECT clave, nombre, existencias, precio, rutaimg " +
                                   " FROM productos " +
                                   " WHERE clave = " + codigo;
                    MySqlCommand command = new MySqlCommand(query, connObj);
                    MySqlDataReader result = command.ExecuteReader();
                    if(result.HasRows)
                    {
                        result.Read();
                        //MessageBox.Show(result.GetString(1));
                        pbScangif.Visible = false;
                        lbProductName.Location = new Point((this.Width / 2) - (pbScangif.Width / 2) - 400,
                                           this.Height / 2 - pbScangif.Height / 2);
                        lbProductName.Text = result.GetString(1)+Environment.NewLine +
                            "Precio: " + result.GetString(3) + Environment.NewLine +
                            "Existencias: " + result.GetString(2);
                        lbProductName.Visible = true;
                        //pbj.Image = Image.FromFile("img/"+result.GetString(4));
                        pbj.Load(result.GetString(4));
                        pbj.Location = pbScangif.Location;
                        pbj.Visible = true;
                        pbj.SizeMode = PictureBoxSizeMode.StretchImage;

                        segundos = 0;
                        timer1.Enabled = true;
                    } else
                    {
                        MessageBox.Show("Lo sentimos, no se encontró el producto asociado al código leído.");
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Error Catastremendófico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                codigo = "";
            }
            else
            {
                codigo += e.KeyChar;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            segundos++;
            if (segundos == 10)
            {
                timer1.Enabled = false;
                pbj.Visible = false;
                pbScangif.Visible = true;
                lbProductName.Text = "";
                timer1.Enabled = false;
                segundos = 0;
            }
        }
    }
}
