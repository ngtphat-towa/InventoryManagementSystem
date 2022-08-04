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
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=elhanaya\elhanaya;Initial Catalog=InventoryDB_Sample;Integrated Security=True;Pooling=False");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DataTable categoryList;

        public OrderModuleForm()
        {
            InitializeComponent();
            categoryList = GetCatetoryList();
            LoadProduct();
            LoadCustomer();
        }


        private DataTable GetCatetoryList()
        {
            DataTable output = new DataTable();
            SqlDataAdapter cmCategory = new SqlDataAdapter("Select * from tbCategory", con);
            cmCategory.Fill(output);
            con.Close();
            return output;
        }
        private int FindCategoryIDByName(string name)
        {
            int output = 0;

            output = categoryList.AsEnumerable()
                               .Where(x => x.Field<string>("name").Contains(name.Trim()) == true)
                               .Select(x => x.Field<int>("Id")).FirstOrDefault();
            return output;
        }
        private string FindCategoryNameById(string id)
        {
            string output = "";

            output = categoryList.AsEnumerable()
                               .Where(x => x.Field<int>("Id") == Convert.ToInt32(id))
                               .Select(x => x.Field<string>("name")).FirstOrDefault();
            return output;
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM dbo.tbProduct  WHERE CONCAT(id, name, qty, price, description) LIKE '%" + txtProductSearch.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), FindCategoryNameById(dr[2].ToString()), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbCustomer WHERE CONCAT(cid, cname, cphone) LIKE '%" + txtCustomerSearch.Text + "%'", con);
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

        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {

        }
        private int currentSelectedQty()
        {
            int output = 0;
            cm = new SqlCommand("SELECT qty FROM tbProduct WHERE id='" + txtProductId.Text + "'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                output = Convert.ToInt32(dr[0].ToString());
            }
            dr.Close();
            con.Close();
            return output;
        }
        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProductId.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtProductName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtProductPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
            
        }
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCustomerId.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCustomerName.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();

        }

        private void txtQty_ValueChanged(object sender, EventArgs e)
        { 
            if (Convert.ToInt16(txtQty.Value) > currentSelectedQty())
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQty.Value = txtQty.Value - 1;
                return;
            }
            if (Convert.ToInt16(txtQty.Value) > 0)
            {
                int total = Convert.ToInt16(txtProductPrice.Text) * Convert.ToInt16(txtQty.Value);
                txtTotal.Text = total.ToString();
            }
        }
    }
}
