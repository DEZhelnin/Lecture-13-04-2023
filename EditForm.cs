using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _13._04._2023_lec_dataBinding
{
    public partial class EditForm : Form
    {
        private Group _gr;
        private bool newRow;
        private Exception _bindingException;
        public EditForm(Group gr, bool newRow = false)
        {
            this.newRow = newRow;
            InitializeComponent();
            button1.Text = newRow ? "Добавить" : "Изменить"; //меняем название
                                                             //в зависимости от цели
            _gr = gr;
            // надо произвести привязку данных
            textBox1.DataBindings.Add(nameof(TextBox.Text), _gr, nameof(Group.Num));
            // первый параметр - имя
            // второй -  откуда берем данные
            // третий - название свойства, которе будем связывать
            numericUpDown1.DataBindings.Add(nameof(NumericUpDown.Value), _gr, nameof(Group.Year));
            textBox2.DataBindings.Add(nameof(TextBox.Text), _gr, nameof(Group.Spec));
            textBox3.DataBindings.Add(nameof(TextBox.Text), _gr, nameof(Group.Department));
            var cbBinding = comboBox1.DataBindings.Add(nameof(ComboBox.Text), _gr, nameof(Group.Level));
            // метод Add возвращаем нам объект класса Binding
            cbBinding.FormattingEnabled = true; // включаем механизм обработки исключений
            cbBinding.BindingComplete += CbBindingOnBindingComplete;// подписываемся на событие,
                                                                    // которое соответствует успешной
                                                                    // привязке данных
        }

        private void CbBindingOnBindingComplete(object? sender, BindingCompleteEventArgs e)
        {
            if (e.BindingCompleteState == BindingCompleteState.Exception)
            {
                e.Cancel = false; // не отменяем событие привязки
                comboBox1.BackColor = Color.LightCoral;
            }
            _bindingException = e.Exception;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_bindingException != null)
                {
                    MessageBox.Show("Указаны неверные данные");
                }
                else
                {
                    if (newRow) DBHelper.GetInstance().InsertNew(_gr); //сначала пытаемся добавить запись и посмотреть, что будет
                                                                       //при такой реализации в случае ошибки окно не закроется сразу
                                                                       //и пользователь сможет поправить 
                                                                       //else dbh.Update(_gr);
                    this.DialogResult = DialogResult.OK;


                }
            }
            catch
            {
                MessageBox.Show("Ошибка добавления новой записи в БД");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.BackColor = Color.White;
        }
    }
}
