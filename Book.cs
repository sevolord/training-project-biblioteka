using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    internal class Book
    {
        public int bookCode;//код издания;
        public string bookName;//название;
        public string bookAuthor;//автор;
        public string bookDescription;//текстовое описание.
        public bool inHands;//статус
        public string bookCurrentReader;//у кого на руках
        List<string> waitingReaders;//очередь ожидающих

        public Book(int bookCode, string bookName, string bookAuthor, string bookDescription)
        {
            this.bookCode = bookCode;
            this.bookName = bookName;
            this.bookAuthor = bookAuthor;
            this.bookDescription = bookDescription;
            this.waitingReaders = new List<string>();
            this.bookCurrentReader = "";
            this.inHands = false;
        }
        public Book(string file) //загружаем данные из указанного файла
        {
            string stroka;
            using (FileStream fileStream = new FileStream(file, FileMode.Open))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                this.bookName = streamReader.ReadLine(); //первая строка это название
                this.bookAuthor = streamReader.ReadLine(); //вторая сторка это автор 
                this.bookCode = Convert.ToInt16(streamReader.ReadLine()); //третья строка это код книги
                this.bookDescription = streamReader.ReadLine(); //четвертая  строка это описание
                this.inHands = Convert.ToBoolean(streamReader.ReadLine()); //пятая строка это на руках или нет
                this.bookCurrentReader = streamReader.ReadLine(); //шестая строка это у кого на руках
                int kolvoReaders = Convert.ToInt16(streamReader.ReadLine()); //седьмая строка это количество ожидающих
                //for (int i = 0; i < kolvoReaders; i++)
                //{
                //    streamWriter.WriteLine(Convert.ToString(this.waitingReaders[i])); //остальные строки это ожидающие
                //}
                streamReader.Close();
            }
        }

        public void SaveInFile()
        {
            string file = "\\Books\\" + this.bookName + "  " + this.bookAuthor + ".txt";
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;

                streamWriter.WriteLine(this.bookName);//первая строка это название
                streamWriter.WriteLine(this.bookAuthor);//вторая сторка это автор              
                streamWriter.WriteLine(this.bookCode);//третья строка это код книги
                streamWriter.WriteLine(this.bookDescription);//четвертая  строка это описание
                streamWriter.WriteLine(Convert.ToString(this.inHands));//пятая строка это на руках или нет
                streamWriter.WriteLine(this.bookCurrentReader);//шестая строка это у кого на руках
                int kolvoReaders = this.waitingReaders.Count();
                streamWriter.WriteLine(Convert.ToString(kolvoReaders));//седьмая строка это количество ожидающих
                for (int i = 0; i < kolvoReaders; i++)
                {
                    streamWriter.WriteLine(Convert.ToString(this.waitingReaders[i])); //остальные строки это ожидающие
                }
                streamWriter.Close();
            }
        }
        public void SaveInFileWaitingBook()
        {
            string file = "\\Books\\Waiting\\" + this.bookName + "  " + this.bookAuthor + ".txt";
            using (FileStream fileStream = new FileStream(file, FileMode.Create))
            {
                StreamWriter streamWriter = new StreamWriter(fileStream);
                streamWriter.AutoFlush = true;

                streamWriter.WriteLine(this.bookName);//первая строка это название
                streamWriter.WriteLine(this.bookAuthor);//вторая сторка это автор              
                streamWriter.WriteLine(this.bookCode);//третья строка это код книги
                streamWriter.WriteLine(this.bookDescription);//четвертая  строка это описание
                streamWriter.WriteLine(Convert.ToString(this.inHands));//пятая строка это на руках или нет
                streamWriter.WriteLine(this.bookCurrentReader);//шестая строка это у кого на руках
                int kolvoReaders = this.waitingReaders.Count();
                streamWriter.WriteLine(Convert.ToString(kolvoReaders));//седьмая строка это количество ожидающих
                for (int i = 0; i < kolvoReaders; i++)
                {
                    streamWriter.WriteLine(Convert.ToString(this.waitingReaders[i])); //остальные строки это ожидающие
                }
                streamWriter.Close();
            }
        }
        public void setInfoBook(int bookCode, string bookName, string bookAuthor, string bookDescription)
        {   //устанавливаем поля книги
            this.bookCode = bookCode;//код издания;
            this.bookName = bookName;//название;
            this.bookAuthor = bookAuthor;//автор;
            this.bookDescription = bookDescription;//текстовое описание.
        }
        public void DeleteFileBook() //удаляет файл указанного читателя
        {
            string file = "\\Books\\" + this.bookName + "  " + this.bookAuthor + ".txt";
            File.Delete(file);
        }

        public int GetbookCode()
        {
            return this.bookCode;
        }
        public String GetbookName()
        {
            return this.bookName;
        }
        public String GetbookAuthor()
        {
            return this.bookAuthor;
        }
        public String GetbookDescription()
        {
            return this.bookDescription;
        }
        public String GetbookStatus()
        {
            if ( this.inHands == true)
            {
                return "На руках";
            }
            else
            {
                return "В библиотеке";
            }
        }
        public void SetReder(string bookCurrentReader)
        {
            this.bookCurrentReader = bookCurrentReader; 
        }

    }
}
