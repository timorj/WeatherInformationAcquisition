using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherInformationAcquisition.Src
{
    /// <summary>
    /// 根据propertyInfo动态创建类
    /// </summary>
    public class ClassHelper
    {
        public static object CreatInstance(string className, List<CustPropertyInfo> lcpi)
        {
            Type t = BuildType(className);
            t = AddPropertyToType(t, lcpi);
            return Activator.CreateInstance(t);
        }

        /// <summary>
        /// 添加属性到类型的实例
        /// </summary>
        /// <param name="name">指定的类型实例</param>
        /// <param name="lcpi"></param>
        /// <returns></returns>
        private static Type AddPropertyToType(Type classType, List<CustPropertyInfo> lcpi)
        {
            AppDomain myDomain = Thread.GetDomain();
            AssemblyName myAsmName = new AssemblyName();

            myAsmName.Name = "MyDynamicAssembly";

            AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.Run);

            ModuleBuilder myModBuidler = myAsmBuilder.DefineDynamicModule(myAsmName.Name);

            TypeBuilder myTypeBuilder = myModBuidler.DefineType(classType.FullName, TypeAttributes.Public);

            AddPropertyToTypeBuilder(myTypeBuilder, lcpi);

            Type retval = myTypeBuilder.CreateType();

            return retval;
        }

        private static void AddPropertyToTypeBuilder(TypeBuilder myTypeBuilder, List<CustPropertyInfo> lcpi)
        {
            PropertyBuilder custNamePropBldr;
            MethodBuilder custNameGetPropMthdBldr;
            MethodBuilder custNameSetPropMthdBldr;
            MethodAttributes getSetAtrr;
            ILGenerator custNameGetIL;
            ILGenerator custNameSetIL;

            //属性Set和Get方法设置一个专门的属性。这里设置为Public
            getSetAtrr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            foreach (CustPropertyInfo cpi in lcpi)
            {
                //定义字段
                FieldBuilder custNameBldr = myTypeBuilder.DefineField(cpi.FieldName, 
                    Type.GetType(cpi.Type), FieldAttributes.Private);

                //定义属性
                custNamePropBldr = myTypeBuilder.DefineProperty(cpi.PropertyName, PropertyAttributes.HasDefault, Type.GetType(cpi.Type), null);

                //定义get
                custNameGetPropMthdBldr = myTypeBuilder.DefineMethod(cpi.GetPropertyMethodName, getSetAtrr,
                    Type.GetType(cpi.Type),
                    Type.EmptyTypes);

                custNameGetIL = custNameGetPropMthdBldr.GetILGenerator();

                try
                {
                    custNameGetIL.Emit(OpCodes.Ldarg_0);
                    custNameGetIL.Emit(OpCodes.Ldfld, custNameBldr);
                    custNameGetIL.Emit(OpCodes.Ret);
                }
                catch(Exception ex)
                {
                    throw;
                }

                //定义set方法
                custNameSetPropMthdBldr = myTypeBuilder.DefineMethod(cpi.SetPropertyMethodName,
                    getSetAtrr,
                    null, new Type[] { Type.GetType(cpi.Type) });

                custNameSetIL = custNameSetPropMthdBldr.GetILGenerator();

                custNameSetIL.Emit(OpCodes.Ldarg_0);
                custNameSetIL.Emit(OpCodes.Ldarg_1);
                custNameSetIL.Emit(OpCodes.Stfld, custNameBldr);
                custNameSetIL.Emit(OpCodes.Ret);

                //把创建的两个方法加入到PropertyBuilder中
                custNamePropBldr.SetGetMethod(custNameGetPropMthdBldr);
                custNamePropBldr.SetSetMethod(custNameSetPropMthdBldr);

            }
        }

        /// <summary>
        /// 根据类名创建一个没有成员的类型的实例
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>创建的类型的实例</returns>
        public static Type BuildType(string className)
        {
            //获取应用程序域
            AppDomain domain = Thread.GetDomain();
            AssemblyName myAsmName = new AssemblyName();
            myAsmName.Name = "MyDynamicAssembly";

            //创建一个永久程序集
            AssemblyBuilder myAsmBuilder = domain.DefineDynamicAssembly(myAsmName, System.Reflection.Emit.AssemblyBuilderAccess.Run);

            //创建一个永久的单模程序块
            ModuleBuilder myModBuilder = myAsmBuilder.DefineDynamicModule(myAsmName.Name);
            
            TypeBuilder myTypeBuilder = myModBuilder.DefineType(className, TypeAttributes.Public);

            //创建类型
            Type retval = myTypeBuilder.CreateType();

            return retval;

        }

        public class CustPropertyInfo
        {
            private string propertyName;
            private string type;

            public string Type { get => type; set => type = value; }
            public string PropertyName { get => propertyName; set => propertyName = value; }

            public CustPropertyInfo()
            {

            }

            public CustPropertyInfo(string type, string propertyName)
            {
                this.Type = type;
                this.PropertyName = propertyName;
            }

            public string FieldName
            {
                get
                {
                    if(propertyName.Length < 1)
                    {
                        return "";
                    }
                    //首字母小写
                    return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
                }
            }

            /// <summary>
            /// 获取属性在IL中的Set方法名
            /// </summary>
            public string SetPropertyMethodName  =>  "set_" + PropertyName;

            /// <summary>
            /// 获取属性在IL中的Get方法名
            /// </summary>
            public string GetPropertyMethodName => "get_" + PropertyName;
            

        }

      
    }
}
