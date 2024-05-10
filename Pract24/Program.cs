using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pract24.Program;

namespace Pract24
{
    internal class Program
    {
        public enum OutputFormat
        {
            Markdown,
            Html
        }
        public interface IListStratergy
        {
            void Start(StringBuilder sb);
            void AddListItem(StringBuilder sb, string item);
            void End(StringBuilder sb);
        }

        public class TextProcessor<LS>
            where LS : IListStratergy, new()
        {
            private StringBuilder _sb = new StringBuilder();
            private IListStratergy _listStrategy=new LS();

            public void AppendList(IEnumerable<string> items)
            {
                _listStrategy.Start(_sb);
                foreach (var item in items)
                    _listStrategy.AddListItem(_sb, item);
                _listStrategy.End(_sb);
            }
            public void Clear()
            {
                _sb.Clear();
            }
            public override string ToString()
            {
                return _sb.ToString();
            }
        }
        public class HtmlListStrategy : IListStratergy
        {
            public void Start(StringBuilder sb) => sb.AppendLine("< ul >");
            public void End(StringBuilder sb) => sb.AppendLine("</ ul >");
            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($"		< li >{ item}</ li >");
            }
        }
        public class MarkdownListStrategy : IListStratergy
        {
            public void Start(StringBuilder sb) { }
            public void End(StringBuilder sb) { }
            public void AddListItem(StringBuilder sb,string item)
            {
                sb.AppendLine($"		* { item}");
            }
        }
        public class StupidJSONStrategy : IListStratergy
        {
            public void Start(StringBuilder sb)
            {
                sb.AppendLine($"List{listNumber++}");
                sb.AppendLine("{");
            }
            public void AddListItem(StringBuilder sb, string item)
            {
                sb.AppendLine($"\titem{itemNumber++}\t=\t{item}");
            }
            public void End(StringBuilder sb)
            {
                sb.AppendLine("}");
                itemNumber = 0;
            }

            uint itemNumber = 0;
            uint listNumber = 0;
        }




        static void Main(string[] args)
        {
            Console.ForegroundColor= ConsoleColor.Blue;
            TextProcessor<HtmlListStrategy> tp = new TextProcessor<HtmlListStrategy>();
            tp.Clear();
            tp.AppendList(new[] {"foo", "bar"});
            Console.WriteLine(tp);


            Console.ForegroundColor = ConsoleColor.Magenta;
            TextProcessor<StupidJSONStrategy> tp2=new TextProcessor<StupidJSONStrategy>();
            tp2.Clear();
            tp2.AppendList(new[] { "Новая игра","Продолжить","Сетевая игра","Выход" });
            tp2.AppendList(new[] { "Новая игра", "Продолжить", "Сетевая игра", "Вход" });
            Console.WriteLine(tp2);



            Console.ForegroundColor = ConsoleColor.Green;
            TextProcessor<MarkdownListStrategy> tp3= new TextProcessor<MarkdownListStrategy>();
            tp3.Clear();
            tp3.AppendList(new[] { "Проект Pract24", "Небольшой проект" });
            Console.WriteLine(tp3);


            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
