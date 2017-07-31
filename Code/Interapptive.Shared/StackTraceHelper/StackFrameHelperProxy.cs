using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// Proxy to StackFrameHelper class (the one that fills StackTrace).
    /// Working with it directly allows for slight performance increase capturing stack traces in managed code.
    /// </summary>
    public class StackFrameHelperProxy
    {
        public static Type UnderlyingType { get; set; }

        private static readonly Func<bool, object> createUnderlyingInstance;
        private static readonly Func<object, int> getNumberOfFramesVal;
        private static readonly Func<object, int, StackFrameSlim> createFrameVal;
        private static readonly Func<IntPtr, MethodBase> getMethodVal;

        /// <summary>
        /// Creates necessary delegates that proxy calls to StackFrameHelper
        /// </summary>
        static StackFrameHelperProxy()
        {
            UnderlyingType = Type.GetType("System.Diagnostics.StackFrameHelper", true);

            var stackFrameHelperCtor = UnderlyingType.GetConstructor(new[] { typeof(bool), typeof(Thread) });
            var currentThreadProperty = typeof(Thread).GetProperty("CurrentThread");

            var needFileLineColInfoParam = Expression.Parameter(typeof(bool));
            createUnderlyingInstance = Expression.Lambda<Func<bool, object>>(
                Expression.New(
                    stackFrameHelperCtor,
                    needFileLineColInfoParam,
                    Expression.Property(null, currentThreadProperty)),
                needFileLineColInfoParam).Compile();


            var stackFrameHelperParam = Expression.Parameter(typeof(Object));

            var stackFrameHelperAsItself = Expression.Convert(stackFrameHelperParam, UnderlyingType);
            getNumberOfFramesVal = Expression.Lambda<Func<Object, int>>(Expression.Field(stackFrameHelperAsItself, UnderlyingType.GetField("iFrameCount", BindingFlags.Instance | BindingFlags.NonPublic)), stackFrameHelperParam).Compile();

            var stackFrameSlimCtor = typeof(StackFrameSlim).GetConstructors().Single();
            var intParam = Expression.Parameter(typeof(int));

            var getters = from param in stackFrameSlimCtor.GetParameters()
                          let field = UnderlyingType.GetField(param.Name, BindingFlags.Instance | BindingFlags.NonPublic)
                          let fieldExpr = Expression.Field(stackFrameHelperAsItself, field)
                          select Expression.Condition(Expression.Equal(fieldExpr, Expression.Constant(null, field.FieldType)), Expression.Default(param.ParameterType), Expression.ArrayAccess(fieldExpr, intParam));
            createFrameVal = Expression.Lambda<Func<Object, int, StackFrameSlim>>(Expression.New(stackFrameSlimCtor, getters), stackFrameHelperParam, intParam).Compile();

            var intPtrParam = Expression.Parameter(typeof(IntPtr));

            var iRuntimeMethodInfoParams = new[] { Type.GetType("System.IRuntimeMethodInfo") };
            getMethodVal = Expression.Lambda<Func<IntPtr, MethodBase>>(
                Expression.Condition(
                    Expression.Call(intPtrParam, typeof(IntPtr).GetMethod("IsNull", BindingFlags.Instance | BindingFlags.NonPublic)),
                    Expression.Constant(null, typeof(MethodBase)),
                    Expression.Call(Type.GetType("System.RuntimeType").GetMethod("GetMethodBase", BindingFlags.Static | BindingFlags.NonPublic, null, iRuntimeMethodInfoParams, null),
                        Expression.Call(typeof(RuntimeMethodHandle).GetMethod("GetTypicalMethodDefinition", BindingFlags.Static | BindingFlags.NonPublic, null, iRuntimeMethodInfoParams, null),
                        Expression.New(Type.GetType("System.RuntimeMethodInfoStub").GetConstructor(new[] { typeof(IntPtr), typeof(Object) }), intPtrParam,
                            Expression.Constant(null, typeof(Object)))))), intPtrParam).Compile();
        }

        /// <summary>
        /// Actual instance of StackFrameHelper
        /// </summary>
        public object UnderlyingInstance { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="needFileLineColInfo">Whether source file information should be extracted</param>
        public StackFrameHelperProxy(bool needFileLineColInfo)
        {
            UnderlyingInstance = createUnderlyingInstance(needFileLineColInfo);
        }

        /// <summary>
        /// How many frames does current stack trace have
        /// </summary>
        /// <returns>Number of frames</returns>
        public int GetNumberOfFrames() => getNumberOfFramesVal(UnderlyingInstance);

        /// <summary>
        /// Fills frame information into a StackFrameSlim
        /// </summary>
        /// <param name="index">Frame index in a stack trace</param>
        /// <returns>New StackFrameSlim</returns>
        public StackFrameSlim CreateFrame(int index) => createFrameVal(UnderlyingInstance, index);

        /// <summary>
        /// Resolve method from handle (this is expensive)
        /// </summary>
        /// <param name="handle">Handle</param>
        /// <returns>MethodBase</returns>
        public static MethodBase GetMethod(IntPtr handle) => getMethodVal(handle);
    }
}