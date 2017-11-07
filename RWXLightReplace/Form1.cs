using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Collections;

namespace RWXLightReplace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openDir = new FolderBrowserDialog();
            openDir.RootFolder = Environment.SpecialFolder.Desktop;

            if (openDir.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Place the dir into the text box
                    texDir.Text = openDir.SelectedPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read directory from disk. Original error: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string ext;

            string FileIn = texDir.Text;
            if (!Directory.Exists(FileIn))
            {
                MessageBox.Show("Directory doesn't exist! Try again.");
                return;
            }


            // first unzip any zipped files and delete the zip, leaving only the zip's contents
            string[] files1 = Directory.GetFiles(FileIn);

            foreach (string x in files1)
            {
                ext = Path.GetExtension(x);
                if (ext == ".zip")
                {
                    try
                    {
                        ZipFile.ExtractToDirectory(x, FileIn);
                        File.Delete(x);
                    }
                    catch {}
                    
                }

            }



            // re-load all the files from the directory into a string
            string[] files2 = Directory.GetFiles(FileIn);

            
            
            foreach (string x in files2)
            {
                Iterate(x, FileIn);
            }






        }


        private void Iterate(string file, string dir)
        {

            //Console.WriteLine(file);
            string ext = Path.GetExtension(file);

            // if rwx, process
            if (ext == ".rwx")
            {
                Process(file, dir);
                return;
            }


           
            

            // if we processed the file, re-zip it (first, for each file deleteing existing zip that has same filename



        }

        private void Process(string file, string dir)
        {
            Console.WriteLine(file);


            // create a new temp file for the RWX copying

            // iterate through each line of the RWX file and replace the light values in the correct tags

            // tag could look like     Surface 1 1 0
            // or ambient 1   or diffuse 1

            // 




        }


        // Documentations

        // StringContains() help & example
        // https://msdn.microsoft.com/en-us/library/dy85x1sa(v=vs.110).aspx

        //

    }
}
