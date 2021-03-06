﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Xml.Linq;
using Path = System.IO.Path;
using Size = System.Drawing.Size;
using AdaptiveVision;
using AvlNet;
using NewElectronicTechnology.SynView;
using System.Data.SqlClient;
using System.Collections;
using WittEyE;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {

    

        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        DataSet ds3 = new DataSet();
        DataSet ds4 = new DataSet();
        DataSet ds5 = new DataSet();

        DataTable dt = new DataTable();



        //private readonly ProgramMacrofilters macros;
        public static int NodeCount;
        public bool camtrig;
        public static
        int? Socet1, Socet2, Socet3;
        private ProgramMacrofilters macros;

      
        int ID = 0;
        int flag = 0;

        IntPtr m_hDisplayWindow;
        IntPtr m_hDisplayWindow2;
        IntPtr m_hDisplayWindow3;
        IntPtr m_hDisplayWindow4;
        IntPtr m_hDisplayWindow5;

        CCamera m_pCamera;
        CCamera2 m_pCamera2;
        CCamera3 m_pCamera3;
        CCamera4 m_pCamera4;
        CCamera5 m_pCamera5;


        LvSystem m_pSystem;
        LvSystem m_pSystem2;
        LvSystem m_pSystem3;
        LvSystem m_pSystem4;
        LvSystem m_pSystem5;

       
        DD_Sql ddsql = new DD_Sql();
        DD_FrontEnd ddfront = new DD_FrontEnd();

       

        BackgroundWorker backgroundWorker1 = new BackgroundWorker();



        public Form1()
        {
            InitializeComponent();

            _Form1 = this;

            try
            {
                string avsProjectPath = @"AVS\Program.avproj";
                macros = ProgramMacrofilters.Create(avsProjectPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


            m_pSystem = null;
            m_pSystem2 = null;
            m_pSystem3 = null;
            m_pSystem4 = null;
            m_pSystem5 = null;


            LvLibrary.ThrowErrorEnable = true;
            // The PictureBoxLive refuses to be used from another thread,
            // but the window handle should be safe.
            m_hDisplayWindow = PictureBoxLive.Handle;
            m_hDisplayWindow2 = pictureBox2.Handle;
            m_hDisplayWindow4 = pbH_0.Handle;
            m_hDisplayWindow3 = pbH_U.Handle;
            m_hDisplayWindow5 = pb_DE.Handle;


            try
            {
                Cursor = Cursors.WaitCursor;
                LvLibrary.OpenLibrary();
                LvSystem.Open("", ref m_pSystem);
                LvSystem.Open("", ref m_pSystem2);
                LvSystem.Open("", ref m_pSystem3);
                LvSystem.Open("", ref m_pSystem4);
                LvSystem.Open("", ref m_pSystem5);

                m_pCamera = new CCamera();
                m_pCamera2 = new CCamera2();
                m_pCamera3 = new CCamera3();
                m_pCamera4 = new CCamera4();
                m_pCamera5 = new CCamera5();
                Cursor = Cursors.Default;

            }
            catch (LvException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }


          //  ConnSQL();
          //  DataTable dt1 = ddsql.DisplaySQLDatai();
          //  DataTable dt2 = ddsql.DisplaySQLDatai();


         //   dataGridView10.DataSource = dt1;
          //  dataGridView11.DataSource = dt2;
          //  ConnSQL();


            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += bgWorker_WorkComplete;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;

        }

        public static Form1 _Form1;

        public void update(string message)
        {
            textBox20.Text = message;
        }



        public static class MyStaticValues
        {
            public static bool camtrig2 { get; set; }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(i);

                if (backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    backgroundWorker1.ReportProgress(0);
                    return;
                }

            }
        }


        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

         
        }


        private void bgWorker_WorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            //e.Error will contain any exceptions caught by the backgroundWorker
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                
                progressBar1.Value = 0;
            }
        }



        protected override void OnClosed(System.EventArgs e)
        {
            //Release resources held by the Macrofilter .NET Interface object
            if (macros != null)
                macros.Dispose();
            base.OnClosed(e);
        }



        //Load XML Artikel
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "wittEyEDataSet.IBC_EB". Sie können sie bei Bedarf verschieben oder entfernen.
            Class1 sample = new Class1();

        }



        private void UpdateXLMDatei()
        {
            try
            {
                XmlReader xmlFile, xmlFile2, xmlFile3, xmlFile4, xmlFile5;

                xmlFile = XmlReader.Create(@"\\Aw-ap2019\interface\VISION\EB_Artikel\Artikel.xml", new XmlReaderSettings());
                ds.ReadXml(xmlFile);
                dataGridView2.DataSource = ds.Tables[0];

                xmlFile2 = XmlReader.Create(@"\\Aw-ap2019\interface\VISION\EB_UNBEARBEITET\Belege_Unbearbeitet.xml", new XmlReaderSettings());
                ds2.ReadXml(xmlFile2);
                dataGridView1.DataSource = ds2.Tables[0];
                xmlFile2.Close();

                xmlFile3 = XmlReader.Create(@"\\Aw-ap2019\interface\VISION\EB_Artikel\Artikel_Lohnumbau.xml", new XmlReaderSettings());
                ds3.ReadXml(xmlFile3);
                dataGridView5.DataSource = ds3.Tables[0];


                xmlFile4 = XmlReader.Create(@"\\Aw-ap2019\interface\VISION\EB_Artikel\Artikel_Lohnreinigung.xml", new XmlReaderSettings());
                ds4.ReadXml(xmlFile4);
                dataGridView6.DataSource = ds4.Tables[0];

                xmlFile5 = XmlReader.Create(@"\\Aw-ap2019\interface\VISION\EB_Artikel\Dienstleistungen.xml", new XmlReaderSettings());
                ds5.ReadXml(xmlFile5);
                dataGridView7.DataSource = ds5.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Refresch_XML()
        {

        }

        private void moveEBtoFertigXML()
        {
            try
            {
                string fileName = label16.Text + ".xml";
                string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen\\");
                string targetPath = @"V:/EB_Fertig";

                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName);

                System.IO.Directory.CreateDirectory(targetPath);

                System.IO.File.Move(sourceFile, destFile);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            Refresch_XML();
            Refresch_EB_Offen();

        }


        private void CMD(string strCmdText)
        {
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        private void Refresch_EB_Offen()
        {
            Refresch_XML();

            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen\\");
            string[] filePaths2 = Directory.GetFiles(destPath);

            DataTable table2 = new DataTable();
            table2.Columns.Add("EB");
            table2.Columns.Add("Ist Stk Zahl");
            table2.Columns.Add("SollStkZahl");
            table2.Columns.Add("Kunde");
            table2.Columns.Add("Lieferdatum");

            DataRow row;
            //  table2.Columns.Add("File Path");



            for (int i = 0; i < filePaths2.Length; i++)
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                XmlNode xmlnode;

                String Slt, Slt3, Slt4, Slt5, Eb;

                FileInfo file = new FileInfo(filePaths2[i]);
                char[] MyChar = { 'x', 'm', 'l', '.' };

                Eb = file.Name.TrimEnd(MyChar);
                EBsucheKunde(Eb, out Slt3, out Slt4, out Slt5);

                FileStream fs = new FileStream(filePaths2[i], FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);

                XmlNodeList nodeListCount = xmldoc.GetElementsByTagName("BelegePositionen.ArtikelNummer");  //Suche Nodes (Pos) Anzahl 
                int nodeCount = nodeListCount.Count;

                Slt = nodeCount.ToString();


                row = table2.NewRow();
                row["EB"] = Eb;
                row["Ist Stk Zahl"] = Slt;
                row["SollStkZahl"] = Slt3;
                row["Kunde"] = Slt4;
                row["Lieferdatum"] = Slt5;
                table2.Rows.Add(row);

            }
            table2.DefaultView.Sort = "SollStkZahl asc";
            dataGridView3.DataSource = table2;




         
        }

        private void EBsucheKunde(String EBNum, out string VEA2, out string Kunde, out string Lieferdatum)
        {

            DataSet ds = new DataSet();
            ds.ReadXml(@"\\Aw-ap2019\interface\VISION\EB_UNBEARBEITET\Belege_Unbearbeitet.xml");
            DataView dv = new DataView(ds.Tables[0]);

            char[] MyChar = { 'x', 'm', 'l', '.' };

            EBNum = EBNum.TrimEnd(MyChar);

            dv.RowFilter = string.Format("Belegnummer LIKE '%{0}%'", EBNum);

            DataTable dt = dv.ToTable();

            // dg1.ItemsSource = dv;

            int dvc = dv.Count;

            if (dvc == 0)
            {
                VEA2 = "nicht gefunden";
                Kunde = "nicht gefunden";
                Lieferdatum = "nicht gefunden";
            }
            else
            {
                VEA2 = dt.Rows[0][1].ToString();
                Kunde = dt.Rows[0][8].ToString();
                Lieferdatum = dt.Rows[0][6].ToString();
            }

        }

        private void ShowSelectedHersteller_UI()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox1.Controls)

            // foreach (Control c in groupBox1.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }

                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();


            if (selectedRb != null)
            {
                selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox1.Text = selectedTag;
            FilterArtikel();

        }

        private void ShowSelecteDPalette_UI()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox2.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }

                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox2.Text = selectedTag;
            Restinhaltnetto_UI(selectedTag);
            FilterArtikel();
            Gewichtermitlung_UI();

            //  MessageBox.Show(result.ToString());
        }

        private void Restinhaltnetto_UI(string Pallete)
        {
            switch (Pallete)
            {
                case "Stahlrahmen":
                    label57.Text = "56";
                    break;
                case "Stahlkufen":
                    label57.Text = "56";
                    break;
                case "PE-Kufen":
                    label57.Text = "57";
                    break;
                case "Holz":
                    label57.Text = "75";
                    break;
                case "Composit":
                    label57.Text = "50";
                    break;

                default:
                    Console.WriteLine("Bitte Palette auswahlen");
                    break;
            }

        }

        private void Gewichtermitlung_UI()
        {
            int Restihalt;

            Restihalt = int.Parse(label55.Text) - int.Parse(label57.Text) - int.Parse(textBox16.Text);


            if (Restihalt > 0)
            {
                textBox19.Text = Restihalt.ToString();
                groupBox16.BackColor = Color.Red;
                label72.Text = "RESTINHALT:";
            }

            if (Restihalt <= 0)
            {
                groupBox16.BackColor = Color.Green;
                label72.Text = "Gewicht i.O";
            }


        }

        private void ShowSelectedEinlauf_UI()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox3.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }

                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox3.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());
        }

        private void ShowSelectedVariante()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox4.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox4.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());
        }

        private void ShowSelectedAuslauf()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox5.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox5.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());
        }

        private void ShowSelectedZulassung()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox6.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                if (selectedRb.Text == "JA")
                {
                    selectedTag = "1";
                }
                else
                {
                    selectedTag = "0";
                }
                // selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox6.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());
        }

        private void ShowSelectedNestbar()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox7.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                if (selectedRb.Text == "JA")
                {
                    selectedTag = "1";
                }
                else
                {
                    selectedTag = "0";
                }
                // selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox7.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());
        }

        private void ShowSelectedEX()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox8.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                if (selectedRb.Text == "JA")
                {
                    selectedTag = "1";
                }
                else
                {
                    selectedTag = "0";
                }
                // selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox8.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());
        }

        private void ShowSelectedBlasenfarbe()
        {
            ResetSonigsten();
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox9.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Text;
                // selectedTag = selectedRb.Text;
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            textBox9.Text = selectedTag;
            FilterArtikel();
            //  MessageBox.Show(result.ToString());

            button6.Enabled = true;
            button7.Enabled = true;
        }

        private void ShowSelektedSoniksten()
        {
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "";

            foreach (Control c in groupBox10.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    RadioButton rb2 = c as RadioButton;
                    if (rb2.Checked)
                    {
                        rb2.BackColor = Color.LimeGreen;
                    }

                    if (rb2.Checked == false)

                    {
                        rb2.BackColor = SystemColors.ControlLight;
                    }
                    buttons.Add((RadioButton)c);
                }
            }



            button6.Enabled = true;
            button7.Enabled = true;

        }

        private void FilterArtikel()
        {

            DataView dv = ds.Tables[0].DefaultView;

            dv.RowFilter = string.Format("K_Modell LIKE '%{0}%' AND " +
                                          "K_PalettenArt LIKE '%{1}%' AND " +
                                          "K_Einlauf LIKE '%{2}%' AND " +
                                          " K_Variante LIKE '%{3}%'  AND" +
                                          " K_Auslauf LIKE '%{4}%' AND" +
                                          " K_Zulassung LIKE '%{5}%' AND" +
                                          " K_Nestbar LIKE '%{6}%' AND" +
                                          " K_EX LIKE '%{7}%' AND" +
                                          " K_Blasenfarbe LIKE '%{8}%' ",
                                          textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text);

            dataGridView2.DataSource = dv;

            if (dataGridView2.Rows[0].Cells[13].Value != null)
            {
                label10.Text = dataGridView2.Rows[0].Cells[13].Value.ToString();
            }
            else
            {
                label10.Text = "Artikel Nicht Gefunden!";
            }

            if (dataGridView2.Rows[0].Cells[1].Value != null)
            {
                label11.Text = dataGridView2.Rows[0].Cells[1].Value.ToString();

            }
            else
            {
                label11.Text = "Artikel Nicht Gefunden!";
            }
        }

        private void SollStkza(string EB)
        {

            DataView dv = ds2.Tables[0].DefaultView;
            dv.RowFilter = string.Format("Belegnummer LIKE '%{0}%'", EB);
            dataGridView1.DataSource = dv;


            if (dataGridView1.Rows[0].Cells[1].Value != null)
            {
                label2.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
                label17.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
                label34.Text = dataGridView1.Rows[0].Cells[8].Value.ToString();

                label43.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();

                label52.Text = dataGridView1.Rows[0].Cells[8].Value.ToString();
                label51.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();
                richTextBox1.Text = dataGridView1.Rows[0].Cells[12].Value.ToString();

                label43.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();
                label43.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();

                label45.Text = dataGridView1.Rows[0].Cells[8].Value.ToString();
                label46.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();

                label48.Text = dataGridView1.Rows[0].Cells[8].Value.ToString();
                label47.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();
            }
            else
            {
                label10.Text = "Soll Stückzahl nicht gefunden";
                label2.Text = "Soll Stückzahl nicht gefunden";
                label17.Text = "Soll Stückzahl nicht gefunden";
                label34.Text = "Soll Stückzahl nicht gefunden";

                label43.Text = "Soll Stückzahl nicht gefunden";

                label52.Text = "Soll Stückzahl nicht gefunden";
                label51.Text = "Soll Stückzahl nicht gefunden";

                label43.Text = "Soll Stückzahl nicht gefunden";
                label43.Text = "Soll Stückzahl nicht gefunden";

                label45.Text = "Soll Stückzahl nicht gefunden";
                label46.Text = "Soll Stückzahl nicht gefunden";

                label48.Text = "Soll Stückzahl nicht gefunden";
                label47.Text = "Soll Stückzahl nicht gefunden";

            }

        }

        //Beenden App
        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Dialogfeld Beeden 
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (PreClosingConfirmation() == System.Windows.Forms.DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        //Dialogfeld Beeden 
        private DialogResult PreClosingConfirmation()
        {
            DialogResult res = System.Windows.Forms.MessageBox.Show(" Do you want to quit?          ", "Quit...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return res;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
        }

        private void userControl11_Load(object sender, EventArgs e)
        {
            ShowSelectedAuslauf();
        }






        private void createNode(string pArtNr, int pKRest, XmlTextWriter writer)
        {
            writer.WriteStartElement("BelegePosition");
            writer.WriteStartElement("BelegePositionen.ArtikelNummer");
            writer.WriteString(pArtNr);
            writer.WriteEndElement();
            writer.WriteStartElement("BelegePositionene.K_Restinhalt");
            //  writer.WriteString(pKRest.ToString());
            writer.WriteValue(pKRest);
            writer.WriteEndElement();
            //  writer.WriteStartElement("BelegePositionen.K_Seriennummer");
            //  writer.WriteString(pKSerien);
            //  writer.WriteEndElement();
            writer.WriteEndElement();
        }




        private void AddNode_XML(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i = 0;
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.Nodes[i];
                    AddNode_XML(xNode, tNode);
                }
            }
            else
            {
                inTreeNode.Text = inXmlNode.InnerText.ToString();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen\\"));
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            string destPath = @"V:\EB_Offen\";
            //    label14.Text = dataGridView3.CurrentCell.Value.ToString();
            label14.Text = destPath + dataGridView3.CurrentRow.Cells[0].Value.ToString();

            //  label14.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString() + "\\" + dataGridView3.CurrentRow.Cells[0].Value.ToString();
            char[] MyChar = { 'x', 'm', 'l', '.' };
            label16.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString().TrimEnd(MyChar);

            SollStkza(label16.Text);
            UpdateTree();
            if (label17.Text != "Soll Stückzahl nicht gefunden")
                SollgleichIST(int.Parse(label18.Text), int.Parse(label17.Text));



        }

        private void deleteEB_XML()
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");
            if (File.Exists(destPath + label16.Text + ".xml") == true)
            {

                File.Delete(destPath + label16.Text + ".xml");
            }
        }

        private void UpdateTree()
        {
            XmlDataDocument xmldoc = new XmlDataDocument();
            XmlNode xmlnode;

            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");


            if (File.Exists(destPath + label16.Text + ".xml") == true)
            {

                FileStream fs = new FileStream((destPath + label16.Text + ".xml"), FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);

                XmlNodeList nodeListCount = xmldoc.GetElementsByTagName("BelegePositionen.ArtikelNummer");  //Suche Nodes (Pos) Anzahl 

                dataGridView8.DataSource = xmldoc.SelectNodes("BelegePositionen.ArtikelNummer");


                int nodeCount = nodeListCount.Count;

                NodeCount = nodeCount;

                label18.Text = nodeCount.ToString();
                label1.Text = nodeCount.ToString();

                xmlnode = xmldoc.ChildNodes[1];

                TreeNode tNode;

                dataGridView8.DataSource = xmlnode;
                fs.Close();
                XMLtoTable();
              //  DataSet ds5 = ddsql.DisplaySQLDataitoDS();
               // dataGridView12.DataSource = ds5;
            }


        }

        private void XMLtoTable()
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

            DataSet de = new DataSet();

            de.ReadXml((destPath + label16.Text + ".xml"));
            DataTable dt = de.Tables[3];
            dataGridView8.DataSource = null;
            dataGridView8.DataSource = dt; // Alle ratikelnnummer
            GC.Collect();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Refresch_XML();
            Refresch_EB_Offen();
        }



        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void NeuePos_XML(string ArtNr, int KreszInh, string Seriennummer, string EB)
        {
            XDocument xDocument = XDocument.Load(EB);
            XElement root = xDocument.Element("Belege");

            //  XmlNodeList node = root.GetElementsByTagName("element");

            IEnumerable<XElement> rows = root.Descendants("BelegePosition");

            XElement firstRow = rows.Last();
            firstRow.AddAfterSelf(
            new XElement("BelegePosition",
            new XElement("BelegePositionen.ArtikelNummer", ArtNr),
            new XElement("BelegePositionen.K_Restinhalt", KreszInh),
            new XElement("BelegePositionen.K_Seriennummer", Seriennummer)));
            xDocument.Save(EB);

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            //  SollStkza(textBox10.Text);

            if (textBox10.TextLength == 12)
            {

                string eb, sn;


                sn = textBox10.Text;
                eb = textBox10.Text.Remove(9, 3);

                panel1.BackColor = Color.Red;
                tabControl1.SelectedTab = tabPage5;
                label16.Text = eb;
                lbSn.Text = sn;
                NodeCount = 0;

                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen\\");

                //   label14.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString() + "\\" + dataGridView3.CurrentRow.Cells[0].Value.ToString();
                label14.Text = destPath + label16.Text + ".xml";
                textBox11.Text = "0";
                textBox12.Text = "1";
                label1.Text = "0";

                SollStkza(eb);
                UpdateTree();
                EB_Offen_Focus();
                ResetSonigsten();
                ResetArtikelAswahl();
            }

            /*
                        if (textBox10.TextLength == 9)
                        {
                            string eb;

                            eb = textBox10.Text;

                            eb = textBox10.Text;

                            panel1.BackColor = Color.Red;
                            tabControl1.SelectedTab = tabPage5;
                            label16.Text = eb;
                            NodeCount = 0;

                            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen\\");

                            //   label14.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString() + "\\" + dataGridView3.CurrentRow.Cells[0].Value.ToString();
                            label14.Text = destPath + label16.Text + ".xml";
                            textBox11.Text = "0";
                            textBox12.Text = "1";
                            label1.Text = "0";

                            SollStkza(eb);
                            UpdateTree();
                            EB_Offen_Focus();
                            ResetSonigsten();
                            ResetArtikelAswahl();
                        }
            */


        }


        private void EB_Offen_Focus()
        {
            string searchValue = textBox10.Text + ".xml";

            dataGridView3.ClearSelection();
            dataGridView3.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells[0].Value.ToString().Equals(searchValue))
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                // MessageBox.Show(exc.Message);
            }

        }

        //Barcodescanner Aktivieren texbox
        private void tabPage1_Enter(object sender, EventArgs e)
        {
            textBox10.Focus();
            panel1.BackColor = Color.Green;

        }

        //Barcodescanner Aktivieren texbox
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                Refresch_XML();
                Refresch_EB_Offen();
                UpdateTree();
                textBox10.Clear();
                textBox10.Focus();

            }

            if (tabControl1.SelectedTab == tabPage2)
            {
             //   showImage();
            }

            if (tabControl1.SelectedTab == tabPage3)
            {


               var xmlinf= ddfront.fileLastmod(@"V:\EB_Kunde_Info\Kn_Prod_Info.xml",
                   @"V:\EB_Artikel\Artikel.xml", @"V:\EB_UNBEARBEITET\Belege_Unbearbeitet.xml", @"V:\EB_Artikel\Dienstleistungen.xml",
                   @"V:\EB_Artikel\Artikel_Lohnumbau.xml", @"V:\EB_Artikel\Artikel_Lohnreinigung.xml");

                label8.Text = xmlinf.KNinfo;
                label77.Text = xmlinf.Artikel;
                label79.Text = xmlinf.UNnBera;
                label81.Text = xmlinf.Dienstl;
                label83.Text = xmlinf.LU;
                label86.Text = xmlinf.LR;

            }


        }

        private void CheckEBEXIST(string ArtikelNummer)
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

            if (File.Exists(destPath + label16.Text + ".xml") == true)
            {


                NeuePos_XML(ArtikelNummer, int.Parse(textBox11.Text), lbSn.Text, (destPath + label16.Text + ".xml"));
            }
            else
            {
                NeuEB(ArtikelNummer);
            }

        }

        private void NeuEB(string Artikelnummer)

        {

            int nc = NodeCount + 1;
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

            XDocument xDokument = new XDocument(

            new XElement("Belege",
            new XElement("Beleg",
            new XElement("Belege.Datum", DateTime.Now.ToShortDateString()),
            new XElement("Belege.K_LagerStatus", "True"),
            new XElement("Belege.Belegnummer", label16.Text),
            new XElement("AuftragsAdressen.AdressNummer"),
            new XElement("BelegePositionen",
            new XElement("Belege",

            new XElement("BelegePosition",
                new XElement("BelegePositionen.ArtikelNummer", Artikelnummer),
                new XElement("BelegePositionen.K_Restinhalt", int.Parse(textBox11.Text)),
                new XElement("BelegePositionen.K_Seriennummer", lbSn.Text))))))); //1 Position

            xDokument.Save(destPath + label16.Text + ".xml");

            // MessageBox.Show("XML File created ! ");
            UpdateTree();

        }


        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void Schütz_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }


        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedHersteller_UI();
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedEinlauf_UI();
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedEinlauf_UI();
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedEinlauf_UI();
        }

        private void radioButton26_CheckedChanged(object sender, EventArgs e)
        {

            ShowSelectedVariante();

        }

        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedVariante();
        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedVariante();
        }

        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedVariante();
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedVariante();
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedVariante();
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedAuslauf();
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedAuslauf();
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedAuslauf();
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedAuslauf();
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedAuslauf();
        }

        private void radioButton27_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedZulassung();

        }

        private void radioButton28_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedZulassung();
        }

        private void radioButton30_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedNestbar();

        }

        private void radioButton29_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedNestbar();
        }

        private void radioButton32_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedEX();

        }

        private void radioButton31_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedEX();
        }

        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedBlasenfarbe();
        }

        private void radioButton33_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedBlasenfarbe();
        }

        private void radioButton35_CheckedChanged(object sender, EventArgs e)
        {
            ShowSelectedBlasenfarbe();
        }

        private void radioButton36_CheckedChanged(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label11.Text = "90010001";
            label10.Text = "90010001";

        }

        private void radioButton37_CheckedChanged(object sender, EventArgs e)
        {

            ResetArtikelAswahl();
            ShowSelektedSoniksten();

            label11.Text = "90010002";
            label10.Text = "90010002";

        }

        private void radioButton38_CheckedChanged(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label11.Text = "91000001";
            label10.Text = "91000001";
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label11.Text = "90000440";
            label10.Text = "90000440";

        }

        private void radioButton41_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void ResetArtikelAswahl()

        {

            button6.Enabled = false;
            button7.Enabled = false;
            label11.Text = "Bitte Artikel Wahlen";
            label10.Text = "Bitte Artikel Wahlen";

            foreach (Control control in groupBox1.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox2.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox3.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox4.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox5.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox6.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox7.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox8.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

            foreach (Control control in groupBox9.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }



        }

        private void ResetSonigsten()
        {

            foreach (Control control in groupBox10.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton rb = control as RadioButton;
                    rb.Checked = false;
                }
            }

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            UpdateTree();
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label11.Text);
                Mehrartikel(1, label11.Text, textBox12.Text);
            }
            else
            {
                Mehrartikel(0, label11.Text, textBox12.Text);
            }
            ResetArtikelAswahl();
            ResetSonigsten();
        }

        private void Mehrartikel(int n, string Artikel, string ANZ)
        {

            for (int i = 0; i < int.Parse(ANZ) - n; i++)
            {
                string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");
                int nc = NodeCount + 1;
                NeuePos_XML(Artikel, int.Parse(textBox11.Text), lbSn.Text, (destPath + label16.Text + ".xml"));

                UpdateTree();
            }

            textBox11.Text = "0";
            textBox12.Text = "1";

            ResetArtikelAswahl();
            ResetSonigsten();
        }

        private void radioButton39_CheckedChanged(object sender, EventArgs e)
        {

            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label11.Text = "90010008";
            label10.Text = "90010008";


        }

        private void button9_Click(object sender, EventArgs e)
        {
            moveEBtoFertigXML();
            Refresch_XML();
        }

    


        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            char[] MyChar = { 'x', 'm', 'l', '.' };
            textBox10.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString().TrimEnd(MyChar);
        }

 

        private void radioButton8_CheckedChanged_1(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton9_CheckedChanged_1(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton10_CheckedChanged_1(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton11_CheckedChanged_1(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void radioButton7_CheckedChanged_1(object sender, EventArgs e)
        {
            ShowSelecteDPalette_UI();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ResetSonigsten();

        }

        private void button13_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            macros.SPS_Komm_Akcept(out Socet1, out Socet2, out Socet3);
            toolStripStatusLabel1.BackColor = Color.LightGreen;
            Cursor = Cursors.Default;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = int.Parse(textBox15.Text);

            if (cbSPSAktiv.Checked == true)
            {
                macros.SPS_Komm_Read(Socet1, Socet3, out int Wal2, out int? Ap1, out int? Ap3, out int? KetteAP1_5, out int? Gewicht, out int? tWal2, out int? tAp1, out int? tAp3, out int? tKetteAP1_5, out int? tGewicht);
                label21.Text = Wal2.ToString();
                label29.Text = Ap1.ToString();

                label30.Text = Ap3.ToString();
                label31.Text = KetteAP1_5.ToString();
                label32.Text = Gewicht.ToString();
                label55.Text = Gewicht.ToString();

                label74.Text = tWal2.ToString();
                label71.Text = tAp1.ToString();

                label75.Text = Ap1.ToString();
                label42.Text = KetteAP1_5.ToString();
               
          
                label75.Text = Ap1.ToString();        // AP1 Status
                label42.Text = KetteAP1_5.ToString(); // Kette Status 
 

                label70.Text = tAp3.ToString();
                label69.Text = tKetteAP1_5.ToString();
              
                label68.Text = tGewicht.ToString();

                macros.SPS_Komm_Send(checkBox9.Checked, checkBox8.Checked, checkBox3.Checked, int.Parse(textBox13.Text), Socet2);
            }

            if (label31.Text == "1")
            {
                textBox13.Text = "9";

                camtrig = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
                checkBox3.Checked = false;
                radioButton43.Checked = false;


                MyStaticValues.camtrig2 = false;

            }




            if (label29.Text == "4" & MyStaticValues.camtrig2 == false)
            {
                MyStaticValues.camtrig2 = true;

            }

        }




        private void label75_TextChanged(object sender, EventArgs e)
        {
            if ((label75.Text == "4") & (cbKamAktiv.Checked == true))
            {
                m_pCamera.Triggr();
                m_pCamera2.Triggr();
                m_pCamera3.Triggr();
                m_pCamera4.Triggr();
                m_pCamera5.Triggr();

                timer3.Enabled = true;

            }

            if (label75.Text == "0")

            {
                checkBox7.BackColor = Color.Gray;
                checkBox8.BackColor = Color.Gray;
                checkBox9.BackColor = Color.Gray;
            }


            if ((label75.Text == "0")&&(dg4.RowCount >=4 ))

            {
                if (flag == 0)
                {
                    button10.PerformClick();
                    button4.PerformClick();
                    flag = 1;      //hilfsflag um click nur einmal zu performen pro neue Bilder
                }
            }
            
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click1(object sender, EventArgs e)
        {
            // CheckEBEXIST(label10.Text);


        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void UpdateSollStkZahl_Click(object sender, EventArgs e)
        {

            //  CMD("");
            System.Diagnostics.Process.Start("CMD.exe");
            //Q:\Versionen\WITT\app\AlphaplanSchnittstellen.exe - run DatenBereitstellung - set "\\Versionen\WITT\app\Data\BelegePositionen.apsdb"
            // Q:\Versionen\WITT\app\Launcher.exe - exe AlphaplanSchnittstellen.exe - job 8483bestellungenimport.als
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", @"V:\EB_Fertig");
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_KeyUp(object sender, KeyEventArgs e)
        {
            string destPath = @"V:\EB_Offen\";
            //    label14.Text = dataGridView3.CurrentCell.Value.ToString();
            label14.Text = destPath + dataGridView3.CurrentRow.Cells[0].Value.ToString();

            //  label14.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString() + "\\" + dataGridView3.CurrentRow.Cells[0].Value.ToString();
            char[] MyChar = { 'x', 'm', 'l', '.' };
            label16.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString().TrimEnd(MyChar);

            SollStkza(label16.Text);
            UpdateTree();
        }

        private void dataGridView3_KeyDown(object sender, KeyEventArgs e)
        {
            string destPath = @"V:\EB_Offen\";
            //    label14.Text = dataGridView3.CurrentCell.Value.ToString();
            label14.Text = destPath + dataGridView3.CurrentRow.Cells[0].Value.ToString();

            //  label14.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString() + "\\" + dataGridView3.CurrentRow.Cells[0].Value.ToString();
            char[] MyChar = { 'x', 'm', 'l', '.' };
            label16.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString().TrimEnd(MyChar);

            SollStkza(label16.Text);
            UpdateTree();
        }

        private void radioButton44_CheckedChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            ResetArtikelAswahl();
            ResetSonigsten();
        }

        private void radioButton45_CheckedChanged(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = Lohnarbeit;
            ResetArtikelAswahl();
            ResetSonigsten();

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }



        private void DrawButtons()
        {
            int LUbn = dataGridView5.Rows.Count;
            int LRbn = dataGridView6.Rows.Count;


            Point newLoc = new Point(5, 5); // Set whatever you want for initial location
            for (int i = 0; i < LRbn - 1; ++i)
            {
                Button b = new Button();

                b.Click += new EventHandler(this.Button3_Click);

                b.Size = new Size(300, 100);
                b.Location = newLoc;
                b.Text = dataGridView6.Rows[i].Cells[0].Value.ToString();
                b.Font = new Font(b.Font.FontFamily, 15);

                if (i % 6 == 0 & i > 0)
                {
                    // newLoc.X = 5;
                    newLoc.Y = 5;
                    newLoc.Offset(b.Width + 20, -(b.Height + 20));
                }

                newLoc.Offset(0, b.Height + 20);

                Lohnarbeit.Controls.Add(b);
            }

            Point newLoc2 = new Point(5, 5); // Set whatever you want for initial location
            for (int i = 0; i < LUbn - 1; ++i)
            {
                Button b = new Button();

                b.Click += new EventHandler(this.button_Click);

                b.Size = new Size(300, 100);
                b.Location = newLoc2;
                b.Text = dataGridView5.Rows[i].Cells[0].Value.ToString();
                b.Font = new Font(b.Font.FontFamily, 15);

                if (i % 6 == 0 & i > 0)
                {
                    // newLoc.X = 5;
                    newLoc2.Y = 5;
                    newLoc2.Offset(b.Width + 20, -(b.Height + 20));
                }

                newLoc2.Offset(0, b.Height + 20);
                tabPage4.Controls.Add(b);
            }



        }

        private void DrawDienstButtons()
        {
            int DIbn = dataGridView7.Rows.Count;


            Point newLoc2 = new Point(5, 5); // Set whatever you want for initial location
            for (int i = 0; i < DIbn - 1; ++i)
            {
                Button b = new Button();

                b.Click += new EventHandler(this.button_Click1);

                b.Size = new Size(300, 100);
                b.Location = newLoc2;
                b.Text = dataGridView7.Rows[i].Cells[0].Value.ToString();
                b.Font = new Font(b.Font.FontFamily, 15);



                if (i % 6 == 0 & i > 0)
                {
                    // newLoc.X = 5;
                    newLoc2.Y = 5;
                    newLoc2.Offset(b.Width + 20, -(b.Height + 20));
                }


                newLoc2.Offset(0, b.Height + 20);
                tabPage6.Controls.Add(b);
            }


        }

        private void label35_Click(object sender, EventArgs e)
        {

        }
        void button_Click1(object sender, EventArgs e)
        {
            // MessageBox.Show();
            label63.Text = ((Button)sender).Text;
            FilterDL(((Button)sender).Text);

        }


        void button_Click(object sender, EventArgs e)
        {
            // MessageBox.Show();
            label38.Text = ((Button)sender).Text;
            FilterLU();
        }

        void Button3_Click(object sender, EventArgs e)
        {
            label39.Text = ((Button)sender).Text;
            FilterLR();
        }

        private void FilterDL(string Bezeichnung)
        {
            DataView dv = ds5.Tables[0].DefaultView;
            dv.RowFilter = string.Format("Bezeichnung LIKE '%{0}%'", Bezeichnung);
            dataGridView5.DataSource = dv;

            if (dataGridView7.Rows[0].Cells[1].Value != null)
            {
                label66.Text = dataGridView7.Rows[0].Cells[1].Value.ToString();
            }
            else
            {
                label66.Text = "Artikel Nicht Gefunden!";
            }

        }





        private void FilterLU()
        {

            DataView dv = ds3.Tables[0].DefaultView;
            dv.RowFilter = string.Format("Bezeichnung LIKE '%{0}%'", label38.Text);
            dataGridView5.DataSource = dv;

            if (dataGridView5.Rows[0].Cells[1].Value != null)
            {
                label36.Text = dataGridView5.Rows[0].Cells[1].Value.ToString();
            }
            else
            {
                label36.Text = "Artikel Nicht Gefunden!";
            }
        }





        private void FilterLR()
        {
            DataView dv = ds4.Tables[0].DefaultView;
            dv.RowFilter = string.Format("Bezeichnung LIKE '%{0}%'", label39.Text);
            dataGridView5.DataSource = dv;

            if (dataGridView6.Rows[0].Cells[1].Value != null)
            {
                label41.Text = dataGridView6.Rows[0].Cells[1].Value.ToString();
            }
            else
            {
                label41.Text = "Artikel Nicht Gefunden!";
            }

        }



        private void button6_Click(object sender, EventArgs e)
        {
            // CheckEBEXIST(label11.Text);

            int Serie = 0;

            Serie = 1 + int.Parse(label1.Text);

          //  ddsql.InsertSQLDatei(label16.Text, label11.Text, int.Parse(textBox11.Text), label16.Text + Serie.ToString("000"));


            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label11.Text);
                Mehrartikel(1, label11.Text, textBox12.Text);
            }
            else
            {
                Mehrartikel(0, label11.Text, textBox12.Text);
            }

            AP3_SN(label11.Text, lbSn.Text);

            ResetArtikelAswahl();
           
            ResetSonigsten();

           

          //  DataTable dt = ddsql.DisplaySQLDatai();


            textBox13.Text = "1";

            if (label17.Text != "Soll Stückzahl nicht gefunden")
                SollgleichIST(int.Parse(label18.Text), int.Parse(label17.Text));


            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }



           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int nc = NodeCount + 1;
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

           // ddsql.InsertSQLDatei(label16.Text, label11.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label11.Text);
                Mehrartikel(1, label11.Text, textBox12.Text);
            }
            else
            {
                Mehrartikel(0, label11.Text, textBox12.Text);
            }


            AP3_SN(label11.Text, lbSn.Text);

            ResetArtikelAswahl();
            ResetSonigsten();

         

            textBox13.Text = "2";


            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            // CheckEBEXIST(label41.Text);
            int nc = NodeCount + 1;
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

         //   ddsql.InsertSQLDatei(label16.Text, label41.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label41.Text);
                Mehrartikel(1, label41.Text, textBox14.Text);
            }
            else
            {
                Mehrartikel(0, label41.Text, textBox14.Text);
            }



            AP3_SN(label41.Text, lbSn.Text);

            ResetArtikelAswahl();
            ResetSonigsten();

           

            textBox13.Text = "1";
            textBox14.Text = "1";

            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");
            int nc = NodeCount + 1;
         //   ddsql.InsertSQLDatei(label41.Text, label11.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label41.Text);
                Mehrartikel(1, label41.Text, textBox14.Text);
            }
            else
            {
                Mehrartikel(0, label41.Text, textBox14.Text);
            }


            AP3_SN(label41.Text, lbSn.Text);


            ResetArtikelAswahl();
            ResetSonigsten();

           

            textBox13.Text = "2";
            textBox14.Text = "1";

            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {


            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");
            int nc = NodeCount + 1;
           // ddsql.InsertSQLDatei(label16.Text, label36.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label36.Text);
                Mehrartikel(1, label36.Text, textBox18.Text);
            }
            else
            {
                Mehrartikel(0, label36.Text, textBox18.Text);
            }

            AP3_SN(label36.Text, lbSn.Text);

            ResetArtikelAswahl();
            ResetSonigsten();

          


            textBox18.Text = "1";
            textBox13.Text = "1";

            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }
        }

        private void button12_Click_2(object sender, EventArgs e)
        {
            int nc = NodeCount + 1;
         //   ddsql.InsertSQLDatei(label16.Text, label36.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));
            CheckEBEXIST(label36.Text);
            textBox13.Text = "2";
            textBox14.Text = "1";

            AP3_SN(label36.Text, lbSn.Text);

            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }


        }





        private void SollgleichIST(int soll, int ist)
        {

            if (soll == ist)
            {

                DialogResult dialogResult = MessageBox.Show("Soll Stückzahl erraicht soll FERTIG?", "Soll Stückzahl erraicht", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    moveEBtoFertigXML();

                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

                tabControl1.SelectedTab = tabPage1;
            }
        }
        ///////////////////////////////////////////////////////////
        ///



        public void ToConsole(string output)
        {
            //  ConsoleOut.AppendText(output + "\n");
        }



        private void button8_Click_2(object sender, EventArgs e)
        {



            macros.Camera_Config();
            toolStripStatusLabel2.BackColor = Color.LightGreen;
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox8.Checked == true)
            {
                checkBox3.Checked = true;
                tabControl1.SelectedTab = tabPage2;

            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                checkBox3.Checked = true;
                tabControl1.SelectedTab = tabPage2;
            }
        }



        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Sicher?", "Löschen EB", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                deleteEB_XML();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }



            Refresch_XML();
            Refresch_EB_Offen();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Process.Start("explorer", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen\\"));
        }

        private void button19_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }

        private void radioButton40_CheckedChanged_1(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label66.Text = "90000440";
            label63.Text = "Restinhalt";

        }

        private void radioButton39_CheckedChanged_1(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label66.Text = "90010008";


        }

        private void radioButton38_CheckedChanged_1(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label66.Text = "91000001";
            label63.Text = "Schrott";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");
            int nc = NodeCount + 1;
         //   ddsql.InsertSQLDatei(label16.Text, label66.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));


            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label66.Text);
                Mehrartikel(1, label66.Text, textBox17.Text);
            }
            else
            {
                Mehrartikel(0, label66.Text, textBox17.Text);
            }


            AP3_SN(label66.Text, lbSn.Text);

            ResetArtikelAswahl();
            ResetSonigsten();

          


            textBox17.Text = "1";
            textBox13.Text = "1";

            if (label17.Text != "Soll Stückzahl nicht gefunden")
                SollgleichIST(int.Parse(label18.Text), int.Parse(label17.Text));




            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");
            int nc = NodeCount + 1;
          //  ddsql.InsertSQLDatei(label16.Text, label66.Text, int.Parse(textBox11.Text), label16.Text + nc.ToString("000"));

            if (File.Exists(destPath + label16.Text + ".xml") == false)
            {
                NeuEB(label66.Text);
                Mehrartikel(1, label66.Text, textBox12.Text);
            }
            else
            {
                Mehrartikel(0, label66.Text, textBox12.Text);
            }


            AP3_SN(label66.Text, lbSn.Text);

            ResetArtikelAswahl();
            ResetSonigsten();

            

            textBox13.Text = "2";



            if (checkBox14.Checked == false)
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage1;
            }

            else
            {
                if (checkBox1.Checked == false)
                    tabControl1.SelectedTab = tabPage5;

            }
        }




        private void radioButton41_CheckedChanged_1(object sender, EventArgs e)
        {
            ResetArtikelAswahl();
            ShowSelektedSoniksten();
            label66.Text = "90000600";


        }

        private void button22_Click(object sender, EventArgs e)
        {
            UpdateXLMDatei();
        }

        private void radioButton45_CheckedChanged_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = Lohnarbeit;
            ResetArtikelAswahl();
            ResetSonigsten();
            radioButton45.Checked = false;
        }

        private void radioButton44_CheckedChanged_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            ResetArtikelAswahl();
            ResetSonigsten();
            radioButton44.Checked = false;
        }



        private void LoeschArtikel()
        {

            string destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EB_Offen//");

            XmlDocument doc = new XmlDocument();
            doc.Load(destPath + label16.Text + ".xml");

            XmlElement root = doc.DocumentElement;

            XmlNodeList nodeListCount = doc.GetElementsByTagName("BelegePosition");

            int i = dataGridView8.CurrentCell.RowIndex;

            nodeListCount[i].ParentNode.RemoveChild(nodeListCount[i]);

            doc.Save(destPath + label16.Text + ".xml");

            XMLtoTable();
            Refresch_EB_Offen();

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void löschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure", "Some Title", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                LoeschArtikel();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

        }

        private void dataGridView8_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);

        }

        private void dataGridView9_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }



        private void textBox10_Leave(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Red;
        }



        private void button27_Click_1(object sender, EventArgs e)
        {
            m_pCamera.StartAcquisition();
            m_pCamera2.StartAcquisition();
            m_pCamera3.StartAcquisition();
            m_pCamera4.StartAcquisition();
            m_pCamera5.StartAcquisition();
        }

        private void StartAcqui()
        {
            m_pCamera.StartAcquisition();
            m_pCamera2.StartAcquisition();
            m_pCamera3.StartAcquisition();
            m_pCamera4.StartAcquisition();
            m_pCamera5.StartAcquisition();
        }

        private void button28_Click_1(object sender, EventArgs e)
        {

        }



        private void button31_Click_1(object sender, EventArgs e)
        {


            if (cbKamAktiv.Checked == true)
            {
                m_pCamera.Triggr();
                m_pCamera2.Triggr();
                m_pCamera3.Triggr();
                m_pCamera4.Triggr();
                m_pCamera5.Triggr();

            }
            RefreschIBCList();
            CheckBilder.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_pCamera.CloseCamera();
            m_pCamera2.CloseCamera();
            m_pCamera3.CloseCamera();
            m_pCamera4.CloseCamera();
            m_pCamera5.CloseCamera();

        }


        private void button32_Click(object sender, EventArgs e)
        {

        //    ConnSQL();

        }

        private void pbH_0_Paint(object sender, PaintEventArgs e)
        {
            m_pCamera4.Repaint();


        }

        private void PictureBoxLive_Paint_1(object sender, PaintEventArgs e)
        {
            m_pCamera.Repaint();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            m_pCamera2.Repaint();
        }

        private void pbH_U_Paint(object sender, PaintEventArgs e)
        {
            m_pCamera3.Repaint();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            m_pCamera.Triggr();
            m_pCamera2.Triggr();
            m_pCamera3.Triggr();
            m_pCamera4.Triggr();
            m_pCamera5.Triggr();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            m_pCamera.OpenCamera(m_hDisplayWindow, m_pSystem);
            toolStripStatusLabel2.BackColor = Color.LightYellow;
            m_pCamera2.OpenCamera(m_hDisplayWindow2, m_pSystem2);
            toolStripStatusLabel3.BackColor = Color.LightYellow;
            m_pCamera3.OpenCamera(m_hDisplayWindow3, m_pSystem3);
            toolStripStatusLabel4.BackColor = Color.LightYellow;
            m_pCamera4.OpenCamera(m_hDisplayWindow4, m_pSystem4);
            toolStripStatusLabel5.BackColor = Color.LightYellow;
            m_pCamera5.OpenCamera(m_hDisplayWindow5, m_pSystem5);
            toolStripStatusLabel5.BackColor = Color.LightYellow;

            StartAcqui();
            toolStripStatusLabel2.BackColor = Color.LightGreen;
            toolStripStatusLabel3.BackColor = Color.LightGreen;
            toolStripStatusLabel4.BackColor = Color.LightGreen;
            toolStripStatusLabel5.BackColor = Color.LightGreen;
            Cursor = Cursors.Default;
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void refreschToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Refresch_XML();
            Refresch_EB_Offen();
        }

        private void eBFertigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moveEBtoFertigXML();
            Refresch_XML();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            RefreschIBCList();
            checkDir();
        }

 
        private void RefreschIBCList()
        {
            string destPath = @"C:\Users\Aufschrauberportal\AWICO\Technik - Witt IBC Bilder\";

            dg4.DataSource = new System.IO.DirectoryInfo(destPath).GetFiles("*.jpg");
            
        }

        private void dg4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string from, to;

            try
            {
                from = @"C:\Users\Aufschrauberportal\AWICO\Technik - Witt IBC Bilder\" + dg4.CurrentRow.Cells[0].Value.ToString();
                to = @"C:\Users\Aufschrauberportal\AWICO\IBC BAK\" + dg4.CurrentRow.Cells[0].Value.ToString() + label9.Text;

                File.Move(from, to); // Try to move
                Console.WriteLine("Moved"); // Success
                RefreschIBCList();
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine(ex); // Write error
            }

        }

        private void MoveBilder()
        {

            MoveAP3_IBCBAC();
            MoveAP2_AP3();
           // MoveAP1_AP2();

            string from, to;
            string destPath = @"C:\Users\Aufschrauberportal\AWICO\Technik - Witt IBC Bilder\";
            int x = new System.IO.DirectoryInfo(destPath).GetFiles().Length;
            int fileCount = Directory.GetFiles(destPath, "*.jpg").Length;



            RefreschIBCList();

            for (int i = 0; i < fileCount; i++)
            {
                from = @"C:\Users\Aufschrauberportal\AWICO\Technik - Witt IBC Bilder\" + dg4.Rows[i].Cells[0].Value.ToString();
                to = @"C:\Users\Aufschrauberportal\AWICO\AP2\" + dg4.Rows[i].Cells[0].Value.ToString();

                File.Move(from, to); // Try to move
            }

            RefreschIBCList();
        }

        private void MoveAP1_AP2()
        {

            try
            {
                string dest = @"C:\Users\Aufschrauberportal\AWICO\AP2";
                foreach (var file in Directory.EnumerateFiles(@"C:\Users\Aufschrauberportal\AWICO\AP1\"))
                {
                    string destFile = Path.Combine(dest, Path.GetFileName(file));
                    if (!File.Exists(destFile))
                        File.Move(file, destFile);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void MoveAP2_AP3()
        {
            {

                try
                {
                    string dest = @"C:\Users\Aufschrauberportal\AWICO\AP3\";
                    foreach (var file in Directory.EnumerateFiles(@"C:\Users\Aufschrauberportal\AWICO\AP2\"))
                    {
                        string destFile = Path.Combine(dest, Path.GetFileName(file));
                        if (!File.Exists(destFile))
                            File.Move(file, destFile);

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }


        }


        string AddPrefix(string filename, string Prefix)
        {
            string fDir = Path.GetDirectoryName(filename);
            string fName = Path.GetFileNameWithoutExtension(filename);
            string fExt = Path.GetExtension(filename);
            return Path.Combine(fDir, String.Concat(Prefix, fName, fExt));
        }

        private void AP3_SN(string ArtikelNr, String SN)
        {
            
               foreach (var file in Directory.EnumerateFiles(@"C:\Users\Aufschrauberportal\AWICO\AP3\"))
               {
                   string Newfilename = AddPrefix(file, lbSn.Text + "_" + ArtikelNr +"_");
                   File.Move(file, Newfilename);

               }
             
        }

        private void MoveAP3_IBCBAC()

        { 

            try
                {
                //Adding SN to EB
                   string dest = @"C:\Users\Aufschrauberportal\AWICO\Technik - IBC BAK\";

           

                //Move AP3 to IBC BACK
                foreach (var file in Directory.EnumerateFiles(@"C:\Users\Aufschrauberportal\AWICO\AP3\"))
                    {
                        string destFile = Path.Combine(dest, Path.GetFileName(file));
                        if (!File.Exists(destFile))
                            File.Move(file, destFile);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
   
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            BarcodeSuche();
        }

        private void BarcodeSuche()
        {
            RefreschIBCList();
            String BC;
            bool DC;
          //  macros.BarcodeRead(out BC, out DC);
           
            label9.Text = "Erkenung Barcode";
          //  label33.Text = BC;


        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            ArchivierenBild();
        }

        private void ArchivierenBild()
        {
            MoveBilder();
            label9.Text = "Bilder archieviert";
            RefreschIBCList();
        }



        private void CheckBilder_Tick(object sender, EventArgs e)
        {
            RefreschIBCList();
            bool DC;
            if (dg4.Rows.Count == 5)
            {
                CheckBilder.Enabled = false;
               
                RefreschIBCList();
                string BC;
               // macros.BarcodeRead(out BC, out DC);
               // label9.Text = BC;

             //   MoveBilder();

            }
        }

        private void btGoto_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\Aufschrauberportal\AWICO\Technik - Witt IBC Bilder\");
        }

        private void button9_Click_2(object sender, EventArgs e)
        {


        }

        private void button9_Click_3(object sender, EventArgs e)
        {
            showImage();
        }

        private void showImage()
        {
            Bilder_AP3 frmSecondForm = new Bilder_AP3();
            frmSecondForm.ShowDialog();

        }

      

        private void button10_Click_1(object sender, EventArgs e)
        {
            checkBox7.BackColor = Color.Gray;
            checkBox8.BackColor = Color.Gray;
            checkBox9.BackColor = Color.Gray;
            Hahhnerkenung();
           // Thread HahnerkenungThread = new Thread(Hahhnerkenung);
           // HahnerkenungThread.Start();

            backgroundWorker1.RunWorkerAsync();


         


        }

        private void BildQuality()
        {
            float ShVO, ShVU, ShHO, ShHU;
            macros.ImageQualityTest(out ShHU, out ShVU, out ShVO, out ShHO);
            label88.Text = ShVO.ToString();
            label90.Text = ShVU.ToString();
            label89.Text = ShHO.ToString();
            label91.Text = ShHU.ToString();
        }


        private void Hahhnerkenung()
        {
           
            bool hahnVU, hahnHU;

           
            macros.HahnerkennungMain(out hahnVU, out hahnHU);



            if (hahnVU == true)
            {
                checkBox8.BackColor = Color.Aqua;
               

                if (cbHahnAuto.Checked == true)
                {
                    checkBox8.Checked = true;
                    checkBox3.Checked = true;
                    tabControl1.SelectedTab = tabPage2;

                }
            }

            if (hahnHU == true)

            {
                checkBox9.BackColor = Color.Aqua;

                
                if (cbHahnAuto.Checked == true)
                {
                    checkBox9.Checked = true;
                    checkBox3.Checked = true;
                    tabControl1.SelectedTab = tabPage2;
                }

            }

            if ((hahnVU == false) && (hahnHU == false))
            {
              
                checkBox7.BackColor = Color.Red;
                
            }
            else
            {
                checkBox7.BackColor = Color.Gray;
               
            }


            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }


        }

    

        private void timer3_Tick(object sender, EventArgs e)
        {
            checkDir();

            if (label53.Text == "5")

            {
                // BarcodeSuche();

                Hahhnerkenung();

                if (cbbildQuality.Checked == true)
                {
                    BildQuality();
                }


                timer3.Enabled = false;
            }  
        }

        private void checkDir()
        {
            int count;
            string myDir = @"C:\Users\Aufschrauberportal\AWICO\Technik - Witt IBC Bilder";

            count = Directory.GetFiles(myDir, "*.jpg", SearchOption.AllDirectories).Length;

            label53.Text = count.ToString();
            
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            m_pCamera.StopAcquisition();
            m_pCamera2.StopAcquisition();
            m_pCamera3.StopAcquisition();
            m_pCamera4.StopAcquisition();
            m_pCamera5.StopAcquisition();

            m_pCamera.CloseCamera();
            toolStripStatusLabel2.BackColor = Color.LightYellow;
            m_pCamera2.CloseCamera();
            toolStripStatusLabel3.BackColor = Color.LightYellow;
            m_pCamera3.CloseCamera();
            toolStripStatusLabel4.BackColor = Color.LightYellow;
            m_pCamera4.CloseCamera();
            toolStripStatusLabel4.BackColor = Color.LightYellow;

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            UpdateXLMDatei();

           // Refresch_EB_Offen();
            DrawButtons();
            DrawDienstButtons();
            ResetSonigsten();
            ResetArtikelAswahl();
            Einschalt();
        }

      
        private void label42_TextChanged(object sender, EventArgs e)
        {
            if  (label42.Text == "1")
               
                {
                    MoveBilder();
                }
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            // backgroundWorker1.RunWorkerAsync();
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void button22_Click_2(object sender, EventArgs e)
        {
            BildQuality();
        }

        private void ConnSQL()
        {
          //  ddsql.ConnSQL();
            toolStripStatusLabel7.BackColor = Color.LightGreen;
        }

   
        private void dataGridView10_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ID = Convert.ToInt32(dataGridView10.Rows[e.RowIndex].Cells[0].Value.ToString());
        }

        private void button8_Click_4(object sender, EventArgs e)
        {
            ddsql.DeleteItem(ID);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Thread cam1 = new Thread(Cam1_00);
            cam1.Start();
        }

        private void Cam1_00()
        {
           // macros.RectifyFusion(image, image2, true, image3);

           // pictureBox1.Image?.Dispose();
           // if (image3.HasValue == true)
           //     pictureBox1.Image = image3.Value.CreateBitmap(); //Image -> Bitmap
         //   else
          //      ToConsole("Bild null");
          //  macros.ResetRectifyFusion();
          //  GC.Collect(); //Garbage Collector Call
        }

      

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void pb_DE_Paint(object sender, PaintEventArgs e)
        {
            m_pCamera5.Repaint();
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Green;
        }

        private void Einschalt()
        {
            Cursor = Cursors.WaitCursor;
         //   m_pCamera.OpenCamera(m_hDisplayWindow, m_pSystem);
         //   toolStripStatusLabel2.BackColor = Color.LightYellow;
          //  m_pCamera2.OpenCamera(m_hDisplayWindow2, m_pSystem2);
          //  toolStripStatusLabel3.BackColor = Color.LightYellow;
         //   m_pCamera3.OpenCamera(m_hDisplayWindow3, m_pSystem3);
         //   toolStripStatusLabel4.BackColor = Color.LightYellow;
         //   m_pCamera4.OpenCamera(m_hDisplayWindow4, m_pSystem4);
         //   toolStripStatusLabel5.BackColor = Color.LightYellow;
         //   StartAcqui();
         //   toolStripStatusLabel2.BackColor = Color.LightGreen;
         //   toolStripStatusLabel3.BackColor = Color.LightGreen;
         //   toolStripStatusLabel4.BackColor = Color.LightGreen;
         //   toolStripStatusLabel5.BackColor = Color.LightGreen;

         //   cbKamAktiv.Checked = true;

          //  macros.SPS_Komm_Akcept(out Socet1, out Socet2, out Socet3);
         //   toolStripStatusLabel1.BackColor = Color.LightGreen;
         //   cbSPSAktiv.Checked = true;



            Cursor = Cursors.Default;

        }

    }


    }
