/*
Библиотека
 Приложение обладает следующим функционалом:
-просмотр, добавление и редактирование информации о читателях;
-просмотр, добавление и редактирование информации о книгах;
-оформление выдачи книги.
Приложение имеет два интерфейса:
1. для читателя
(читателя может просмотреть список книг, искать книгу по автору или по названию, если книги нет в наличии, то отправить заявку);
2.для сотрудника библиотеки
(сотрудник библиотеки может добавить, удалить или отредактировать информацию о книге, оформить выдачу и прием книг от
читателей, если вернули книгу, на которую есть заявка, то отображается уведомление).

Система хранит данный в файлах.
При регистрации читателя необходимо указать следующую информацию:
фамилия, имя, отчество;
дата рождения;
номер телефона.
После регистрации для каждого читателя также доступна информация:
номер читательского билета;
список книг, которые в данный момент у читателя;
список книг, которые читатель вернул.

При добавлении книги необходимо указать следующую информацию:
название;
автор;
код издания;
текстовое описание.
Также для каждой книги можно просмотреть статус(на руках или в библиотеке), очередь желающих взять книгу
*/
namespace Biblioteka
{
    public partial class FLibrary : Form
    {
        Librarian currentLibrarian;//объявляем текущего сотрудника 
        Reader currentReader;//объявляем текущего пользователя 
        Book currentBook;//текущая книга
        public FLibrary()
        {
            InitializeComponent();
            UpdateReaders();
            UpdateBooks();
        }

        private void BLogin_Click(object sender, EventArgs e)
        {
            //кнопка логина. Проверяет наличие файла пользователя и сравнивает пароль
            String username = TBLogin.Text;
            String pass = TBPassword.Text;
            String papka;
            if (RBLoginLibrarian.Checked) papka = "\\Librarians\\";
            else papka = "\\Readers\\";
            String file = papka + "\\" + username + ".txt";
            if (File.Exists(file)) //если файл этого пользователя есть 
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open)) //читаем файл
                {
                    StreamReader streamReader = new StreamReader(fileStream);
                    String parolfile = streamReader.ReadLine(); //читаем первую строку
                    streamReader.Close();
                    if (pass.Equals(parolfile))// если введенный пароль эквивалентен прочитанному из файла
                    {
                        if (RBLoginLibrarian.Checked) Authorization(true, file); // входим как сотрудник
                        else Authorization(false, file); // входим как читатель
                    }
                    else MessageBox.Show("Неправильный пароль!", "Ошибка", MessageBoxButtons.OK); //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.messagebox?view=windowsdesktop-6.0

                }
            }
            else { MessageBox.Show("Пользователь не найден !", "Ошибка", MessageBoxButtons.OK); }//всплывающее окно, что такой пользователь не найден 

        }

        private void Authorization(bool Librarian, String file)
        {
            //Удаляем вкладки, что бы остался только интерфейс для конкретного пользоватлея 
            //https://www.cyberforum.ru/vb-net/thread1410072.html
            tabControl1.TabPages.Remove(TPLogin);
            tabControl1.TabPages.Remove(TPRegister);
            if (Librarian)
            {
                //здесь нужно удалять вкладки читателя, но непонятно как
                //загружаем данные сотрудника
                currentLibrarian = new Librarian(file);
                this.Text = "Библиотека: сотрудник " + currentLibrarian.GetFIO();                
                tabControl1.TabPages.Remove(TPListBook);
            }
            else
            {
                currentReader = new Reader(file);
                this.Text = "Библиотека: читатель " + currentReader.GetFIO();
                //здесь нужно удалять вкладки сотрудника
                tabControl1.TabPages.Remove(TPBookWork);
                tabControl1.TabPages.Remove(TPReaderWork);
                
            }

        }

        private void BRegister_Click(object sender, EventArgs e)
        {
            //кнопка регистрации. Создает файл пользователя и заполняет его
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
            //Кнопка взятия книги. Присваевает выбранной книге что ее взял читатель, а читателю добавляет книгу в список
            currentBook.SetReder(currentReader.GetFIO());
            currentReader.AddBookInHands(currentBook.GetbookName() + " " + currentBook.GetbookAuthor());
            currentReader.SaveInFile();
            currentBook.SaveInFile();
        }

        private void BOrderBookReader_Click(object sender, EventArgs e)
        {
            //Выводит сообщение о том что заполните поля, добавляет книгу с отметкой что ее нужно заказать
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.messagebox?view=windowsdesktop-6.0 инструкция по всплывающему окну
            DialogResult result = MessageBox.Show("Заказать книгу, данные которые введены?", "Внимание!", MessageBoxButtons.OKCancel); 
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //создаем книгу
                currentBook = new Book(Convert.ToInt16(TBCodeBookReader.Text),
                    TBNameBookReader.Text,
                    TBAuthorBookReader.Text,
                    RTBCommentBookReader.Text
                    );
                currentBook.SaveInFileWaitingBook(); //сохраняем книгу для ожидания
                // и как-то эту книгу ждем...
            }
        }

        private void BRentBook_Click(object sender, EventArgs e)
        {
            //Кнопка выдачи книги. почти тоже самое что взятие книги, только еще читателя надо указать на вкладке с чителями
            currentBook.SetReder(currentReader.GetFIO());
            currentReader.AddBookInHands(currentBook.GetbookName() + " " + currentBook.GetbookAuthor());
            currentReader.SaveInFile();
            currentBook.SaveInFile();
        }

        private void BReturnBook_Click(object sender, EventArgs e)
        {
            //Кнопка возврата книги. Обнулить книге что она у кого то на руках и читателю отметки о ее взятии
            currentBook.SetReder("");
            //Дописать ее в список прочитанных книг читателя
            currentReader.AddBookReads(currentBook.GetbookName() + " " + currentBook.GetbookAuthor());
        }

        private void BAddBook_Click(object sender, EventArgs e)
        {
            //Добавляет файл книги
            currentBook = new Book(Convert.ToInt32(TBCodeBookLibrarian.Text),
                    TBNameBookLibrarian.Text,
                    TBAuthorBookLibrarian.Text,
                    RTBCommentBookLibrarian.Text
                    ); //конструктор объекта книг ис введенными данными
            currentBook.SaveInFile(); // сохраняем в файл
            //обновляем список книг
            UpdateBooks();
        }

        private void BEditBook_Click(object sender, EventArgs e)
        {
            //Перезаписывает файл книги с новыми данными
            currentBook.setInfoBook(Convert.ToInt32(TBCodeBookLibrarian.Text),
                    TBNameBookLibrarian.Text,
                    TBAuthorBookLibrarian.Text,
                    RTBCommentBookLibrarian.Text);
            currentBook.SaveInFile(); // сохраняем в файл
            //обновляем список книг
            UpdateBooks();
        }

        private void BDelBook_Click(object sender, EventArgs e)
        {
            //Удаляет файл книги
            currentBook.DeleteFileBook();
            UpdateBooks();
        }

        private void BAddREader_Click(object sender, EventArgs e)
        {
            //Добавляет читателя с указанными данными
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
            //Записывает заново указанного читателя
            //currentReader.Set - здесь записываем заново все поля текущего читателя
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
            //Удаляет файл указанного читателя
            currentReader.DeleteFileUser();
            UpdateReaders();
        }

        public void UpdateReaders()
        {
            //функция заполения списка читателей
            string papka = "\\Readers\\";
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.io.directory.getfiles?view=net-6.0 про список файлов
            string[] fileEntries = Directory.GetFiles(papka);
            foreach (string fileName in fileEntries)
            {
                LBReaderList.Items.Add(fileName);
            }
        }

        private void LBReaderList_Click(object sender, EventArgs e)
        {
            String file = LBReaderList.SelectedItem.ToString(); // берем текст выбранного пункта
            currentReader = new Reader(file);                   //открываем файл
            TBReaderTicket.Text = Convert.ToString(currentReader.GetTicketNumber()); //помещаем данные из файла в поля 
            TBLRLogin.Text = currentReader.GetLogin();
            TBLRPassword.Text = currentReader.GetPassword();
            TBLRName.Text = currentReader.GetfirstName();
            TBLRLastName.Text = currentReader.GetlastName();
            TBLRMidName.Text = currentReader.GetMiddleName();   
            DTPLRBirthday.Value = currentReader.GetBirthday();
            TBLRTelNum.Text = currentReader.GetTel();
            List<string> booksInHands = currentReader.GetbooksInHands();
            int kolvo = booksInHands.Count(); //количество книг на руках
            for (int i = 0; i < kolvo; i++)
            {
                LBRedersBooks.Items.Add(booksInHands[i]);
            }
            List<string> booksRead = currentReader.GetbooksRead();
            kolvo = booksRead.Count(); //количество книг прочитанных
            for (int i = 0; i < kolvo; i++)
            {
                LBRedersBooksRetrieved.Items.Add(booksRead[i]);
            }
        }
        public void UpdateBooks()
        {
            //функция заполения списка книг
            string papka = "\\Books\\";
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.io.directory.getfiles?view=net-6.0 про список файлов
            string[] fileEntries = Directory.GetFiles(papka);
            LBBookListLibrarian.Items.Clear();
            LBBookListReader.Items.Clear();
            foreach (string fileName in fileEntries)
            {
                LBBookListLibrarian.Items.Add(fileName);
                LBBookListReader.Items.Add(fileName);
            }
        }
        public void UpdateBooks(string search) //перегрузка
        {
            //функция заполения списка книг
            string papka = "\\Books\\";
            //https://docs.microsoft.com/ru-ru/dotnet/api/system.io.directory.getfiles?view=net-6.0 про список файлов
            string[] fileEntries = Directory.GetFiles(papka, search);
            LBBookListLibrarian.Items.Clear();
            LBBookListReader.Items.Clear();
            foreach (string fileName in fileEntries)
            {
                LBBookListLibrarian.Items.Add(fileName);
                LBBookListReader.Items.Add(fileName);
            }
        }

        private void LBBookListLibrarian_Click(object sender, EventArgs e) //когда нажимаем на книгу в списке
        {
            String file = LBBookListLibrarian.SelectedItem.ToString(); // берем текст выбранного пункта
            currentBook = new Book(file);                   //открываем файл

            TBCodeBookLibrarian.Text = Convert.ToString(currentBook.GetbookCode());
            TBStatusBookLibrarian.Text = currentBook.GetbookStatus();
            TBNameBookLibrarian.Text = currentBook.GetbookName();
            TBAuthorBookLibrarian.Text = currentBook.GetbookAuthor();
            RTBCommentBookLibrarian.Text = currentBook.GetbookDescription();
        }

        private void LBBookListReader_Click(object sender, EventArgs e)//когда нажимаем на книгу в списке
        {
            String file = LBBookListReader.SelectedItem.ToString(); // берем текст выбранного пункта
            currentBook = new Book(file); 
            //открываем файл
            TBCodeBookReader.Text = Convert.ToString(currentBook.GetbookCode());
            TBStatusBookReader.Text = currentBook.GetbookStatus();
            TBNameBookReader.Text = currentBook.GetbookName();
            TBAuthorBookReader.Text = currentBook.GetbookAuthor();
            RTBCommentBookReader.Text = currentBook.GetbookDescription();

        }

        private void BBookSearchReader_Click(object sender, EventArgs e)
        {
            string SearchText = "*" + TBBookSearchReader.Text + "*"; //звездочки нужны, что бы искалось, даже если название введено частично  
            UpdateBooks(SearchText);
        }

        private void BSearchBookLibrarian_Click(object sender, EventArgs e)
        {
            
            string SearchText = "*" + TBSearchBookLibrarian.Text + "*"; //звездочки нужны, что бы искалось, даже если название введено частично  
            UpdateBooks(SearchText);
        }
    }
}