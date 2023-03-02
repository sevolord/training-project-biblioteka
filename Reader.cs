using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    internal class Reader : User //унаследован от интерфейса User
    {
        public int readersTicketNumber;//номер читательского билета;
        public string login;//login
        public string password;//password
        public string lastName;//фамилия, 
        public string firstName;//имя, 
        public string middleName;//отчество;
        public DateTime birthday;//дата рождения;
        public string tel;//номер телефона.
        List<string> booksInHands;//список книг, которые в данный момент у читателя;
        List<string> booksRead;//список книг, которые читатель вернул.
        //конструкторы
        public Reader(string login, string password, string lastName, string firstName, string middleName, DateTime birthday, string tel)
        {
            Random rnd = new Random();
            int value = rnd.Next(22222, 99999); //генерируем случайное id
            this.readersTicketNumber = value;
            this.login = login;
            this.password = password;
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = middleName;
            this.birthday = birthday;
            this.tel = tel;
            this.booksInHands = new List<string>();
            this.booksRead = new List<string>();
        }
        public Reader(string file) //перегрузка: загружаем данные из указанного файла
        {
            string stroka;
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                this.password = streamReader.ReadLine(); //первая строка это пароль
                this.readersTicketNumber = Convert.ToInt32(streamReader.ReadLine()); //вторая сторка это id
                this.login = streamReader.ReadLine(); //третья строка это логин
                this.lastName = streamReader.ReadLine(); //четвертая  строка это фамилия
                this.firstName = streamReader.ReadLine(); //пятая строка это имя
                this.middleName = streamReader.ReadLine(); //шестая строка это отчетство
                this.birthday = Convert.ToDateTime(streamReader.ReadLine()); //седьмая строка это др
                this.tel = streamReader.ReadLine(); //восьмая строка это телефон
                int kolvo = Convert.ToInt16(streamReader.ReadLine()); //девятая строка это кол-во книг на руках
                this.booksInHands = new List<string>();
                for (int i = 0; i < kolvo; i++) // строки с книгами на руках
                {
                    this.booksInHands.Add(streamReader.ReadLine());
                }
                kolvo = Convert.ToInt16(streamReader.ReadLine()); //потом строка это кол-во книг прочитанных
                this.booksRead = new List<string>();
                for (int i = 0; i < kolvo; i++) // строки с книгами прочитанными
                {
                    this.booksRead.Add(streamReader.ReadLine());
                }
                streamReader.Close();
            }
        }
        public void SaveInFile() //сохраняем читателя в файл
        {
            string file = "\\Readers\\" + this.login + ".txt";
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;

                streamWriter.WriteLine(this.password);//первая строка это пароль
                streamWriter.WriteLine(this.readersTicketNumber);//вторая сторка это номер читательского               
                streamWriter.WriteLine(this.login);//третья строка это логин
                streamWriter.WriteLine(this.lastName);//четвертая  строка это фамилия
                streamWriter.WriteLine(this.firstName);//пятая строка это имя
                streamWriter.WriteLine(this.middleName);//шестая строка это отчетство
                streamWriter.WriteLine(Convert.ToString(this.birthday));//седьмая строка это др
                streamWriter.WriteLine(this.tel); //восьмая строка это телефон                
                int kolvo = this.booksInHands.Count();
                streamWriter.WriteLine(Convert.ToString(kolvo));  //девятая строка это кол-во книг на руках 
                for (int i = 0; i < kolvo; i++) // строки с книгами на руках
                {
                    streamWriter.WriteLine(Convert.ToString(this.booksInHands[i]));
                }
                kolvo = this.booksRead.Count();
                streamWriter.WriteLine(Convert.ToString(kolvo));  //потом строка это кол-во книг прочитанных
                for (int i = 0; i < kolvo; i++)// строки с книгами прочитанными
                {
                    streamWriter.WriteLine(Convert.ToString(this.booksRead[i]));
                }
                streamWriter.Close();
            }
        }
        public void DeleteFileUser() //удаляет файл указанного читателя
        {
            string file = "\\Readers\\" + this.login + ".txt";
            File.Delete(file);
        }
        //конструкции get - возращают данные о читателе
        public string GetFIO()
        {
            string FIO = this.lastName + " " + this.firstName + " " + this.middleName;
            return FIO;
        }

        public int GetTicketNumber()//номер читательского билета;
        {
            return this.readersTicketNumber;
        }
        public string GetLogin()//login
        {
            return this.login;
        }
        public string GetPassword()//password
        {
            return this.password;
        }
        public string GetlastName()//фамилия, 
        {
            return this.lastName;
        }
        public string GetfirstName()//имя, 
        {
            return this.firstName;
        }
        public string GetMiddleName()//отчество, 
        {
            return this.middleName;
        }        
        public DateTime GetBirthday()//дата рождения;
        {
            return this.birthday;
        }
        public string GetTel()//номер телефона, 
        {
            return this.tel;
        }

        public List<string> GetbooksInHands()//список книг, которые в данный момент у читателя;
        {
            return this.booksInHands;
        }
        public List<string> GetbooksRead()//список книг, которые читатель вернул.
        {
            return this.booksRead;
        }

        //конструкции set - записывают данные о читателе
        public void SetTicketNumber(int readersTicketNumber)//номер читательского билета;
        {
            this.readersTicketNumber= readersTicketNumber;
        }
        public void SetLogin(string login)//login
        {
            this.login = login;
        }
        public void SetPassword(string password)//password
        {
            this.password = password;
        }
        public void SetlastName(string lastName)//фамилия, 
        {
            this.lastName = lastName;
        }
        public void SetfirstName(string firstName)//имя, 
        {
            this.firstName = firstName;
        }
        public void SetMiddleName(string middleName)//отчество, 
        {
            this.middleName = middleName;
        }
        public void SetBirthday(DateTime birthday)//дата рождения;
        {
            this.birthday = birthday;
        }
        public void SetTel(string tel)//номер телефона, 
        {
            this.tel = tel;
        }

        public void SetTooksInHands(List<string> booksInHands)//список книг, которые в данный момент у читателя;
        {
            this.booksInHands = booksInHands;
        }
        public void SetbooksRead(List<string> booksRead)//список книг, которые читатель вернул.
        {
            this.booksRead = booksRead;
        }
        public void AddBookInHands(string booksInHands)//добавляет книгу на руки в список
        {
            this.booksInHands.Add(booksInHands);
        }
        public void AddBookReads(string bookRead)//доабвлет книгу в список 
        {
            this.booksRead.Add(bookRead);
        }


    }
}
