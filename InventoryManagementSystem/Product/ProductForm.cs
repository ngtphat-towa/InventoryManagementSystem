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
    public partial class ProductForm : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=elhanaya\elhanaya;Initial Catalog=InventoryDB_Sample;Integrated Security=True;Pooling=False");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DataTable categoryList;
        public ProductForm()
        {
            InitializeComponent();
            categoryList = GetCatetoryList();
            LoadProduct();
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
            cm = new SqlCommand("SELECT * FROM dbo.tbProduct  WHERE CONCAT(id, name, qty, price, description) LIKE '%" + txtSearch.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(),FindCategoryNameById( dr[2].ToString()), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
            foreach (var item in dgvProduct.Rows)
            {
               
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductModuleForm formModule = new ProductModuleForm();
            formModule.btnSave.Enabled = true;
            formModule.btnUpdate.Enabled = false;
            formModule.ShowDialog();
            LoadProduct();

        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                ProductModuleForm productModule = new ProductModuleForm();
                productModule.lbProductID.Text= dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                productModule.txtName.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                productModule.cbCategory.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                productModule.txtQty.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                productModule.txtPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                productModule.txtDescription.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                productModule.btnSave.Enabled = false;
                productModule.btnUpdate.Enabled = true;
                productModule.ShowDialog();
                LoadProduct();

            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand($"DELETE FROM tbProduct WHERE id LIKE '{ dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString()}'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                    LoadProduct();

                }
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();

        }
    }
}
