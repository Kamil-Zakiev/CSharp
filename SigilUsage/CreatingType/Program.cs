using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Sigil;

// Источник: https://habrahabr.ru/sandbox/107626/
namespace CreatingType
{
    /// <summary> Описание поля </summary>
    internal class FieldDescription
    {
        /// <summary> Признак ключевого поля </summary>
        public bool IsKey;

        /// <summary> Название поля </summary>
        public string Name;

        /// <summary> Тип поля </summary>
        public Type Type;
    }

    public interface IHasKey
    {
        string Key { get; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // список полей, входящих в тип
            var fields = new List<FieldDescription>
            {
                new FieldDescription {Name = "FirstName", Type = typeof(string), IsKey = false},
                new FieldDescription {Name = "FriendsCount", Type = typeof(int), IsKey = true},
                new FieldDescription {Name = "Birthday", Type = typeof(DateTime), IsKey = false}
            };

            // создание типа с использованием Sigil
            var generatedType = CreateType("MyNamespace", "MyType", fields);
            var obj = (IHasKey) Activator.CreateInstance(generatedType);

            // по-другому, без dynamic, к полям объекта никак не обратиться
            dynamic just4Test = obj;
            just4Test.FriendsCount = 123;

            Console.WriteLine($"Magic happends, object key is: {obj.Key} Object type is {obj.GetType()}");
            Console.ReadLine();
        }

        private static Type CreateType(string typeNamespace, string typeName,
            IEnumerable<FieldDescription> fieldDescriptions)
        {
            var assembleName = new AssemblyName {Name = typeName};
            // создаем сборку
            var assembleBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(assembleName, AssemblyBuilderAccess.RunAndCollect);

            // внутри сборки создаем управляемый модуль
            var moduleBuilder = assembleBuilder.DefineDynamicModule(assembleName.Name);

            // внутри модуля создаем тип 
            var typeBuilder = moduleBuilder.DefineType($"{typeNamespace}.{typeName}",
                TypeAttributes.Class | TypeAttributes.Public);

#if DEBUG
            var allowUnverifiableCode = false;
            var doVerify = true;
#else
            var allowUnverifiableCode = true;
            var doVerify = false;
#endif
            // добавляем поля в созданный тип
            FieldBuilder keyFieldBuilder = null;
            FieldDescription keyFieldDescription = null;
            foreach (var fieldDescription in fieldDescriptions)
            {
                if (fieldDescription.IsKey)
                {
                    keyFieldBuilder = typeBuilder.DefineField(fieldDescription.Name, fieldDescription.Type,
                        FieldAttributes.Public);
                    keyFieldDescription = fieldDescription;
                    continue;
                }
                typeBuilder.DefineField(fieldDescription.Name, fieldDescription.Type, FieldAttributes.Public);
            }

            // добавляем реализацию интерфейса
            typeBuilder.AddInterfaceImplementation(typeof(IHasKey));

            // создаем метод экземляра типа
            var keyGetter = Emit<Func<string>>.BuildInstanceMethod(typeBuilder, "get_Key",
                MethodAttributes.Public | MethodAttributes.HideBySig |
                MethodAttributes.SpecialName /* Имя специальное, т.к. мы создаем геттер */ |
                MethodAttributes.Virtual, allowUnverifiableCode, doVerify);

            // загружаем объект, значение которого будет возвращаться
            keyGetter.LoadArgument(0);
            // ReSharper disable once PossibleNullReferenceException
            if (keyFieldDescription.Type == typeof(string))
            {
                keyGetter.LoadField(keyFieldBuilder);
            }
            else
            {
                if (keyFieldDescription.Type.IsValueType)
                {
                    keyGetter.LoadFieldAddress(keyFieldBuilder);
                }
                else
                {
                    keyGetter.LoadField(keyFieldBuilder);
                }

                // вызов метода ToString()
                keyGetter.CallVirtual(keyFieldDescription.Type.GetMethods()
                    .First(m => m.Name == "ToString" && !m.GetParameters().Any()));
            }

            // заканчиваем функцию, стек должен остаться пустым
            keyGetter.Return();

            // запись IL-кода в MethodBuilder, последующие изменения метода невозможны
            var methodBuilder = keyGetter.CreateMethod();

            return typeBuilder.CreateType();
        }
    }
}