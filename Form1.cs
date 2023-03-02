/*
����������
 ���������� �������� ��������� ������������:
-��������, ���������� � �������������� ���������� � ���������;
-��������, ���������� � �������������� ���������� � ������;
-���������� ������ �����.
���������� ����� ��� ����������:
1. ��� ��������
(�������� ����� ����������� ������ ����, ������ ����� �� ������ ��� �� ��������, ���� ����� ��� � �������, �� ��������� ������);
2.��� ���������� ����������
(��������� ���������� ����� ��������, ������� ��� ��������������� ���������� � �����, �������� ������ � ����� ���� ��
���������, ���� ������� �����, �� ������� ���� ������, �� ������������ �����������).

������� ������ ������ � ������.
��� ����������� �������� ���������� ������� ��������� ����������:
�������, ���, ��������;
���� ��������;
����� ��������.
����� ����������� ��� ������� �������� ����� �������� ����������:
����� ������������� ������;
������ ����, ������� � ������ ������ � ��������;
������ ����, ������� �������� ������.

��� ���������� ����� ���������� ������� ��������� ����������:
��������;
�����;
��� �������;
��������� ��������.
����� ��� ������ ����� ����� ����������� ������(�� ����� ��� � ����������), ������� �������� ����� �����
*/
namespace Biblioteka
{
    public partial class FLibrary : Form
    {
        Librarian currentLibrarian;//��������� �������� ���������� 
        Reader currentReader;//��������� �������� ������������ 
        Book currentBook;//������� �����
        public FLibrary()
        {
            InitializeComponent();
            UpdateReaders();
            UpdateBooks();
        }

        private void BLogin_Click(object sender, EventArgs e)
        {
            //������ ������. ��������� ������� ����� ������������ � ���������� ������
            String username = TBLogin.Text;
            String pass = TBPassword.Text;
            String papka;
            if (RBLoginLibrarian.Checked) papka = "\\Librarians\\";
            else papka = "\\Readers\\";
            String file = papka + "\\" + username + ".txt";
            if (File.Exists(file)) //���� ���� ����� ������������ ���� 
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open)) //������ ����
                {
                    StreamReader streamReader = new StreamReader(fileStream);
                    String parolfile = streamReader.ReadLine(); //������ ������ ������
                    streamReader.Close();
                    if (pass.Equals(parolfile))// ���� ��������� ������ ������������ ������������ �� �����
                    {
                        if (RBLoginLibrarian.Checked) Authorization(true, file); // ������ ��� ���������
                        else Authorization(false, file); // ������ ��� ��������
                    }
                    else MessageBox.Show("������������ ������!", "������", MessageBoxButtons.OK); //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.messagebox?view=windowsdesktop-6.0

                }
            }
            else { MessageBox.Show("������������ �� ������ !", "������", MessageBoxButtons.OK); }//����������� ����, ��� ����� ������������ �� ������ 

        }

        private void Authorization(bool Librarian, String file)
        {
            //������� �������, ��� �� ������� ������ ��������� ��� ����������� ������������ 
            //https://www.cyberforum.ru/vb-net/thread1410072.html
            tabControl1.TabPages.Remove(TPLogin);
            tabControl1.TabPages.Remove(TPRegister);
            if (Librarian)
            {
                //����� ����� ������� ������� ��������, �� ��������� ���
                //��������� ������ ����������
                currentLibrarian = new Librarian(file);
                this.Text = "����������: ��������� " + currentLibrarian.GetFIO();                
                tabControl1.TabPages.Remove(TPListBook);
            }
            else
            {
                currentReader = new Reader(file);
                this.Text = "����������: �������� " + currentReader.GetFIO();
                //����� ����� ������� ������� ����������
                tabControl1.TabPages.Remove(TPBookWork);
                tabControl1.TabPages.Remove(TPReaderWork);
                
            }

        }

        private void BRegister_Click(object sender, EventArgs e)
        {
            //������ �����������. ������� ���� ������������ � ��������� ���
            String papka;
            if (RBRegLibrarian.Checked)
            {
                currentLibrarian = new Librarian(TBRegLogin.Text,
                    TBRegPassword.Text,
                    TBRegLastName.Text,
                    TBRegName.Text,
                    TBRegMidName.Text,
                    DTPRegBirthday.Value,
                    TBRegTelNum.Text);
                currentLibrarian.SaveInFile();

            }
            else papka = "\\Readers\\";
            {
                currentReader = new Reader(TBRegLogin.Text,
                    TBRegPassword.Text,
                    TBRegLastName.Text,
                    TBRegName.Text,
                    TBRegMidName.Text,
                    DTPRegBirthday.Value,
                    TBRegTelNum.Text
                    );
                currentReader.SaveInFile(); 
            }


        }

        private void BPickBookReader_Click(object sender, EventArgs e)
        {
            //������ ������ �����. ����������� ��������� ����� ��� �� ���� ��������, � �������� ��������� ����� � ������
            currentBook.SetReder(currentReader.GetFIO());
            currentReader.AddBookInHands(currentBook.GetbookName() + " " + currentBook.GetbookAuthor());
            currentReader.SaveInFile();
            currentBook.SaveInFile();
        }

        private void BOrderBookReader_Click(object sender, EventArgs e)
        {
            //������� ��������� � ��� ��� ��������� ����, ��������� ����� � �������� ��� �� ����� ��������
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.messagebox?view=windowsdesktop-6.0 ���������� �� ������������ ����
            DialogResult result = MessageBox.Show("�������� �����, ������ ������� �������?", "��������!", MessageBoxButtons.OKCancel); 
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //������� �����
                currentBook = new Book(Convert.ToInt16(TBCodeBookReader.Text),
                    TBNameBookReader.Text,
                    TBAuthorBookReader.Text,
                    RTBCommentBookReader.Text
                    );
                currentBook.SaveInFileWaitingBook(); //��������� ����� ��� ��������
                // � ���-�� ��� ����� ����...
            }
        }

        private void BRentBook_Click(object sender, EventArgs e)
        {
            //������ ������ �����. ����� ���� ����� ��� ������ �����, ������ ��� �������� ���� ������� �� ������� � ��������
            currentBook.SetReder(currentReader.GetFIO());
            currentReader.AddBookInHands(currentBook.GetbookName() + " " + currentBook.GetbookAuthor());
            currentReader.SaveInFile();
            currentBook.SaveInFile();
        }

        private void BReturnBook_Click(object sender, EventArgs e)
        {
            //������ �������� �����. �������� ����� ��� ��� � ���� �� �� ����� � �������� ������� � �� ������
            currentBook.SetReder("");
            //�������� �� � ������ ����������� ���� ��������
            currentReader.AddBookReads(currentBook.GetbookName() + " " + currentBook.GetbookAuthor());
        }

        private void BAddBook_Click(object sender, EventArgs e)
        {
            //��������� ���� �����
            currentBook = new Book(Convert.ToInt32(TBCodeBookLibrarian.Text),
                    TBNameBookLibrarian.Text,
                    TBAuthorBookLibrarian.Text,
                    RTBCommentBookLibrarian.Text
                    ); //����������� ������� ���� �� ���������� �������
            currentBook.SaveInFile(); // ��������� � ����
            //��������� ������ ����
            UpdateBooks();
        }

        private void BEditBook_Click(object sender, EventArgs e)
        {
            //�������������� ���� ����� � ������ �������
            currentBook.setInfoBook(Convert.ToInt32(TBCodeBookLibrarian.Text),
                    TBNameBookLibrarian.Text,
                    TBAuthorBookLibrarian.Text,
                    RTBCommentBookLibrarian.Text);
            currentBook.SaveInFile(); // ��������� � ����
            //��������� ������ ����
            UpdateBooks();
        }

        private void BDelBook_Click(object sender, EventArgs e)
        {
            //������� ���� �����
            currentBook.DeleteFileBook();
            UpdateBooks();
        }

        private void BAddREader_Click(object sender, EventArgs e)
        {
            //��������� �������� � ���������� �������
            currentReader = new Reader(TBLRLogin.Text,
                TBLRPassword.Text,
                TBLRLastName.Text,
                TBLRName.Text,
                TBLRMidName.Text,
                DTPLRBirthday.Value,
                TBLRTelNum.Text);
            currentReader.SaveInFile();
            UpdateReaders();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //���������� ������ ���������� ��������
            //currentReader.Set - ����� ���������� ������ ��� ���� �������� ��������
            currentReader.SetTicketNumber(Convert.ToInt16( TBReaderTicket.Text));
            currentReader.SetLogin(TBLRLogin.Text);
            currentReader.SetPassword(TBLRPassword.Text);
            currentReader.SetlastName(TBLRLastName.Text);
            currentReader.SetfirstName(TBLRName.Text);
            currentReader.SetMiddleName(TBLRMidName.Text);
            currentReader.SetBirthday(DTPLRBirthday.Value);
            currentReader.SetTel(TBLRTelNum.Text);
            
            currentReader.SaveInFile();
            UpdateReaders();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //������� ���� ���������� ��������
            currentReader.DeleteFileUser();
            UpdateReaders();
        }

        public void UpdateReaders()
        {
            //������� ��������� ������ ���������
            string papka = "\\Readers\\";
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.io.directory.getfiles?view=net-6.0 ��� ������ ������
            string[] fileEntries = Directory.GetFiles(papka);
            foreach (string fileName in fileEntries)
            {
                LBReaderList.Items.Add(fileName);
            }
        }

        private void LBReaderList_Click(object sender, EventArgs e)
        {
            String file = LBReaderList.SelectedItem.ToString(); // ����� ����� ���������� ������
            currentReader = new Reader(file);                   //��������� ����
            TBReaderTicket.Text = Convert.ToString(currentReader.GetTicketNumber()); //�������� ������ �� ����� � ���� 
            TBLRLogin.Text = currentReader.GetLogin();
            TBLRPassword.Text = currentReader.GetPassword();
            TBLRName.Text = currentReader.GetfirstName();
            TBLRLastName.Text = currentReader.GetlastName();
            TBLRMidName.Text = currentReader.GetMiddleName();   
            DTPLRBirthday.Value = currentReader.GetBirthday();
            TBLRTelNum.Text = currentReader.GetTel();
            List<string> booksInHands = currentReader.GetbooksInHands();
            int kolvo = booksInHands.Count(); //���������� ���� �� �����
            for (int i = 0; i < kolvo; i++)
            {
                LBRedersBooks.Items.Add(booksInHands[i]);
            }
            List<string> booksRead = currentReader.GetbooksRead();
            kolvo = booksRead.Count(); //���������� ���� �����������
            for (int i = 0; i < kolvo; i++)
            {
                LBRedersBooksRetrieved.Items.Add(booksRead[i]);
            }
        }
        public void UpdateBooks()
        {
            //������� ��������� ������ ����
            string papka = "\\Books\\";
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.io.directory.getfiles?view=net-6.0 ��� ������ ������
            string[] fileEntries = Directory.GetFiles(papka);
            LBBookListLibrarian.Items.Clear();
            LBBookListReader.Items.Clear();
            foreach (string fileName in fileEntries)
            {
                LBBookListLibrarian.Items.Add(fileName);
                LBBookListReader.Items.Add(fileName);
            }
        }
        public void UpdateBooks(string search) //����������
        {
            //������� ��������� ������ ����
            string papka = "\\Books\\";
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.io.directory.getfiles?view=net-6.0 ��� ������ ������
            string[] fileEntries = Directory.GetFiles(papka, search);
            LBBookListLibrarian.Items.Clear();
            LBBookListReader.Items.Clear();
            foreach (string fileName in fileEntries)
            {
                LBBookListLibrarian.Items.Add(fileName);
                LBBookListReader.Items.Add(fileName);
            }
        }

        private void LBBookListLibrarian_Click(object sender, EventArgs e) //����� �������� �� ����� � ������
        {
            String file = LBBookListLibrarian.SelectedItem.ToString(); // ����� ����� ���������� ������
            currentBook = new Book(file);                   //��������� ����

            TBCodeBookLibrarian.Text = Convert.ToString(currentBook.GetbookCode());
            TBStatusBookLibrarian.Text = currentBook.GetbookStatus();
            TBNameBookLibrarian.Text = currentBook.GetbookName();
            TBAuthorBookLibrarian.Text = currentBook.GetbookAuthor();
            RTBCommentBookLibrarian.Text = currentBook.GetbookDescription();
        }

        private void LBBookListReader_Click(object sender, EventArgs e)//����� �������� �� ����� � ������
        {
            String file = LBBookListReader.SelectedItem.ToString(); // ����� ����� ���������� ������
            currentBook = new Book(file); 
            //��������� ����
            TBCodeBookReader.Text = Convert.ToString(currentBook.GetbookCode());
            TBStatusBookReader.Text = currentBook.GetbookStatus();
            TBNameBookReader.Text = currentBook.GetbookName();
            TBAuthorBookReader.Text = currentBook.GetbookAuthor();
            RTBCommentBookReader.Text = currentBook.GetbookDescription();

        }

        private void BBookSearchReader_Click(object sender, EventArgs e)
        {
            string SearchText = "*" + TBBookSearchReader.Text + "*"; //��������� �����, ��� �� ��������, ���� ���� �������� ������� ��������  
            UpdateBooks(SearchText);
        }

        private void BSearchBookLibrarian_Click(object sender, EventArgs e)
        {
            
            string SearchText = "*" + TBSearchBookLibrarian.Text + "*"; //��������� �����, ��� �� ��������, ���� ���� �������� ������� ��������  
            UpdateBooks(SearchText);
        }
    }
}