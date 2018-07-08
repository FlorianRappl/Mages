namespace Mages.Plugins.Transpilers.TreeWalkers
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class JavaScriptTreeWalker : TranspilerTreeWalker
    {
        private readonly List<String> _lines;
        private readonly List<Dependency> _dependencies;
        private readonly StringBuilder _buffer;
        private Int32 _indentation;

        public JavaScriptTreeWalker()
        {
            _lines = new List<String>();
            _buffer = new StringBuilder();
            _dependencies = new List<Dependency>();
            _indentation = 0;
        }

        protected override void InsertAdd(Action left, Action right)
        {
            InsertOperator("+", left, right);
        }

        protected override void InsertAnd(Action left, Action right)
        {
            InsertOperator("&&", left, right);
        }

        protected override void InsertArguments(Int32 length, Action<Int32> argument)
        {
            if (length > 0)
            {
                argument.Invoke(0);

                for (var i = 1; i < length; i++)
                {
                    _buffer.Append(", ");
                    argument.Invoke(i);
                }
            }
        }

        protected override void InsertAssignment(Action slot, Action value)
        {
            InsertOperator("=", slot, value);
        }

        protected override void InsertAwait(Action payload)
        {
            throw new NotImplementedException();
        }

        protected override void InsertBlock(Action body)
        {
            AddLine("{");
            Indent(body);
            AddLine("}");
        }

        protected override void InsertBreak()
        {
            AddLine("break;");
        }

        protected override void InsertCall(Boolean isAssigned, Action function, Action arguments)
        {
            Call(function, arguments);
        }

        protected override void InsertCase(Action condition, Action body)
        {
            throw new NotImplementedException();
        }

        protected override void InsertCondition(Action condition, Action primary, Action secondary)
        {
            Enclose(condition);
            _buffer.Append(" ? ");
            Enclose(primary);
            _buffer.Append(" : ");
            Enclose(secondary);
        }

        protected override void InsertConstant(Object value)
        {
            if (value is String)
            {
                Escape((String)value);
            }
            else if (value is Double)
            {
                _buffer.Append(((Double)value).ToString(CultureInfo.InvariantCulture));
            }
            else if (value is Boolean)
            {
                _buffer.Append((Boolean)value ? "true" : "false");
            }
        }

        protected override void InsertContinue()
        {
            AddLine("continue;");
        }

        protected override void InsertDefVariable(String name)
        {
            _buffer.Append(name);
        }

        protected override void InsertDelMember(Action obj, String name)
        {
            _buffer.Append("delete ");
            obj.Invoke();
            _buffer.Append('[');
            Escape(name);
            _buffer.Append(']');
        }

        protected override void InsertDelVariable(String name)
        {
            _buffer.Append("delete ");
            _buffer.Append(name);
        }

        protected override void InsertDiv(Action left, Action right)
        {
            InsertOperator("/", left, right);
        }

        protected override void InsertEq(Action left, Action right)
        {
            InsertOperator("===", left, right);
        }

        protected override void InsertFactorial(Action expression)
        {
            Include(Dependency.Factorial);
            Call("__magesFactorial__", expression);
        }

        protected override void InsertFor(Action init, Action condition, Action post, Action body)
        {
            _buffer.Append("for ");
            Enclose(() =>
            {
                init.Invoke();
                _buffer.Append("; ");
                condition.Invoke();
                _buffer.Append("; ");
                post.Invoke();
            });
            CloseLine();
            Indent(body);
        }

        protected override void InsertFunction(Boolean isMethod, Action parameters, Action body)
        {
            _buffer.Append("(function ");
            Enclose(parameters);
            _buffer.Append(" {");
            CloseLine();
            Indent(body);
            _buffer.Append("})");

        }

        protected override void InsertGeq(Action left, Action right)
        {
            InsertOperator(">=", left, right);
        }

        protected override void InsertGetMember(Action obj, Action property)
        {
            obj.Invoke();
            _buffer.Append('.');
            property.Invoke();
        }

        protected override void InsertGetVariable(String name)
        {
            _buffer.Append(name);
        }

        protected override void InsertGt(Action left, Action right)
        {
            InsertOperator(">", left, right);
        }

        protected override void InsertIdentifier(String name)
        {
            _buffer.Append(name);
        }

        protected override void InsertIf(Action condition, Action primary, Action secondary)
        {
            _buffer.Append("if ");
            Enclose(condition);
            CloseLine();
            Indent(primary);
            _buffer.Append("else");
            CloseLine();
            Indent(secondary);//TODO can be empty expression
        }

        protected override void InsertMod(Action left, Action right)
        {
            InsertOperator("%", left, right);
        }

        protected override void InsertPow(Action left, Action right)
        {
            Call("Math.pow", () =>
            {
                left.Invoke();
                _buffer.Append(", ");
                right.Invoke();
            });
        }

        protected override void InsertInterpolatedString(String formattedString, Int32 length, Action<Int32> replacement)
        {
            var args = new String[length];
            var tmp = _buffer.ToString();
            _buffer.Clear();

            for (var i = 0; i < length; i++)
            {
                _buffer.Append("${ ");
                replacement.Invoke(i);
                _buffer.Append(" }");
                args[i] = _buffer.ToString();
                _buffer.Clear();
            }
            
            _buffer.Append(tmp);
            _buffer.Append('`');
            _buffer.Append(String.Format(formattedString, args));
            _buffer.Append('`');
        }

        protected override void InsertLeq(Action left, Action right)
        {
            InsertOperator("<=", left, right);
        }

        protected override void InsertLookupType(Action expression)
        {
            Call("typeof", expression);
        }

        protected override void InsertLt(Action left, Action right)
        {
            InsertOperator("<", left, right);
        }

        protected override void InsertMatch(Action reference, Action cases)
        {
            throw new NotImplementedException();
        }

        protected override void InsertMatrix(Int32 cols, Int32 rows, Action<Int32, Int32> cell)
        {
            _buffer.Append('[');

            for (var j = 0; j < rows; j++)
            {
                _buffer.Append(" [");
                cell.Invoke(0, j);

                for (var i = 1; i < cols; i++)
                {
                    _buffer.Append(", ");
                    cell.Invoke(i, j);
                }

                _buffer.Append((j + 1 == rows) ? "] " : "],");
            }

            _buffer.Append(']');
        }

        protected override void InsertMul(Action left, Action right)
        {
            InsertOperator("*", left, right);
        }

        protected override void InsertNegative(Action expression)
        {
            InsertPrefixed("-", expression);
        }

        protected override void InsertNeq(Action left, Action right)
        {
            InsertOperator("!==", left, right);
        }

        protected override void InsertNot(Action expression)
        {
            InsertPrefixed("!", expression);
        }

        protected override void InsertObject(Int32 length, Action<Int32> property)
        {
            Enclose(() =>
            {
                _buffer.Append('{');

                if (length > 0)
                {
                    _buffer.AppendLine();
                    Indent(() => 
                    {
                        for (var i = 0; i < length; i++)
                        {
                            property.Invoke(i);
                            _buffer.AppendLine(",");
                        }
                    });
                }

                _buffer.Append('}');
            });
        }

        protected override void InsertOptionalParameter(String name, Action defaultValue)
        {
            throw new NotImplementedException();
        }

        protected override void InsertOr(Action left, Action right)
        {
            InsertOperator("||", left, right);
        }

        protected override void InsertParameters(Int32 length, Action<Int32> parameter)
        {
            if (length > 0)
            {
                parameter.Invoke(0);

                for (var i = 1; i < length; i++)
                {
                    _buffer.Append(", ");
                    parameter.Invoke(i);
                }
            }
        }

        protected override void InsertPipe(Action left, Action right)
        {
            Call(right, left);
        }

        protected override void InsertPositive(Action expression)
        {
            InsertPrefixed("+", expression);
        }

        protected override void InsertPostDecrement(Action expression)
        {
            InsertPostfixed("--", expression);
        }

        protected override void InsertPostIncrement(Action expression)
        {
            InsertPostfixed("++", expression);
        }

        protected override void InsertPreDecrement(Action expression)
        {
            InsertPrefixed("--", expression);
        }

        protected override void InsertPreIncrement(Action expression)
        {
            InsertPrefixed("++", expression);
        }

        protected override void InsertProperty(Action name, Action value)
        {
            _buffer.Append(' ', _indentation);
            name.Invoke();
            _buffer.Append(": ");
            value.Invoke();
        }

        protected override void InsertRange(Action from, Action to, Action step)
        {
            Include(Dependency.Range);
            Call("__magesRange__", () =>
            {
                from.Invoke();
                _buffer.Append(", ");
                to.Invoke();
                _buffer.Append(", ");
                step.Invoke();
            });
        }

        protected override void InsertRequiredParameters(String name)
        {
            InsertIdentifier(name);
        }

        protected override void InsertReturn(Action payload)
        {
            payload.Invoke();
         
            if (_buffer.Length > 0)
            {
                _buffer.Insert(0, "return ");
                _buffer.Append(';');
                CloseLine();
            }
            else
            {
                AddLine("return;");
            }
        }

        protected override void InsertReverseDiv(Action left, Action right)
        {
            InsertDiv(right, left);
        }

        protected override void InsertSetMember(Action obj, Action property)
        {
            InsertGetMember(obj, property);
        }

        protected override void InsertSetVariable(String name)
        {
            InsertGetVariable(name);
        }

        protected override void InsertStatement(Action body)
        {
            body.Invoke();
            _buffer.Append(';');
            CloseLine();
        }

        protected override void InsertSub(Action left, Action right)
        {
            InsertOperator("-", left, right);
        }

        protected override void InsertTranspose(Action expression)
        {
            Include(Dependency.Transpose);
            Call("__magesTranspose__", expression);
        }

        protected override void InsertWhile(Action condition, Action body)
        {
            _buffer.Append("while ");
            Enclose(condition);
            CloseLine();
            Indent(body);
        }

        protected override String Stringify()
        {
            var libs = _dependencies.Select(m => m.Code).ToArray();
            var content = String.Join(Environment.NewLine, _lines);

            if (libs.Length != 0)
            {
                var header = String.Join(Environment.NewLine, libs);
                return String.Concat(header, Environment.NewLine, content);
            }

            return content;
        }

        private void AddLine(String content)
        {
            _buffer.Append(' ', _indentation);
            _buffer.Append(content);
            _lines.Add(_buffer.ToString());
            _buffer.Clear();
        }

        private void CloseLine()
        {
            _buffer.Insert(0, " ", _indentation);
            _lines.Add(_buffer.ToString());
            _buffer.Clear();
        }

        private void Escape(String value)
        {
            using (var writer = new StringWriter(_buffer))
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(value), writer, null);
                }
            }
        }

        private void Call(Action function, Action arguments)
        {
            function.Invoke();
            Enclose(arguments);
        }

        private void Call(String name, Action parameters)
        {
            _buffer.Append(name);
            Enclose(parameters);
        }

        private void Enclose(Action callback)
        {
            _buffer.Append('(');
            callback.Invoke();
            _buffer.Append(')');
        }

        private void Indent(Action callback)
        {
            _indentation += 2;
            callback.Invoke();
            _indentation -= 2;
        }

        private void InsertOperator(String operation, Action left, Action right)
        {
            left.Invoke();
            _buffer.Append(' ');
            _buffer.Append(operation);
            _buffer.Append(' ');
            right.Invoke();
        }

        private void InsertPrefixed(String operation, Action expression)
        {
            _buffer.Append(operation);
            expression.Invoke();
        }

        private void InsertPostfixed(String operation, Action expression)
        {
            expression.Invoke();
            _buffer.Append(operation);
        }

        private void Include(Dependency dependency)
        {
            if (!_dependencies.Contains(dependency))
            {
                _dependencies.Add(dependency);
            }
        }

        private sealed class Dependency
        {
            private readonly String _code;

            private Dependency(String code)
            {
                _code = code;
            }

            public String Code
            {
                get { return _code; }
            }

            public static readonly Dependency Range = new Dependency("function __magesRange__(start, stop, step) { /* ... */ }");

            public static readonly Dependency Factorial = new Dependency("function __magesFactorial__(value) { /* ... */ }");

            public static readonly Dependency Transpose = new Dependency("function __magesTranspose__(value) { /* ... */ }");
        }
    }
}
