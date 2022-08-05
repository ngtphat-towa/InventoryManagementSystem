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
    public partial class ProductModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=elhanaya\elhanaya;Initial Catalog=InventoryDB_Sample;Integrated Security=True;Pooling=False");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        private void LoadCategory()
        {
            cbCategory.Items.Clear();
            cm = new SqlCommand("Select * from dbo.tbCategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbCategory.Items.Add(dr[1].ToString());
            }
          
            dr.Close();
            con.Close();
        }

        private DataTable GetCatetoryList()
        {
            DataTable output = new DataTable();
            SqlDataAdapter cmCategory = new SqlDataAdapter("Select * from tbCategory", con);
            con.Open();
            cmCategory.Fill(output);
            con.Close();
            return output;
        }
        private int FindCategoryIDByName(string name)
        {
            int output = 0;

            output = GetCatetoryList().AsEnumerable()
                               .Where(x => x.Field<string>("name").Contains(name.Trim()) == true)
                               .Select(x => x.Field<int>("Id")).FirstOrDefault();
            return output;
        }
        
        public void Clear()
        {
            txtName.Clear();
            txtQty.Clear();
            txtPrice.Clear();
            txtDescription.Clear();
            
            cbCategory.Text = "";
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure you want to update this product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cm = new SqlCommand("UPDATE tbProduct SET name = @name, categoryId=@categoryId, qty=@qty, price=@price, description=@description WHERE id LIKE '" + lbProductID.Text + "' ", con);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@categoryId", FindCategoryIDByName(cbCategory.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt16(txtQty.Text));
                    cm.Parameters.AddWithValue("@price", Convert.ToDouble(txtPrice.Text));
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully updated!");
                    this.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (MessageBox.Show("Are you sure you want to save this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbProduct(name,categoryId,qty,price,description)VALUES(@name, @categoryId, @qty, @price, @description)", con);
                    cm.Parameters.AddWithValue("@name", txtName.Text);
                    cm.Parameters.AddWithValue("@categoryId", FindCategoryIDByName(cbCategory.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt16(txtQty.Text));
                    cm.Parameters.AddWithValue("@price",Convert.ToDouble( txtPrice.Text));
                    cm.Parameters.AddWithValue("@description", txtDescription.Text);

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been successfully saved.");
                    Clear();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
