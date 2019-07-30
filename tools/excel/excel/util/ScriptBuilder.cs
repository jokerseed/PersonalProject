using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace excel.util
{
    class ScriptBuilder
    {
        private StringBuilder _builder = new StringBuilder();
        private StringBuilder _blank = new StringBuilder();

        //添加一行
        public void AppendLine(string message)
        {
            _builder.Append(_blank);
            _builder.AppendLine(message);
        }

        //     \t == tab
        //添加一个tab
        private void BeginBlank()
        {
            _blank.Append('\t');
        }

        private void EndBlank()
        {
            _blank.Remove(_blank.Length - 1, 1);
        }

        private void BeginNamespace(string name)
        {
            BeginBrace($"namespace {name}");
        }

        private void EndNamespace()
        {
            EndBrace();
        }

        private void BeginBrace(string code)
        {
            AppendLine($"{code}");
            AppendLine("{");
            BeginBlank();
        }

        private void EndBrace()
        {
            EndBlank();
            AppendLine("}");
        }
    }
}
