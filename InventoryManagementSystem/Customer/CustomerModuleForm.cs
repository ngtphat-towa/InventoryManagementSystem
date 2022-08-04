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
    public partial class CustomerModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=elhanaya\elhanaya;Initial Catalog=InventoryDB_Sample;Integrated Security=True;Pooling=False");
        SqlCommand cm = new SqlCommand();
        public CustomerModuleForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
           
                if (MessageBox.Show("Are you sure you want to save this customer?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("INSERT INTO tbCustomer(cname,cphone)VALUES(@cname,@cphone)", con);
                    cm.Parameters.AddWithValue("@cname", txtFullName.Text);
                    cm.Parameters.AddWithValue("@cphone", txtPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Customer has been successfully saved.");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
        }
        private void Clear()
        {
            txtFullName.Clear();
            txtPhone.Clear();

            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this Customer?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("UPDATE tbCustomer SET cname = @cname,cphone=@cphone WHERE cid LIKE '" + lbCustomerID.Text + "' ", con);
                    cm.Parameters.AddWithValue("@cname", txtFullName.Text);
                    cm.Parameters.AddWithValue("@cphone", txtFullName.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Customer has been successfully updated!");
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
