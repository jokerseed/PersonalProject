using System.Text;

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

        //命名空间
        public void BeginNamespace(string name)
        {
            BeginBrace($"namespace {name}");
        }

        public void EndNamespace()
        {
            EndBrace();
        }

        //类
        public void BeginClass(string modifer, string className, string superclass)
        {
            BeginBrace($"public {modifer} class {className} : {superclass}");
        }

        public void BeginClass(string modifer, string className)
        {
            BeginBrace($"public {modifer} class {className}");
        }

        public void BeginClass(string className)
        {
            BeginBrace($"public class {className}");
        }

        public void EndClass()
        {
            EndBrace();
        }

        //方法
        public void BeginMethod(string method)
        {
            BeginBrace(method);
        }

        public void EndMethod()
        {
            EndBrace();
        }

        //属性
        public void BeginProperty(string property)
        {
            BeginBrace(property);
        }

        public void EndProperty()
        {
            EndBrace();
        }

        //写入
        public void BeginBrace(string code)
        {
            AppendLine($"{code}");
            AppendLine("{");
            BeginBlank();
        }

        public void EndBrace()
        {
            EndBlank();
            AppendLine("}");
        }

        //获得最终代码
        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
