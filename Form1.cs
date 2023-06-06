using System.ComponentModel;
namespace _13._04._2023_lec_dataBinding
{
    public partial class Form1 : Form
    {
        BindingList<Group> groups = new(); //������������� ������
        private DBHelper dbh;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           var dbh = DBHelper.GetInstance("localhost", 3306, "root", "", "student");

            groups = dbh.GetGroups();//�������, ���� ����� ��� ������
            dataGridView1.DataSource = groups;// ��������� �������� DataSource ��������� �������� ����
        }

        private void ��������������1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var group in groups)// ���������� �� ����� ������
            {
                group.Year++;
            }
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newGr = new Group() // ������� ��������� � ������ ����� � �������� �� ������ 
            {
                Department = "���",
                Year = DateTime.Now.Year,
            };
            var ef = new EditForm(newGr, true);//���� �������, ��� �� ������ � EditForm
                                               //������ ��� ���������
            if (ef.ShowDialog() == DialogResult.OK)// ������� � ���, ��� ����� ��������� ������� 
            {
                    groups.Add(newGr);
                
                
            }
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {// �������� �� �������� � �����������, ���� �� � ��������� �� ��,
         // ��� �� ������� �� ������� newGr, � ����������� ������
         if (dataGridView1.SelectedRows.Count > 0) // �������� ���� �� 1 ������
            {
                var index = dataGridView1.SelectedRows[0].Index; // �������� ������, �� �������� 
                                                                // ����� ���������� � ������

                var gr = groups[index]; // �������� �� ������ ���������� ������ �� �������
                var gr_copy = new Group();
                gr.CopyTo(gr_copy);  //������� �����, � ������� �������� � EditForm
                var ef  = new EditForm(gr_copy, false); // ������� ����� ����� � ���������� ��
                if (ef.ShowDialog() == DialogResult.OK)//������������� ����������� �������
                {
                    gr_copy.CopyTo(gr); // �������� ������ �������
                    // ����� �� ���� ��� ������ ���� ����� UPDATE ��� ��
                }
            }

        }
    }
}