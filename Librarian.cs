using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    internal class Librarian : User //унаследован от интерфейса User
    {
        public int librarianID;//ID
        public string login;//login
        public string password;//password
        public string lastName;//фамилия, 
        public string firstName;//имя, 
        public string middleName;//отчество;
        public DateTime birthday;//дата рождения;
        public string tel;//номер телефона.

        //конструкторы
        public Librarian() //  конструктор  с нуля
        {
            this.librarianID = 0;
            this.login = "admin";
            this.password = "admin";
            this.lastName = "Иванов";
            this.firstName = "Иван";
            this.middleName = "Иванович";
            this.birthday = Convert.ToDateTime("10.02.1999");
        }
        public Librarian(string file) //загружаем данные из указанного файла
        {
            string stroka;
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                this.password = streamReader.ReadLine(); //первая строка это пароль
                this.librarianID = Convert.ToInt32(streamReader.ReadLine()); //вторая сторка это id
                this.login = streamReader.ReadLine(); //третья строка это логин
                this.lastName = streamReader.ReadLine(); //четвертая  строка это фамилия
                this.firstName = streamReader.ReadLine(); //пятая строка это фамилия
                this.middleName = streamReader.ReadLine(); //шестая строка это отчетство
                this.birthday = Convert.ToDateTime(streamReader.ReadLine()); //седьмая строка это др
                this.tel = streamReader.ReadLine(); //восьмая строка это телефон             
                streamReader.Close();
            }
        }
        public Librarian(string login, string password,string lastName, string firstName, string middleName, DateTime birthday, string tel)
        {
            Random rnd = new Random();
            int value = rnd.Next(22222, 99999); //генерируем случайное id
            this.librarianID = value;
            this.login = login;
            this.password = password;
            this.lastName = lastName;
            this.firstName = firstName;
            this.middleName = middleName;
            this.birthday = birthday;
            this.tel = tel;
        }
        public void SaveInFile()//сохранение информации о сотруднике в файл 
        {
            string file = "\\Librarians\\" + this.login + ".txt";
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;

                streamWriter.WriteLine(this.password);//первая строка это пароль
                streamWriter.WriteLine(this.librarianID);//вторая сторка это id                
                streamWriter.WriteLine(this.login);//третья строка это логин
                streamWriter.WriteLine(this.lastName);//четвертая  строка это фамилия
                streamWriter.WriteLine(this.firstName);//пятая строка это фамилия
                streamWriter.WriteLine(this.middleName);//шестая строка это отчетство
                streamWriter.WriteLine(Convert.ToString(this.birthday));//седьмая строка это др
                streamWriter.WriteLine(this.tel); //восьмая строка это телефон       
                streamWriter.Close();
            }
        }
        public void DeleteFileUser()
        {
            //
        }
        public string GetFIO()
        {
           string FIO = this.lastName + " " + this.firstName + " " + this.middleName;
            return FIO; 
        }
    }
}
