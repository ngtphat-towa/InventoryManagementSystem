using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class UserForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=elhanaya\elhanaya;Initial Catalog=InventoryDB_Sample;Integrated Security=True;Pooling=False");
        SqlCommand cm = new SqlCommand();
        public UserForm()
        {
            InitializeComponent();
        }
        private void LoadUser()
        {
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser", con);
        }
    }
}
