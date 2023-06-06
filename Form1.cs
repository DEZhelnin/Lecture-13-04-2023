using System.ComponentModel;
namespace _13._04._2023_lec_dataBinding
{
    public partial class Form1 : Form
    {
        BindingList<Group> groups = new(); //прив€зываемый список
        private DBHelper dbh;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           var dbh = DBHelper.GetInstance("localhost", 3306, "root", "", "student");

            groups = dbh.GetGroups();//говорим, чему равен наш список
            dataGridView1.DataSource = groups;// использу€ свойство DataSource указываем источник инфы
        }

        private void увеличить√одЌа1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var group in groups)// проходимс€ по всему списку
            {
                group.Year++;
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newGr = new Group() // создаем экземпл€р в первой форме и передаем во вторую 
            {
                Department = "»ћћ",
                Year = DateTime.Now.Year,
            };
            var ef = new EditForm(newGr, true);//флаг говорит, что мы делаем с EditForm
                                               //мен€ем или добавл€ем
            if (ef.ShowDialog() == DialogResult.OK)// говорит о том, что форма закрылась успешно 
            {
                    groups.Add(newGr);
                
                
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {// делаетс€ по аналогии с добавлением, толь ко с поправкой на то,
         // что мы создаем не создаем newGr, а редактируем старую
         if (dataGridView1.SelectedRows.Count > 0) // выделена хот€ бы 1 строка
            {
                var index = dataGridView1.SelectedRows[0].Index; // получаем индекс, по которому 
                                                                // будем обращатьс€ в списке

                var gr = groups[index]; // получаем из списка конкретную группу по индексу
                var gr_copy = new Group();
                gr.CopyTo(gr_copy);  //создаем копию, с которой работаем в EditForm
                var ef  = new EditForm(gr_copy, false); // создаем новую форму и показываем ее
                if (ef.ShowDialog() == DialogResult.OK)//редактироваие завершилось успешно
                {
                    gr_copy.CopyTo(gr); // копируем данные обратно
                    // здесь по идее еще должен быть вызов UPDATE дл€ Ѕƒ
                }
            }

        }
    }
}