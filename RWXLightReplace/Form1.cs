using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;


namespace RWXLightReplace
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static class Globals
        {
            public static bool UnzipBefore;
            public static bool ZipAfter;
            public static string Ambient;
            public static string Diffuse;

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

            // read in the checkboxes
            if (checkBox1.Checked)
            {
                Globals.UnzipBefore = true;
            }
            else
            {
                Globals.UnzipBefore = false;
            }

            if (checkBox2.Checked)
            {
                Globals.ZipAfter = true;
            }
            else
            {
                Globals.ZipAfter = false;
            }

            // read in the textboxes for Ambient and Diffuse
            Globals.Ambient = textBox2.Text;
            Globals.Diffuse = textBox3.Text;


            // If the checkbox to unzip before processing is checked...
            if (Globals.UnzipBefore)
            {
                // Unzip any zipped files and delete the zip, leaving only the zip's contents
                // Will skip any zips that contain duplicate RWXs (fix later)
                string[] files1 = Directory.GetFiles(FileIn);

                foreach (string x in files1)
                {
                    ext = Path.GetExtension(x);
                    if (ext.ToLower() == ".zip")
                    {
                        try
                        {
                            ZipFile.ExtractToDirectory(x, FileIn);
                            File.Delete(x);
                        }
                        catch { }

                    }

                }
            }
            

            // Load all the files from the directory into a string
            string[] files2 = Directory.GetFiles(FileIn);
            
            // Start the processing
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
            if (ext.ToLower() == ".rwx")
            {
                Process(file, dir);
                return;
            }

        }

        private void Process(string file, string dir)
        {
            //Console.WriteLine(file);
            string line, replacewith, linebuild, linepad;
            int found, index;

            //StringComparison comp = StringComparison.Ordinal;

            // create a new temp file for the RWX output
            System.IO.StreamWriter outfile = new System.IO.StreamWriter(dir + "\\output.txt", true);

            // open the input file for reading
            System.IO.StreamReader sfile = new System.IO.StreamReader(file);

            // read each line, and push to the temp file; if the line contains a surface, ambient, or diffuse tag, update it
            while ((line = sfile.ReadLine()) != null)
            {
                found = 0;
                replacewith = "";
                index = 0;
                linepad = "";
                line = line.ToLower();

                if (line.Contains("surface"))
                {
                    // find the position of the tag in the string
                    found = 1;
                    index = line.IndexOf("surface");
                    replacewith = "surface";
                }
                else if (line.Contains("ambient"))
                {
                    found = 1;
                    index = line.IndexOf("ambient");
                    replacewith = "ambient";
                }
                else if (line.Contains("diffuse"))
                {
                    found = 1;
                    index = line.IndexOf("diffuse");
                    replacewith = "diffuse";
                }

                // If the line contains one of the tags, build the new tag and write it to outfile - else write the old line to outfile
                if (found == 1)
                {

                    linebuild = linepad.PadRight(index);
                    linebuild = linebuild + replacewith;

                    if (replacewith == "surface")
                    {
                        linebuild = linebuild + " " + Globals.Ambient + " " + Globals.Diffuse + " 0";
                    }
                    else if (replacewith == "ambient")
                    {
                        linebuild = linebuild + " " + Globals.Ambient;
                    }
                    else
                    {
                        linebuild = linebuild + " " + Globals.Diffuse;
                    }

                    outfile.WriteLine(linebuild);
                    
                }
                else
                {
                    outfile.WriteLine(line);
                }

            }
            sfile.Close();
            outfile.Close();


            // After both files are closed, delete the input file and rename the output file to input file
            File.Delete(file);
            File.Move(dir + "\\output.txt", file.ToLower());

            
            // Now zip it all up if check for zipafter is checked
            if (Globals.ZipAfter)
            {
                string zipfile = Path.ChangeExtension(file.ToLower(), ".zip");
                string fileentry = Path.GetFileName(file.ToLower());

                using (ZipArchive newFile = ZipFile.Open(zipfile, ZipArchiveMode.Create))
                {
                    //Console.WriteLine(file + " : " + fileentry);
                    newFile.CreateEntryFromFile(file, fileentry);
                }
                File.Delete(file);
            }
        
        }

    }
}
