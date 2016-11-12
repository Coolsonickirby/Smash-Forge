﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace Smash_Forge
{
    public partial class NUTEditor : Form
    {
        private NUT selected;

        public NUTEditor()
        {
            InitializeComponent();
            FillForm();
            if (Runtime.TextureContainers.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void FillForm()
        {
            listBox1.Items.Clear();
            foreach(NUT n in Runtime.TextureContainers)
            {
                listBox1.Items.Add(n);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex >= 0)
            {
                SelectNUT((NUT)listBox1.SelectedItem);
            }
        }

        private void SelectNUT(NUT n)
        {
            selected = n;
            
            listBox2.Items.Clear();
            foreach (NUT.NUD_Texture tex in n.textures)
            {
                listBox2.Items.Add(tex);
            }
        }

        private void openNUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Namco Universal Texture (.nut)|*.nut|" +
                             "All files(*.*)|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.EndsWith(".nut"))
                    {
                        Runtime.TextureContainers.Add(new NUT(ofd.FileName));
                        FillForm();
                    }
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox2.SelectedIndex >= 0)
            {
                NUT.NUD_Texture tex = ((NUT.NUD_Texture)listBox2.SelectedItem);
                textBox1.Text = tex.ToString();
                label2.Text = "Format: " + tex.type;
                label3.Text = "Width: " + tex.width;
                label4.Text = "Height:" + tex.height;
                label5.Text = "Mipmap:" + tex.mipmaps.Count;
                RenderTexture();
            }
            else
            {
                textBox1.Text = "";
                label2.Text = "Format:";
                label3.Text = "Width:";
                label4.Text = "Height:";
                label5.Text = "Mipmap:";
            }
        }

        private void RenderTexture()
        {
            glControl1.MakeCurrent();
            GL.ClearColor(Color.Red);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Enable(EnableCap.Texture2D);

            if (listBox1.SelectedItem == null || listBox2.SelectedItem == null)
                return;

            int rt = ((NUT)listBox1.SelectedItem).draw[((NUT.NUD_Texture)listBox2.SelectedItem).id];

            GL.BindTexture(TextureTarget.Texture2D, rt);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-1, -1);
            GL.TexCoord2(1, 0);
            GL.Vertex2(1, -1);
            GL.TexCoord2(1, 1);
            GL.Vertex2(1, 1);
            GL.TexCoord2(0, 1);
            GL.Vertex2(-1, 1);
            GL.End();

            glControl1.SwapBuffers();
        }
    }
}