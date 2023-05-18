using Dictionaries;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace Dictionaries
{


    internal class Program
    {



        static void Main(string[] args)
        {

            FileStream file;
            DataContractJsonSerializer _json = new DataContractJsonSerializer(typeof(Vocabularies));

            Vocabularies vocabularies = new Vocabularies();

            try
            {
                file = new FileStream("Dictionaries.json", FileMode.Open);
                vocabularies = (Vocabularies)_json.ReadObject(file);
                file.Close();

            }catch (FileNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Словари не были загружены\n\n");
            }



            int choice = 0;
            do
            {
                vocabularies.OutputShortDictionary();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("________________________");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("0) Закрыть программу");
                Console.WriteLine("1) Добавить словарь");
                Console.WriteLine("2) Редактировать словарь");
                Console.WriteLine("3) Удалить словарь");
                Console.WriteLine("4) Показать словарь");
                Console.WriteLine("5) Найти слово");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Введите действие: ");
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (Exception ex) { choice = 1; Console.Clear(); continue; }
                switch (choice)
                {
                    case 0:
                        {
                            file = new FileStream("Dictionaries.json", FileMode.Create);
                            _json.WriteObject(file, vocabularies);
                            file.Close();
                            return;
                        }
                    case 1:
                        {
                            vocabularies.AddVocabularies();
                            break;
                        }
                    case 2:
                        {
                            vocabularies.EditDictionary();
                            break;
                        }
                    case 3:
                        {
                            vocabularies.RemoveVocabularies();
                            break;
                        }
                    case 4:
                        {
                            vocabularies.OutputFullDictionary();
                            break;
                        }
                    case 5:
                        {
                            vocabularies.SearchWord();
                            break;
                        }
                }
                Thread.Sleep(1000);
                file = new FileStream("Dictionaries.json", FileMode.Create);
                _json.WriteObject(file, vocabularies);
                file.Close();
                Console.Clear();

            } while (choice != 0);
        }


       

    }




    /*=========================================================
                           КЛАСС СЛОВАРИ
     ========================================================*/
    [DataContract]
    public class Vocabularies
    {
        [DataMember]
        public List<dictionary> vocabularies { set; get; }
        public Vocabularies()
        {
            vocabularies = new List<dictionary>();
        }

        // Добавляем словарь
        public void AddVocabularies() 
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\t(Пример: англо - русский)");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите тип словаря: ");
            string nameDictionary = Console.ReadLine();
            if (ChakBack(nameDictionary, true)) return;
            dictionary tempDictionary = new dictionary(nameDictionary);
            vocabularies.Add(tempDictionary);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\tСловарь успешно создан!!!");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }


        // Удаляем словарь
        public void RemoveVocabularies() 
        {
            OutputShortDictionary();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите номер словаря который хотите удалить: ");
            string indx = Console.ReadLine();
            if (ChakBack(indx, false)) return;
            while(int.Parse(indx) > vocabularies.Count)
            {
                Console.Write("Вы ввели ошибочный номер словаря!!!\nВведите заново: ");
                indx = Console.ReadLine();
                if (ChakBack(indx, false)) return;
            }
            vocabularies.Remove(vocabularies[int.Parse(indx) - 1]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\tСловарь удален!\n");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }


        // Редактирование словаря
        public void EditDictionary()
        {
            OutputShortDictionary();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите номер словаря который хотите редактировать: ");
            int indx;
            string MaybeMistake = Console.ReadLine();
            if (ChakBack(MaybeMistake, true)) return;
            try
            {
                while (int.Parse(MaybeMistake) > vocabularies.Count)
                {
                    Console.Write("Вы ввели ошибочный номер словаря!!!\nВведите заново: ");
                    MaybeMistake = Console.ReadLine();
                    if (ChakBack(MaybeMistake, false)) return;
                }
            }
            catch (Exception ex) { Console.Write("Вы что то ввели не правильно!!!"); return; };
            indx = int.Parse(MaybeMistake) - 1;
           
            int choice = 0;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan; 
                Console.WriteLine($"_______ {vocabularies[indx].nameDictionary} _______");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("1) Изменить тип");
                Console.WriteLine("2) Добавить слово");
                Console.WriteLine("3) Удалить слово");
                Console.WriteLine("4) Изменить слово");
                Console.WriteLine("5) Добавить перевод");
                Console.WriteLine("6) Удалить перевод");
                Console.WriteLine("7) Изменить перевод");
                Console.WriteLine("8) Вывести словарь");
                Console.WriteLine("0) Закрыть редактор");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Введите действие: ");

                try { choice = int.Parse(Console.ReadLine()); } 
                catch(Exception ex) { Console.WriteLine("\n\n\t\tВы ввели неверное действие"); Thread.Sleep(1000); choice++; continue; }

                switch (choice)
                {
                    case 1:
                        {
                            vocabularies[indx].ChangeNameDictionary();
                            break;
                        }
                    case 2:
                        {
                            vocabularies[indx].AddWord();
                            break;
                        }
                    case 3:
                        {
                            vocabularies[indx].RemoveWord();
                            break;
                        }
                    case 4:
                        {
                            vocabularies[indx].ChangeWord();
                            break;
                        }
                    case 5:
                        {
                            vocabularies[indx].AddTranslate();
                            break;
                        }
                    case 6:
                        {
                            vocabularies[indx].RemoveTranslate();
                            break;
                        }
                    case 7:
                        {
                            vocabularies[indx].ChangeTranslate();
                            break;
                        }
                    case 8:
                        {
                            OutputDictionary(indx);
                            break;
                        }
                }
            } while (choice != 0);
        }


        // Вывод только названия словаря и его порядковый номер
        public void OutputShortDictionary()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            int i = 1;
            foreach (dictionary dictionary in vocabularies)
            {
                Console.WriteLine($"#{i} {dictionary.nameDictionary}");
                i++;
            }
            return;
        }


        // Вывод словаря с его наполнением
        public void OutputFullDictionary()
        {
            OutputShortDictionary();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите номер словаря который хотите вывести: ");
            int indx = int.Parse(Console.ReadLine());
            if (ChakBack(indx.ToString(), true)) return;
            indx--;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\t_______ {vocabularies[indx].nameDictionary} _______");
            Console.ForegroundColor = ConsoleColor.White;
            vocabularies[indx].OutputFullWord();
            Console.Write("нажмите любую клавишу..."); Console.ReadLine();

        }

        public void OutputDictionary(int indx)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\t_______ {vocabularies[indx].nameDictionary} _______");
            Console.ForegroundColor = ConsoleColor.White;
            vocabularies[indx].OutputFullWord();
            Console.Write("нажмите любую клавишу..."); Console.ReadLine();
        }



        
        public void SearchWord()
        {
            bool flag = false;
            Console.Write("Введите слово: ");
            string word = Console.ReadLine();
            if (ChakBack(word, true)) return;
            int indx = 0;
            foreach (var item in vocabularies)
            {
                if (item.vocabluary.ContainsKey(word))
                {
                    Console.WriteLine(vocabularies[indx].nameDictionary);
                    vocabularies[indx].OutputTranslates(word);
                    flag = false;
                    indx++;
                }
                else
                {
                    Console.WriteLine(vocabularies[indx].nameDictionary);
                    flag = vocabularies[indx].OutputSearchTranslate(word);
                    indx++;
                }
            }
            if(flag) Console.WriteLine("\n\n\t\tВашего слова в словарях нету!!!\n\n");
            Console.Write("нажмите любую клавишу..."); Console.ReadLine();
        }
        

        private bool ChakBack(string str, bool flag)
        {
            if (flag)
            {
                try
                {
                    int temp = int.Parse(str);
                    if (temp == 0) return true;
                }
                catch (Exception ex) { }
                return false;
            }
            else
            {
                try
                {
                    int temp = int.Parse(str);
                    if (temp == 0) return true;
                }
                catch (Exception ex) { return true; }
                return false;
            }
            return true;
        }
    }






    /*=========================================================
                           КЛАСС СЛОВАРЬ
     ========================================================*/
    [DataContract]
    public class dictionary
    {
        [DataMember]
        public Dictionary<string, List<string>> vocabluary { set; get; }
        [DataMember]
        public string nameDictionary { set; get; }

        public dictionary(string nameDictionary)
        {
            vocabluary = new Dictionary<string, List<string>>();
            this.nameDictionary = nameDictionary;
        }

        public void ChangeNameDictionary()
        {
            Console.Write("Введите новый тип словаря: ");
            string name = Console.ReadLine();
            if (ChackBack(name)) return;
            while (name == "")
            {
                Console.Write("Вы ввели что то не так!!!\nВведите правильно: ");
                name = Console.ReadLine();
            }
            nameDictionary = name;
            return;

        }

        // Добавляем слово
        public void AddWord()
        {
            string word = null;
            List<string> translate = new List<string>();
            string _translate = null;


            Console.Write("Введите слово: ");
            word = Console.ReadLine();
            if (ChackBack(word)) return;
            while (vocabluary.ContainsKey(word) || word == "")
            {
                Console.WriteLine("\n\n\t\tЭто слово уже есть в словаре или вы неверно ввели!!!\n\n");
                Console.Write("Введите слово: ");
                word = Console.ReadLine();
                if (ChackBack(word)) return;
            }


            Console.Write("Введите перевод: ");
            _translate = Console.ReadLine();
            if (ChackBack(_translate)) return;
            while(_translate == "")
            {
                Console.WriteLine("\n\tПеревод введен не верно или такой перевод уже есть!!!\n");
                Console.Write("Введите перевод правильно: ");
                _translate = Console.ReadLine();
                if (ChackBack(_translate)) return;
            }

            translate.Add(_translate);
            vocabluary.Add(word, translate);
            Sorted();
        }

        // Изменить слово
        public void ChangeWord()
        {

            OutputShortWord();
            Console.Write("Введите слово которое хотите изменить: ");
            string temp = Console.ReadLine();

            if (ChackBack(temp)) return;
            while (!vocabluary.ContainsKey(temp))
            {
                Console.Write("Такого слова в словаре нету!!!\nВведите слово правильно: ");
                temp = Console.ReadLine();
                if (ChackBack(temp)) return;
            }

            List<string> list = vocabluary[temp];
            vocabluary.Remove(temp);
            string help = temp;

            Console.Write("Введите измененное слово: ");
            temp = Console.ReadLine();
            if (ChackBack(temp))
            {
                vocabluary[help] = list;
                Sorted();
                return;
            }
            while (temp == "" || vocabluary.ContainsKey(temp))
            {
                Console.Write("Вы неверно ввели слово или это слово уже есть в словаре!!!\nВведите слово правильно: ");
                temp = Console.ReadLine();
                if (ChackBack(temp)) return;
            }

            vocabluary[temp] = list;
            Sorted();
        }

        // Удаляем слово
        public void RemoveWord()
        {
            OutputShortWord();
            Console.Write("Введите cловo которое хотите удалить: ");
            string word = Console.ReadLine();
            if (ChackBack(word)) return;
            while (!vocabluary.ContainsKey(word))
            {
                Console.Write("Такого слова в словаре нету!!!\nВведите слово правильно: ");
                word = Console.ReadLine();
                if (ChackBack(word)) return;
            }
            vocabluary.Remove(word);
            return;
        }

        // Добавляем перевод
        public void AddTranslate()
        {
            OutputShortWord();
            Console.Write("Введите слово к которому хотите добавить перевод: ");
            string temp = Console.ReadLine();
            if (ChackBack(temp)) return;
            while (!vocabluary.ContainsKey(temp))
            {
                Console.Write("Такого слова в словаре нету!!!\nВведите слово правильно: ");
                temp = Console.ReadLine();
                if (ChackBack(temp)) return;
            }

            OutputTranslates(temp);

            string _translate;
            Console.Write("Введите еще один перевод: ");
            _translate = Console.ReadLine();
            if (ChackBack(_translate)) return;
            while (_translate == "" || vocabluary[temp].Contains(_translate))
            {
                Console.WriteLine("\n\tПеревод введен не верно или такой перевод уже есть!!!\n");
                Console.Write("Введите перевод правильно: ");
                _translate = Console.ReadLine();
                if (ChackBack(_translate)) return;
            }
            vocabluary[temp].Add(_translate);
            vocabluary[temp].Sort();
            return;
        }

        // Изменить перевод
        public void ChangeTranslate()
        {
            OutputShortWord();
            Console.Write("Введите слово у которого хотите изменить перевод: ");
            string temp = Console.ReadLine();
            if (ChackBack(temp)) return;
            while (!vocabluary.ContainsKey(temp))
            {
                Console.Write("Такого слова в словаре нету!!!\nВведите слово правильно: ");
                temp = Console.ReadLine();
                if (ChackBack(temp)) return;

            }

            OutputTranslates(temp);
            
            Console.Write("Введите перевод, который хотите изменить: ");
            string _translate = Console.ReadLine();
            if (ChackBack(_translate)) return;
            while (_translate == "" || !vocabluary[temp].Contains(_translate))
            {
                Console.WriteLine("\n\tПеревод введен не верно!!!\n");
                Console.Write("Введите перевод правильно: ");
                _translate = Console.ReadLine();
                if (ChackBack(_translate)) return;
            }
            vocabluary[temp].Remove(_translate);

            Console.Write("Введите измененный перевод: ");
            string _translate1 = Console.ReadLine();
            if (ChackBack(_translate1)) return;
            while (_translate1 == "" || vocabluary[temp].Contains(_translate1))
            {
                Console.WriteLine("\n\tПеревод введен не верно!!!\n");
                Console.Write("Введите перевод правильно: ");
                _translate = Console.ReadLine();
            }
            vocabluary[temp].Add(_translate1);
            vocabluary[temp].Sort();
            return;
        }

        // Удалить перевод
        public void RemoveTranslate()
        {
            OutputShortWord();
            Console.Write("Введите слово у которого хотите удалить перевод: ");
            string temp = Console.ReadLine();
            if (ChackBack(temp)) return;
            while (!vocabluary.ContainsKey(temp))
            {
                Console.Write("Такого слова в словаре нету!!!\nВведите слово правильно: ");
                temp = Console.ReadLine();
                if (ChackBack(temp)) return;

            }
            if (vocabluary[temp].Count == 1)
            {
                Console.WriteLine("\n\n\t\tВы не можете удалить перевод, так как он единственный в словаре!!!\n");
                return;
            }
            OutputTranslates(temp);
            Console.Write("Введите перевод, который хотите удалить: ");
            string _translate = Console.ReadLine();
            if (ChackBack(_translate)) return;
            while (_translate == "" || !vocabluary[temp].Contains(_translate))
            {
                Console.WriteLine("\n\tПеревод введен не верно!!!\n");
                Console.Write("Введите перевод правильно: ");
                _translate = Console.ReadLine();
                if (ChackBack(_translate)) return;
            }
            vocabluary[temp].Remove(_translate);
            return;
        }

        // Вывод слов
        private void OutputShortWord()
        {
            Console.Clear();
            int helper = 1;
            foreach (var word in vocabluary)
            {
                Console.Write(word.Key + "\t\t\t");
                if (helper % 3 == 0) Console.WriteLine();
                helper++;
            }
            Console.WriteLine("\n\n");
        }

        public void OutputTranslates(string word)
        {
            Console.Write(word + " - ");
            List<string> words = vocabluary[word];
            int flag = 1;
            foreach(var item in words)
            {
                Console.Write(item);
                if (flag != words.Count)
                    Console.Write(", ");
                flag++;
            }
            Console.WriteLine("\n\n");
        }

        public bool OutputSearchTranslate(string word)
        {

            string temp = null;
            foreach (var item in vocabluary)
            {
                foreach(var translate in item.Value)
                {
                    if(translate == word)
                    {
                        temp = item.Key;
                    }
                }
            }
            if (temp == null) return true;
            List<string> words = vocabluary[temp];
            int flag = 1;

            Console.Write(temp + " - ");
            foreach (var item in words)
            {
                Console.Write(item);
                if (flag != words.Count)
                    Console.Write(", ");
                flag++;
            }
            Console.WriteLine("\n");
            return false;

        }

        // Вывод слов с переводом
        public void OutputFullWord()
        {
            int flag = 1;
            Console.WriteLine("\tСлово\t\t\tПеревод\n");
            foreach (var word in vocabluary)
            {
                Console.Write("\t" + word.Key + "\t\t\t");
                foreach (var translates in word.Value)
                {
                    Console.Write(translates);
                    if (flag != word.Value.Count)
                        Console.Write(", ");
                    flag++;
                }
                Console.WriteLine();
                flag = 1;
            }

        }

        // Сортировка словаря
        private void Sorted()
        {
            var temp = vocabluary.OrderBy(x => x.Key);
            Dictionary<string, List<string>> helper = new Dictionary<string, List<string>>();
            foreach (var word in temp)
            {
                helper.Add(word.Key, word.Value);
            }
            vocabluary = helper;
        }

        // Проверяем на нажатый ноль
        private bool ChackBack(string str)
        {
            bool flag = false;
            try
            {
                int temp = int.Parse(str);
                if (temp == 0) flag = true;
            }
            catch (Exception ex) { }
            return flag;
        }
    }
}






