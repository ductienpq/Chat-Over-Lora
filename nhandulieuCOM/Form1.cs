using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// Thêm 3 em này vào là OK, để sài SerialPort
using System.IO;
using System.IO.Ports;
using System.Xml;
// Bắt đầu code

namespace nhandulieuCOM
{
    public partial class Form1 : Form
    {
        string InputData = String.Empty; // Khai báo string buff dùng cho hiển thị dữ liệu sau này.
        delegate void SetTextCallback(string text); // Khai bao delegate SetTextCallBack voi tham so string
        public Form1()
        {
            InitializeComponent();
            // Khai báo hàm delegate bằng phương thức DataReceived của Object SerialPort;
            // Cái này khi có sự kiện nhận dữ liệu sẽ nhảy đến phương thức DataReceive
            // Nếu ko hiểu đoạn này bạn có thể tìm hiểu về Delegate, còn ko cứ COPY . Ko cần quan tâm
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceive);
            string[] BaudRate = { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" };
            comboBox2.Items.AddRange(BaudRate);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();
            comboBox2.SelectedIndex = 3;
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
            {

                label5.Text = ("Chưa kết nối");
                label5.ForeColor = Color.Red;
            }
            else if (serialPort1.IsOpen)
            {

                label5.Text = ("Đã kết nối");
                label5.ForeColor = Color.Green;

            }
        }
        private void DataReceive(object obj, SerialDataReceivedEventArgs e)
        {
            InputData = serialPort1.ReadExisting();
            if (InputData != String.Empty)
            {
                // txtIn.Text = InputData; // Ko dùng đc như thế này vì khác threads .
                SetText(InputData); // Chính vì vậy phải sử dụng ủy quyền tại đây. Gọi delegate đã khai báo trước đó.
            }

        }
        // Hàm của em nó là ở đây. Đừng hỏi vì sao lại thế.
        private void SetText(string text)
        {
            if (this.textBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText); // khởi tạo 1 delegate mới gọi đến SetText
                this.Invoke(d, new object[] { text });
            }
            else this.textBox1.Text += text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] st = new string[10];
            if (txtID.Text != "" && !serialPort1.IsOpen)
            {
                i++;
              
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                serialPort1.Open();
                string[] Member = { txtID.Text };
                st[i] = txtID.Text;
             //   comboBox3.Items.AddRange(Member);
                listMember.Items.AddRange(Member);
                txtID.Enabled = false;
            }
     /*       for(int m= 0; m<=10; m++)
            {
                for (int n = m; n <= 10; n++)
                    if (st[n] == st[m]) st[n] = "";       
            }
            comboBox3.Items.AddRange(st);*/
            else MessageBox.Show("Bạn chưa nhập tên!","Cảnh báo");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            txtID.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string d = richTextBox1.Text;
            serialPort1.Write(d);
            richTextBox1.Text = "";


        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //MessageBox.Show("Cuộn");
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();

            }
            else
            {
                ContextMenu blankContextMenu = new ContextMenu();
                textBox1.ContextMenu = blankContextMenu;
                //MessageBox.Show("Ko Cuộn");
            }
        }

    }


}

