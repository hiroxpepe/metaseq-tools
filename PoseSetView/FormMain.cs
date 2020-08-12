using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PoseSetCore;

namespace PoseSetView {
    public partial class FormMain : Form {

        Context context;

        public FormMain() {
            context = new Context();

            InitializeComponent();
        }

        private void buttonRead_Click(object sender, EventArgs e) {
            context.Exec();
        }
    }
}
