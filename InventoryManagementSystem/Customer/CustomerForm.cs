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
    public partial class CustomerForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=elhanaya\elhanaya;Initial Catalog=InventoryDB_Sample;Integrated Security=True;Pooling=False");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomer();


        }
        private void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbCustomer", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerModuleForm customerModule = new CustomerModuleForm();
            customerModule.btnSave.Enabled = true;
            customerModule.btnUpdate.Enabled = false;
            customerModule.ShowDialog();
            LoadCustomer();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCustomer.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CustomerModuleForm customerModule = new CustomerModuleForm();
                customerModule.lbCustomerID.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                customerModule.txtFullName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                customerModule.txtPhone.Text = dgvCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();

                customerModule.btnSave.Enabled = false;
                customerModule.btnUpdate.Enabled = true;
                customerModule.ShowDialog();
                LoadCustomer();

            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this customer?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbCustomer WHERE cid LIKE '" + dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                    LoadCustomer();
                }
            }
        }
    }

}
