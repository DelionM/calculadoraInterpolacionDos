using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace calculadoraInterpolacionDos
{
    public partial class Form_ID : Form
    {
        public string ID_Name { get { return txt_idName.Text; } }
        public string ID_Value { get { return txt_idValue.Text; } }

        public Form_ID()
        {
            InitializeComponent();
        }

        private void Form_ID_Load(object sender, EventArgs e)
        {

        }
    }
}
