using Dbcontextclass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OlympiadApp {
    public partial class Form1 : Form {
         public  DataClasses1DataContext _dbcontext = new DataClasses1DataContext();
        public Form1() {
            InitializeComponent();
        }


         private void SetDataSourceByName(string name) {
            switch (name) {

                case "Countries": dataGridView1.DataSource= _dbcontext.T_Countries; break;
                case "Cities": dataGridView1.DataSource = _dbcontext.T_Cities; break;
                case "Olympiads": dataGridView1.DataSource = _dbcontext.T_Olympiads; break;
                case "Sports": dataGridView1.DataSource = _dbcontext.T_Sports; break;
                case "SportTypes": dataGridView1.DataSource = _dbcontext.T_SportTypes; break;
                case "Players": dataGridView1.DataSource = _dbcontext.T_Players; break;
                case "CProfiles": dataGridView1.DataSource = _dbcontext.T_CProfiles; break;
                case "Medals": dataGridView1.DataSource = _dbcontext.T_Medals; break;
                default: throw new ArgumentException();
            }
        
        
        
        
        }






        private void Form1_Load(object sender, EventArgs e) {
            comboBox1.Items.Clear();
            for(int i = 0; i < DbDataConnect.TableNames.Count; i++) {
                comboBox1.Items.Insert(i,DbDataConnect.TableNames[i]);
            }

            comboBox3.Items.Clear();
            for (int i = 0; i < DbDataConnect.SelectNames.Count; i++) {
                comboBox3.Items.Insert(i, DbDataConnect.SelectNames[i]);
            }


            comboBox2.Items.Add("Alter");
            comboBox2.Items.Add("Select");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
            if (comboBox2.SelectedItem == "Alter") {

                label1.Visible = true;
                label3.Visible = true;
                comboBox1.Visible = true;
                button1.Visible = true;



                label2.Visible = false;
                comboBox3.Visible = false;
                label5.Visible = false;
                textBox1.Visible = false;
                button2.Visible = false;
            }
            else if(comboBox2.SelectedItem == "Select") {
                label2.Visible = true;
                comboBox3.Visible = true;
                label5.Visible= true;
                textBox1.Visible = true;
                button2.Visible = true;

                label1.Visible = false;
                label3.Visible = false;
                comboBox1.Visible = false;
                button1.Visible = false;


            }


            dataGridView1.DataSource = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var val = comboBox1.SelectedItem.ToString ();
            SetDataSourceByName(val);


        }

        private void button1_Click(object sender, EventArgs e) {
            _dbcontext.SubmitChanges();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) {




        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private async void button2_Click(object sender, EventArgs e) {
            int indx = comboBox3.SelectedIndex;
            string select = DbDataConnect.AllSelects[indx];
            if(indx > 3) {
                var words = textBox1.Text.ToString().Split(' ');
                for (int i = 0; i < words.Length; i++) {
                    select = select.Replace(i.ToString(), words[i]);
                }

            }


            dataGridView1.DataSource=await DbDataConnect.GetTableBySelect(select, DbDataConnect.GetConnectionStringByProvider("System.Data.SqlClient"));
        }

        private void label5_Click(object sender, EventArgs e) {

        }

        private void label2_Click(object sender, EventArgs e) {

        }

        private void label1_Click(object sender, EventArgs e) {

        }

        private void label3_Click(object sender, EventArgs e) {

        }
    }
}
