using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class AMC : Form
    {
        bool isConnected = false; // Статус подключения.
        bool status = true; // Переменная для центральной кнопки ВКЛ/ВЫКЛ.
        int speed = 50; // Начальная скорость.
        int baudRate = 9600; // Скорость передачи при запуске программы.
        public AMC()
        {
            InitializeComponent();
        }

        private void AMC_Load(object sender, EventArgs e)
        {
            string[] portnames = SerialPort.GetPortNames(); // Получаем список COM портов доступных в системе.
            if (portnames.Length == 0) // Проверяем есть ли доступные.
            {
                MessageBox.Show("ERROR: Any COM PORT not found!"); // Выводим сообщение об ошибке.
            }
            foreach (string portName in portnames)
            {
                ComboBox1.Items.Clear(); // Очищаем все COM-порты.
                ComboBox1.Items.Add(portName); // Добавляем доступные COM-порты в список.
                Console.WriteLine(portnames.Length); // Выводим COM-порты в консоль.
                if (portnames[0] != null){ComboBox1.SelectedItem = portnames[0];} // Непонятная хероборина. Не трогать!
            }
            toolStripStatusLabel6.Text = "  Speed:" + "50"; // Изменяем скорость в статус баре.
            SerialPort1.BaudRate = baudRate; // Устанавливаем скорость передачи.
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // При закрытии программы, отправляем 0 и закрываем порт.
            if (SerialPort1.IsOpen) SerialPort1.Write("0"); SerialPort1.Close();
        }

        private void Button2_Click(object sender, EventArgs e) // Button +
        {
            if (isConnected){
                if (ProgressBar1.Value < 100){ // Если скорость меньше 100, то выполняем:
                    ProgressBar1.Value += 5; // Добавляем 5 к скорости.
                    speed = ProgressBar1.Value; // Значние переменной ставим равной значению прогресс-бара.
                    SerialPort1.Write(speed.ToString()); // Отправляем данные через последовательный порт.
                    toolStripStatusLabel6.Text = "  Speed:" + ProgressBar1.Value; // Обновляем статус-бар.
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e) // Button -
        {
            if (isConnected){
                if (ProgressBar1.Value > 0){ // Если скорость больше 0, то выполняем:
                    ProgressBar1.Value -= 5; // Отнимаем 5 от скорости.
                    speed = ProgressBar1.Value; // Значние переменной ставим равной значению прогресс-бара.
                    SerialPort1.Write(speed.ToString()); // Отправляем данные через последовательный порт.
                    toolStripStatusLabel6.Text = "  Speed:" + ProgressBar1.Value; // Обновляем статус-бар.
                }                
            }
        }

        private void Button3_Click(object sender, EventArgs e) // Middle button
        {
            if(status == true && isConnected){
                status = false; // Переменную статуса ставим равной false.
                speed = 0; // Значние переменной ставим равной 0.
                ProgressBar1.Value = 0; // Значние прогресс-бара ставим равной 0.
                SerialPort1.Write("0"); // Отправляем 0 через последовательный порт.
                toolStripStatusLabel6.Text = "  Speed:" + ProgressBar1.Value; // Обновляем статус-бар.
            }
            else if (status == false && isConnected){
                status = true; // Переменную статуса ставим равной true.
                speed = 100; // Значние переменной ставим равной 100.
                ProgressBar1.Value = 100; // Значние прогресс-бара ставим равной 100.
                SerialPort1.Write("100"); // Отправляем 100 через последовательный порт.
                toolStripStatusLabel6.Text = "  Speed:" + ProgressBar1.Value; // Обновляем статус-бар.
            }
        }      
        private void connectToArduino() // Connect function
        {
            isConnected = true; // Переменную подключения ставим равной true.
            string selectedPort = ComboBox1.GetItemText(ComboBox1.SelectedItem); // Ставим значние переменной selectedPort равной выбранному COM-порту.
            SerialPort1.PortName = selectedPort; // Устанавливаем выбранный COM-порт.
            SerialPort1.Open(); // Подключаемся к выбранному COM-порту.
            Button4.Text = "Disconnect"; // Надпись на кнопке подключения ставим: Disconnect (Отключиться).
            toolStripStatusLabel2.Text = "Connected"; // В статус баре пишем: Connected (Подключено).
            toolStripStatusLabel5.Text = SerialPort1.PortName; // Пишем в статус бар номер подключенного порта.
        }

        private void disconnectFromArduino() // Disconnect function
        {
            isConnected = false; // Переменную подключения ставим равной false.
            SerialPort1.Write("0"); // Отправляем 0 в последовательный порт.
            SerialPort1.Close(); // Закрываем последовательный порт.
            Button4.Text = "Connect"; // Надпись на кнопке подключения ставим: Connect (Подключиться).
            toolStripStatusLabel2.Text = "Not connected"; // В статус баре пишем: Not connected (Не подключено).
            toolStripStatusLabel5.Text = "COM0"; // Пишем в статус бар нулевой порт.
        }
        private void Button4_Click(object sender, EventArgs e) // Connect button
        {
            if (!isConnected){connectToArduino();} // Если не подключено, то выполняем функцию connectToArduino();
            else{disconnectFromArduino();} // Иначе выполняем функцию disconnectFromArduino();
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e) // Radiobutton 1
        {
            int baudRate = 9600; // Устанавливаем переменную скорости передачи 9600.
            SerialPort1.BaudRate = baudRate; // Устанавливаем скорость передачи равной переменной baudRate.
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e) // Radiobutton 2
        {
            int baudRate = 19200; // Устанавливаем переменную скорости передачи 19200.
            SerialPort1.BaudRate = baudRate; // Устанавливаем скорость передачи равной переменной baudRate.
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e) // Radiobutton 3
        {
            int baudRate = 115200; // Устанавливаем переменную скорости передачи 115200.
            SerialPort1.BaudRate = baudRate; // Устанавливаем скорость передачи равной переменной baudRate.
        }
    }
}
