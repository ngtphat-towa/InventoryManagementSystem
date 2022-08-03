using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class CustomIconButton : PictureBox
    {
        private Image normalImage;
        private Image hoverImage;
        public CustomIconButton()
        {
            InitializeComponent();
        }

        public Image ImageNormal { get => normalImage; set => normalImage = value; }
        public Image ImageHover { get => hoverImage; set => hoverImage = value; }

        private void CustomIconButton_MouseHover(object sender, EventArgs e)
        {
            this.Image = ImageHover;
        }

        private void CustomIconButton_MouseLeave(object sender, EventArgs e)
        {
            this.Image = ImageNormal;
        }
    }
}
